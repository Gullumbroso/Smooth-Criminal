using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private const int HOUSE_1_IDX = 0;
	private const int HOUSE_2_IDX = 1;
	private const int CHORUS_1_IDX = 2;
	private const int CHORUS_2_IDX = 3;
	private const int CHORUS_3_IDX = 4;
	private const int APEX_IDX = 5;

	public delegate void Apex ();
	public static event Apex OnApexPrepare;
	public static event Apex OnApexStarted;
	public static event Apex OnApexEnded;

	public AudioSource[] verses;

	int[] versesCount;
	int counter = 0;
	int curVerseIdx;
	int nextVerseIdx;
	int level = 0;
	int apexCount = 0;

	float prepareTime = 1.2f;

	bool apex;

	float verseTimer;


	// Use this for initialization
	void Start () {
		versesCount = new int[verses.Length];
		apex = false;
		startMusic ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!apex && nextVerseIdx == APEX_IDX && verseTimer < prepareTime) {
			prepareForApex ();
		}

		if (nextVerseIdx == APEX_IDX && verseTimer < 0.335f) {
			nextVerse ();
		} if (verseTimer < 0.117f) {
			nextVerse ();
		} else {
			verseTimer -= Time.deltaTime;
		}
	}

	void startMusic() {
		curVerseIdx = 0;
		nextVerseIdx = 0;
		AudioSource house1 = verses [curVerseIdx];
		verseTimer = house1.clip.length;
		house1.Play ();
	}

	void nextVerse () {

		// If played apex - notify that it's over
		if (curVerseIdx == APEX_IDX) {
			apex = false;
			if (OnApexEnded != null) {
				OnApexEnded ();
			} else {
				Debug.Log ("OnApexEnded is null!");
			}
		}

		// Play current verse. If apex - notify that it's started
		curVerseIdx = nextVerseIdx;
		if (curVerseIdx == APEX_IDX) {
			if (OnApexStarted != null) {
				OnApexStarted ();
			} else {
				Debug.Log ("OnApexStarted is null!");
			}
		}
		verses [curVerseIdx].Play ();
		verseTimer = verses [curVerseIdx].clip.length;

		// Prepare the next verse
		nextVerseIdx = selectNext ();
  	}

	int selectNext() {

		counter++;

		switch (level) {

		case 0:
			while (versesCount [0] < 2) {
				versesCount [0]++;
				return 0;
			}
			while (versesCount [1] < 4) {
				versesCount [1]++;
				return 1;
			}
			while (versesCount [2] < 3) {
				versesCount [2]++;
				return 2;
			}
			while (versesCount [3] < 1) {
				versesCount [3]++;
				return 3;
			}
			while (versesCount [4] < 1) {
				versesCount [4]++;
				return 4;
			}
			while (versesCount [2] < 4) {
				versesCount [2]++;
				return 2;
			}
			print ("Reached the apex!");
			level++;
			return 5;

		case 1:
			if (curVerseIdx != APEX_IDX && counter % 2 == 0) {
				return APEX_IDX;
			} else if (curVerseIdx == APEX_IDX) {
				counter = 0;
				return HOUSE_1_IDX;
			} else {
				return (curVerseIdx + 1) % verses.Length;
			} 
		
		default:
			return HOUSE_2_IDX;
		}
	}

	void prepareForApex() {
		apex = true;
		if (OnApexPrepare != null) {
			OnApexPrepare ();
		} else {
			Debug.Log ("OnApexPrepare is null!");
		}
	}
}
