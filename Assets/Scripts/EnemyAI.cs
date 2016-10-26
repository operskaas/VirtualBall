using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float runSpeed = 5f;
	public float delayTimeBeforeShoot = 2f;
	public Transform[] bunkerWayPoints;
	public string team;
	public string enemyTeam;

	private AISight aiSight;
	private NavMeshAgent nav;
	private Transform[] enemyTeamMembers;
	private PlayerHealth playerHealth;
	private GlobalLastPlayerSighting globalLastPlayerSighting;
	private float timerBeforeShoot;
	private int wayPointIndex;

	void Awake(){
		nav = GetComponent<NavMeshAgent> ();
		aiSight = GetComponent<AISight> ();
		globalLastPlayerSighting = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GlobalLastPlayerSighting> ();
		if (team == "Team0")
			enemyTeam = "Team1";
		else
			enemyTeam = "Team0";
			
		//enemyTeamMembers = GameObject.FindGameObjectsWithTag("GameController").
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
