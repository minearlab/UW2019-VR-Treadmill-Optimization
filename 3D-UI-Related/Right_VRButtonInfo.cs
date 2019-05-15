using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Highlight respective callout on button press
// Author: Adrian Barberis

public class Right_VRButtonInfo : MonoBehaviour {

    // there are better ways to do this but I'm feeling lazy and this is straight forward
    private Color highlightColor = Color.green;
    public Color m_BaseColor;
    public GameObject rightPointerBar_Upper;
    public GameObject rightPointerBar_Lower;
    public GameObject rightPointerBar_Straight;
    public GameObject calibrationMenu;

    public Color[] highlightColors = new Color[]
    {
        new Color(),
        new Color(),
        new Color(),
        Color.green
    };

    [SerializeField] private Material m_RBarUpperMat;
    [SerializeField] private Material m_RBarLowerMat;
    [SerializeField] private Material m_RBarStraightMat;

    [SerializeField] private GameObject m_RBarUpperMesh;
    [SerializeField] private GameObject m_RBarLowerMesh;
    [SerializeField] private GameObject m_RBarStraightMesh;


    private GameObject m_Slider = null;
    private SteamVR_TrackedController m_DeviceRight = null;
    private ShowMenu m_MenuData;


    private void Start()
    {
        
        m_DeviceRight = GetComponent<SteamVR_TrackedController>();
        m_MenuData = calibrationMenu.GetComponent<ShowMenu>();
        if (m_MenuData == null)
        {
            m_MenuData = GameObject.FindGameObjectWithTag("SettingsMenu").GetComponent<ShowMenu>();
        }

        if (m_DeviceRight != null)
        {
            m_DeviceRight.Gripped += OnGripPress;
            m_DeviceRight.Ungripped += OnGripUnPress;
            m_DeviceRight.PadTouched += OnPadTouched;
            m_DeviceRight.PadUntouched += OnPadUnTouch;
            m_DeviceRight.TriggerHeld += OnTriggerPressed;
            m_DeviceRight.TriggerUnclicked += OnTriggerUnPress;
        }

    }

    private void Update()
    {
        m_Slider = GameObject.FindWithTag("activeSlider");

        if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
        {
            highlightColor = highlightColors[0];
        }
        else if(m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
        {
            highlightColor = highlightColors[1];

        }
        else if(m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
        {
            highlightColor = highlightColors[2];
        }
        else
        {
            highlightColor = highlightColors[3];
        }
    }



    private void OnPadTouched(object sender, ClickedEventArgs e)
    {
        m_RBarUpperMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_RBarUpperMat.color = highlightColor;
        DebugLogger.Log("[Right_VRButtonInfo] :: Circle Pad Pressed\r\n");
    }

    private void OnPadUnTouch(object sender, ClickedEventArgs e)
    {
        m_RBarUpperMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_RBarUpperMat.color = m_BaseColor;
        DebugLogger.Log("[Right_VRButtonInfo] :: Circle Pad Released\r\n");
    }

    private void OnGripPress(object sender, ClickedEventArgs e)
    {
        m_RBarStraightMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_RBarStraightMat.color = highlightColor;
        DebugLogger.Log("[Right_VRButtonInfo] :: Grip Pressed\r\n");
    }

    private void OnGripUnPress(object sender, ClickedEventArgs e)
    {
        m_RBarStraightMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_RBarStraightMat.color = m_BaseColor;
        DebugLogger.Log("[Right_VRButtonInfo] :: Grip Released\r\n");
    }

    private void OnTriggerPressed(object sender, ClickedEventArgs e)
    {
        m_RBarLowerMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_RBarLowerMat.color = highlightColor;
        if (m_Slider != null) m_Slider.GetComponent<SliderSettings>().knobColor = highlightColor;
        DebugLogger.Log("[Right_VRButtonInfo] :: Trigger Pressed\r\n");
    }

    private void OnTriggerUnPress(object sender, ClickedEventArgs e)
    {
        m_RBarLowerMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_RBarLowerMat.color = m_BaseColor;
        if (m_Slider != null) m_Slider.GetComponent<SliderSettings>().knobColor = new Color(1, 1, 1, 1);
        DebugLogger.Log("[Right_VRButtonInfo] :: Trigger Released\r\n");
    }



}
