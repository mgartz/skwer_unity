using UnityEngine;
using System.Collections;

public class LogoLight : MonoBehaviour {
	float amp = 0.2f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.localPosition;
		pos.z = 0.5f+amp * Mathf.Sin(Time.time);
		transform.localPosition = pos;
	}
}
