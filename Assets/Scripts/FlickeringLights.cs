using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour {

	GameObject[] lights;
	float alpha;
	bool flickering;
	bool flickerDown;
	const float flickerStep = 0.126f;

	// Use this for initialization
	void Start () {
		alpha = 1.0f;
		flickerDown = true;
		flickering = false;
		lights = GameObject.FindGameObjectsWithTag ("Lights");
	}

	void OnEnable() {
		MusicPlayer.OnApexPrepare += flicker;
		MusicPlayer.OnApexEnded += stopFlicker;
	}

	void OnDisable() {
		MusicPlayer.OnApexPrepare -= flicker;
		MusicPlayer.OnApexEnded -= stopFlicker;
	}
	
	// Update is called once per frame
	void Update () {
		if (flickering) {
			if (flickerDown && alpha > 0.2f) {
				alpha -= flickerStep;
			} else if (!flickerDown && alpha < 1.0f) {
				alpha += flickerStep;
			} else {
				flickerDown = !flickerDown; 
			}
			dim ();
		} else {
			if (alpha < 1.0f) {
				alpha += flickerStep;
				dim ();
			}
		}
	}

	void flicker() {
		flickering = true;
	}

	void stopFlicker() {
		flickerDown = true;
		flickering = false;
	}

	void dim() {
		foreach (GameObject go in lights) {
			Color color = go.GetComponent<SpriteRenderer> ().color;
			color.a = alpha;
			go.GetComponent<SpriteRenderer> ().color = color;
		}
	}
}
