using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 6/13/2018
//
// Handle some Unity UI slider stuff to make the sliders a bit more interesting
// Mainly, this just makes it easier to deal with filling slider with color as the slider value increases
// There is also support for up to four colors which will fade into each other as values increase or decrease

public class SliderSettings : MonoBehaviour
{

    public int upperBound;
    public float fadeDelay = 4; // increase/decrease how long it takes to fade into a color

    public GameObject background;
    public Color backgroundColor;
    private Image m_BackgroundImage;

    public GameObject fill;
    public Color fillColor;
    private Image m_FillImage;

    public GameObject knob;
    public Color knobColor;
    private Image m_KnobImage;

    public Color gradientQuarter1;
    public Color gradientQuarter2;
    public Color gradientQuarter3;
    public Color gradientQuarter4;


    private void Awake()
    {
        m_BackgroundImage = background.GetComponent<Image>();
        m_BackgroundImage.color = backgroundColor;

        m_FillImage = fill.GetComponent<Image>();
        m_FillImage.color = fillColor;

        m_KnobImage = knob.GetComponent<Image>();
        m_KnobImage.color = knobColor;

        gameObject.GetComponent<Slider>().maxValue = upperBound;
    }

    private void Update()
    {
        m_KnobImage.color = knobColor;
        // Get percentage of slider that is filled
        var level = gameObject.GetComponent<Slider>().value / upperBound;


        if (level < 0.25) // first quarter
        {
            // Fade in color as values increase
            // Fade speed is dictated by what percentage of the quarter is filled (level / 0.25) times the [ fadeDelay ] and is smoothed using [ Time.deltaTime ]
            fillColor = Color.Lerp(m_FillImage.GetComponent<Image>().color, gradientQuarter1, (level / 0.25f) * fadeDelay * Time.deltaTime);
            fill.GetComponent<Image>().color = fillColor;
        }
        else if (level >= 0.25 && level < 0.5) // second quarter
        {
            fillColor = Color.Lerp(m_FillImage.GetComponent<Image>().color, gradientQuarter2, (level / 0.5f) * fadeDelay * Time.deltaTime);
            fill.GetComponent<Image>().color = fillColor;
        }
        else if (level >= 0.5 && level < 0.75) // third quarter
        {
            fillColor = Color.Lerp(m_FillImage.GetComponent<Image>().color, gradientQuarter3, (level / 0.75f) * fadeDelay * Time.deltaTime);
            fill.GetComponent<Image>().color = fillColor;
        }
        else if (level > 0.75) // fourth quarter
        {
            fillColor = Color.Lerp(m_FillImage.GetComponent<Image>().color, gradientQuarter4, (level / 1f) * fadeDelay * Time.deltaTime);
            fill.GetComponent<Image>().color = fillColor;
        }

    }
}
