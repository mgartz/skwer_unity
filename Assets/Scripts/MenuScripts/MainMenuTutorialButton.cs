using UnityEngine;
using System.Collections;

public class MainMenuTutorialButton : MonoBehaviour {
	MainMenu mainMenu;
	
	// Use this for initialization
	void Start () {
		mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
		if (!mainMenu.isDisappearing){
			Global.stackLevel = "tutorial";
			mainMenu.disappear("TutorialScene");
		}
	}
}
