using UnityEngine;
using System.Collections;

public class BallShooter : MonoBehaviour {

	public GameObject ball;
	GameObject ball_instance;
	Vector3 ball_position = new Vector3 (0f, 0f, 0f);
	Vector3 ball_direction;
	Vector3 ballSpin;
	Vector3 transformForwardHolder;
	float spinInfluence = .01f;
	public float initial_velocity = 40f; // is overridden in inspector
	Rigidbody ball_rb;
	HopperController hc;
	MiniGameController mgc;
	ConstantForce constantForce;

	AudioSource audioSource;
	AudioSource noMoreShotsAudioSource;
	public AudioClip noMoreShotsClip;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		hc = FindObjectOfType<HopperController> ();
		noMoreShotsAudioSource = AddAudio (noMoreShotsClip, false, false, 0.07f);
		mgc = FindObjectOfType<MiniGameController> ();
		constantForce = GetComponent<ConstantForce> ();
	}
	
	// Update is called once per frame 

	// THIS WAS USED WHEN FIRST DEVELOPING, USES MOUSE TO SHOOT. NOW THIS SCRIPT ONLY SHOOTS THE BALL
//	void Update () {
//		if (Input.GetKeyDown(KeyCode.Mouse0)) {
//			//print ("mousekey0down");
//			audioSource.Play();
//			ball_instance = Instantiate (ball);
//			ball_instance.transform.localPosition = transform.position;
//
//			ball_rb = ball_instance.GetComponent<Rigidbody> ();
//			ball_rb.velocity = (transform.forward * initial_velocity);
//		}
//	}

	public void ShootBall (){

		if (hc.shotsLeft > 0) {
			if (mgc.gamePhase == 1) {
				mgc.UsedAPaintball ();
			}
			ball_instance = Instantiate (ball);
			ball_instance.transform.localPosition = transform.position;
			ball_instance.transform.localRotation = transform.rotation;
			ball_rb = ball_instance.GetComponent<Rigidbody> ();
			ball_rb.AddForce (transform.forward * initial_velocity * Random.Range(0.985f, 1.015f), ForceMode.VelocityChange);
			ballSpin.Set (Random.value, Random.value, Random.value);
			//transformForwardHolder = transform.forward;
			//Vector3.OrthoNormalize (ref transformForwardHolder, ref ballSpin);
			 //AddForce (ballSpin * spinInfluence, ForceMode.Force);
			ball_instance.GetComponent<ConstantForce>().relativeForce = new Vector3 (Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f)) * spinInfluence;
			hc.OneLessShot ();
			audioSource.Play ();
		} else {
			noMoreShotsAudioSource.Play ();
		}


	}

	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol){
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		newAudio.spatialBlend = 1.0f;
		return newAudio;
	}


}
