using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string PLAYER1_TAG = "Player1";
	private string PLAYER2_TAG = "Player2";

	public GameManager manager;
	private Sounds sounds;
	private KnifeFeedback knifeFeeback;
	private CircleCollider2D selfCollider;
	public PlayerAnimation playerAnimation;
	private int direction = 0; // 0-down, 1-left, 2-up, 3-right
	private float velUnit;
	private float cooldownTimer;
	private bool cooldown = false;
	private float dancingTimer;
	private bool dancing = false;
	private bool dancingOnCue = false;
	private bool preparing = false;

	private bool winDanceMove;
	private bool dancingAsWinner;
	private float winDanceMoveTimer1;
	private float winDanceMoveTimer2;


	void OnEnable() {
		MusicPlayer.OnApexPrepare += prepareStart;
		MusicPlayer.OnApexStarted += prepareClose;
		MusicPlayer.OnApexEnded += apexEnded;
		GameManager.OnRestart += resetPlayer;
	}

	void OnDisable() {
		MusicPlayer.OnApexPrepare -= prepareStart;
		MusicPlayer.OnApexStarted -= prepareClose;
		MusicPlayer.OnApexEnded += apexEnded;
		GameManager.OnRestart -= resetPlayer;
	}

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		if (tag == PLAYER1_TAG) {
			knifeFeeback = GameObject.Find ("Blue Knife").GetComponent<KnifeFeedback> ();
		} else {
			knifeFeeback = GameObject.Find ("Red Knife").GetComponent<KnifeFeedback> ();
		}
		sounds = GameObject.Find ("Sounds").GetComponent<Sounds> ();
		selfCollider = GetComponent<CircleCollider2D> ();
		playerAnimation = GetComponent<PlayerAnimation> ();
		velUnit = manager.velUnit;
		cooldownTimer = manager.postMurderCooldown;
		placeInScreen ();
		winDanceMove = false;
		winDanceMoveTimer1 = 1.5f;
		winDanceMoveTimer2 = 4.0f;
		dancingAsWinner = false;
	}

	void resetPlayer() {
		print ("Reset Player " + tag + " !");
		cooldownTimer = manager.postMurderCooldown;
		winDanceMove = false;
		winDanceMoveTimer1 = 1.5f;
		winDanceMoveTimer2 = 4.0f;
		dancingAsWinner = false;
		playerAnimation.reset ();
		placeInScreen ();
	}
	
	// Update is called once per frame
	void Update () {

		if (manager.playing) {

			if (!dancing && !dancingOnCue) {
				getInput ();
			} else {
				dancingTimer -= Time.deltaTime;
				if (dancingTimer <= 1.4 && dancingTimer >= 1.3) {
					sounds.auw ();
				} else if (dancingTimer <= 0) {
					// Continue playing
					dancing = false;
					playerAnimation.stopDance ();
					dancingTimer = manager.danceLength;
				}
			}

			if (cooldown) {
				cooldownTimer -= Time.deltaTime;
				if (cooldownTimer <= 0) {
					// reload the stabbing ability
					cooldown = false;
					knifeFeeback.cooldownEnd ();
					cooldownTimer = manager.postMurderCooldown;
				}
			}
		
		} else if (winDanceMove) {
			winDanceMoveTimer1 -= Time.deltaTime;
			if (winDanceMoveTimer1 >= 0) {
				var pos = new Vector3 (transform.position.x - velUnit * Time.deltaTime, transform.position.y, 0);
				transform.position = manager.depthSim (pos);
				direction = 3;
				playerAnimation.moveRight ();
			} else if (winDanceMoveTimer2 >= 0) {
				winDanceMoveTimer2 -= Time.deltaTime;
				if (!dancingAsWinner) {
					sounds.apex ();
					dance ();
					dancingAsWinner = true;
				}
			} else {
				print ("Restart!");
				manager.restart ();
			}
		}
	}

	public void placeInScreen() {
		transform.position = new Vector3 (Random.Range (-7, 7), Random.Range (-7, 1), 0);
	}

	private void getInput() {

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, 0);
		float step = velUnit * Time.deltaTime;

		if (gameObject.tag == PLAYER1_TAG) {
			
			if (Input.GetKey (KeyCode.UpArrow)) {
				pos = new Vector3 (transform.position.x, transform.position.y + step, 0);
				direction = 2;
				playerAnimation.moveUp ();
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				pos = new Vector3 (transform.position.x, transform.position.y - step, 0);
				direction = 0;
				playerAnimation.moveDown ();
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				pos = new Vector3 (transform.position.x + step, transform.position.y, 0);
				direction = 3;
				playerAnimation.moveRight ();
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				pos = new Vector3 (transform.position.x - step, transform.position.y, 0);
				direction = 1;
				playerAnimation.moveLeft ();
			} else if (Input.GetKey(KeyCode.K)) {
				if (preparing) {
					dancingOnCue = true;
				} else {
					dance ();
				}
			} else {
				playerAnimation.stand ();
			}

			if (Input.GetKey(KeyCode.L)) {
				if (!cooldown) {
					stab();
				}
			}
				
		} else if (gameObject.tag == PLAYER2_TAG) {

			if (Input.GetKey (KeyCode.W)) {
				pos = new Vector3 (transform.position.x, transform.position.y + step, 0);
				direction = 2;
				playerAnimation.moveUp();
			} else if (Input.GetKey (KeyCode.S)) {
				pos = new Vector3 (transform.position.x, transform.position.y - step, 0);
				direction = 0;
				playerAnimation.moveDown();
			} else if (Input.GetKey (KeyCode.D)) {
				pos = new Vector3 (transform.position.x + step, transform.position.y, 0);
				direction = 3;
				playerAnimation.moveRight();
			} else if (Input.GetKey (KeyCode.A)) {
				pos = new Vector3 (transform.position.x - step, transform.position.y, 0);
				direction = 1;
				playerAnimation.moveLeft();
			} else if (Input.GetKey(KeyCode.Tab)) {
				if (preparing) {
					dancingOnCue = true;
				} else {
					dance ();
				}
			} else {
				playerAnimation.stand();
			}

			if (Input.GetKey(KeyCode.Q)) {
				if (cooldown) {
					Debug.Log ("Can't stab yet!");
				} else {
					stab();
				}
			}
		}
			
		transform.position = manager.depthSim(pos);
	}

	Collider2D[] onlyInFront(Collider2D[] colliders) {
		ArrayList newColliders = new ArrayList(); 
		foreach (Collider2D col in colliders) {
			switch (direction) {
			case 0:
				if (col.transform.position.y < selfCollider.transform.position.y) {
					newColliders.Add (col);
				}
				break;
			case 1:
				if (col.transform.position.x < selfCollider.transform.position.x) {
					newColliders.Add (col);
				}
				break;
			case 2:
				if (col.transform.position.y > selfCollider.transform.position.y) {
					newColliders.Add (col);
				}
				break;
			case 3:
				if (col.transform.position.x > selfCollider.transform.position.x) {
					newColliders.Add (col);
				}
				break;
			default:
				break;
			}
		}

		Collider2D[] colArray = new Collider2D[newColliders.Count];
		newColliders.CopyTo(colArray);
		return colArray;
	}

	void stab() {

		sounds.missedStabbed ();
		float curStabRadius = manager.stabbingRadius;

//		if (direction == 0) {
//			curStabRadius = manager.stabbingRadius + 0.3f;
//		} else if (direction == 2) {
//			curStabRadius = manager.stabbingRadius - 1.1f;
//		} else {
//			curStabRadius = manager.stabbingRadius;
//		}
			
		var pos = new Vector3 (selfCollider.transform.position.x, selfCollider.transform.position.y - 1.05f, 0);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, curStabRadius);

		colliders = onlyInFront (colliders);

		foreach (Collider2D collider in colliders)
		{
			var agent = collider.gameObject;

			if (agent.name == tag) {
				continue;
			
			} else if (tag == PLAYER1_TAG && agent.name == "Player2") {
				print ("Player 1 WINS!");
				var playerAnimationScript = agent.GetComponent<PlayerAnimation>();
				playerAnimationScript.murdered ();
				manager.endGame ("Player 1");
				return;
			
			} else if (tag == PLAYER2_TAG && agent.name == "Player1") {
				print ("Player 2 WINS!");
				var playerAnimationScript = agent.GetComponent<PlayerAnimation>();
				playerAnimationScript.murdered ();
				manager.endGame ("Player 2");
				return;
			
			} else {
				var agentScript = agent.GetComponent<Agent> ();
				if (agentScript == null) {
					continue;
				} else {
					cooldown = true;
					knifeFeeback.cooldownStart ();
					agentScript.murdered ();
					return;
				}
			}
		}
	}

	void dance() {
		dancing = true;
		playerAnimation.dance();
	}

	void prepareStart() {
		preparing = true;
	}

	void prepareClose() {
		preparing = false;
		if (dancingOnCue) {
			playerAnimation.dance ();
		}
	}

	void apexEnded() {
		if (dancingOnCue) {
			dancingOnCue = false;
			playerAnimation.stopDance ();
		}
	}

	public void winningAnimation() {
		winDanceMove = true;
	}
}
