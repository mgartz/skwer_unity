using UnityEngine;
using System.Collections;

public class RotatingArrow : MonoBehaviour {
	Quaternion rotationDelta;
	
	// Use this for initialization
	void Start () {
		rotationDelta = Quaternion.Euler(3,0,0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localRotation = transform.localRotation * rotationDelta;
	}
}
