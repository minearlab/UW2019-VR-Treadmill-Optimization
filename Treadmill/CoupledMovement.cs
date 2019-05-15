using System;
using System.Collections;
using CybSDK;
using UnityEngine;


// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 7/06/2018
// 
// This script handles how the player moves when operating the Virtualizer Omindirectional Treadmill
// The player is only able to move forwards (strafing is not handled well by the Virtualizer Omindirectional Treadmill)
// (Implementing backwards movement is NOT advised)
// 
// This script assumes that critical additions to the [ CVirtDeviceNative.cs ] script  have been made
// Specifically, that functions named [ GetDirectionRaw() ] and [ GetOrientationRaw() ] have been added.
//
// The modified file can be found in the CybSDK file in this repository 
// (all the files necesarry are the property of Cyberith and as such we cannot upload them to this repo) 
//
// This script is a modified version of the Unity standard assets [ RigidBodyFirstPersonController ] script



[RequireComponent(typeof(CVirtDeviceController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CoupledMovement : MonoBehaviour{

    public Camera cam;
    public GameObject bodyRotation;
    public GameObject leftController;
    public GameObject rightController;
    public AdvancedSettings advancedSettings = new AdvancedSettings();
    public bool useTriggersToTurn = false;
    [Range(0f, 1f)] public float smoothingConstant = 1f;

    private float m_MoveSpeed = 0f;
    private float m_SensorThreshold = 0f;
    private float m_RotationThreshold = 0f;
    private float m_PrevOutput = 0f;
    private float m_OldAngle = 0f;

    private bool m_IsGrounded = false;
    private bool m_PreviouslyGrounded = false;
    private bool m_ResetOrientation = true;

    private CapsuleCollider m_Capsule;
    private Rigidbody m_RigidBody;
    private CVirtDeviceController m_DeviceController = null;
    private CVirtDevice m_Device = null;


    private Vector3 m_GroundContactNormal;
    private Quaternion m_OldRotation;


    [Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        [Tooltip("Set it to 0.1 or more if you get stuck in a wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }



    private void Awake()
    {
        PlayerPrefs.SetFloat("MoveSpeed", 0f);
        PlayerPrefs.SetString("Version", "Coupled");
    }

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_DeviceController = GetComponent<CVirtDeviceController>();
        m_ResetOrientation = true;

        bodyRotation.transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
        transform.parent = null;

    }

    // Rotation is handled in the Update (since it is a more "sensitive" operation)
    private void Update()
    {
        if (m_DeviceController != null)
        {
            DebugLogger.Log("[CoupledMovement.cs] :: [1] :: Getting Virtualizer Device\r\n");
            m_Device = m_DeviceController.GetDevice();
        }
        else
        {
            DebugLogger.Log("[CoupledMovement.cs] :: [ERROR] :: Failed To Get Virtualizer Device!\r\n" +
                                                                "           -- m_DeviceController value is NULL\r\n" +
                                                                "           -- Is the Virtualizer plugged in?\r\n\r\n");
            Debug.LogException(new System.Exception("[Virtualizer Rig Error]: Device Controller Value Is Null"));
        }

        if(m_Device != null)
        {
            DebugLogger.Log("[CoupledMovement.cs] :: [2] :: Polling Rotation Threshold...\r\n");
            m_RotationThreshold = PlayerPrefs.GetFloat("RotSens");
            DebugLogger.Log("[CoupledMovement.cs] :: [2.1] :: Retrieved a value of [" + m_RotationThreshold.ToString() + "]\r\n");
            if (m_ResetOrientation)
            {
                DebugLogger.Log("[CoupledMovement.cs] :: [2.2] :: Resetting Player Orientation");
                m_Device.ResetPlayerOrientation();
                m_ResetOrientation = false;
            }

            if (useTriggersToTurn)
            {
                DebugLogger.Log("[CoupledMovement.cs] :: [2.3] :: Triggers ARE being used to turn\r\n");
                if (leftController.GetComponent<SteamVR_TrackedController>().triggerFullPress
               || rightController.GetComponent<SteamVR_TrackedController>().triggerFullPress)
                {
                    bodyRotation.transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
                }
            }
            else
            {
                DebugLogger.Log("[CoupledMovement.cs] :: [2.3] :: Triggers are NOT being used to turn\r\n");


                // the analog nature of the treadmill's rotation ring makes this a somewhat unreliable way 
                // of polling the user's rotation, motion capture would be preferable.  
                DebugLogger.Log("[CoupledMovement.cs] :: [2.4] :: Polling Raw Device Orientation...\r\n");
                float rotation_raw = m_Device.GetOrientationRaw();
                DebugLogger.Log("       [2.4.1] :: Retrieved [" + rotation_raw.ToString() + "]\r\n");


                DebugLogger.Log("[CoupledMovement.cs] :: [2.5] :: Smoothing raw rotation...\r\n");
                // Smooth the analog data using standard signal smoothing math
                float rotation_smoothed = Filter(rotation_raw);
                DebugLogger.Log("       [2.5.1] :: Result [" + rotation_smoothed.ToString() +"]\r\n");


                // Get degree representation of rotation value
                float rotation_deg = rotation_smoothed * 360;

                //print("Rotation (deg): " + rotation_deg);

                // Create a new rotation from the smoothed data
                Quaternion ring_rot = Quaternion.Euler(0, rotation_deg, 0);


                /* TO DO:
                 * The folowing is not super stable, occasionally the orientation will reset or fail to set correctly
                 * this is needs to be fixed by some more rigourous testing of the angle changing process
                 */


                // Get the angle between the ring rotation and the previously saved rotation
                var curr_angle = Quaternion.Angle(ring_rot, m_OldRotation);


                // If the angle between the current rotation and the old angle is greater than some threshold value then change orientation of user
                if(Mathf.Abs(curr_angle - m_OldAngle) > m_RotationThreshold)
                {
                    bodyRotation.transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
                    m_OldRotation = bodyRotation.transform.rotation;
                    m_OldAngle = curr_angle;
                }

            }
            
        }

    }

    // Movement is handled in the fixed update
    private void FixedUpdate()
    {

        // Get the Virtualizer device, open it, and begin reading data from it

        if (m_Device != null)
        {
            if (!m_Device.IsOpen())
            {
                print("[Opening Virtualizer...]");
                m_Device.Open();
            }


            m_MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
            m_SensorThreshold = PlayerPrefs.GetFloat("MoveSens");

            int direction = (int)m_Device.GetDirectionRaw();
            float speed = m_Device.GetMovementSpeed();


            GroundCheck();
            if ((direction == 0 || direction == 1) && speed > m_SensorThreshold)
            {
                m_RigidBody.Sleep();
                Vector3 forwardMove = bodyRotation.transform.forward;
                forwardMove = Vector3.ProjectOnPlane(forwardMove, m_GroundContactNormal).normalized;

                forwardMove.x = forwardMove.x * m_MoveSpeed;
                forwardMove.z = forwardMove.z * m_MoveSpeed;
                forwardMove.y = forwardMove.y * m_MoveSpeed;

                if (m_RigidBody.velocity.sqrMagnitude < m_MoveSpeed)
                {
                    Debug.LogWarning("ADDING FORCE!");
                    m_RigidBody.AddForce(forwardMove, ForceMode.Impulse);
                }
                
            }
            else
            {
                m_RigidBody.Sleep();
            }


        }
    }

    ///<summary> 
    /// Sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    /// <para>With current setup, tends to return FALSE most of the time and so isn't super useful but it is used in the Standard assets FPController</para>
    /// </summary>
    /// <return>void</return>
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }

    }

    ///<summary>
    /// <para>Smooth the Virtualizers Rotation Ring analog data to get more reliable values.</para>
    /// Uses Brown's Simple Exponential Smoothing, an algorithm used in signal processing to achieve this.
    /// </summary>
    /// <return>A float value ranging from 0 to 0.99</return>
    private float Filter(float raw_data)
    {
        float smoothed_value = (smoothingConstant * raw_data) + ((1 - smoothingConstant) * m_PrevOutput);
        m_PrevOutput = smoothed_value;
        return smoothed_value;
    }


    ///<summary>
    /// Handle quitting of game, critical errors (Unity crashing) will be thrown if this is not kept/defined
    /// </summary> 
    /// <return>void</return>
    public void OnDestroy()
    {
        if (m_DeviceController != null)
        {
            CVirtDevice device = m_DeviceController.GetDevice();
            if (device != null)
            {
                if (device.IsOpen() == true)
                {
                    device.Close();
                }
            }
        }

    }




}



    



































































