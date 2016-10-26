using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

	public GameObject target;
	public int score = 0;
	public int localHighScore;
	public int targetsHit = 0;
	public int targetsSpawned = 0;
	public int paintballsUsed = 0;
	public int gamePhase = 0;
	float timeBetweenTargets;
	public float resetTimeBetweenTargets;
	float miniGameLength;
	float resetMiniGameLength = 45f;
	int numTargetsPerGame = 30;
	Vector3[] positions = new Vector3[9];


	// Use this for initialization
	void Start () {
		positions[0] = new Vector3(-6.053f, 1.21f, 20.83f);
		positions[1] = new Vector3(6.053f, 1.21f, 20.83f);
		positions[2] = new Vector3(0.95f, 1.383f, 27.542f);
		positions[3] = new Vector3(-0.95f, 1.383f, 27.542f);
		positions[4] = new Vector3(-4.627f, 1.72f, 30.361f);
		positions[5] = new Vector3(4.627f, 1.72f, 30.361f);
		positions[6] = new Vector3(8.3f, 1.8f, 33.17f);
		positions[7] = new Vector3(-8.3f, 1.8f, 33.17f);
		positions[8] = new Vector3(0f, 1.551f, 27.259f);

		resetTimeBetweenTargets = resetMiniGameLength / numTargetsPerGame;
		timeBetweenTargets = resetTimeBetweenTargets;
		miniGameLength = resetMiniGameLength;
	}
	
	// Update is called once per frame
	void Update () {

		if (gamePhase == 1) {
			timeBetweenTargets -= Time.deltaTime;
			if (timeBetweenTargets < 0) {
				timeBetweenTargets = resetTimeBetweenTargets;

				Instantiate (target);
				target.transform.position = positions [Random.Range (0, positions.Length)];
			}

			miniGameLength -= Time.deltaTime;
			if (miniGameLength < 0) {
				gamePhase = 2;
				StoreLocalHighScore (score);
			}
		}
	}

	public void IncrementScore(int increment){
		score += increment;
	}

	public void ResetMiniGame(){
		miniGameLength = resetMiniGameLength;
		score = 0;
		gamePhase = 1;
		targetsHit = 0;
		targetsSpawned = 0;
		paintballsUsed = 0;
	}

	public void UsedAPaintball(){
		paintballsUsed++;
	}

	public void TargetHit(){
		targetsHit++;
		score += 100;
	}

	public void TargetSpawned(){
		targetsSpawned++;
	}

	void StoreLocalHighScore (int newHighScore){
		int oldHighScore = PlayerPrefs.GetInt ("localhighscore", 0);
		if (newHighScore > oldHighScore) {
			PlayerPrefs.SetInt ("localhighscore", newHighScore);
		}
		localHighScore = PlayerPrefs.GetInt ("localhighscore", 0);
	}
}
