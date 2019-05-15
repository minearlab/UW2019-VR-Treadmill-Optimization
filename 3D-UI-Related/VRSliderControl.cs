using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 6/13/2018
//
// This script works with others to allow a player to change their movement speed in game by adjusting a m_Slider
// The m_Slider adjustments are handled in this script
//
// !!IMPORTANT!!
// This script assumes that the [SteamVR_TrackedController.cs] script has been edited to include a triggered event for the Holding down the Vive conroller's trigger button

[RequireComponent(typeof(SteamVR_TrackedController))]
public class VRSliderControl : MonoBehaviour {

    enum Preset { SpeedLow = 1,
                  SpeedMid,
                  SpeedHigh,
                  SensLow,
                  SensMid,
                  SensHigh,
                  RotLow,
                  RotMid,
                  RotHigh,
                  Init
                };

    // Value within the advanced settings should be changed internally
    [Serializable] public class AdvancedSettings
    {
        
        // Changing this value will change by how much the m_Slider value is incremented
        [SerializeField] private float m_IncrementBy = 0.1f;
        public float IncrementBy
        {
            get { return m_IncrementBy; }
            set { m_IncrementBy = value; }
        }

        // Changing this value will cause the m_Slider m_Knob to "move faster"
        [SerializeField] private float m_PosIncrement = 0.01f; 
        public float PosIncrement
        {
            get { return m_PosIncrement; }
            set { m_PosIncrement = value; }
        }
    }
 
    public GameObject menu;  // A [Calibration Menu] prefab (not Unity UI standard)
    public float dead = 3f; // Changing this value will increase controller motion sensitivity

    [SerializeField] [ReadOnly] private AdvancedSettings m_AdvSettings;

    private Slider m_Slider;  // a standard Unity UI m_Slider
    private GameObject m_Label; // a m_Label prefab (not Unity UI standard)
    private Image m_Knob; // the m_Knob of the UI m_Slider
    private ShowMenu m_MenuData;

    private float m_OldX = 0f;
    private SteamVR_TrackedController m_Device; // controller
    private Vector3 m_PrevPos;
    private Dictionary<Preset, float[]> m_Presets = new Dictionary<Preset, float[]>()
    {
        {Preset.SpeedLow, new[] {1.0f, 0.1f} },
        {Preset.SpeedMid, new[] {2.5f, 1.5f} },
        {Preset.SpeedHigh, new[] {4.2f, 2f} },
        {Preset.SensHigh, new[] {0.2f, 0.1f} },
        {Preset.SensMid, new[] {0.5f, 1.5f} },
        {Preset.SensLow, new[] {0.8f, 2f} },
        {Preset.RotLow, new[] {10f, 0.1f} },
        {Preset.RotMid, new[] {25f, 1.5f} },
        {Preset.RotHigh, new[] {40f, 2f} },
        {Preset.Init, new[] {0f, 0f}}

    };
    private float m_MoveSpeed = 0f;
    private Preset m_CurMovePreset = Preset.Init;
    private Preset m_CurSensPreset = Preset.Init;
    private Preset m_CurRotPreset = Preset.Init;


