using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	const float MAX_MOVE_DURATION = 4;
	const float MIN_MOVE_DURATION = 1;

	// up - 1
	// right - 2
	// down - 3
	// left - 4
	// stand - 5

	private int[] allMoves = new int[] {1,2,3,4,5};
	private int[] noUp = new int[] {2,3,4,5};
	private int[] noRight = new int[] {1,3,4,5};
	private int[] noDown = new int[] {1,2,4,5};
	private int[] noLeft = new int[] {1,2,3,5};
	private int[] noStand = new int[] {1,2,3,4};

	GameManager manager;
	Sounds sounds;
	PlayerAnimation playerAnimation;
	CircleCollider2D col2d;
	float velUnit;

	private bool alive;
	private float moveTimer = 0;
	private int currentMove = 0;

	float auwTimer;

	bool dancing;

	void OnEnable() {
		MusicPlayer.OnApexStarted += dance;
		MusicPlayer.OnApexEnded += stopDance;
		GameManager.OnRestart += resetAgent;
	}

	void OnDisable() {
		MusicPlayer.OnApexStarted -= dance;
		MusicPlayer.OnApexEnded -= stopDance;
		GameManager.OnRestart -= resetAgent;
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		sounds = GameObject.Find ("Sounds").GetComponent<Sounds> ();
		playerAnimation = GetComponent<PlayerAnimation> ();
		col2d = GetComponent<CircleCollider2D> ();
		velUnit = manager.velUnit;
		resetAgent ();
	}

	public void resetAgent() {
		auwTimer = manager.danceLength;
		alive = true;
		dancing = false;
		col2d.enabled = true;
		playerAnimation.moveDown ();
		auwTimer = manager.danceLength;
		playerAnimation.reset ();
		GetComponent<SpriteRenderer> ().sortingOrder = 0;
	}

	// Update is called once per frame
	void Update () {

		if (alive && manager.playing) {

			if (!dancing) {
				makeMove ();
			} else {
				auwTimer -= Time.deltaTime;
				if (auwTimer <= 1.3f && auwTimer >= 1.2f) {
					sounds.auwPlural ();
				}
			}

			moveTimer -= Time.deltaTime;

			if (moveTimer <= 0) {

				// Make a decision what to do
				float duration = Random.Range (MIN_MOVE_DURATION, MAX_MOVE_DURATION);
				int move = getNextMove (currentMove);

				// Update the movement values
				moveTimer = duration;
				currentMove = move;
			}
		} else if (!manager.playing) {
			if (alive) {
				playerAnimation.stand ();
			}
		}
	}
		
	void makeMove() {

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, 0);
		float step = velUnit * Time.deltaTime;

		switch (currentMove) {
		case 1:
			pos = new Vector3 (transform.position.x, transform.position.y + step, 0);
			playerAnimation.moveUp();
			break;
		case 2:
			pos = new Vector3 (transform.position.x + step, transform.position.y, 0);
			playerAnimation.moveRight();
			break;
		case 3:
			pos = new Vector3 (transform.position.x, transform.position.y - step, 0);
			playerAnimation.moveDown ();
			break;
		case 4:
			pos = new Vector3 (transform.position.x - step, transform.position.y, 0);
			playerAnimation.moveLeft ();
			break;
		default:
			playerAnimation.stand ();
			break;
		}

		transform.position = manager.depthSim(pos);
	}

	int getNextMove(int exclude) {

		int[] moves;

		switch (exclude) {
		case 1:
			moves = noUp;
			break;
		case 2:
			moves = noRight;
			break;
		case 3:
			moves = noDown;
			break;
		case 4:
			moves = noLeft;
			break;
		case 5:
			moves = noStand;
			break;
		default:
			moves = allMoves;
			break;
		}

		return moves[Random.Range(0, moves.Length)];
	}

	void OnCollisionEnter2D (Collision2D collision) {
		int lotto = Random.Range (0, 100);
		int threshold;
		if (collision.gameObject.tag == "Walls") {
			// Collided with a wall - change direction most of the times
			threshold = 10;
		} else {
			// Collided with another agent or player - change directions every second time
			threshold = 50;
		}
		if (lotto > threshold) {
			moveTimer = 0;
		}
	}

	public void murdered() {
		alive = false;
		sounds.stabbed ();
		playerAnimation.murdered ();
		GetComponent<SpriteRenderer> ().sortingOrder = -1;
		col2d.enabled = false;
	}

	void dance() {	
		if (alive) {
			dancing = true;
			playerAnimation.dance();
		}
	}

	void stopDance() {
		dancing = false;
		auwTimer = manager.danceLength;
		playerAnimation.stopDance();
	}
}
