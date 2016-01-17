using UnityEngine;
using System.Collections;

public class MainMenuTile : MonoBehaviour {
	
	Quaternion plane0deltaAngle;
	Quaternion plane1deltaAngle;
	Transform plane0;
	Transform plane1;
	Color[] planeColor;
	
	// Use this for initialization
	void Start () {
		float[,] rgbState = new float[3,3];
		rgbState[0,0] = 1.0f;
		rgbState[0,1] = 0.0f;
		rgbState[0,2] = 0.5f;
		
		rgbState[1,0] = 0.0f;
		rgbState[1,1] = 0.9f;
		rgbState[1,2] = 0.18f;
		
		rgbState[2,0] = 0.0f;
		rgbState[2,1] = 0.5f;
		rgbState[2,2] = 1.0f;
		
		planeColor = new Color[2];
		int state0 = Random.Range(2,3);
		int state1 = Random.Range(2,3);
		planeColor[0] = new Color(rgbState[state0,0],rgbState[state0,1],rgbState[state0,2],1);
		planeColor[1] = new Color(rgbState[state1,0],rgbState[state1,1],rgbState[state1,2],1);
		
		plane0 = transform.FindChild("Plane0");
		plane1 = transform.FindChild("Plane1");
		
		plane0deltaAngle = Quaternion.Euler(Random.Range(0,3.0f),Random.Range(0,3.0f),Random.Range(0,3.0f));
		plane1deltaAngle = Quaternion.Euler(Random.Range(0,3.0f),Random.Range(0,3.0f),Random.Range(0,3.0f));
		
		plane0.renderer.material.color = planeColor[0];
		plane1.renderer.material.color = planeColor[1];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		plane0.localRotation = plane0.localRotation * plane0deltaAngle;
		plane1.localRotation = plane1.localRotation * plane1deltaAngle;
	}
}
