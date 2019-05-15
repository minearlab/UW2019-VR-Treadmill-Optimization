using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 6/13/2018
// [Edited: 11/18/2018]
//
// This script handles the displaying of the [Calibration Menu] prefab
// The menu is displayed upon a Button press, in this case, the HTC Vive controller Gripp press is the trigger
// Menu position is dictated by the [m_TargetPosition] game object
//
// [NOTE]: Player will NOT be able to move while Menu is active

public enum PageID { Rotation, Sensitivity, Movement, Record };

public class ShowMenu : MonoBehaviour {
 
    [Serializable]
    public class MenuItems
    { 
        private static GameObject moveSpeedMenu = null;
        private static GameObject moveSensitivityMenu = null;
        private static GameObject rotationSensitivityMenu = null;
        private static GameObject endCalibrationMenu = null;

        private Dictionary<PageID, Delegate> m_ToggleDict = new Dictionary<PageID, Delegate>()
        {

            {PageID.Movement, new Func<PageID,bool>(ToggleSpeed) },
            {PageID.Sensitivity, new Func<PageID,bool>(ToggleSensitivity) },
            {PageID.Rotation, new Func<PageID,bool> (ToggleRotation)},
            {PageID.Record, new Func<PageID,bool>(ToggleRecord) }
        };
        private static Dictionary<PageID, bool> currentlyActive = new Dictionary<PageID, bool>()
        {
            {PageID.Movement, true },
            {PageID.Sensitivity, false },
            {PageID.Rotation, false },
            {PageID.Record, false }
        };


        public void Init()
        {
            moveSpeedMenu = GameObject.FindGameObjectWithTag("MovementSettings");
            moveSensitivityMenu = GameObject.FindGameObjectWithTag("SensitivitySettings");
            moveSensitivityMenu.SetActive(false);
            rotationSensitivityMenu = GameObject.FindGameObjectWithTag("RotationSettings");
            rotationSensitivityMenu.SetActive(false);
            endCalibrationMenu = GameObject.FindGameObjectWithTag("RecordSettings");
            endCalibrationMenu.SetActive(false);
        }
        private static bool ToggleSpeed(PageID page)
        {
            DebugLogger.Log("[ShowMenu] :: Toggling Speed\r\n");
            if (!moveSpeedMenu.activeSelf)
            {
                moveSpeedMenu.SetActive(true);
                currentlyActive[PageID.Movement] = true;
            }
            else
            {

                moveSpeedMenu.SetActive(false);
                currentlyActive[PageID.Movement] = false;
            }
            return moveSpeedMenu.activeSelf;
        }
        private static bool ToggleSensitivity(PageID page)
        {
            DebugLogger.Log("[ShowMenu] :: Toggling Sensitivity\r\n");
            if (!moveSensitivityMenu.activeSelf)
            {
                moveSensitivityMenu.SetActive(true);
                currentlyActive[PageID.Sensitivity] = true;
            }
            else
            {
                moveSensitivityMenu.SetActive(false);
                currentlyActive[PageID.Sensitivity] = false;
            }
            return moveSensitivityMenu.activeSelf;
        }
        private static bool ToggleRotation(PageID page)
        {
            DebugLogger.Log("[ShowMenu] :: Toggling Rotation\r\n");
            if (!rotationSensitivityMenu.activeSelf)
            {
                rotationSensitivityMenu.SetActive(true);
                currentlyActive[PageID.Rotation] = true;
            }
            else
            {
                rotationSensitivityMenu.SetActive(false);
                currentlyActive[PageID.Rotation] = false;
            }
            return rotationSensitivityMenu.activeSelf;
        }
        private static bool ToggleRecord(PageID page)
        {
            DebugLogger.Log("[ShowMenu] :: Toggling Record\r\n");
            if (!endCalibrationMenu.activeSelf)
            {
                endCalibrationMenu.SetActive(true);
                currentlyActive[PageID.Record] = true;
            }
            else
            {
                endCalibrationMenu.SetActive(false);
                currentlyActive[PageID.Record] = false;
            }
            return endCalibrationMenu.activeSelf;
        }
        public bool IsActive(PageID page)
        {
            return currentlyActive[page];
        }
        public string GetActive()
        {
            foreach(KeyValuePair<PageID, bool> entry in currentlyActive)
            {
                if (currentlyActive[entry.Key])
                {
                    if (IsActive(PageID.Movement)) { return "[ActivePage]::[Movement]"; }
                    else if(IsActive(PageID.Sensitivity)) { return "[ActivePage]::[Sensitivity]"; }
                    else if (IsActive(PageID.Rotation)) { return "[ActivePage]::[Rotation]"; }
                    else if (IsActive (PageID.Record)){ return "[ActivePage] :: [Record]"; }
                }
            }
            return "[ActivePage]::[None]";
        }
        public void Toggle(PageID page)
        {
            m_ToggleDict[page].DynamicInvoke(page);
        }
        public void Cycle(PageID from, PageID to)
        {
            m_ToggleDict[from].DynamicInvoke(from);
            m_ToggleDict[to].DynamicInvoke(to);
        }
        public void DisableAll()
        {
            var buffer = new List<PageID>(currentlyActive.Keys);
            foreach(PageID key in buffer)
            {
                if(currentlyActive[key])
                {
                    currentlyActive[key] = false;
                    Toggle(key);
                }
            }
        }
       
    }

