using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeFeedback : MonoBehaviour {

	GameManager manager;

	SpriteRenderer spriteRnederer;

	bool cooldown = false;

	private int cooldownBoolAnimParamId;

	private Animator animator;

	[Header("Animation")]	
	[SerializeField] string cooldownBoolAnimParamName;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		spriteRnederer = GetComponent<SpriteRenderer> ();
			cooldownBoolAnimParamId = Animator.StringToHash(cooldownBoolAnimParamName);
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (manager.playing) {
			spriteRnederer.color = new Color (1, 1, 1, 1);
		} else {
			spriteRnederer.color = new Color (1, 1, 1, 0);
		}
		animator.SetBool(cooldownBoolAnimParamId, cooldown);
	}

	public void cooldownStart() {
		cooldown = true;
	}

	public void cooldownEnd() {
		cooldown = false;
	}
}
