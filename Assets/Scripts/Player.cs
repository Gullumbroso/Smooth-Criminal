using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string PLAYER1_TAG = "Player1";
	private string PLAYER2_TAG = "Player2";

	public GameManager manager;
	private Sounds sounds;
	private CircleCollider2D selfCollider;
	public PlayerAnimation playerAnimation;
	GameObject[] markers;
	private int direction = 0; // 0-down, 1-left, 2-up, 3-right
	private float velUnit;
	private float cooldownTimer;
	private bool cooldown = false;
	private float dancingTimer;
	private bool dancing = false;
	private bool dancingOnCue = false;
	private bool preparing = false;


	void OnEnable() {
		MusicPlayer.OnApexPrepare += prepareStart;
		MusicPlayer.OnApexStarted += prepareClose;
		MusicPlayer.OnApexEnded += apexEnded;

	}

	void OnDisable() {
		MusicPlayer.OnApexPrepare -= prepareStart;
		MusicPlayer.OnApexStarted -= prepareClose;
		MusicPlayer.OnApexEnded += apexEnded;
	}

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		sounds = GameObject.Find ("Sounds").GetComponent<Sounds> ();
		selfCollider = GetComponent<CircleCollider2D> ();
		playerAnimation = GetComponent<PlayerAnimation> ();
		velUnit = manager.velUnit;
		cooldownTimer = manager.postMurderCooldown;
		placeInScreen ();

		if (manager.debugMode) {
			drawMarkers ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (manager.playing) {

			if (!dancing) {
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
					cooldownTimer = manager.postMurderCooldown;
				}
			}
		}

		if (manager.debugMode) {
			positionMarkers ();
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
			
		print (curStabRadius);

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

	void drawMarkers() {

		markers[0] = Instantiate(manager.markers[0], selfCollider.transform.position, Quaternion.identity);
		markers[1] = Instantiate(manager.markers[1], selfCollider.transform.position, Quaternion.identity);
	}

	void positionMarkers() {
		var pos = selfCollider.transform.position;

		markers [0].transform.position = pos;
		markers [1].transform.position = pos;
	}
}
