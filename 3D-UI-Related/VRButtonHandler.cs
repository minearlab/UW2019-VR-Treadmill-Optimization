using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 7/10/2018
//
// This script handles a VR style button press and writes out the current calibration data
// The button is considered "pressed" when it has been filled all the way with the chosen color
// (i.e. when the m_Timer hits the fillDuration value)

public class VRButtonHandler : MonoBehaviour {

    public float fillDuration = 5f;
    public Color fillColor = new Color(178f / 255f, 34f / 255f, 34f / 255f, 1);
    public GameObject calibrationMenu;
    public Button button;

    private Image m_ButtonImage;
    private ShowMenu m_MenuData;
    private SteamVR_TrackedController m_Controller;
    

    private float m_Timer;        // controls length of fill effect
    private bool m_Wrote = false; // did we write out the player data yet?
    private string PATH = @"Assets/SavedCalibrations/";


	// Use this for initialization
	void Start () {

        
        m_Controller = GetComponent<SteamVR_TrackedController>();
        m_Controller.TriggerHeld += OnTriggerHeld;
        m_Controller.TriggerClicked += OnTriggerHeld;
        m_Controller.TriggerUnclicked += OnTriggerRelease;

        m_ButtonImage = button.GetComponent<Image>();
        m_ButtonImage.fillMethod = Image.FillMethod.Horizontal;
        m_ButtonImage.fillAmount = 0f;
        m_Timer = 0f;
        m_MenuData = calibrationMenu.GetComponent<ShowMenu>();

        if(!Directory.Exists(PATH))
        {
            DebugLogger.Log("[VRButtonHandler] :: Requested directory does not exist; Creating it\r\n");
            Directory.CreateDirectory(PATH);
        }

        DebugLogger.Log("[VRButtonHandler] :: All components succesfully initialized\r\n");
	}

    void OnTriggerHeld(object sender, ClickedEventArgs e)
    {
        if(calibrationMenu.activeSelf && m_MenuData.m_MenuItems.IsActive(PageID.Record) && !m_Wrote)
        {
            if (m_Timer < fillDuration)
            {
                // Fill the button
                m_ButtonImage.color = fillColor;
                m_ButtonImage.fillAmount = m_Timer / fillDuration;
                m_Timer += Time.deltaTime;
            }
            else
            {
                DebugLogger.Log("[VRButtonHandler] :: Begin write operation...\r\n");
                // Button is fully filled, write data out
                var filepath = PATH;
                if(!Directory.Exists(PATH))
                {
                    DebugLogger.Log("[VRButtonHandler] :: Requested directory does not exist; Creating it\r\n");
                    Directory.CreateDirectory(PATH);
                }
                filepath += "CalibrationData.txt";
                DebugLogger.Log("[VRButtonHandler] :: File Path [" + filepath + "]\r\n");

                if (!File.Exists(filepath))
                {
                    File.WriteAllText(filepath, "Participant ID,Movement Speed,Sensor Sensitivity,Rotation Threshold\r\n");
                }
                else
                {
                    DebugLogger.Log("[VRButtonHandler] :: File already exists, will NOT write header\r\n");
                }


                var datastring = "";
                if (PlayerPrefs.GetString("Participant") == "")
                {
                    datastring = "None," + PlayerPrefs.GetFloat("SpeedSliderValue").ToString("0.000") + "," +
                                    PlayerPrefs.GetFloat("MoveSens").ToString("0.000") +
                                    "," + PlayerPrefs.GetFloat("RotSens").ToString("0.000") + "\r\n";
                }
                else
                {
                    var p = PlayerPrefs.GetString("Participant").ToLower();
                    datastring = p + "," + PlayerPrefs.GetFloat("SpeedSliderValue").ToString("0.000") + "," +
                                    PlayerPrefs.GetFloat("MoveSens").ToString("0.000") +
                                    "," + PlayerPrefs.GetFloat("RotSens").ToString("0.000") + "\r\n";
                }


                
                File.AppendAllText(filepath, datastring);
                m_Wrote = true;

                m_ButtonImage.color = Color.green;
                button.GetComponentInChildren<Text>().text = "Recorded!";
                DebugLogger.Log("[VRButtonHandler] :: Write operation completed!\r\n");
            }

        }
    }
        

    void OnTriggerRelease(object sender, ClickedEventArgs e)
    {
        // Reset timer and fill amount/color
        m_Timer = 0f;
        m_ButtonImage.color = new Color(1, 1, 1, 1);
        m_ButtonImage.fillAmount = 1f;
        button.GetComponentInChildren<Text>().text = "Record";
        m_Wrote = false;
    }
}
