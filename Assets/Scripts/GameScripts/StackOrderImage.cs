using UnityEngine;
using System.Collections;

public class StackOrderImage : MonoBehaviour {

	public bool isBlinkingTutorial;
	int blinkingTutorialCounter;
	float blinkingTutorialPeriod = 30;
	bool blinkState;
	
	public void FixedUpdate(){
		if (isBlinkingTutorial){
			if (++blinkingTutorialCounter == blinkingTutorialPeriod){
				blinkingTutorialCounter = 0;
				blinkState = !blinkState;
				blink(blinkState);
			}
		}
	}
	
	public void blinkTutorial(){
		isBlinkingTutorial = true;
		blinkingTutorialCounter = 0;
	}
	
	void blink(bool on){
		if (transform.renderer.material.color == Color.black)
			isBlinkingTutorial = false;
		else {
			if (on)
				transform.renderer.material.color = new Color(0.7f,1,0.8f);
			else
				transform.renderer.material.color = new Color (0, 0.9f, 0.18f);
		}
	}
	
}
