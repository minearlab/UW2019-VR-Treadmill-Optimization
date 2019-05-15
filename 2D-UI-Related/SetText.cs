using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {

    public Text movespeed;
    public Text sensitivity;
    public Text rotation;

    private void Update()
    {
        movespeed.text = "Current Movespeed: " + PlayerPrefs.GetFloat("MoveSpeed").ToString();
        sensitivity.text = "Current Sensitivity: " + PlayerPrefs.GetFloat("MoveSens").ToString();
        rotation.text = "Current Rotation Thresh: " + PlayerPrefs.GetFloat("RotSens").ToString();
    }
}
