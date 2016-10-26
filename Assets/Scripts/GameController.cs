using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	public int[] numHumanPlayers = new int[2]; 
	public int[] numAIPlayers = new int[2];
	public int numPlayersPerTeam = 5; //maybe make this non-static in the future, for different maps or user selectable. 
	public GameObject aIPlayer;
	public float fieldLength = 40f; //TODO if going to have multiple fields, need to update field length based on field. maybe have same field length. 

	public Vector3[] teamStartingPositions; 
	private float startingDistanceBetweenPlayers = 0.5f;

	void Awake(){
		numHumanPlayers = new int[] {1,0}; //TODO will have to make nonstatic when create networking...
		//making the reference position for each starting team be to the left of center so that when they are instantiated they can spread out evenly
		teamStartingPositions = new Vector3[2] {new Vector3(-1f*(numPlayersPerTeam-1)*startingDistanceBetweenPlayers/2f,0f,0f), new Vector3(-1f*(numPlayersPerTeam-1)*startingDistanceBetweenPlayers/2f, 0f, fieldLength)}; 
	}
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < (numPlayersPerTeam - numHumanPlayers [i]); j++) {
				Instantiate (aIPlayer);
				//aIPlayer.GetComponent<EnemyAI> ().team = "Team" + i;
				aIPlayer.tag = "Team" + i; //TODO not sure if should store team in both places? 
				aIPlayer.transform.position = teamStartingPositions[i] + j * startingDistanceBetweenPlayers * Vector3.right; // spreads them out, side by side.
				if (i == 0)
					aIPlayer.transform.forward = Vector3.forward; // trying to face them the right way
				else
					aIPlayer.transform.forward = Vector3.back;


				//TODO HAVE TO SET aiplayer position, and probably some other things, like maybe the difficulty, which way it's aiming depending on which team it's on...
				//TODO probably also save a reference to it in an array. 
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
