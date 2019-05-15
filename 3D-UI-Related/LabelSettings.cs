using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 6/13/2018
// 
// Makes the label prefab easier to work with by simplifying some actions such as updating label text
//
public class LabelSettings : MonoBehaviour
{

    public Text labelText;
    public string labelString;
    public FontStyle labelTextFontStyle;
    public Color labelTextColor;
    public Color labelBackgroundColor;

    private Image m_LabelBackground;
    

    private void Awake()
    {
        labelText.text = labelString;
        labelText.fontStyle = labelTextFontStyle;
        labelText.color = labelTextColor;

        m_LabelBackground = GetComponent<Image>();
        m_LabelBackground.color = labelBackgroundColor;
    }

    public void SetLabelText(string text)
    {
        labelText.text = text;
    }
}
