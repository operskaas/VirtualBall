using UnityEngine;
using System.Collections;

public class SplatterController : MonoBehaviour {

	AudioSource aSource;
	float timeAlive = 10.0f;
	//ParticleEmitter splatterParticleEmitter;

	// Use this for initialization
	void Start () {
		aSource = GetComponent<AudioSource> ();
		aSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		timeAlive -= Time.deltaTime;
		if (timeAlive <= 0) {
			Destroy (gameObject);
		}
		
	}
}
