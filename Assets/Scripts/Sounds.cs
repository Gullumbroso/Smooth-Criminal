using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public AudioSource[] sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void stabbed() {
		sounds [0].Play ();
	}

	public void missedStabbed() {
		sounds [1].Play ();
	}

	public void auw() {
		if (!sounds [2].isPlaying) {
			sounds [2].Play ();
		}
	}

	public void auwPlural() {
		if (!sounds [3].isPlaying) {
			sounds [3].Play ();
		}
	}

	public void drum() {
		if (!sounds [4].isPlaying) {
			sounds [4].Play ();
		}
	}

	public void apex() {
		if (!sounds [5].isPlaying) {
			sounds [5].Play ();
		}
	}

	public void winningDrums() {
		if (!sounds [6].isPlaying) {
			sounds [6].Play ();
		}
	}

	public bool isAuwPlaying() {
		return sounds [2].isPlaying;
	}
}
