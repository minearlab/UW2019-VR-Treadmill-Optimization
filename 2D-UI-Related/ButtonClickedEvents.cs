using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickedEvents : MonoBehaviour {

    public GameObject CalibrationMenu;
    private ShowMenu m_Menu;

    private void Start()
    {
        m_Menu = CalibrationMenu.GetComponent<ShowMenu>();
    }

    public void SetDefaults()
    {
        PlayerPrefs.SetFloat("RotSens", 0f);
        PlayerPrefs.SetFloat("MoveSpeed", 1.5f);
        PlayerPrefs.SetFloat("MoveSens", 0f);
        m_Menu.m_MenuItems.DisableAll();
        if (CalibrationMenu.activeSelf) { CalibrationMenu.SetActive(false); }
        DebugLogger.Log("[ButtonClickedEvents] :: Settings reset to defaults!\r\n");
    }

    public void All3()
    {
        PlayerPrefs.SetFloat("RotSens", 0f);
        PlayerPrefs.SetFloat("MoveSpeed", 0f);
        PlayerPrefs.SetFloat("MoveSens", 0f);
        m_Menu.m_MenuItems.DisableAll();
        m_Menu.m_MenuItems.Toggle(PageID.Movement);
    }

    public void JustMovement()
    {
        PlayerPrefs.SetFloat("RotSens", 0f);
        PlayerPrefs.SetFloat("MoveSens", 0f);
        m_Menu.m_MenuItems.DisableAll();
        m_Menu.m_MenuItems.Toggle(PageID.Movement);
    }

    public void JustSensitivity()
    {
        PlayerPrefs.SetFloat("MoveSpeed", 1.5f);
        PlayerPrefs.SetFloat("RotSens", 0f);
        m_Menu.m_MenuItems.DisableAll();
        m_Menu.m_MenuItems.Toggle(PageID.Sensitivity);
    }

    public void JustRotation()
    {
        PlayerPrefs.SetFloat("MoveSpeed", 1.5f);
        PlayerPrefs.SetFloat("MoveSens", 0f);
        m_Menu.m_MenuItems.DisableAll();
        m_Menu.m_MenuItems.Toggle(PageID.Rotation);
    }

}
