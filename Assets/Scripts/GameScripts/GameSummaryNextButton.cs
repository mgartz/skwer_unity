using UnityEngine;
using System.Collections;

public class GameSummaryNextButton : MonoBehaviour {
	Game game;
	public bool isAvailable;
	Vector3 position0;
	bool isShaking;
	int shakeCounter;
	float shakePeriod = 30.0f;
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
		position0 = transform.localPosition;
	}
	
	void FixedUpdate(){
		if (isShaking){
			shakeCounter++;
			float rand1 = Random.Range(-1.0f,1.0f);
			float rand2 = Random.Range(-1.0f,1.0f);
			
			transform.localPosition = new Vector3(rand1,0,rand2) + position0;

			if (shakeCounter >= shakePeriod){
				transform.localPosition = position0;
				isShaking = false;
			}
		}
	}
	
	void OnMouseEnter(){
		if (isAvailable){
			this.renderer.material.color = Color.Lerp(Color.red,Color.white,0.1f);
			this.transform.localScale = this.transform.localScale*1.1f;
		}
	}
	void OnMouseExit(){
		if (isAvailable){
			this.renderer.material.color = Color.white;
			this.transform.localScale = this.transform.localScale/1.1f;
		}
	}
	void OnMouseUpAsButton(){
		if (isAvailable){
			if (game.isTutorial){
				game.hideGameSummary();
				game.goToFirstLevel();
			}
			else {
				int currentLevel = System.Convert.ToInt32(Global.stackLevel.Substring(5,2));
				Global.stackLevel = "stack" + (currentLevel+1).ToString("D2");
				game.readStack();
				game.hideGameSummary();
				game.retryWholeStack();
				if (game.isQuickGame)
					game.showGUI();
			}
		}
		else
			shake();
	}
	
	public void shake(){
		isShaking = true;
		shakeCounter = 0;
	}
}
