using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only one screenFader allowed per component (to make more add the component instead of calling the static version.)
/// Fader is slower when fadeRate is larger.
/// Fader must have canvas group for now.
/// </summary>

public class Fader : MonoBehaviour
{
    public static Fader GC;

    float fadeOut, fadeIn;
    float fadeRateOut, fadeRateIn;
    bool startFadeIn, startFadeOut;

    GameObject screenFaderOut, screenFaderIn;
    public string fadeScreenName = "fadeScreen";
    public bool fadeCompleteOut, fadeCompleteIn;

    void Awake()
    {
        GC = GetComponent<Fader>();
    }
    void Start ()
    {
        /*
        if (!GC)
        {
            GC = GetComponent<Fader>();
        }
        */
	}
	
	void Update ()
    {
        if (screenFaderIn != null)
        {
            screenFaderIn.GetComponent<CanvasGroup>().alpha = fadeIn;
        }
        if (screenFaderOut != null)
        {
            screenFaderOut.GetComponent<CanvasGroup>().alpha = fadeOut;
        }


        if (startFadeIn)
        {

            fadeIn -= (1 / fadeRateIn) * Time.deltaTime;
            if (fadeIn <= 0)
            {
                fadeCompleteIn = true;
                startFadeIn = false;
                RemoveFader(screenFaderIn);
            }
        }
        else
        {

            if (startFadeOut && fadeOut <= 1)
            {
                fadeOut += (1 / fadeRateOut) * Time.deltaTime;
            }
            else if (fadeOut >= 1)
            {
                fadeCompleteOut = true;
                startFadeOut = false;
            }
        }
        
       
    }
    
    /// <summary>
    /// will autmatically delete when alpha is at 0.
    /// </summary>
    /// <param name="fadeRate"></param>

    public void FadeOut(float fadeRate) 
    {
        if (screenFaderOut == null)
        {
            fadeCompleteOut = false;
            screenFaderOut = (GameObject)Instantiate(Resources.Load("UI/" + fadeScreenName));
            startFadeOut = true;
            fadeRateOut = fadeRate;
            
        }
        
    }

    public void FadeIn(float fadeRate)
    {
        if (screenFaderIn == null)
        {
            fadeCompleteIn = false;
            screenFaderIn = (GameObject)Instantiate(Resources.Load("UI/" + fadeScreenName));
            startFadeIn = true;
            fadeRateIn = fadeRate;
            fadeIn = 1;
        }
    }
    
    /// <summary>
    /// Deletes fader
    /// </summary>

    public void RemoveFader(GameObject screenFader)
    {
        if (screenFader != null)
        {
            Destroy(screenFader);
        }
    }

    public float GetFadeRate()
    {
        return fadeRateOut;
    }
}
