using UnityEngine;
using System.Collections;

public class SkipButton : MonoBehaviour {
	bool isAvailable = true;
	Game game;
	bool isShaking;
	int shakeCounter;
	float shakePeriod = 30.0f;
	Vector3 position0;
	
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
		if (!Global.isGamePaused && isAvailable){
			this.renderer.material.color = Color.Lerp(Color.red,Color.white,0.1f);
			this.transform.localScale = new Vector3(1.5f*1.1f,1*1.1f,1*1.1f);
		}
	}
	void OnMouseExit(){
		if (!Global.isGamePaused && isAvailable){
			this.renderer.material.color = Color.white;
			this.transform.localScale = new Vector3(1.5f,1,1);
		}
	}
	void OnMouseUpAsButton(){
		if (!game.isBlockingGUIInput && !Global.isGamePaused && isAvailable){
			game.skipLevel();
		}
		else{
			shake();
		}
	}
	
	public void makeAvailable(){
		isAvailable = true;
		this.renderer.material.color = Color.white;
		this.transform.localScale = new Vector3(1.5f,1,1);
	}
	public void makeUnavailable(){
		isAvailable = false;
		this.renderer.material.color = new Color(0.2f,0.2f,0.2f,1);
		this.transform.localScale = new Vector3(1.5f,1,1);
	}
	
	public void shake(){
		isShaking = true;
		shakeCounter = 0;
	}
	
}
