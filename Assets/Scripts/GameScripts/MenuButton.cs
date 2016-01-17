using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	Game game;
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUpAsButton(){
		game.openMenu();
	}
}
