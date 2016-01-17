using UnityEngine;
using System.Collections;

public class Medal : MonoBehaviour {
	Light light0, light1;
	Transform outerCube;
	MedalTile medalTile;
	public int index;
	public Game game;
	
	// Use this for initialization
	void Start () {
		light0 = transform.FindChild("Light0").GetComponent<Light>();
		light1 = transform.FindChild("Light1").GetComponent<Light>();
		light0.range = 0;
		light1.range = 0;
		
		outerCube = transform.FindChild("MedalObject");
		medalTile = outerCube.GetComponent<MedalTile>();
		
		if (game == null){
			outerCube.localScale = Vector3.zero;
			light0.intensity = 3;
			light1.intensity = 6;
			light1.transform.localPosition = new Vector3(0,5,0);
		}
		else{
			light0.intensity = 1;
			light1.intensity = 1;
		}
	}
	
	public void showInGameSummary(int levelStars){
		light0.range = 40;
		light1.range = 40;
		if (index == 0)
			transform.localPosition = new Vector3(0.0f - 7.5f*(levelStars-6),0,0);
		else if (index == 1)
			transform.localPosition = new Vector3(7.5f - 7.5f*(levelStars-7),0,0);
		else if (index == 2)
			transform.localPosition = new Vector3(15.0f- 7.5f*(levelStars-8),0,0);
		else if (index == 3)
			transform.localPosition = new Vector3(22.5f- 7.5f*(levelStars-9),0,0);
		else if (index == 4)
			transform.localPosition = new Vector3(30.0f- 7.5f*(levelStars-10),0,0);
		if (index >= levelStars - 5){
			transform.localScale = new Vector3(0,0,0);
			medalTile.shouldRotate = false;
		}
		else{
			transform.localScale = new Vector3(1,1,1);
			medalTile.shouldRotate = true;
		}
	}
	public void hideInGameSummary(){
		light0.range = 0;
		light1.range = 0;
	}
	
	public void showInStackMenu(string stackLevel){
		light0.range = 40;
		light1.range = 40;
		if (PlayerPrefs.HasKey(stackLevel)){
			if (index >= PlayerPrefs.GetInt(stackLevel) - 5){
				outerCube.localScale = new Vector3(0,0,0);
				medalTile.shouldRotate = false;
			}
			else{
				outerCube.localScale = new Vector3(50,50,50);
				medalTile.shouldRotate = true;
			}
		}
		else{
			outerCube.localScale = new Vector3(0,0,0);
			medalTile.shouldRotate = false;
		}
	}	
	public void hideInStackMenu(){
		light0.range = 0;
		light1.range = 0;
		outerCube.localScale = Vector3.zero;
		medalTile.shouldRotate = false;
	}
}
