﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	public int numOfAgents = 9;

	[SerializeField]
	public float velUnit = 0.25f;

	[SerializeField]
	public float stabbingRadius = 1.5f;

	[SerializeField]
	public int postMurderCooldown = 15;

	public GameObject[] prefabs;

	public Player player1;
	public Player player2;

	public bool playing = true;

	public Text winnerTitle;
	private int blinkTime = 0;
	private bool blink = false;

	public Text newGame;
	private bool showNewGame;

	void Start () {
		spawnAgents ();
		winnerTitle.canvasRenderer.SetAlpha (0);
	}
	
	// Update is called once per frame
	void Update () {

		if (!playing) {

			if (showNewGame) {
				if (Input.anyKey) {
					startNewGame ();
				}
			} else {
				blinkTime++;
				if (blinkTime % 40 == 0) {
					blink = !blink;
				}
				winnerTitle.canvasRenderer.SetAlpha (blink ? 1 : 0);

				if (blinkTime % 480 == 0) {
					blinkTime = 0;
					blink = false;
					showNewGame = true;
					newGame.canvasRenderer.SetAlpha (1);
				}
			}
		}

	}

	void spawnAgents() {

		Vector3 randPos;

		for (int i = 0; i < numOfAgents; i++) {
			randPos = new Vector3 (Random.Range (-4, 4), Random.Range (-4, 4), Random.Range (-4, 4));
			Instantiate(prefabs[0], randPos, Quaternion.identity);
		}
	}

	public Vector3 depthSim(Vector3 pos) {
		pos.z = pos.y;
		return pos;
	}

	public void endGame(string winner) {
		print ("Game ended!");
		playing = false;
		Time.timeScale = 0.25f;
		winnerTitle.text = winner + " Wins!";
	}

	private void startNewGame() {
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		foreach(GameObject obj in agents) {
			Destroy(obj);
		}
	}
}
