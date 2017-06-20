using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public static MenuController instance;
	public Button audioON, audioOFF;
	private Animator animSoundToggle;
	private bool audioMuted = false;

	private void Awake()
	{
		//this method is called BEFORE Start()
		//enforce our singleton pattern by making sure there are no other instances online
		if (instance == null) {
			instance = this;
		} else {
			//destroy the gameobject this instance is attached to
			Destroy (gameObject);
		}

		audioMuted = AudioListener.pause;
		if (!audioMuted) {
			audioON.gameObject.SetActive (true);
			audioOFF.gameObject.SetActive (false);
		} else {
			audioON.gameObject.SetActive (false);
			audioOFF.gameObject.SetActive (true);
		}
	}

	public void ChangeToScene (int sceneToChangeTo) {
		SceneManager.LoadScene (sceneToChangeTo);
	}

	public void ChangeToScene (string sceneToChangeTo) {
		SceneManager.LoadScene (sceneToChangeTo);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void ToggleAudio()
	{
		if (!audioMuted) {
			AudioListener.pause = true;
			audioMuted = true;
			audioON.gameObject.SetActive (false);
			audioOFF.gameObject.SetActive (true);
		} else {
			AudioListener.pause = false;
			audioMuted = false;
			audioON.gameObject.SetActive (true);
			audioOFF.gameObject.SetActive (false);
		}
	}

	public void GitHub()
	{
		Application.OpenURL("https://github.com/Cesarsk/Unity-Screaming-Monkey");
	}
}