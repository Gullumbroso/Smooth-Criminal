using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	public int numOfAgents = 9;

	[SerializeField]
	public float velUnit = 0.25f;

	[SerializeField]
	public float stabbingRadius = 1.5f;

	public GameObject[] prefabs;

	void Start () {
		spawnAgents ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void spawnAgents() {

		Vector3 randPos;

		for (int i = 0; i < numOfAgents; i++) {
			randPos = new Vector3 (Random.Range (-4, 4), Random.Range (-4, 4), Random.Range (-4, 4));
			Instantiate(prefabs[0], randPos, Quaternion.identity);
		}
	}
}
