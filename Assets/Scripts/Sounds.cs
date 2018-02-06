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
}
