using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Author: Adrian Barberis
// For: Dr. Meredith Minear, University of Wyoming, 11/18/2018

// Setup styling for UI buttons (2D)

[RequireComponent(typeof(EventTrigger))]
public class ButtonStyle : MonoBehaviour {

    public bool interactable = true;
    public Color defaultColor;
    public Color disabledColor;
    public Color clickedColor;
    //public Color mouseOverColor;
    public Color textColor;
    public float fadeDuration = 0.1f;
    public string buttonText = "";
    public int textResolution = 1;
    public bool isBold = false;
    public bool isItalic = false;


    [ReadOnly][SerializeField] private bool m_MouseInButton = false;
    [ReadOnly] public bool m_Clicked = false;

    private Button m_Button = null;
    private Text m_ButtonText = null;
    private ColorBlock m_ColorBlock;

    // Use this for initialization
    void Awake()
    {
        // Get Button Components
        #region
        m_Button = GetComponent<Button>();
        if (m_Button == null)
        {
            Debug.LogError("[ButtonStyle.cs] :: [Error] :: No Button Component Found!");
            DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: No Button Component Found!");
            return;
        }

        m_ButtonText = GetComponentInChildren<Text>();
        if (m_ButtonText == null)
        {
            Debug.LogError("[ButtonStyle.cs] :: [Error] :: No Text Component Found!");
            DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: No Text Component Found!");
            return;
        }

        if(GetComponent<EventTrigger>() == null)
        {
            Debug.LogError("[ButtonStyle.cs] :: [Error] :: Missing Event Trigger Script!");
            DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: Missing Event Trigger Script!");
        }
        else if(GetComponent<EventTrigger>().triggers.Count < 3)
        {
            Debug.LogError("[ButtonStyle.cs] :: [Error] :: Missing Event Triggers For [" + buttonText + "]!");
            DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: Missing Event Triggers For [" + buttonText + "]");
        }
        #endregion


        // Set Button Style
        #region

        m_ColorBlock = m_Button.colors;
        m_Button.interactable = interactable;
        DebugLogger.Log("[ButtonStyle.cs] :: Button Interactability Set To [" + interactable.ToString() + "]\r\n");

        m_ColorBlock.normalColor = defaultColor;
        DebugLogger.Log("[ButtonStyle.cs] :: Default Color Set To [" + defaultColor.ToString() + "]\r\n");

        m_ColorBlock.disabledColor = disabledColor;
        DebugLogger.Log("[ButtonStyle.cs] :: Disabled Color Set To [" + disabledColor.ToString() + "]\r\n");

        m_ColorBlock.highlightedColor = clickedColor;
        DebugLogger.Log("[ButtonStyle.cs] :: Mouse Over Color Set To [" + clickedColor.ToString() + "]\r\n");

        m_ColorBlock.fadeDuration = fadeDuration;
        DebugLogger.Log("[ButtonStyle.cs] :: Fade duration Set To [" + fadeDuration.ToString() + "]\r\n");

        m_Button.colors = m_ColorBlock;

        m_ButtonText.text = buttonText;
        DebugLogger.Log("[ButtonStyle.cs] :: Button Text Set To [" + buttonText + "]\r\n");
        #endregion


        // Set Text Style
        #region
        m_ButtonText.fontSize = 10;
        m_ButtonText.fontSize *= textResolution;
        m_ButtonText.rectTransform.localScale = new Vector3(m_ButtonText.rectTransform.localScale.x / textResolution, m_ButtonText.rectTransform.localScale.y / textResolution, 1);
        DebugLogger.Log("[ButtonStyle.cs] :: Text Resolution Set To [" + textResolution.ToString() + "]\r\n");

        if(isBold && !isItalic)
        {
            m_ButtonText.fontStyle = FontStyle.Bold;
            DebugLogger.Log("[ButtonStyle.cs] :: Text Style Set To [Bold]\r\n");
        }
        else if(isItalic && !isBold)
        {
            m_ButtonText.fontStyle = FontStyle.Italic;
            DebugLogger.Log("[ButtonStyle.cs] :: Text Style Set To [Italic]\r\n");
        }
        else if(isBold && isItalic)
        {
            m_ButtonText.fontStyle = FontStyle.BoldAndItalic;
            DebugLogger.Log("[ButtonStyle.cs] :: Text Style Set To [Bold & Italic]\r\n");
        }
        else
        {
            m_ButtonText.fontStyle = FontStyle.Normal;
            DebugLogger.Log("[ButtonStyle.cs] :: Text Style Set To [Normal]\r\n");
        }

        m_ButtonText.color = textColor;
        DebugLogger.Log("[ButtonStyle.cs] :: Text Color Set To [" + textColor.ToString() + "]\r\n");
        #endregion


        gameObject.tag = "UIButton";
        DebugLogger.Log("[ButtonStyle.cs] :: Button Styles Set Successfully\r\n\r\n");
    }

    

    public void MouseIn()
    {
        m_MouseInButton = true;
        if (!m_Clicked)
        {
            m_ColorBlock = m_Button.colors;
            m_ColorBlock.highlightedColor = clickedColor;
            m_Button.colors = m_ColorBlock;
        }
       
    }
    public void MouseOut()
    {
        m_MouseInButton = false;

        if (!m_Clicked)
        {           
            m_ColorBlock = m_Button.colors;
            m_ColorBlock.highlightedColor = defaultColor;
            m_Button.colors = m_ColorBlock;
        }
        
    }
    public void MouseClick()
    {
        if(m_MouseInButton)
        {
            if (!m_Clicked)
            {
                m_Clicked = true;
                try
                {
                    foreach (GameObject button in GameObject.FindGameObjectsWithTag("ActiveButton"))
                    {
                        button.GetComponent<ButtonStyle>().m_Clicked = false;
                        try { button.tag = "UIButton"; }
                        catch { DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: Missing Tag [UIButton]\r\n"); }
                    }
                    gameObject.tag = "ActiveButton";     
                }
                catch { DebugLogger.Log("[ButtonStyle.cs] :: [Error] :: Missing Tag [ActiveButton]\r\n"); }

                m_ColorBlock = m_Button.colors;
                m_ColorBlock.highlightedColor = clickedColor;
                m_Button.colors = m_ColorBlock;

                DebugLogger.Log("[ButtonStyle.cs] :: Button [" + buttonText + "] Clicked and Set to [ON]\r\n");
            }
            else
            {
                m_Clicked = false;
                m_ColorBlock = m_Button.colors;
                m_ColorBlock.highlightedColor = defaultColor;
                m_Button.colors = m_ColorBlock;

                DebugLogger.Log("[ButtonStyle.cs] :: Button [" + buttonText + "] Clicked and Set To [OFF]\r\n");
            }
        }   
    }

}
