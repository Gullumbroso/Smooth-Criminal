using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	const float INIT_SCALE = 6.0f;

	GameManager manager;

	public Text countdown; 

	int number;

	bool countingDown;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		resetCountdown ();
	}

	void resetCountdown() {
		number = 3;
		countdown.text = number.ToString();
		countingDown = false;
		countdown.rectTransform.localScale = new Vector3(INIT_SCALE, INIT_SCALE, 0);
		countdown.canvasRenderer.SetAlpha (0);
	}
	
	// Update is called once per frame
	void Update () {
		if (countingDown) {
			countdown.canvasRenderer.SetAlpha (1);
			if (countdown.rectTransform.localScale.x > 4.0f) {
				var scale = new Vector3 (
					            countdown.rectTransform.localScale.x - (Time.deltaTime * 20.0f),
					            countdown.rectTransform.localScale.y - (Time.deltaTime * 20.0f),
					            0); 
				countdown.rectTransform.localScale = scale;

			} else if (countdown.rectTransform.localScale.x <= 4.0f && countdown.rectTransform.localScale.x > 2.0f) {
				var scale = new Vector3 (
					            countdown.rectTransform.localScale.x - (Time.deltaTime / 0.4f),
					            countdown.rectTransform.localScale.y - (Time.deltaTime / 0.4f),
					            0);
				countdown.rectTransform.localScale = scale;

			} else if (countdown.rectTransform.localScale.x <= 2.0f && countdown.rectTransform.localScale.x > 0) {
				var scale = new Vector3 (
					            countdown.rectTransform.localScale.x - (Time.deltaTime * 20.0f),
					            countdown.rectTransform.localScale.y - (Time.deltaTime * 20.0f),
					            0); 
				countdown.rectTransform.localScale = scale;

			} else {
				countdown.rectTransform.localScale = new Vector3 (INIT_SCALE, INIT_SCALE, 0);
				number--;
				countdown.text = number.ToString ();
				if (number < 1) {
					finishCountdown ();
				}
			}
		} else {
			countdown.canvasRenderer.SetAlpha (0);
		}
	}

	public void startCountDown() {
		countingDown = true;
	}

	void finishCountdown() {
		resetCountdown ();
		manager.countdownFinished ();
	}
}
