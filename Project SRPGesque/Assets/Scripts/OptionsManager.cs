using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{

    public int resolutionOption;
    public string[] resolutions;
    public int qualityOption;    
    public string[] qualities;

    // Use this for initialization
    void Start ()
    {
        resolutions = new string[3];
        resolutions[0] = "1280x720";
        resolutions[1] = "1600x900";
        resolutions[2] = "1920x1080";

        qualities = new string[4];
        qualities[0] = "FAST";
        qualities[1] = "GOOD";
        qualities[2] = "GREAT";
        qualities[3] = "ULTRA";

        resolutionOption = PlayerPrefs.GetInt("currentResolution",0);
        qualityOption = PlayerPrefs.GetInt("currentQuality",0);
        UpdateResolution();
    }
	

    public void ChangeResolution(bool whichArrow)
    {
        if (whichArrow)
        {
            resolutionOption++;
            if (resolutionOption >= resolutions.Length) resolutionOption = 0;            
        }
        else
        {
            resolutionOption--;
            if (resolutionOption <= -1) resolutionOption = resolutions.Length - 1;            
        }
        //PlayerPrefs for now, this can be changed to be saved into a options file directly at a later point.
        PlayerPrefs.SetInt("currentResolution", resolutionOption);
        UpdateResolution();
    }
    public void ChangeQuality(bool whichArrow)
    {
        if (whichArrow)
        {
            qualityOption++;
            if (qualityOption >= qualities.Length) qualityOption = 0;
        }
        else
        {
            qualityOption--;
            if (qualityOption <= -1) qualityOption = qualities.Length - 1;
        }
        PlayerPrefs.SetInt("currentQuality", resolutionOption);

    }
    public void UpdateResolution()
    {
        if(resolutionOption == 0) Screen.SetResolution(1280, 720, true);
        else if(resolutionOption == 1) Screen.SetResolution(1600, 900, true);
        else if(resolutionOption == 2) Screen.SetResolution(1920, 1080, true);
        Debug.Log("current resolution is " + Screen.currentResolution);
    }
}
