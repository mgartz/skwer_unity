using UnityEngine;
using System.Collections;

public class TutorialText : MonoBehaviour {
	bool isAppearing;
	bool isDisappearing;
	int movementCounter;
	float movementPeriod = 20.0f;
	
	Vector3 backPosition;
	Vector3 screenPosition;
	Vector3 frontPosition;
	
	
	void Start(){
		frontPosition = new Vector3(0,10,-1.55f);
		screenPosition = new Vector3(0,-10,-1.55f);
		backPosition = new Vector3(0,-300,-40);
	}
	
	void Update () {
		if (isAppearing){
			if (++movementCounter == movementPeriod)
				isAppearing = false;
			transform.localPosition = Vector3.Lerp(backPosition, screenPosition, movementCounter/movementPeriod);
		}
		if (isDisappearing){
			if (++movementCounter == movementPeriod)
				isDisappearing = false;
			transform.localPosition = Vector3.Lerp(screenPosition, frontPosition, movementCounter/movementPeriod);
		}			
	}
	
	public void appear(){
		isAppearing = true;
		isDisappearing = false;
		movementCounter = 0;
	}
	public void disappear(){
		isAppearing = false;
		isDisappearing = true;
		movementCounter = 0;
		
	}
}
