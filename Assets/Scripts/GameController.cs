using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour {
	public static GameController instance;
	public GameObject gameOverText;
	public bool gameOver = false;
    public static bool isPaused = false;
	public float scrollSpeed = -1.5f;
    private int score = 0;
    private int record = 0;
	public Text scoreText;
    public Text recordText;
    public GameObject tutorialText;
    public Button pauseButton;
	public GameObject pauseMenu;
    private Animator animSoundToggle;
    private bool adEnabled = true;
    private bool adShown = false;

    private const int AD_CHANCE = 15;

    // Use this for initialization
    void Awake () {
		//this method is called BEFORE Start()
		//enforce our singleton pattern by making sure there are no other instances online
		if (instance == null) {
			instance = this;
		} else {
			//destroy the gameobject this instance is attached to
			Destroy (gameObject);
		}
	}

    void Start()
    {
        //for testing purpose
        //DeletePrefs();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        LoadRecord();
        //LoadCoins ();
        LoadPrefs();
    }

    private void LoadPrefs()
    {
        int inputModeInt = PlayerPrefs.GetInt("inputMode", 0);
        if (inputModeInt == 0) SettingsController.inputMode = false;
        else if (inputModeInt == 1) SettingsController.inputMode = true;
        if (!SettingsController.inputMode) tutorialText.GetComponent<TextMesh>().text = "Scream to Jump";
        else tutorialText.GetComponent<TextMesh>().text = "Tap to Jump";
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameOver)
            {
                if (!isPaused) PauseGame();
                else ResumeGame();
            }
        }
    }

    public void MonkeyDied() {
		gameOverText.SetActive (true);
		pauseButton.gameObject.SetActive (false);
		gameOver = true;
        SaveRecord();

        if (adEnabled) ShowAd();

	}

    public void ShowAd()
    {
        if (!adShown)
        {
            int adRate = UnityEngine.Random.Range(1, 101);
            if (adRate >= 1 && adRate <= AD_CHANCE)
            {
                if (Advertisement.IsReady())
                {
                    adShown = true;
                    Advertisement.Show();
                }
            }
        }
    }

    public void MonkeyScored()
	{
		if (!gameOver) {
			score++;
			scoreText.text = "Score: " + score.ToString();
		}
	}

    public void LoadRecord()
    {
        record = PlayerPrefs.GetInt("record", record);
        recordText.text = "Record: " + record.ToString();
    }

    public void SaveRecord()
    {
        if (score > PlayerPrefs.GetInt("record")) {
            PlayerPrefs.SetInt("record", score);
            PlayerPrefs.Save();
            recordText.text = "Record: " + record.ToString();
        }
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

	public void ChangeToScene (string sceneToChangeTo) {
		if (isPaused)
			ResumeGame ();
		SceneManager.LoadScene (sceneToChangeTo);
	}

	public void RestartGame(string sceneToChangeTo)
	{
		if (isPaused)
			ResumeGame();
        adShown = false;
		SceneManager.LoadScene(sceneToChangeTo);
	}


	public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
			pauseButton.gameObject.SetActive (false);
			pauseMenu.SetActive (true);
			Time.timeScale = 0;
        }
    }

	public void ResumeGame()
	{
		if (isPaused) {
			isPaused = false;
			pauseButton.gameObject.SetActive (true);
			pauseMenu.SetActive (false);
			Time.timeScale = 1;
		}
	}

    public void ToggleAudio()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = true;
            animSoundToggle.SetTrigger("AudioButtonOFF");
        }
		else if(!AudioListener.pause)
        {
            AudioListener.pause = false;
            animSoundToggle.SetTrigger("AudioButtonON");
        }
    }

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions {
				resultCallback = HandleShowResult
			};
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

}
