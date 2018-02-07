using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour {

	GameObject[] lights;
	GameObject[] shades;
	float alpha;
	bool flickering;
	bool flickerDown;
	const float flickerStep = 4.18f;

	// Use this for initialization
	void Start () {
		alpha = 1.0f;
		flickerDown = true;
		flickering = false;
		lights = GameObject.FindGameObjectsWithTag ("Lights");
		shades = GameObject.FindGameObjectsWithTag ("Shades");
	}

	void OnEnable() {
		MusicPlayer.OnApexPrepare += flicker;
		MusicPlayer.OnApexEnded += stopFlicker;
	}

	void OnDisable() {
		MusicPlayer.OnApexPrepare -= flicker;
		MusicPlayer.OnApexEnded -= stopFlicker;
	}
	
	void Update () {

//		foreach (GameObject go in lights) {
//			Color color = go.GetComponent<SpriteRenderer> ().color;
//			color.r = 0.5f;
//			go.GetComponent<SpriteRenderer> ().color = color;
//		}

		float step = flickerStep * Time.deltaTime;
		if (flickering) {
			if (flickerDown && alpha > 0.2f) {
				alpha -= step;
			} else if (!flickerDown && alpha < 1.0f) {
				alpha += step;
			} else {
				flickerDown = !flickerDown; 
			}
			dim ();
		} else {
			if (alpha < 1.0f) {
				alpha += step;
				dim ();
			}
		}
	}

	void flicker() {
		flickering = true;
	}

	void stopFlicker() {
		flickerDown = false;
		flickering = false;
	}

	void dim() {
		foreach (GameObject go in lights) {
			Color color = go.GetComponent<SpriteRenderer> ().color;
			color.a = Mathf.Clamp01(alpha);
			go.GetComponent<SpriteRenderer> ().color = color;
		}
		foreach (GameObject go2 in shades) {
			Color color = go2.GetComponent<SpriteRenderer> ().color;
			float val = (alpha + 1.0f) / 2.0f * 1.1f;
			color.a = Mathf.Clamp01(val);
			go2.GetComponent<SpriteRenderer> ().color = color;
		}
	}
}
