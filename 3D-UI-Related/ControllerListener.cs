using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerListener : MonoBehaviour {

    public GameObject m_Controller;
    private SteamVR_TrackedController m_TrackedC;

    // Use this for initialization
    void Start () {
        m_TrackedC = m_Controller.GetComponent<SteamVR_TrackedController>();
        if(m_TrackedC != null)
        {
            m_TrackedC.TriggerHeld += OnTriggerHold;
            m_TrackedC.TriggerClicked += OnTriggerClick;
            m_TrackedC.TriggerUnclicked += OnTriggerRelease;
        }
    }

    void OnTriggerHold(object sender, ClickedEventArgs e)
    {
        GetComponent<TextMeshPro>().text = "Trigger (Held)";
    }

    void OnTriggerClick(object sender, ClickedEventArgs e)
    {
        GetComponent<TextMeshPro>().text = "Trigger (Clicked)";
    }

    void OnTriggerRelease(object sender, ClickedEventArgs e)
    {
        GetComponent<TextMeshPro>().text = "Trigger";
    }
}
