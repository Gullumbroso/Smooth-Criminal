using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private const int VERSE_1_IDX = 0;
	private const int VERSE_2_IDX = 1;
	private const int CHORUS_1_IDX = 2;
	private const int CHORUS_2_IDX = 3;
	private const int CHORUS_3_IDX = 4;
	private const int APEX_IDX = 5;
	private const float VERSE_1_VERSE_2_DELAY = 0.65f;

	public delegate void Apex ();
	public static event Apex OnApexPrepare;
	public static event Apex OnApexStarted;
	public static event Apex OnApexEnded;

	GameManager manager;
	Sounds sounds;

	public AudioSource[] verses;


	int[] versesCount;
	int curVerseIdx;
	int nextVerseIdx;
	int counter = 0;
	int level = 0;
	int didApexCounter;

	float prepareTime = 1.45f;

	int lastLevelRandomFactor = 60;

	bool stopped;
	bool playedFirstVerse;
	bool apex;

	bool debugMode = false;

	float verseTimer;

	void OnEnable() {
		GameManager.OnRestart += resetMusicPlayer;
	}

	void OnDisable() {
		GameManager.OnRestart -= resetMusicPlayer;
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		sounds = GameObject.Find ("Sounds").GetComponent<Sounds> ();
		versesCount = new int[verses.Length];
		playedFirstVerse = false;
		apex = false;
		stopped = false;
		resetCounters ();
		startMusic ();
		if (debugMode) {
			level = 3;
			lastLevelRandomFactor = 85;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (manager.playing) {
			if (!apex && nextVerseIdx == APEX_IDX && verseTimer < prepareTime) {
				prepareForApex ();
			}

			if (!playedFirstVerse && !sounds.isAuwPlaying ()) {
				verses [curVerseIdx].Play ();
				playedFirstVerse = true;
			}

			if (verseTimer < 0.117f) {
				nextVerse ();
			} else {
				verseTimer -= Time.deltaTime;
			}
		
		} else if (!manager.opening && !manager.starting) {
			if (!stopped) {
				stopMusic ();
			}
		}
	}

	void startMusic() {
		curVerseIdx = selectNext();
		nextVerseIdx = selectNext();
		if (curVerseIdx == VERSE_1_IDX && nextVerseIdx == VERSE_2_IDX) {
			// Add a tiny fraction of delay for a perefect transition between verse 1 and verse 2.
			verseTimer = verses [curVerseIdx].clip.length + VERSE_1_VERSE_2_DELAY;
		}
	}

	void resetMusicPlayer() {
		Debug.Log ("Reseting Music Player!");
		playedFirstVerse = false;
		level = 0;
		apex = false;
		stopped = false;
		resetCounters ();
		startMusic ();
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

		// Prepare the next verse
		nextVerseIdx = selectNext ();
		verseTimer = verses [curVerseIdx].clip.length;

//		if (curVerseIdx == VERSE_1_IDX && nextVerseIdx == VERSE_2_IDX) {
//			// Add a tiny fraction of delay for a perefect transition between verse 1 and verse 2.
//			verseTimer = verses [curVerseIdx].clip.length + VERSE_1_VERSE_2_DELAY;
//		} else {
//			verseTimer = verses [curVerseIdx].clip.length;
//		}
  	}

	int selectNext() {

		counter++;

		switch (level) {

		case 0:
			print ("Level " + level);
			while (versesCount [VERSE_1_IDX] < 1) {
				versesCount [VERSE_1_IDX]++;
				return VERSE_1_IDX;
			}
			while (versesCount [VERSE_2_IDX] < 1) {
				versesCount [VERSE_2_IDX]++;
				return VERSE_2_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 1) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			while (versesCount [CHORUS_2_IDX] < 1) {
				versesCount [CHORUS_2_IDX]++;
				return CHORUS_2_IDX;
			}
			while (versesCount [CHORUS_3_IDX] < 1) {
				versesCount [CHORUS_3_IDX]++;
				return CHORUS_3_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 2) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			level++;
			resetCounters ();
			return APEX_IDX;

		case 1:
			print ("Level " + level);
			while (versesCount [VERSE_1_IDX] < 1) {
				versesCount [VERSE_1_IDX]++;
				return VERSE_1_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 1) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			while (versesCount [CHORUS_2_IDX] < 1) {
				versesCount [CHORUS_2_IDX]++;
				return CHORUS_2_IDX;
			}
			while (versesCount [CHORUS_3_IDX] < 1) {
				versesCount [CHORUS_3_IDX]++;
				return CHORUS_3_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 2) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			level++;
			resetCounters ();
			return APEX_IDX;

		case 2:
			print ("Level " + level);
				while (versesCount [VERSE_1_IDX] < 1) {
				versesCount [VERSE_1_IDX]++;
				return VERSE_1_IDX;
			}
			if (didApexCounter < 1) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto > 40) {
					return APEX_IDX;
				}
			} 
			while (versesCount [VERSE_2_IDX] < 1) {
				versesCount [VERSE_2_IDX]++;
				return VERSE_2_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 1) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			if (didApexCounter < 2) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto > 40) {
					return APEX_IDX;
				}
			}
			while (versesCount [CHORUS_2_IDX] < 1) {
				versesCount [CHORUS_2_IDX]++;
				return CHORUS_2_IDX;
			}
			while (versesCount [CHORUS_3_IDX] < 1) {
				versesCount [CHORUS_3_IDX]++;
				return CHORUS_3_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 2) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			if (didApexCounter < 3) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto > 40) {
					level++;
					resetCounters ();
					return APEX_IDX;
				}
			}
			while (versesCount [CHORUS_1_IDX] < 3) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			level++;
			resetCounters ();
			return APEX_IDX;

		default:
			print ("Level " + level);
				while (versesCount [VERSE_1_IDX] < 1) {
				versesCount [VERSE_1_IDX]++;
				return VERSE_1_IDX;
			}
			if (didApexCounter < 1) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto < lastLevelRandomFactor) {
					return APEX_IDX;
				}
			}
			while (versesCount [VERSE_2_IDX] < 1) {
				versesCount [VERSE_2_IDX]++;
				return VERSE_2_IDX;
			}
			if (didApexCounter < 2) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto < lastLevelRandomFactor) {
					return APEX_IDX;
				}
			}
			while (versesCount [CHORUS_1_IDX] < 1) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			if (didApexCounter < 3) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto < lastLevelRandomFactor) {
					return APEX_IDX;
				}
			}
			while (versesCount [CHORUS_2_IDX] < 1) {
				versesCount [CHORUS_2_IDX]++;
				return CHORUS_2_IDX;
			}
			while (versesCount [CHORUS_3_IDX] < 1) {
				versesCount [CHORUS_3_IDX]++;
				return CHORUS_3_IDX;
			}
			while (versesCount [CHORUS_1_IDX] < 2) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			if (didApexCounter < 4) {
				didApexCounter++;
				int lotto = Random.Range (0, 100);
				if (lotto < lastLevelRandomFactor) {
					return APEX_IDX;
				}
			}
			while (versesCount [CHORUS_1_IDX] < 3) {
				versesCount [CHORUS_1_IDX]++;
				return CHORUS_1_IDX;
			}
			level++;
			resetCounters ();
			return APEX_IDX;
		}
	}

	void resetCounters() {
		didApexCounter = 0;
		for (int i = 0; i < versesCount.Length; i++) {
			versesCount [i] = 0;
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

	public void stopMusic() {
		if (!stopped) {
			stopped = true;
			verses [curVerseIdx].Stop ();
			verses [6].Play ();
		}
	}
}
