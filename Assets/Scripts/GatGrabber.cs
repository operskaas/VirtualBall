using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SteamVR_TrackedObject))]

public class GatGrabber : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;
	Vector3 localGatPosition = new Vector3(0f, 0.2f, -0.24f);
	Vector3 localGatEulerRotation = new Vector3(63f, 0f, 0f);
	BallShooter ballShooter;

	Vector2 touchpadInput = Vector2.zero;
	Vector3 translationVector = Vector3.zero;
	float movementSpeed = 0.05f;
	Transform playerTransform;
	Transform mainCameraTransform;
	MiniGameController mgc;
	bool gunAttached;

	// Changed from Start to Awake to make sure it runs whether it's enabled or not 
	// so that we don't get null pointer error
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		mainCameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		mgc = FindObjectOfType<MiniGameController> ();
	}
	
	// Changed from Update to FixedUpdate because dealing with physics. 
	//Update is rendered every frame, want to use to get input events, then "record event with flag", and use 
	//FixedUpdate to respond to the input. 
	//If have lots of physics may want to leave fixed timestp at 1/50 or less and use interpolation to strain cpu less

	void Update() {

		//Getting touchpad input for movement
		if (device.GetTouch (SteamVR_Controller.ButtonMask.Touchpad)) {
			touchpadInput = device.GetAxis ();
		} else {
			touchpadInput = Vector2.zero;
		}

		//Getting trigger and grip input for gun control
		if (gunAttached) {
			if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
				ballShooter.ShootBall ();
				RumbleController (0.012f, 800f);
				if (mgc.gamePhase == 0) {
					mgc.gamePhase = 1;
				}
			}
			if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
				ballShooter.ShootBall ();
				RumbleController (0.012f, 800f);
			}
		}



	}
	void FixedUpdate () {

		//Moving around
		device = SteamVR_Controller.Input ((int)trackedObj.index);
		translationVector = touchpadInput.y * mainCameraTransform.forward + touchpadInput.x * mainCameraTransform.right;
		translationVector.y = 0f;
		//translationVector.Set (touchpadInput.x, 0f, touchpadInput.y);
		translationVector.Normalize ();
		//Debug.Log (translationVector);
		playerTransform.Translate (translationVector * movementSpeed);



	}

	void OnTriggerStay(Collider collider){
		if (collider.name == "Gat") {
			//Debug.Log ("triggered with with gatttttt");
			if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Grip)) {

				if (!gunAttached) {
					//Debug.Log ("you have pushed down the grip while triggered");
					gunAttached = true;
					RumbleController (0.03f, 700f);
					collider.gameObject.transform.SetParent (gameObject.transform);
					collider.gameObject.transform.localPosition = localGatPosition;
					collider.gameObject.transform.localEulerAngles = localGatEulerRotation;
					collider.attachedRigidbody.isKinematic = true;

					ballShooter = GetComponentInChildren<BallShooter> ();
				}

				if (mgc.gamePhase == 2) {
					mgc.ResetMiniGame ();
				}
			}

		}

	}

	void RumbleController (float duration, float strength){
		StartCoroutine (RumbleControllerRoutine (duration, strength));
	}

	IEnumerator RumbleControllerRoutine( float duration, float strength){
		device = SteamVR_Controller.Input ((int)trackedObj.index);
		//strength = Mathf.Clamp01 (strength);
		float startTime = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup - startTime <= duration) {
			device.TriggerHapticPulse ((ushort)strength);
			yield return null;
		}
	}
}