    // Use this for initialization
    void Start ()
    {

        PlayerPrefs.SetFloat("MoveSpeed", 0f);
        PlayerPrefs.SetFloat("MoveSens", 0f);
        PlayerPrefs.SetFloat("RotSens", 0f);

        m_MenuData = menu.GetComponent<ShowMenu>();

        // Capture Trigger Held Event
        m_Device = GetComponent<SteamVR_TrackedController>();
        m_Device.TriggerHeld += OnTriggerHeld;
        m_Device.TriggerClicked += OnTriggerClick;


        m_AdvSettings = new AdvancedSettings();
        
    }

    
    void OnTriggerHeld(object sender, ClickedEventArgs e)
    {
        // set-up advanced settings based on which page is being viewed
        if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
        {
            m_AdvSettings.IncrementBy = 0.1f;
            m_AdvSettings.PosIncrement = 0.01f;
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
        {
            m_AdvSettings.IncrementBy = 0.01f;
            m_AdvSettings.PosIncrement = 0.001f;
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
        {
            m_AdvSettings.IncrementBy = 1f;
            m_AdvSettings.PosIncrement = 0.1f;
        }
        

 
        if (menu.activeSelf && !m_MenuData.m_MenuItems.IsActive(PageID.Record))
        {

            // The proper components should be tagged accordingly before hand within the Editor
            // Use the [SetTag.cs] script to make it so the correct tag is always set upon object enabling
            m_Slider = GameObject.FindWithTag("activeSlider").GetComponent<Slider>();
            m_Label = GameObject.FindWithTag("activeLabel");
            m_Knob = GameObject.FindWithTag("activeKnob").GetComponent<Image>();



            // Get position relative to screen (hand calculate controller velocity)
            // this allows the motion controlls to behave the same irrespective of which direction the player is facing
            var projected_vec = (transform.InverseTransformDirection((transform.position - m_PrevPos) / Time.deltaTime));

            // Get the projected x coordinate of the controller 
            var newX = projected_vec.x;
            var distance = Mathf.Abs(newX - m_OldX);


            // Make sure distance that controller traveled is greater than dead-threshold
            if(Mathf.Abs(distance) < dead) return;

            // Update previous position
            m_PrevPos = transform.position;


            // Increase m_Slider values if the controller is moved to the right
            if (newX > 0)
            {
                // time.deltatime here smooths the m_Slider handle movement
                var pos = m_Knob.rectTransform.anchoredPosition;
                pos.x += (m_AdvSettings.PosIncrement * Time.deltaTime);
                m_Knob.rectTransform.anchoredPosition = pos;

                m_OldX = newX;

                m_Slider.value += m_AdvSettings.IncrementBy;
                if (m_Slider.value < m_Slider.maxValue)
                {
                    SteamVR_Controller.Input((int)m_Device.controllerIndex).TriggerHapticPulse(1000);
                }
            }
            else if (newX < 0) // Decrease m_Slider values if controller is moved to the left
            {
                var pos = m_Knob.rectTransform.anchoredPosition;
                pos.x -= (m_AdvSettings.PosIncrement * Time.deltaTime);
                m_Knob.rectTransform.anchoredPosition = pos;

                m_OldX = newX;

                m_Slider.value -= m_AdvSettings.IncrementBy;  
                if (m_Slider.value > 0)
                {
                    SteamVR_Controller.Input((int)m_Device.controllerIndex).TriggerHapticPulse(1000);
                }
            }
           
            // Update the m_Slider's m_Label so that the user knows what the m_Slider value is
            if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
            {
                if (m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F1"));

                // Record value globally for use in other scripts
                PlayerPrefs.SetFloat("SpeedSliderValue", m_Slider.value);
            }
            else if(m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
            {
                if (m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F2"));
                PlayerPrefs.SetFloat("MoveSens", (1 - m_Slider.value));
            }
            else if(m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
            {
                if (m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F1"));
                PlayerPrefs.SetFloat("RotSens", m_Slider.value);
            }
            
        }

    }


    void OnTriggerClick(object sender, ClickedEventArgs e)
    {
        SteamVR_Controller.Input((int)m_Device.controllerIndex).TriggerHapticPulse(6000);
        print(m_MenuData.m_MenuItems.GetActive());

        if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
        {
            CyclePresets();
            SnapKnob(m_CurMovePreset);
            m_Slider.value = m_Presets[m_CurMovePreset][0];
            PlayerPrefs.SetFloat("SpeedSliderValue", m_Slider.value);
            if(m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F1"));
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
        {
            CyclePresets();
            SnapKnob(m_CurSensPreset);
            m_Slider.value = m_Presets[m_CurSensPreset][0];
            PlayerPrefs.SetFloat("MoveSens", m_Slider.value);
            if (m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F2"));
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
        {
            CyclePresets();
            SnapKnob(m_CurRotPreset);
            m_Slider.value = m_Presets[m_CurRotPreset][0];
            PlayerPrefs.SetFloat("RotSens", m_Slider.value);
            if (m_Label.activeSelf) m_Label.GetComponent<LabelSettings>().SetLabelText(m_Slider.value.ToString("F1"));
        }
    }

    void SnapKnob(Preset value)
    {
        // TO DO: Set dead threshold for trigger hold
        var pos = m_Knob.rectTransform.anchoredPosition;
        pos.x = m_Presets[value][1];
        m_Knob.rectTransform.anchoredPosition = pos;
    }
    void CyclePresets()
    {
        if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
        {
            if(m_CurMovePreset == Preset.SpeedLow) { m_CurMovePreset = Preset.SpeedMid; }
            else if(m_CurMovePreset == Preset.SpeedMid) { m_CurMovePreset = Preset.SpeedHigh; }
            else if(m_CurMovePreset == Preset.SpeedHigh) { m_CurMovePreset = Preset.Init; }
            else if(m_CurMovePreset == Preset.Init) { m_CurMovePreset = Preset.SpeedLow; }
            else { m_CurMovePreset = Preset.Init; }
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
        {
            if (m_CurSensPreset == Preset.SensLow) { m_CurSensPreset = Preset.SensMid; }
            else if (m_CurSensPreset == Preset.SensMid) { m_CurSensPreset = Preset.SensHigh; }
            else if (m_CurSensPreset == Preset.SensHigh) { m_CurSensPreset = Preset.Init; }
            else if (m_CurSensPreset == Preset.Init) { m_CurSensPreset = Preset.SensLow; }
            else { m_CurSensPreset = Preset.Init; }
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
        {
            if (m_CurRotPreset == Preset.RotLow) { m_CurRotPreset = Preset.RotMid; }
            else if (m_CurRotPreset == Preset.RotMid) { m_CurRotPreset = Preset.RotHigh; }
            else if (m_CurRotPreset == Preset.RotHigh) { m_CurRotPreset = Preset.Init; }
            else if (m_CurRotPreset == Preset.Init) { m_CurRotPreset = Preset.RotLow; }
            else { m_CurRotPreset = Preset.Init; }
        }
    }

   
}
