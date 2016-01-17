using UnityEngine;
using System.Collections;

public class ExtraChanceText : MonoBehaviour {
	bool isAppearing;
	int appearCounter;
	float appearPeriod = 30;
	
	// Use this for initialization
	void Start () {
		transform.GetComponent<TextMesh>().text = "unlocked * one extra chance per puzzle";
	}
	
	void FixedUpdate(){
		if (isAppearing){
			if (appearCounter >= 0){
				float size = (float) (5.5f - 5.5f*Mathf.Cos(appearCounter/appearPeriod*0.55f*2*3.141592f));
				transform.localScale = new Vector3(size/10*0.6f,size/10*0.6f,size/10*0.6f);
			}
			if (++appearCounter > appearPeriod)
				isAppearing = false;
		}
	}
	
	public void appear(){
		isAppearing = true;
		appearCounter = -10;
	}
	public void hide(){
		transform.localScale = Vector3.zero;
	}
}
