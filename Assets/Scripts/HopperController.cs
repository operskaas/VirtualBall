using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HopperController : MonoBehaviour {

	Text shotsLeftText;
	public int shotsLeft = 200;
	// Use this for initialization
	void Start () {
		shotsLeftText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		shotsLeftText.text = "Shots Left: " + shotsLeft;
	}

	public void OneLessShot(){
		shotsLeft --;
	}

	public void Reload (){
		shotsLeft += 140;
		if (shotsLeft > 200) {
			shotsLeft = 200;
		}
	}
}
