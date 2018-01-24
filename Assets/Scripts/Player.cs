using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string PLAYER1_TAG = "Player1";
	private string PLAYER2_TAG = "Player2";

	public GameManager manager;
	public PlayerAnimation playerAnimation;
	private float velUnit;
	private float stabbingRadius;
	private float cooldownTimer;
	private bool cooldown = false;

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		playerAnimation = GetComponent<PlayerAnimation> ();
		velUnit = manager.velUnit;
		stabbingRadius = manager.stabbingRadius;
		cooldownTimer = manager.postMurderCooldown;
		placeInScreen ();
	}
	
	// Update is called once per frame
	void Update () {

		if (manager.playing) {

			getInput ();

			if (cooldown) {
				cooldownTimer -= Time.deltaTime;
				if (cooldownTimer <= 0) {
					// reload the stabbing ability
					cooldown = false;
					cooldownTimer = manager.postMurderCooldown;
				}
			}
		}
	}

	public void placeInScreen() {
		transform.position = new Vector3 (Random.Range (-7, 7), Random.Range (-7, 7), Random.Range (-7, 7));
	}

	private void getInput() {

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, 0);
		if (gameObject.tag == PLAYER1_TAG) {
			
			if (Input.GetKey (KeyCode.UpArrow)) {
				pos = new Vector3 (transform.position.x, transform.position.y + velUnit, 0);
				playerAnimation.moveUp ();
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				pos = new Vector3 (transform.position.x, transform.position.y - velUnit, 0);
				playerAnimation.moveDown ();
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				pos = new Vector3 (transform.position.x + velUnit, transform.position.y, 0);
				playerAnimation.moveRight ();
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				pos = new Vector3 (transform.position.x - velUnit, transform.position.y, 0);
				playerAnimation.moveLeft ();
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
				pos = new Vector3 (transform.position.x, transform.position.y + velUnit, 0);
				playerAnimation.moveUp();
			} else if (Input.GetKey (KeyCode.S)) {
				pos = new Vector3 (transform.position.x, transform.position.y - velUnit, 0);
				playerAnimation.moveDown();
			} else if (Input.GetKey (KeyCode.D)) {
				pos = new Vector3 (transform.position.x + velUnit, transform.position.y, 0);
				playerAnimation.moveRight();
			} else if (Input.GetKey (KeyCode.A)) {
				pos = new Vector3 (transform.position.x - velUnit, transform.position.y, 0);
				playerAnimation.moveLeft();
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

	void stab() {

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stabbingRadius);

		foreach (Collider2D collider in colliders)
		{
			var agent = collider.gameObject;

			if (agent.name == PLAYER1_TAG) {
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
					Debug.Log ("agentScript is null! Agent type: " + agent.GetType());
					return;
				} else {
					cooldown = true;
					agentScript.murdered ();
				}
			}
		}
	}
}
