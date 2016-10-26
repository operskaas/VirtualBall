using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour {

	public GameObject explosionPrefab;
	GameObject explosionInstance;
	MiniGameController mgc;
	float timeAlive;

	// Use this for initialization
	void Start () {
		mgc = FindObjectOfType<MiniGameController> ();
		mgc.TargetSpawned ();
		timeAlive = mgc.resetTimeBetweenTargets;
	}
	
	// Update is called once per frame
	void Update () {
		timeAlive -= Time.deltaTime;
		if (timeAlive < 0) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision){
		//Debug.Log ("target collision with " + collision.collider.name);
		if (collision.collider.name == "Paintball(Clone)") {
			mgc.TargetHit ();
			explosionInstance = Instantiate (explosionPrefab);
			explosionInstance.transform.position = transform.position;
			Destroy (gameObject);
		}

	}
}
