using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Monkey : MonoBehaviour {

	private bool isDead;
	private Rigidbody2D rb2d;
	private const float voiceUpForce = 180f;
    private const float touchUpForce = 200f;
	private Animator anim;
	private AudioSource[] monkeySounds;
	private AudioSource flapSound;
	private AudioSource deadSound;
    public Text debugText;
    public Text debugText2;
    private bool debugMode = false;
	
    // variables handling input through microphone
    private AudioClip microphoneInput;
	private bool flapped;

    private void Awake()
    {
        if(Microphone.devices.Length > 0)
        {
            //initializing scripting
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);
        }
    }

    // Use this for initialization
    void Start () {

        LoadInputMode();
        LoadVoiceSensitivity();
        isDead = false;
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		monkeySounds = GetComponents<AudioSource>();
		flapSound = monkeySounds [0];
		deadSound = monkeySounds [1];
        FlapMonkey(); 
	}
	
	// Update is called once per frame
	void Update () {
		//if the player is dead, stop the game
		if (isDead == false)
		{
            if(SettingsController.inputMode) OnMouseDown();
			if (!SettingsController.inputMode) FlapMonkeyWithVoice();
		}
	}

    void OnMouseDown()
    {
        // Detect mouse event
        if (IsPointerOverUIObject())
        {
            Debug.Log("return mouse");
            return;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            FlapMonkeyWithTouch();
        }
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //handling the collision between ground and monkey or tubes and monkey
    void OnCollisionEnter2D()
	{
        if(!isDead)deadSound.Play();
        isDead = true;
        //avoid a little slide if the monkey dies because of the backgroundScrolling
        rb2d.velocity = Vector2.zero;
		anim.SetTrigger ("Dead");
        GameController.instance.MonkeyDied();
	}

    void FlapMonkey()
    {
        //velocity is either rising or falling. Every time we push the jump button we get always the time response.
        rb2d.velocity = Vector2.zero;
        //adding some force to rb2d (we do not want to change horizontally the player because the world is moving around him)
        if(!SettingsController.inputMode)rb2d.AddForce(new Vector2(0, voiceUpForce));
        else if (SettingsController.inputMode) rb2d.AddForce(new Vector2(0, touchUpForce));
        anim.SetTrigger("Flap");
		flapSound.Play ();
	}

    void FlapMonkeyWithVoice()
    {
		//get mic volume
		int dec = 128;
		float[] waveData = new float[dec];
		int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
		microphoneInput.GetData(waveData, micPosition);

		// Getting a peak on the last 128 samples
		float levelMax = 0;
		
        for (int i = 0; i < dec; i++)
		{
			float wavePeak = waveData[i] * waveData[i];
			if (levelMax < wavePeak) levelMax = wavePeak;
		}

        float level = Mathf.Sqrt(Mathf.Sqrt(levelMax));
		//debug variables
		if(debugMode)
        {
            debugText.gameObject.SetActive(true);
            debugText2.gameObject.SetActive(true);
            debugText.text = "level:" + level + "\nsensitivity: " + SettingsController.voiceSensitivity;
            if (!flapped) debugText2.text = "flapped = False";
            if (flapped) debugText2.text = "flapped = True";
        }

        if (level > SettingsController.voiceSensitivity && !flapped)
        {
            if (!GameController.isPaused) FlapMonkey();
            flapped = true;
        }

		if (level < SettingsController.voiceSensitivity && flapped) flapped = false;
	}

    void FlapMonkeyWithTouch()
    {
		//Left mouse button
		   if(Input.GetMouseButtonDown(0))
		   {
			   if(!GameController.isPaused)FlapMonkey();
		   }
		   //Touch input
		   if(Input.touchCount > 0)
		   {
			   Input.GetTouch(0);
		   }
	}

    private void LoadInputMode()
    {
        int inputModeInt = PlayerPrefs.GetInt("inputMode", 0);
        if (inputModeInt == 0) SettingsController.inputMode = false;
        else if (inputModeInt == 1) SettingsController.inputMode = true;
    }

    private void LoadVoiceSensitivity()
    {
        SettingsController.voiceSensitivity = PlayerPrefs.GetFloat("sensitivity", 0.71f);
    }
}