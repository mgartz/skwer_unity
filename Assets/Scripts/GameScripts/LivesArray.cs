using UnityEngine;
using System.Collections;

public class LivesArray : MonoBehaviour {
	public Transform[] lives;
	Game game;
	
	bool isBlendingIn;
	bool isBlendingOut;
	int blendCounter;
	float blendPeriod = 30;
	
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
		lives = new Transform[20];
		lives[0] = transform.FindChild("LivesIcon00");
		lives[1] = transform.FindChild("LivesIcon01");
		lives[2] = transform.FindChild("LivesIcon02");
		lives[3] = transform.FindChild("LivesIcon03");
		lives[4] = transform.FindChild("LivesIcon04");
		lives[5] = transform.FindChild("LivesIcon10");
		lives[6] = transform.FindChild("LivesIcon11");
		lives[7] = transform.FindChild("LivesIcon12");
		lives[8] = transform.FindChild("LivesIcon13");
		lives[9] = transform.FindChild("LivesIcon14");
		lives[10] = transform.FindChild("LivesIcon20");
		lives[11] = transform.FindChild("LivesIcon21");
		lives[12] = transform.FindChild("LivesIcon22");
		lives[13] = transform.FindChild("LivesIcon23");
		lives[14] = transform.FindChild("LivesIcon24");
		lives[15] = transform.FindChild("LivesIcon30");
		lives[16] = transform.FindChild("LivesIcon31");
		lives[17] = transform.FindChild("LivesIcon32");
		lives[18] = transform.FindChild("LivesIcon33");
		lives[19] = transform.FindChild("LivesIcon34");
		isBlendingIn = false;
		isBlendingOut = false;
	}
	
	void FixedUpdate(){
		if (isBlendingIn){
			Color color = new Color(1,1,1,blendCounter/blendPeriod);
			for (int i=0; i<20; i++)
				lives[i].renderer.material.color = color;
			if (++blendCounter > blendPeriod){
				isBlendingIn = false;
				isBlendingOut = false;
			}
		}
		else if (isBlendingOut){
			Color color = new Color(1,1,1,1-blendCounter/blendPeriod);
			for (int i=0; i<20; i++)
				lives[i].renderer.material.color = color;
			if (++blendCounter > blendPeriod){
				isBlendingIn = false;
				isBlendingOut = false;
			}
		}
	}
	
	public void setLives(int livesLeft){
		for (int i=0; i<20; i++){
			int maxLives;
			if (game.isQuickGame)
				maxLives = Global.quickGameLives;
			else
				maxLives = Global.getGameLives();
			
			if (i >= maxLives)
				lives[i].localScale = Vector3.zero;
			else{ 
				lives[i].localScale = new Vector3(2,2,2);
				if (i >= livesLeft)
					lives[i].renderer.material.color = new Color(1,1,1,0.3f);
				else
					lives[i].renderer.material.color = new Color(1,1,1,1);
			}
		}
	}
	public void setLives(int livesLeft, int maxLives){
		for (int i=0; i<20; i++){
			if (i >= maxLives)
				lives[i].localScale = Vector3.zero;
			else{ 
				lives[i].localScale = new Vector3(2,2,2);
				if (i >= livesLeft)
					lives[i].renderer.material.color = new Color(1,1,1,0.3f);
				else
					lives[i].renderer.material.color = new Color(1,1,1,1);
			}
		}
	}
	public void blendInAll(){
		isBlendingIn = true;
		isBlendingOut = false;
		blendCounter = 0;
		for (int i=0; i<20; i++){
			if (i >= (Global.getGameLives()-1))
				lives[i].localScale = Vector3.zero;
			else
				lives[i].localScale = new Vector3(2,2,2);
		}
		blendPeriod = 30;
	}
	public void blendOutAll(){
		for (int i=0; i<20; i++){
			if (i >= Global.getGameLives())
				lives[i].localScale = Vector3.zero;
			else
				lives[i].localScale = new Vector3(2,2,2);
		}
		isBlendingOut = true;
		isBlendingIn = false;
		blendCounter = 0;
		blendPeriod = 100;
	}
}
