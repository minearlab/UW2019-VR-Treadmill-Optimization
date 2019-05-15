using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming 6/11/18
//
// Simple script that makes setting up button interaction a bit easier

public class ButtonHandler : MonoBehaviour {

    public Text inputFieldText;
    public Text buttonText;

    private Button m_Button;
	// Use this for initialization
	void Start ()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If input field is empty, deactivate button
        if (inputFieldText.text == "")
        {
            m_Button.interactable = false;
            return;
        }

        
        m_Button.interactable = true;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.color = new Color(1, 1, 1, 1);

        // Record data if enter is pressed and button is active as well
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PlayerPrefs.SetString("Participant", inputFieldText.text);
            SceneManager.LoadScene("CalibrationScene");
        }
        
	}

    void OnClick()
    {
        // Record data on button click
        if (m_Button.interactable)
        {
            PlayerPrefs.SetString("Participant", inputFieldText.text);
            SceneManager.LoadScene("CalibrationScene");
        }
    }
}
