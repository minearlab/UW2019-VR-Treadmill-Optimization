using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Left_VRButtonInfo : MonoBehaviour {

    // there are better ways to do this but I'm feeling lazy and this is straight forward
    private Color highlightColor = Color.green;
    public Color m_BaseColor;
    public GameObject leftPointerBar_Upper;
    public GameObject leftPointerBar_Lower;
    public GameObject leftPointerBar_Straight;
    public GameObject calibrationMenu;

    public Color[] highlightColors = new Color[]
    {
        new Color(),
        new Color(),
        new Color(),
        Color.green
    };

    [SerializeField] private Material m_LBarUpperMat;
    [SerializeField] private Material m_LBarLowerMat;
    [SerializeField] private Material m_LBarStraightMat;

    [SerializeField] private GameObject m_LBarUpperMesh;
    [SerializeField] private GameObject m_LBarLowerMesh;
    [SerializeField] private GameObject m_LBarStraightMesh;

    private GameObject m_Slider = null;
    private SteamVR_TrackedController m_DeviceLeft = null;
    private ShowMenu m_MenuData;



    private void Start()
    {
        m_DeviceLeft = GetComponent<SteamVR_TrackedController>();
        m_MenuData = calibrationMenu.GetComponent<ShowMenu>();
        if (m_MenuData == null)
        {
            m_MenuData = GameObject.FindGameObjectWithTag("SettingsMenu").GetComponent<ShowMenu>();
        }

        if (m_DeviceLeft != null)
        {
            m_DeviceLeft.Gripped += OnGripPress;
            m_DeviceLeft.Ungripped += OnGripUnPress;
            m_DeviceLeft.PadTouched += OnPadTouched;
            m_DeviceLeft.PadUntouched += OnPadUnTouch;
            m_DeviceLeft.TriggerHeld += OnTriggerPressed;
            m_DeviceLeft.TriggerUnclicked += OnTriggerUnPress;
        }

    }

    private void Update()
    {
        m_Slider = GameObject.FindWithTag("activeSlider");

        if (m_MenuData.m_MenuItems.IsActive(PageID.Movement))
        {
            highlightColor = highlightColors[0];
        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Sensitivity))
        {
            highlightColor = highlightColors[1];

        }
        else if (m_MenuData.m_MenuItems.IsActive(PageID.Rotation))
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
        m_LBarUpperMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_LBarUpperMat.color = highlightColor;
        DebugLogger.Log("[Left_VRButtonInfo] :: Circle Pad Pressed\r\n");
    }

    private void OnPadUnTouch(object sender, ClickedEventArgs e)
    {
        m_LBarUpperMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_LBarUpperMat.color = m_BaseColor;
        DebugLogger.Log("[Left_VRButtonInfo] :: Circle Pad Released\r\n");
    }

    private void OnGripPress(object sender, ClickedEventArgs e)
    {
        m_LBarStraightMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_LBarStraightMat.color = highlightColor;
        DebugLogger.Log("[Left_VRButtonInfo] :: Grip Pressed\r\n");
    }

    private void OnGripUnPress(object sender, ClickedEventArgs e)
    {
        m_LBarStraightMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_LBarStraightMat.color = m_BaseColor;
        DebugLogger.Log("[Left_VRButtonInfo] :: Grip Released\r\n");
    }

    private void OnTriggerPressed(object sender, ClickedEventArgs e)
    {
        m_LBarLowerMesh.GetComponent<TextMeshPro>().color = highlightColor;
        m_LBarLowerMat.color = highlightColor;
        if (m_Slider != null) m_Slider.GetComponent<SliderSettings>().knobColor = highlightColor;
        DebugLogger.Log("[Left_VRButtonInfo] ::Trigger Pressed\r\n");
    }

    private void OnTriggerUnPress(object sender, ClickedEventArgs e)
    {
        m_LBarLowerMesh.GetComponent<TextMeshPro>().color = m_BaseColor;
        m_LBarLowerMat.color = m_BaseColor;
        if (m_Slider != null) m_Slider.GetComponent<SliderSettings>().knobColor = new Color(1, 1, 1, 1);
        DebugLogger.Log("[Left_VRButtonInfo] :: Trigger Released\r\n");
    }


}
