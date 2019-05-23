using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingChange : MonoBehaviour
{
    public Material on;
    public Material off;

    public TextMeshPro text;

    public List<GameObject> settingObjects = new List<GameObject>();

    public enum settings { fullScreen, resolution, graphics, whichSetting}
    public enum direction { down, change, up }

    public settings settingChoice;
    public direction directionChoice;

    private Resolution[] resolutions;

    private static int selectionObjects;
    private static int resolutionNo;
    private static int graphicsNo;
    // Start is called before the first frame update
    void Start()
    {
        if (settingChoice == settings.resolution)
        {
            resolutions = Screen.resolutions;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                    resolutionNo = i;
            }
            if (text != null)
            {
                string width = resolutions[resolutionNo].width.ToString();
                string height = resolutions[resolutionNo].height.ToString();
                text.text = width + " x " + height;
            }
        }
        if (settingChoice == settings.graphics)
            text.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    // Update is called once per frame
    void Update()
    {
        if (settingChoice == settings.fullScreen)
        {
            if (Screen.fullScreen == true)
                GetComponent<Renderer>().material = on;
            else
                GetComponent<Renderer>().material = off;
        }
        if (settingChoice == settings.whichSetting)
        {
            for (int i = 0; i < settingObjects.Count; i++)
            {
                if (i == selectionObjects)
                    settingObjects[i].SetActive(true);
                else
                    settingObjects[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (settingChoice == settings.fullScreen)
            {
                if (directionChoice == direction.change)
                    Screen.fullScreen = !Screen.fullScreen;
                if (directionChoice == direction.up)
                    Screen.fullScreen = true;
                if (directionChoice == direction.down)
                    Screen.fullScreen = false;
            }
            else
                GetComponent<Renderer>().material = off;
            if (settingChoice == settings.whichSetting)
            {
                if (directionChoice == direction.down)
                    selectionObjects--;
                if (directionChoice == direction.up)
                    selectionObjects++;
                if (selectionObjects < 0)
                    selectionObjects = settingObjects.Count - 1;
                if (selectionObjects == settingObjects.Count)
                    selectionObjects = 0;
            }
            if (settingChoice == settings.resolution)
            {
                if (directionChoice == direction.down)
                    resolutionNo--;
                if (directionChoice == direction.up)
                    resolutionNo++;
                if (resolutionNo < 0)
                    resolutionNo = resolutions.Length - 1;
                if (resolutionNo == resolutions.Length)
                    resolutionNo = 0;

                Screen.SetResolution(resolutions[resolutionNo].width, resolutions[resolutionNo].height, Screen.fullScreen);
                if (text != null)
                {
                    string width = resolutions[resolutionNo].width.ToString();
                    string height = resolutions[resolutionNo].height.ToString();
                    text.text = width + " x " + height;
                }
            }
            if (settingChoice == settings.graphics)
            {
                if (directionChoice == direction.down)
                    QualitySettings.DecreaseLevel();
                if (directionChoice == direction.up)
                    QualitySettings.IncreaseLevel();
                text.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            GetComponent<Renderer>().material = on;
    }
}
