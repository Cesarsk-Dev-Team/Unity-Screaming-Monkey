using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsController : MonoBehaviour {

    //false = microphone
    //true = touch
    public static bool inputMode = false;
    public Button voiceInputButton, touchInputButton;
    [HideInInspector]
    public static float voiceSensitivity = 0.71f;
    public Text sliderLabel;
    public Text inputModeLabel;
    public Slider slider;
    //write a method with shar prefs which handles the input mode and put a button in the menu

    public void ChangeToScene (string sceneToChangeTo) {
		SceneManager.LoadScene (sceneToChangeTo);
	}

    public void Start()
    {
        LoadPrefs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeToScene("Menu");
    }

    public void ToggleInput()
    {
        inputMode = !inputMode;
        if (!inputMode)
        {
            voiceInputButton.gameObject.SetActive(true);
            touchInputButton.gameObject.SetActive(false);
            slider.interactable = true;
            inputModeLabel.text = "Scream to Jump";
            PlayerPrefs.SetInt("inputMode", 0);
        }
        else
        {
            voiceInputButton.gameObject.SetActive(false);
            touchInputButton.gameObject.SetActive(true);
            slider.interactable = false;
            inputModeLabel.text = "Touch to Jump";
            PlayerPrefs.SetInt("inputMode", 1);
        }
    }

	public void AboutUs()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=Cesarsk+Dev+Team");
	}

    public void AdjustVoiceSensitivity(float newSensitivity)
    {
        voiceSensitivity = newSensitivity;
        sliderLabel.text = "Voice Sensitivity: " + voiceSensitivity.ToString("0.00"); //2dp Number
        PlayerPrefs.SetFloat("sensitivity", newSensitivity);
    }

    private void LoadPrefs()
    {
        //loading input mode
        int inputModeInt = PlayerPrefs.GetInt("inputMode", 0);
        if (inputModeInt == 0) inputMode = false;
        else if (inputModeInt == 1) inputMode = true;
        if (!inputMode)
        {
            voiceInputButton.gameObject.SetActive(true);
            touchInputButton.gameObject.SetActive(false);
            slider.interactable = true;
            PlayerPrefs.SetInt("inputMode", 0);
            inputModeLabel.text = "Scream to Jump";
        }
        else
        {
            voiceInputButton.gameObject.SetActive(false);
            touchInputButton.gameObject.SetActive(true);
            slider.interactable = false;
            PlayerPrefs.SetInt("inputMode", 1);
            inputModeLabel.text = "Touch to Jump";
        }

        //loading Voice Sensitivity
        voiceSensitivity = PlayerPrefs.GetFloat("sensitivity", 0.71f);
        slider.value = voiceSensitivity;
        sliderLabel.text = "Voice Sensitivity: " + voiceSensitivity.ToString("0.00"); //2dp Number
    }
}
