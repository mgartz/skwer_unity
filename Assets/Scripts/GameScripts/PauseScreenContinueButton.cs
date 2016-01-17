using UnityEngine;
using System.Collections;

public class PauseScreenContinueButton : MonoBehaviour {
	Game game;
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
	}
	
	void OnMouseEnter(){
		this.renderer.material.color = Color.Lerp(Color.red,Color.white,0.1f);
		this.transform.localScale = this.transform.localScale*1.1f;
	}
	void OnMouseExit(){
		this.renderer.material.color = Color.white;
		this.transform.localScale = this.transform.localScale/1.1f;
	}
	void OnMouseUpAsButton(){
		game.togglePauseScreen();
	}
}
