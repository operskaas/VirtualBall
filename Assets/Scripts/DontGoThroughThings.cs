using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour {
	//I HAVE SEPARATED CODE I CUT FROM dontgothroughthings CODE FROM MY STUFF WITH =====

	// Careful when setting this to true - it might cause double
	// events to be fired - but it won't pass through the trigger
	public bool sendTriggerMessage = false; 	

	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 

	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody myRigidbody;
	private Collider myCollider;
	//===================================================================
	private AudioSource aSource;
	float collisionAngle;
	int numCollisions;
	int minSplatterAngle = 110;
	public GameObject splatter;
	GameObject splatter_instance;
	float timeAlive = 4.0f;
	private Vector3 velocityAtCollision;
	private Vector3 previousVelocity;

	//initialize values 
	void Start() 
	{ 
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		aSource = GetComponent<AudioSource> ();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 
		numCollisions = 0;
	} 

	void Update(){
		timeAlive -= Time.deltaTime;
		if (timeAlive <= 0) {
			Destroy (gameObject);
		}
	}

	void FixedUpdate() 
	{ 
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = myRigidbody.position - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;

		if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit hitInfo; 

			//check for obstructions we might have missed 
			if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
			{
				if (!hitInfo.collider)
					return;

				if (hitInfo.collider.isTrigger) {
					hitInfo.collider.SendMessage ("OnTriggerEnter", myCollider);
					//print ("triggerred");
				}

				if (!hitInfo.collider.isTrigger) {
					myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; 
					velocityAtCollision = previousVelocity;
				}

			}
		} 

		previousPosition = myRigidbody.position; 
		previousVelocity = movementThisStep / Time.fixedDeltaTime;
	}

	void OnCollisionEnter(Collision collision){
		
		float actualRelativeVelocity;
		numCollisions++;
		//aSource.Play ();
		if (numCollisions < 3) {
			if (collision.relativeVelocity.magnitude == 0) {
				actualRelativeVelocity = 80;
			} else {
				actualRelativeVelocity = collision.relativeVelocity.magnitude;
			}
			//Debug.Log ("colision with collider " + collision.collider);
			//print("collision! with relative velocity: " + actualRelativeVelocity);
			//Debug.Log ("this balls velocity is: " + velocityAtCollision);
			collisionAngle = Vector3.Angle (velocityAtCollision, collision.contacts [0].normal);
			//print ("angle of collision is: " + collisionAngle);
			if (actualRelativeVelocity > 10 && !collision.collider.CompareTag ("NoSound") && collisionAngle > minSplatterAngle ) {
				Splatter (collision);
				//Debug.Log ("splatter");
			}
		}
	}

	void Splatter(Collision collision){
		//print ("splatter at" + collision.contacts[0].point);
		Destroy (gameObject);

		splatter_instance = Instantiate (splatter);
		splatter_instance.transform.SetParent(collision.collider.GetComponent<Transform>()); //TODO known bug, splatter matches scale of parent. won't be an issue when projections are incorporated?		
		splatter_instance.transform.position = collision.contacts [0].point;
		splatter_instance.transform.forward = collision.contacts [0].normal;
		splatter_instance.transform.Rotate (0f, 0f, Random.Range(0f,360f));
	}
}
