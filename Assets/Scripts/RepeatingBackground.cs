using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	private BoxCollider2D groundCollider;
	private float groundHorizontalLength;

	// Use this for initialization
	void Start () {
		groundCollider = GetComponent<BoxCollider2D> ();
		groundHorizontalLength = groundCollider.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		//check if it's time to reposition
		//if one of our object is scrolled to its full length to the left, in this case, then, it's ready to be repositioned
		if (transform.position.x < - groundHorizontalLength) {
			RepositionBackground ();
		}
	}

	void RepositionBackground()
	{
		//double the length of the collider and jump our object into a new position in front so it's ready to scroll
		Vector2 groundOffset = new Vector2 (groundHorizontalLength * 2f, 0);
		transform.position = (Vector2)transform.position + groundOffset; 
	}
}
