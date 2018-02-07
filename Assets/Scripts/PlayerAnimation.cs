using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

	private bool standing;
	private bool movingUp;
	private bool movingDown;
	private bool movingRight;
	private bool movingLeft;
	private bool alive;
	private bool dancing;

	private int standingBoolAnimParamId;
	private int walkingUpBoolAnimParamId;
	private int walkingDownBoolAnimParamId;
	private int walkingRightBoolAnimParamId;
	private int walkingLeftBoolAnimParamId;
	private int aliveBoolAnimParamId;
	private int dancingBoolAnimParamId;

	private Animator animator;

	[Header("Animation")]	
	[SerializeField] string standingBoolAnimParamName;
	[SerializeField] string walkingUpBoolAnimParamName;
	[SerializeField] string walkingDownBoolAnimParamName;
	[SerializeField] string walkingRightBoolAnimParamName;
	[SerializeField] string walkingLeftBoolAnimParamName;
	[SerializeField] string aliveBoolAnimParamName;
	[SerializeField] string dancingBoolAnimParamName;


	// Use this for initialization
	void Start () {
		alive = true;
		movingDown = true;
		dancing = false;

		animator = GetComponent<Animator> ();
		standingBoolAnimParamId = Animator.StringToHash(standingBoolAnimParamName);
		walkingUpBoolAnimParamId = Animator.StringToHash(walkingUpBoolAnimParamName);
		walkingDownBoolAnimParamId = Animator.StringToHash(walkingDownBoolAnimParamName);
		walkingRightBoolAnimParamId = Animator.StringToHash(walkingRightBoolAnimParamName);
		walkingLeftBoolAnimParamId = Animator.StringToHash(walkingLeftBoolAnimParamName);
		aliveBoolAnimParamId = Animator.StringToHash(aliveBoolAnimParamName);
		dancingBoolAnimParamId = Animator.StringToHash(dancingBoolAnimParamName);
	}
	
	// Update is called once per frame
	void Update () {

		animator.SetBool(standingBoolAnimParamId, standing);
		animator.SetBool(walkingUpBoolAnimParamId, movingUp);
		animator.SetBool(walkingDownBoolAnimParamId, movingDown);
		animator.SetBool(walkingRightBoolAnimParamId, movingRight);
		animator.SetBool(walkingLeftBoolAnimParamId, movingLeft);
		animator.SetBool(aliveBoolAnimParamId, alive);
		animator.SetBool(dancingBoolAnimParamId, dancing);
	}

	public void logStatus() {
		if (movingUp)
			Debug.Log ("UP");
		if (movingDown)
			Debug.Log ("DOWN");
		if (movingRight)
			Debug.Log ("RIGHT");
		if (movingLeft)
			Debug.Log ("LEFT");
		if (standing)
			Debug.Log ("STAND");
	}

	public void setAlive() {
		alive = true;
	}

	public void moveUp() {
		if (!movingUp) {

			movingUp = true;
			movingDown = false;
			movingRight = false;
			movingLeft = false;
			standing = false;
		}
	}

	public void moveDown() {
		if (!movingDown) {
			movingUp = false;
			movingDown = true;
			movingRight = false;
			movingLeft = false;
			standing = false;
		}
	}

	public void moveRight() {
		if (!movingRight) {
			movingUp = false;
			movingDown = false;
			movingRight = true;
			movingLeft = false;
			standing = false;
		}
	}

	public void moveLeft() {
		if (!movingLeft) {
			movingUp = false;
			movingDown = false;
			movingRight = false;
			movingLeft = true;
			standing = false;
		}
	}

	public void stand() {
		if (!standing) {
			movingUp = false;
			movingDown = false;
			movingRight = false;
			movingLeft = false;
			standing = true;
		}
	}

	public void murdered() {
		movingUp = false;
		movingDown = false;
		movingRight = false;
		movingLeft = false;
		standing = false;
		dancing = false;
		alive = false;
	}

	public void dance() {
		if (alive) {
			dancing = true;
			movingUp = false;
			movingDown = false;
			movingRight = false;
			movingLeft = false;
			standing = false;
		}
	}

	public void stopDance() {
		if (alive) {
			dancing = false;
			movingUp = false;
			movingDown = false;
			movingRight = false;
			movingLeft = false;
			standing = true;
		}
	}
}
