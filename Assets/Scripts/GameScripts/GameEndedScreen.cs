using UnityEngine;
using System.Collections;

public class GameEndedScreen : MonoBehaviour {
	Transform title;
	Game game;
	
	public bool isOnScreen;
	int isOnScreenCounter;
	float isOnScreenPeriod = 80.0f;
	
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
		title = transform.FindChild("Title");
	}
	
	void FixedUpdate(){
		if (isOnScreen){
			isOnScreenCounter++;
			if (isOnScreenCounter == isOnScreenPeriod){
				isOnScreen = false;
				game.hideGameEndedScreen();
				game.showGameSummary();
			}
		}
	}
	
	public void showGameEndedScreen(){
		if (game.livesLeft == 0)
			title.GetComponent<TextMesh>().text = "out of moves";
		else if (game.levelStars > 5)
			title.GetComponent<TextMesh>().text = "level clear";
		else
			title.GetComponent<TextMesh>().text = "level failed";
		isOnScreen = true;
		isOnScreenCounter = 0;
	}
}
