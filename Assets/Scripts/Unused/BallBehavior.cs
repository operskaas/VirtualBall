using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {
	Rigidbody rb;
	AudioSource aSource;
	public GameObject splatter;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		aSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		print("trigger!");
	}

	void OnCollisionEnter(Collision collision){
		
		//print("collision! with relative velocity: " + collision.relativeVelocity.magnitude);
		if (collision.relativeVelocity.magnitude > 10 && !collision.collider.CompareTag("NoSound")) {
			aSource.Play ();
			Splatter (collision);
		}
	}

	void Splatter(Collision collision){
		//Destroy (gameObject);
		//Instantiate (splatter);
		// print ("hit at" + collision.contacts [0]);
	}
}
