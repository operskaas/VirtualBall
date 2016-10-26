using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartEndDisplay : MonoBehaviour {


	MiniGameController mgc;
	Text displayText;
	Vector3 defaultSize = new Vector3 (0.01381144f, 0.01381144f, 0.01381144f);
	RectTransform parentCanvasRectTransform;
	// Use this for initialization
	void Start () {
		mgc = FindObjectOfType<MiniGameController> ();
		displayText = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mgc.gamePhase == 0) {
			transform.localScale = defaultSize;
		} else if (mgc.gamePhase == 1) {
			transform.localScale = Vector3.zero;
		} else if (mgc.gamePhase == 2) {
			transform.localScale = defaultSize;
			displayText.text = "Time is up!\nYour score was: " + mgc.score + "\nYou hit " + mgc.targetsHit + "/" 
				+ mgc.targetsSpawned + " targets\nand used " + mgc.paintballsUsed + " paintballs\nThe local high score is: " 
				+ mgc.localHighScore + "\nSqueeze the grip buttons to try again!" ;
		}

	
	}
}