    [Serializable]
    public class ExperimenterButtons
    {
        public GameObject defaults;
        public GameObject freeCalibration;
        public GameObject lock_RS;
        public GameObject lock_MS;
        public GameObject lock_MR;
    }


    #region /* Member Variables */
    public bool isCalibrationScene = true;
  
    [SerializeField] private ExperimenterButtons m_Buttons;
    [SerializeField] private GameObject m_TargetPosition; // where menu will spawn
    [SerializeField] private GameObject m_LeftController;
    [SerializeField] private GameObject m_RightController;

    [ReadOnly] [SerializeField] private bool m_LockSpeed = false;
    [ReadOnly] [SerializeField] private bool m_LockSensors = false;
    [ReadOnly] [SerializeField] private bool m_LockRotation = false;
    [SerializeField] public MenuItems m_MenuItems = new MenuItems();

    private SteamVR_TrackedController m_DeviceRight = null;
    private SteamVR_TrackedController m_DeviceLeft = null;

    private Vector3 m_PreviousPos; // previous controller position
    private Vector3 m_OldMenuPos;

    private float m_Dead = 0.3f; // controller motion sensitivity threshold
    private float m_OldX = 0f;
    private float m_SavedSpeed = 0; // Saved player movement speed

    #endregion


    void Awake ()
    {

        #region /* Debug Print */ 
        DebugLogger.Log("[ShowMenu.cs] :: [0] :: Debugging Enabled\r\n");
        DebugLogger.Log("[ShowMenu.cs] :: [1] :: Beginning Initializations...\r\n");
        DebugLogger.Log("[ShowMenu.cs] :: [1.1] :: Looking For Controllers...\r\n");
        #endregion

        if (m_LeftController.activeSelf && !m_RightController.activeSelf)
        {
            m_DeviceLeft = m_LeftController.GetComponent<SteamVR_TrackedController>();

            #region /* Debug Print */ 
             DebugLogger.Log("[ShowMenu.cs] :: [1.2] :: Left Controller Found\r\n\r\n");
            #endregion
        }
        else if (m_RightController.activeSelf && !m_LeftController.activeSelf)
        {
            m_DeviceRight = m_RightController.GetComponent<SteamVR_TrackedController>();

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [1.2] :: Right Controller Found\r\n\r\n");
            #endregion
        }
        else
        {
            m_DeviceLeft = m_LeftController.GetComponent<SteamVR_TrackedController>();
            m_DeviceRight = m_RightController.GetComponent<SteamVR_TrackedController>();

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [1.2] :: Left & Right Controllers Found\r\n\r\n");
            #endregion
        }



        #region /* Debug Print */ 
        DebugLogger.Log("[ShowMenu.cs] :: [1.3] :: Setting-Up Controller Triggered Events...\r\n");
        #endregion
        // Make sure to handle, all, or one or the other cases

        if (m_DeviceRight != null && m_DeviceLeft == null)
        {
            m_DeviceRight.Gripped += OnGripPress;

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [1.4] :: Right Controller Triggers Set\r\n\r\n");
            #endregion
        }
        else if (m_DeviceLeft != null && m_DeviceRight == null)
        {
            m_DeviceLeft.Gripped += OnGripPress;

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [1.4] :: Left Controller Triggers Set\r\n\r\n");
            #endregion
        }
        else
        {
            m_DeviceLeft.Gripped += OnGripPress;
            m_DeviceRight.Gripped += OnGripPress;

            m_DeviceLeft.PadClicked += OnPadClick;
            m_DeviceRight.PadClicked += OnPadClick;

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [1.4] :: Left & Right Triggers Set\r\n\r\n");
            #endregion

        }

        m_MenuItems.Init();
        // start with the menu inactive
        PlayerPrefs.SetString("Type", "Free Calibration");

        #region /* Debug Print */ 
        DebugLogger.Log("[ShowMenu.cs] :: [1.5] :: Initializations Completed\r\n\r\n");
        #endregion
        
        gameObject.SetActive(false);
        
        

    }
    private void Update()
    {

        m_SavedSpeed = PlayerPrefs.GetFloat("SpeedSliderValue");
        if (m_Buttons.lock_RS.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [4] :: [Just Movement] Button Clicked\r\n");
            #endregion
            m_LockSensors = true;
            m_LockRotation = true;
            m_LockSpeed = false;
            PlayerPrefs.SetString("Type", "Speed Only");
        }
        else if(m_Buttons.lock_MS.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [4] :: [Just Rotation] Button Clicked\r\n");
            #endregion
            m_LockSpeed = true;
            m_LockSensors = true;
            m_LockRotation = false;
            PlayerPrefs.SetString("Type", "Rotation Only");
        }
        else if(m_Buttons.lock_MR.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [4] :: [Just Sensitivity] Button Clicked\r\n");
            #endregion
            m_LockSpeed = true;
            m_LockRotation = true;
            m_LockSensors = false;
            PlayerPrefs.SetString("Type", "Sensitivity Only");
        }
        else if(m_Buttons.defaults.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [4] :: [Revert To Defaults] Button Clicked\r\n");
            #endregion
            m_LockRotation = true;
            m_LockSensors = true;
            m_LockSpeed = true;
            PlayerPrefs.SetString("Type", "Defaults");
        }
        else if(m_Buttons.freeCalibration.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [4] :: [Free Calibration] Button Clicked\r\n");
            #endregion
            m_LockRotation = false;
            m_LockSensors = false;
            m_LockSpeed = false;
            PlayerPrefs.SetString("Type", "Free Calibration");
        }

    }
    private void OnPadClick(object sender, ClickedEventArgs e)
    {
        #region /* Debug Print */ 
        DebugLogger.Log("[ShowMenu.cs] :: [2] :: Circle Pad Clicked...\r\n");
        DebugLogger.Log("[ShowMenu.cs] :: [2] :: " + m_MenuItems.GetActive() + "\r\n");
        if (m_LockSpeed) DebugLogger.Log("[ShowMenu.cs] :: [2.1] :: Movement Speed Locked\r\n\r\n");
        if (m_LockSensors) DebugLogger.Log("[ShowMenu.cs] :: [2.1] :: Treadmill Sensors Locked\r\n\r\n");
        if (m_LockRotation) DebugLogger.Log("[ShowMenu.cs] :: [2.1] :: Rotation Sensor Locked\r\n\r\n");
        if (!m_LockRotation && !m_LockSensors && !m_LockSpeed) DebugLogger.Log("[ShowMenu.cs] :: [2.1] :: All Settings Unlocked\r\n\r\n");
        #endregion

        // cycle menu pages
        if (gameObject.activeSelf)
        {

            if (m_LockRotation && m_LockSensors)
            {
                if (m_MenuItems.IsActive(PageID.Movement))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Movement] To [Record]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Movement, PageID.Record);
                }
                else if (m_MenuItems.IsActive(PageID.Record))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Record] To [Movement]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Record, PageID.Movement);
                }
            }
            else if (m_LockSpeed && m_LockSensors)
            {
                if (m_MenuItems.IsActive(PageID.Rotation))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Rotation] To [Record]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Rotation, PageID.Record);
                }
                else if (m_MenuItems.IsActive(PageID.Record))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Record] To [Rotation]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Record, PageID.Rotation);
                }
            }
            else if (m_LockSpeed && m_LockRotation)
            {
                if (m_MenuItems.IsActive(PageID.Sensitivity))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Sensitivity] To [Record]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Sensitivity, PageID.Record);
                }
                else if (m_MenuItems.IsActive(PageID.Record))
                {
                    #region /* Debug Print */ 
                    DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Record] To [Sensitivity]\r\n");
                    #endregion
                    m_MenuItems.Cycle(PageID.Record, PageID.Sensitivity);
                }
            }
            else if (!m_LockSpeed && !m_LockSensors && !m_LockRotation)
            {
                if (e.padX > 0)
                {
                    if (m_MenuItems.IsActive(PageID.Movement))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Movement] To [Sensitivity]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Movement, PageID.Sensitivity);
                    }
                    else if (m_MenuItems.IsActive(PageID.Sensitivity))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Sensitivity] To [Rotation]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Sensitivity, PageID.Rotation);
                    }
                    else if (m_MenuItems.IsActive(PageID.Rotation))
                    {
                        if (!isCalibrationScene)
                        {
                            #region /* Debug Print */ 
                            DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Rotation] To [Movement]\r\n");
                            #endregion
                            m_MenuItems.Cycle(PageID.Rotation, PageID.Movement);
                        }
                        else
                        {
                            #region /* Debug Print */ 
                            DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Rotation] To [Record]\r\n");
                            #endregion
                            m_MenuItems.Cycle(PageID.Rotation, PageID.Record);
                        }
                    }
                    else if (m_MenuItems.IsActive(PageID.Record))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Record] To [Movement]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Record, PageID.Movement);
                    }
                    else
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: No Active Pages!\r\n");
                        #endregion
                        Debug.LogError("No Active Pages!");
                    }
                }
                else if(e.padX < 0)
                {
                    if (m_MenuItems.IsActive(PageID.Movement))
                    {
                        if (isCalibrationScene)
                        {
                            #region /* Debug Print */ 
                            DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Movement] To [Record]\r\n");
                            #endregion
                            m_MenuItems.Cycle(PageID.Movement, PageID.Record);
                        }
                        else
                        {
                            #region /* Debug Print */ 
                            DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Movement] To [Rotation]\r\n");
                            #endregion
                            m_MenuItems.Cycle(PageID.Movement, PageID.Rotation);
                        }
                    }
                    else if (m_MenuItems.IsActive(PageID.Sensitivity))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Sensitivity] To [Movement]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Sensitivity, PageID.Movement);
                    }
                    else if (m_MenuItems.IsActive(PageID.Rotation))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Rotation] To [Sensitivity]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Rotation, PageID.Sensitivity);
                    }
                    else if (m_MenuItems.IsActive(PageID.Record))
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: Cycling From [Record] To [Rotation]\r\n");
                        #endregion
                        m_MenuItems.Cycle(PageID.Record, PageID.Rotation);
                    }
                    else
                    {
                        #region /* Debug Print */ 
                        DebugLogger.Log("[ShowMenu.cs] :: [2.2] :: No Active Pages!\r\n");
                        #endregion
                        Debug.LogError("No Active Pages!");
                    }
                }
            }
            
        }

        #region /* Debug Print */
        DebugLogger.Log("[ShowMenu.cs] :: [2] :: Circle Released\r\n\r\n\r\n");
        #endregion
    }
    private void OnGripPress(object sender, ClickedEventArgs e)
    {
        // Reveal/Conceal menu on grip press
        if (!m_Buttons.defaults.GetComponent<ButtonStyle>().m_Clicked)
        {
            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [3] :: Grip Pressed\r\n");
            #endregion

            if (gameObject.activeSelf)
            {
                PlayerPrefs.SetFloat("MoveSpeed", m_SavedSpeed);
                gameObject.SetActive(false);

                #region /* Debug Print */ 
                DebugLogger.Log("    [3.1] :: Deactivating Menu...\r\n");
                DebugLogger.Log("        [3.1.a] :: Setting Movement Speed...\r\n");
                DebugLogger.Log("            [3.1.a.1] :: Speed Set To [" + m_SavedSpeed.ToString() + "]\r\n");
                #endregion
            }
            else
            {
                transform.position = m_TargetPosition.transform.position;
                transform.rotation = new Quaternion(0, m_TargetPosition.transform.rotation.y, 0, m_TargetPosition.transform.rotation.w);
                gameObject.SetActive(true);
                PlayerPrefs.SetFloat("MoveSpeed", 0);

                #region /* Debug Print */ 
                DebugLogger.Log("    [3.2] :: Activating Menu...\r\n");
                DebugLogger.Log("        [3.2.a] :: Setting Movement Speed...\r\n");
                DebugLogger.Log("            [3.2.a.1] :: Speed Set To [" + m_SavedSpeed.ToString() + "]\r\n");
                #endregion
            }

            #region /* Debug Print */ 
            DebugLogger.Log("[ShowMenu.cs] :: [3] :: Grip Released\r\n\r\n\r\n");
            #endregion
        }

    }
    public bool IsMovementOnly() { return (m_LockSensors && m_LockRotation); }
    public bool IsRotationOnly() { return (m_LockSpeed && m_LockSensors); }
    public bool IsSensorsOnly() { return (m_LockSpeed && m_LockRotation); }
    public bool IsDefaults() { return (m_LockRotation && m_LockSensors && m_LockSpeed); }
}


