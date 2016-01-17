using UnityEngine;
using System.Collections;

public class LevelIndicator : MonoBehaviour {
	public int state;
	public Transform stateIndicator;
	Texture2D rightTex, wrongTex, currentTex;
	
	// Use this for initialization
	void Start () {
		state = 0;
		stateIndicator = transform.FindChild("StateIndicator");
		rightTex = Resources.Load("skwer_right") as Texture2D;
		wrongTex = Resources.Load("skwer_wrong") as Texture2D;
		currentTex = Resources.Load("skwer_current") as Texture2D;
		stateIndicator.localScale = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void setSkipped(){
		state = 0;
		stateIndicator.renderer.material.mainTexture = wrongTex;
		stateIndicator.localScale = new Vector3(0.8f,0.8f,0.8f);
	}
	public void setPassed(){
		state = 1;
		stateIndicator.renderer.material.mainTexture = rightTex;
		stateIndicator.localScale = new Vector3(0.8f,0.8f,0.8f);
	}
	public void setCurrent(){
		state = 0;
		stateIndicator.renderer.material.mainTexture = currentTex;
		stateIndicator.localScale = new Vector3(1,1,1);
	}
	public void setNull(){
		state = 0;
		stateIndicator.localScale = new Vector3(0,0,0);
	}
}
