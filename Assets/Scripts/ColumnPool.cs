using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour {

	private GameObject[] columns;
	public int columnPoolSize = 5;

	public GameObject columnPrefab;
	//this is a position off screen of the columns. when we are going to be using we spawn them offscreen before moving them in front of us
	private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
	private float timeSinceLastSpawned;

	public float spawnRate = 4f;
	public float columnYMin = -2f;
	public float columnYMax = 4f;
	public float columnXMin = 10f;
	public float columnXMax = 20f;

	private int currentColumn = 0;

	// Use this for initialization
	void Start () {
		columns = new GameObject[columnPoolSize];
		for (int i = 0; i < columnPoolSize; i++) {
			columns [i] = (GameObject) Instantiate (columnPrefab, objectPoolPosition, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {

		timeSinceLastSpawned += Time.deltaTime; //time to render the last frame, add that to our time. (to sync with frames?)

		if (!GameController.instance.gameOver && timeSinceLastSpawned >= spawnRate) {
			//generate a random position
			timeSinceLastSpawned = 0;

			float spawnYPosition = Random.Range(columnYMin, columnYMax);
			float spawnXPosition = Random.Range(columnXMin, columnXMax);

			columns[currentColumn].transform.position = new Vector2 (spawnXPosition, spawnYPosition);

            currentColumn++;
			if (currentColumn >= columnPoolSize) currentColumn = 0;
		}
	}
}