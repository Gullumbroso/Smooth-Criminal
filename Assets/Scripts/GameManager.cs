using System.Collections;
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

	[SerializeField]
	public float danceLength = 15f;

	[SerializeField]
	public bool debugMode;

	[SerializeField]
	public float blackFadeStep = 1.0f;

	public GameObject openingScreen;
	public GameObject blackScreen;
	public Sounds sounds;
	public AudioSource themeSong;

	public CountDown countDown;

	public GameObject[] prefabs;

	public Player player1;
	public Player player2;

	public bool opening;
	public bool playing;

	public Text winnerTitle;

	public Text newGame;
	private bool showNewGame;

	SpriteRenderer openingScreenSprite;
	SpriteRenderer blackScreenSprite;

	bool starting;
	bool fadingOut;
	bool fadingIn;
	bool isCountdownFinished;

	Color transparent;
	Color nonTransparent;


	void Start () {
		openingScreenSprite = openingScreen.GetComponent<SpriteRenderer> ();
		blackScreenSprite = blackScreen.GetComponent<SpriteRenderer> ();
		sounds = GameObject.Find("Sounds").GetComponent<Sounds> ();
		countDown = GameObject.Find("CountDown").GetComponent<CountDown> ();
		transparent = new Color (1.0f, 1.0f, 1.0f, 0);
		nonTransparent = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		winnerTitle.canvasRenderer.SetAlpha (0);
		newGame.canvasRenderer.SetAlpha (0);
		showOpeninigScreen ();
	}

	void showOpeninigScreen() {
		opening = true;
		playing = false;
		starting = false;
		openingScreenSprite.color = nonTransparent;
		blackScreenSprite.color = transparent;
		themeSong.Play ();
	}

	void hideOpeningScreen() {
		opening = false;
		openingScreenSprite.color = transparent;
	}

	void startNewGame() {
		opening = false;
		starting = true;
		fadingOut = true;
		isCountdownFinished = false;
		spawnAgents ();
	}

	void startPlaying() {
		playing = true;
		openingScreenSprite.color = transparent;
		blackScreenSprite.color = transparent;
		sounds.auw ();
		sounds.drum ();
	}
		
	// Update is called once per frame
	void Update () {

		if (starting) {
			if (fadingOut) {
				blackFadeIn ();
				exitMusic ();
			} else if (!isCountdownFinished) {
				countDown.startCountDown ();
			} else {
				starting = false;
				startPlaying ();
			}
		}

		if (opening) {
			if (Input.anyKey) {
				startNewGame ();
			}
		} else {
			// Playing
		}
	}

	void blackFadeIn() {
		if (blackScreenSprite.color.a < 1.0f) {
			var alpha = blackScreenSprite.color.a;
			blackScreenSprite.color = new Color (1, 1, 1, alpha + blackFadeStep * Time.deltaTime);
		} else {
			fadingOut = false;
		}
	}

	void blackFadeOut() {
		if (blackScreenSprite.color.a > 0) {
			var alpha = blackScreenSprite.color.a;
			blackScreenSprite.color = new Color (1, 1, 1, alpha - blackFadeStep * Time.deltaTime);
		} else {
			fadingIn = false;
		}
	}

	void exitMusic () {
		if (themeSong.isPlaying) {
			if (themeSong.volume > 0) {
				themeSong.volume -= blackFadeStep * Time.deltaTime;
			} else {
				themeSong.volume = 1.0f;
				themeSong.Stop ();
			}
		}
	}

	public void countdownFinished() {
		isCountdownFinished = true;
		fadingIn = true;
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

	private void restart() {
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		foreach(GameObject obj in agents) {
			Destroy(obj);
		}
		player1.placeInScreen ();
		spawnAgents ();
	}
}
