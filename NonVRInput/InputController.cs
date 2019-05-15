using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Inputs;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class InputController : MonoBehaviour
{
    public enum ControllerType { Joystick, Keyboard, Treadmill, Teleport }
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 0f;     // Speed when walking forward
        public float BackwardSpeed = 0f;    // Speed when walking backwards
        public float StrafeSpeed = 0f;      // Speed when walking sideways
        public float RunMultiplier = 0f;    // Speed when sprinting
        public float SlowdownRate = 20f;
        public void SetUniformSpeed(float speed)
        {
            ForwardSpeed = speed;
            BackwardSpeed = speed;
            StrafeSpeed = speed;
        } // This sets all speeds to be the same
    }
    [Serializable]
    public class LookSettings
    {
        public bool ClampVerticalRotation = true;
        public bool InvertXAxis = false;
        public bool InvertYAxis = false;
        public bool Smooth = false;
        public bool LockCursor = false;

        public float SmoothTime = 0f;
        public float XSensitivity = 1f;
        public float YSensitivity = 1f;
        public float MinimumX = -45f;
        public float MaximumX = 45f;

        private Quaternion m_CharacterTargetRot;
        private List<Quaternion> m_CameraTargetRots = new List<Quaternion>();
        private bool m_cursorIsLocked = true;
        private List<Transform> m_CameraTransforms = new List<Transform>();

        public void SetUniformSensitivity(float sensitivity)
        {
            XSensitivity = sensitivity;
            YSensitivity = sensitivity;
        }

        public void Init(Transform character, List<Camera> cameras)
        {
            m_CharacterTargetRot = character.localRotation;
            foreach (Camera cam in cameras)
            {
                m_CameraTransforms.Add(cam.transform);
                m_CameraTargetRots.Add(cam.transform.localRotation);
            }
        }

        public void Reorient(Transform character, float yRot)
        {
            float xRot = 0f;
           
            if (InvertXAxis && !InvertYAxis) { xRot = -xRot; }
            else if (InvertYAxis && !InvertXAxis) { yRot = -yRot; }
            else if (InvertXAxis && InvertYAxis) { xRot = -xRot; yRot = -yRot; }


            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            for (int i = 0; i < m_CameraTargetRots.Count; i++)
            {
                m_CameraTargetRots[i] *= Quaternion.Euler(-xRot, 0f, 0f);
            }

            if (ClampVerticalRotation)
            {
                for (int i = 0; i < m_CameraTargetRots.Count; i++)
                {
                    m_CameraTargetRots[i] = ClampRotationAroundXAxis(m_CameraTargetRots[i]);
                }
            }

            if (Smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    SmoothTime * Time.deltaTime);

                for (int i = 0; i < m_CameraTransforms.Count; i++)
                {
                    m_CameraTransforms[i].localRotation = Quaternion.Slerp(m_CameraTransforms[i].localRotation, m_CameraTargetRots[i],
                    SmoothTime * Time.deltaTime);
                }
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                for (int i = 0; i < m_CameraTransforms.Count; i++)
                {
                    m_CameraTransforms[i].localRotation = m_CameraTargetRots[i];
                }
            }

            UpdateCursorLock();
        }

        public void LookRotation(Transform character, ControllerType ctype)
        {
            float yRot = 0f;
            float xRot = 0f;
            if (ctype == ControllerType.Keyboard)
            {
                yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
                xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
            }
            else if(ctype == ControllerType.Joystick)
            {
                yRot = LogitechExtreme3DPro.StickRotate(AxisState.Raw) * XSensitivity;
                xRot = LogitechExtreme3DPro.StickY(AxisState.Raw) * YSensitivity;
            }
            if (InvertXAxis && !InvertYAxis) { xRot = -xRot; }
            else if(InvertYAxis && !InvertXAxis) { yRot = -yRot; }
            else if(InvertXAxis && InvertYAxis) { xRot = -xRot; yRot = -yRot; }


            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            for(int i = 0; i < m_CameraTargetRots.Count; i++)
            {
                m_CameraTargetRots[i] *= Quaternion.Euler(-xRot, 0f, 0f);
            }

            if (ClampVerticalRotation)
            {
                for (int i = 0; i < m_CameraTargetRots.Count; i++)
                {
                    m_CameraTargetRots[i] = ClampRotationAroundXAxis(m_CameraTargetRots[i]);
                }
            }

            if (Smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    SmoothTime * Time.deltaTime);

                for (int i = 0; i < m_CameraTransforms.Count; i++)
                {
                    m_CameraTransforms[i].localRotation = Quaternion.Slerp(m_CameraTransforms[i].localRotation, m_CameraTargetRots[i],
                    SmoothTime * Time.deltaTime);
                }
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                for (int i = 0; i < m_CameraTransforms.Count; i++)
                {
                    m_CameraTransforms[i].localRotation = m_CameraTargetRots[i];
                }
            }

            UpdateCursorLock();
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        public void SetCursorLock(bool value)
        {
            LockCursor = value;
            if (!LockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (LockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public List<Camera> cameras; // Camera 0 is master
    public ControllerType controllerType;
    public MovementSettings movementSettings = new MovementSettings();
    public LookSettings lookSettings = new LookSettings();
    public bool pauseInput = false;
    [HideInInspector] public bool forceRotation = false;
    [HideInInspector] public float CurrentTargetSpeed = 0f;
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (movementSettings.ForwardSpeed == 0) { Debug.LogWarning("Forward movement speed is set to 0"); }
        else if(movementSettings.BackwardSpeed == 0) { Debug.LogWarning("Backward movement speed is set to 0"); }
        else if(movementSettings.StrafeSpeed == 0) { Debug.LogWarning("Strafe movement speed is set to 0"); }
        else if(movementSettings.SlowdownRate == 0) { Debug.LogWarning("Slowdown rate is set to 0"); }
        if(PlayerPrefs.GetFloat("MoveSpeed") != 0)
        {
            movementSettings.ForwardSpeed = PlayerPrefs.GetFloat("MoveSpeed");
            movementSettings.BackwardSpeed = PlayerPrefs.GetFloat("MoveSpeed");
            movementSettings.StrafeSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        }
        if(PlayerPrefs.GetFloat("RotSens") != 0)
        {
            lookSettings.XSensitivity = PlayerPrefs.GetFloat("RotSens");
            lookSettings.YSensitivity = PlayerPrefs.GetFloat("RotSens");
        }

        m_Rigidbody = GetComponent<Rigidbody>();
        lookSettings.Init(transform, cameras);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pauseInput) RotateView();
    }
    void FixedUpdate()
    {
        if(!pauseInput)
        {
            var input = GetInput(controllerType);
            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon))
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cameras[0].transform.forward * input.y + cameras[0].transform.right * input.x;
                //desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.zero).normalized;

                desiredMove.x = desiredMove.x * CurrentTargetSpeed;
                desiredMove.z = desiredMove.z * CurrentTargetSpeed;
                desiredMove.y = desiredMove.y * CurrentTargetSpeed;

                if (m_Rigidbody.velocity.sqrMagnitude < (CurrentTargetSpeed * CurrentTargetSpeed))
                {
                    m_Rigidbody.AddForce(desiredMove, ForceMode.Impulse);
                }
                else
                {
                    m_Rigidbody.drag = movementSettings.SlowdownRate;
                }

            }
        }
        
    }

    private Vector2 GetInput(ControllerType ctype)
    {
        Vector2 input = Vector2.zero;
        if (ctype == ControllerType.Joystick)
        {
            input.x = LogitechExtreme3DPro.HatX(AxisState.Raw);
            input.y = LogitechExtreme3DPro.HatY(AxisState.Raw);
            UpdateDesiredTargetSpeed(input);
            return input;
        }
        else if (ctype == ControllerType.Keyboard)
        {
            input.x = CrossPlatformInputManager.GetAxis("Horizontal");
            input.y = CrossPlatformInputManager.GetAxis("Vertical");
            UpdateDesiredTargetSpeed(input);
            return input;
        }
        else return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        lookSettings.LookRotation(transform, controllerType);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
    }

    public void ForceRotate(float yRot=180)
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        lookSettings.Reorient(transform, yRot);

        // Rotate the rigidbody velocity to match the new direction that the character is looking
        Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
    }

    public void UpdateDesiredTargetSpeed(Vector2 input)
    {
        if (input == Vector2.zero) return;
        if (input.x > 0 || input.x < 0) CurrentTargetSpeed = movementSettings.StrafeSpeed;
        else if (input.y < 0) CurrentTargetSpeed = movementSettings.BackwardSpeed;
        else if (input.y > 0) CurrentTargetSpeed = movementSettings.ForwardSpeed;
    }
}
