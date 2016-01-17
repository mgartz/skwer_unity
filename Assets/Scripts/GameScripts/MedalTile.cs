using UnityEngine;
using System.Collections;

public class MedalTile : MonoBehaviour {
	
	public Quaternion deltaAngle;
	public bool shouldRotate;
	
	// Use this for initialization
	void Start () {
		deltaAngle = Quaternion.Euler(Random.Range(1.0f,2.5f),Random.Range(1.0f,2.5f),Random.Range(1.0f,2.5f));
		shouldRotate = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (shouldRotate)
			transform.localRotation = transform.localRotation * deltaAngle;
	}
}
