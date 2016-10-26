using UnityEngine;
using System.Collections;

public class GatRotator : MonoBehaviour {

	Quaternion rotation_amount;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rotation_amount.eulerAngles = new Vector3 (0f, Input.GetAxis ("Mouse X") * 2, 0f);
		transform.localRotation = rotation_amount * transform.localRotation;
	}
}
