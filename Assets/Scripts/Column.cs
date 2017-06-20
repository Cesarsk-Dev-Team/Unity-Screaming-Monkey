using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

	private AudioSource scoreAudio;
	// Use this for initialization
	void Start () {
		scoreAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//it's different than collider, trigger is enabled if "isTriggered" is true on the object
		if (other.GetComponent<Bird> () != null) {
			//if it does have a bird component, let's call our gameController
			GameController.instance.BirdScored();
			scoreAudio.Play ();
		}
	}
}
