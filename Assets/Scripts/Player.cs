using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string PLAYER1_TAG = "Player1";
	private string PLAYER2_TAG = "Player2";

	public GameManager manager;
	private float velUnit;
	private float stabbingRadius;

	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		velUnit = manager.velUnit;
		stabbingRadius = manager.stabbingRadius;
		transform.position = new Vector3 (Random.Range (-4, 4), Random.Range (-4, 4), Random.Range (-4, 4));

	}
	
	// Update is called once per frame
	void Update () {
		getInput ();
	}

	private void getInput() {

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, 0);
		if (gameObject.tag == PLAYER1_TAG) {
			
			if (Input.GetKey (KeyCode.UpArrow)) {
				pos = new Vector3(transform.position.x, transform.position.y + velUnit, 0);
			}

			if (Input.GetKey(KeyCode.DownArrow)) {
				pos = new Vector3(transform.position.x, transform.position.y - velUnit, 0);
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				pos = new Vector3(transform.position.x + velUnit, transform.position.y, 0);
			}

			if (Input.GetKey(KeyCode.LeftArrow)) {
				pos = new Vector3(transform.position.x - velUnit, transform.position.y, 0);
			}

			if (Input.GetKey(KeyCode.L)) {
				stab();
			}

		} else if (gameObject.tag == PLAYER2_TAG) {

			if (Input.GetKey (KeyCode.W)) {
				pos = new Vector3(transform.position.x, transform.position.y + velUnit, 0);
			}

			if (Input.GetKey(KeyCode.S)) {
				pos = new Vector3(transform.position.x, transform.position.y - velUnit, 0);
			}

			if (Input.GetKey (KeyCode.D)) {
				pos = new Vector3(transform.position.x + velUnit, transform.position.y, 0);
			}

			if (Input.GetKey(KeyCode.A)) {
				pos = new Vector3(transform.position.x - velUnit, transform.position.y, 0);
			}

			if (Input.GetKey(KeyCode.Q)) {
				stab();
			}
		}

		transform.position = pos;
	}

	void stab() {
		Debug.Log ("Stab!");
		Debug.Log (stabbingRadius);

		Collider2D collider = Physics2D.OverlapCircle(transform.position, stabbingRadius);
		int i = 0;
		if (collider != null)
		{
			var agent = collider.gameObject;
			Debug.Log (agent.name);
			if (tag == PLAYER1_TAG && agent.name == "Player2") {
				Debug.Log ("Player 1 WINS!");
				Time.timeScale = 0;
				return;
			} else if (tag == PLAYER2_TAG && agent.name == "Player1") {
				Debug.Log ("Player 2 WINS!");
				Time.timeScale = 0;
				return;
			} else {
				if (tag == PLAYER1_TAG) {
					Debug.Log ("Player 1 Lose!");
				} else {
					Debug.Log ("Player 2 Lose!");
				}
				Time.timeScale = 0;
				return;
			}
		}
	}
}
