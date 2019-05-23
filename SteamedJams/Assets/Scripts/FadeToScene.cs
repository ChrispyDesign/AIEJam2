using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class FadeToScene : MonoBehaviour {

    public float tarFogStrength;
    public float fadeSpeed;

    public bool fadeTrue = false;

    public bool restartScene = true;
    public int tarSceneNumber = 0;

	private float startVignetteStrength = -1;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startVignetteStrength == -1) {
            //PostProcessLayer
			PostProcessProfile mainCamProfile = CameraMovement.activeCamera.GetComponentInChildren<PostProcessVolume>().profile;
			startVignetteStrength = mainCamProfile.GetSetting<Vignette>().intensity;
		}
        if (fadeTrue == true)
            Fade();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            fadeTrue = true;
    }

    private void Fade ()
    {
        if (RenderSettings.fogDensity < tarFogStrength)
        {
            RenderSettings.fogDensity += fadeSpeed * Time.deltaTime;

            PostProcessProfile mainCamProfile = CameraMovement.activeCamera.GetComponentInChildren<PostProcessVolume>().profile;
            var vignette = mainCamProfile.GetSetting<Vignette>();

            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, fadeSpeed * Time.deltaTime);
            mainCamProfile.settings[0] = vignette;
        }
        else
        {
            PostProcessProfile mainCamProfile = CameraMovement.activeCamera.GetComponentInChildren<PostProcessVolume>().profile;
            var vignette = mainCamProfile.GetSetting<Vignette>();

            vignette.intensity.value = startVignetteStrength;
            mainCamProfile.settings[0] = vignette;
            if (restartScene == true)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else
            {
                if (tarSceneNumber == -1)
                    Application.Quit();
                else
                    SceneManager.LoadScene(tarSceneNumber);
            }
        }
    }
}