using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWins : MonoBehaviour {

	GameManager manager;

	SpriteRenderer spriteRnederer;

	bool winIn = false;
	bool win = false;
	float alpha = 0;
	float appearanceStep = 3.0f;

	private Animator animator;

	private int winnerBoolAnimParamId;

	[Header("Animation")]	
	[SerializeField] string winnerBoolAnimParamName;

	[SerializeField]
	public float presentationTimer = 4.0f;


	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		spriteRnederer = GetComponent<SpriteRenderer> ();
		winnerBoolAnimParamId = Animator.StringToHash(winnerBoolAnimParamName);
		animator = GetComponent<Animator> ();
		reset ();
	}

	public void reset() {
		win = false;
		winIn = false;
		presentationTimer = 4.0f;
		alpha = 0;
		spriteRnederer.color = new Color (1, 1, 1, alpha);
	}
	
	// Update is called once per frame
	void Update () {

		animator.SetBool(winnerBoolAnimParamId, win);

		if (winIn) {
			if (alpha < 1.0f) {
				alpha += appearanceStep * Time.deltaTime;
				spriteRnederer.color = new Color (1, 1, 1, alpha);
			} else {
				win = true;
				presentationTimer -= Time.deltaTime;
				if (presentationTimer <= 0) {
					manager.restart ();
				}
			}
		}
	}

	public void setWin() {
		winIn = true;
	}
}
