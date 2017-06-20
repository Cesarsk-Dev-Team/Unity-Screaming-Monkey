using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour {

	private bool isDead;
	private Rigidbody2D rb2d;
	private float voiceUpForce = 180f;
    private float touchUpForce = 200f;
	private Animator anim;
	private AudioSource[] birdSounds;
	private AudioSource flapSound;
	private AudioSource deadSound;
    public Text debugText;
    public Text debugText2;
    private bool debugMode = false;
    private bool inputMode;
    private float voiceSensitivity;
	
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
		birdSounds = GetComponents<AudioSource>();
		flapSound = birdSounds [0];
		deadSound = birdSounds [1];
        FlapBird(); 
	}
	
	// Update is called once per frame
	void Update () {
		//if the player is dead, stop the game
		if (isDead == false)
		{
            if(!inputMode)FlapBirdWithVoice();
            else if(inputMode)FlapBirdWithTouch();
		}
	}

	//handling the collision between ground and bird or tubes and bird
	void OnCollisionEnter2D()
	{
        if(!isDead)deadSound.Play();
        isDead = true;
		//avoid a little slide if the bird dies because of the backgroundScrolling
		rb2d.velocity = Vector2.zero;
		anim.SetTrigger ("Dead");
        GameController.instance.BirdDied();
	}

    void FlapBird()
    {
        //velocity is either rising or falling. Every time we push the jump button we get always the time response.
        rb2d.velocity = Vector2.zero;
        //adding some force to rb2d (we do not want to change horizontally the player because the world is moving around him)
        if(!inputMode)rb2d.AddForce(new Vector2(0, voiceUpForce));
        else if (inputMode) rb2d.AddForce(new Vector2(0, touchUpForce));
        anim.SetTrigger("Flap");
		flapSound.Play ();
	}

    void FlapBirdWithVoice()
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
            debugText.text = "level:" + level + "\nsensitivity: " + voiceSensitivity;
            if (!flapped) debugText2.text = "flapped = False";
            if (flapped) debugText2.text = "flapped = True";
        }

        if (level > voiceSensitivity && !flapped)
        {
            if (!GameController.isPaused) FlapBird();
            flapped = true;
        }

		if (level < voiceSensitivity && flapped) flapped = false;
	}

    void FlapBirdWithTouch()
    {
		//Left mouse button
		   if(Input.GetMouseButtonDown(0))
		   {
			   if(!GameController.isPaused)FlapBird();
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
        if (inputModeInt == 0) inputMode = false;
        else if (inputModeInt == 1) inputMode = true;
    }

    private void LoadVoiceSensitivity()
    {
        voiceSensitivity = PlayerPrefs.GetFloat("sensitivity", 0.71f);
    }
}