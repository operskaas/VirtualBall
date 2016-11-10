using UnityEngine;
using System.Collections;

public class AISight : MonoBehaviour {

	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 personalLastSighting;

	private NavMeshAgent nav;
	//private SphereCollider col;
	private Animator anim;
	private GlobalLastPlayerSighting globalLastPlayerSighting; // stores global last player sighting
	private GameObject playerHead; //might not need, they only say they're using it to tell if it is in the collider, which is to tell if they can hear the player??
	private Animator playerAnim;
	private PlayerHealth playerHealth;
	private HashIDs hash; //stores the ids for the different animation states?
	private Vector3 previousSighting; //position sighted in previous frame
	private BallShooter ballShooter; // the script that fires the paintball
	private float bps = 6;
	private bool coroutineRunning;



	void Awake (){
		coroutineRunning = false;
		ballShooter = GetComponentInChildren<BallShooter> ();
		nav = GetComponent<NavMeshAgent> ();
		//col = GetComponent<SphereCollider> ();
		anim = GetComponent<Animator>();
		globalLastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalLastPlayerSighting>();
		playerHead = GameObject.FindGameObjectWithTag("MainCamera"); // TODO might need to make more robust when doing multiplayer
		//playerAnim = player.GetComponent<Animator> ();
		//playerHealth = player.GetComponent <PlayerHealth> ();
		hash = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HashIDs> ();

		//personalLastSighting = globalLastPlayerSighting.resetPosition; // reset the position so they don't start the game chasing the player
		//previousSighting = globalLastPlayerSighting.resetPosition;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if (globalLastPlayerSighting.position != previousSighting)
		//	personalLastSighting = globalLastPlayerSighting.position;

		//previousSighting = globalLastPlayerSighting.position;

		//if (playerHealth.health > 0)
			//anim.SetBool (hash.playerInSightBool, playerInSight);
		//else
			//anim.SetBool (hash.playerInSightBool, false);
		playerInSight = false;


		Vector3 direction = playerHead.transform.position - transform.position;
		float angle = Vector3.Angle (direction, transform.forward);


		if (angle < fieldOfViewAngle * 0.5f) {
			//Debug.Log ("Angle = " + angle + ", which is within the FOV");
			RaycastHit hit;

			if (Physics.Raycast (transform.position, direction.normalized, out hit, 100)) {
				Debug.Log(hit.collider.gameObject);
				if (hit.collider.gameObject == playerHead) {
					playerInSight = true;
					Debug.Log ("Can see player");
					//globalLastPlayerSighting.position = player.transform.position;
					if (!coroutineRunning) {
						
						StartCoroutine (ShootAtPlayer (direction.normalized));
					}
				}
			}
		}
	}


			


	IEnumerator ShootAtPlayer(Vector3 directionToShoot){
		coroutineRunning = true;
		if (directionToShoot != transform.forward) {
			yield return new WaitForSeconds (1);
			Vector3 direction = playerHead.transform.position - transform.position;
			Debug.Log ("distance is: " + direction.magnitude);
			float travelTime = direction.magnitude / ballShooter.initial_velocity;//(-1f * 0.003f / 0.5f) * Mathf.Log (1f - (direction.magnitude * 0.5f / (ballShooter.initial_velocity * 0.003f)));
			Debug.Log ("traveltime is: " + travelTime);
			float drop = 4.9f * Mathf.Pow(travelTime,2f);
			Debug.Log ("drop is: " + drop);
			Vector3 targetPosition = playerHead.transform.position + Vector3.up * drop;
			transform.forward = (targetPosition - transform.position).normalized; //aim so that ball arcs properly
		}
		ballShooter.ShootBall ();
		yield return new WaitForSeconds (1f/bps);
		coroutineRunning = false;
	}
}
