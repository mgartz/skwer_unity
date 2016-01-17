using UnityEngine;
using System.Collections;

public class StackMenuTilesArrayButton : MonoBehaviour {
	
	StackMenuTilesArray tilesArray;
	GameObject mainCamera;
	StackMenuCamera cameraScript;
	TextMesh levelNumber; 
	Medal[] medals;
	
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find("Camera");
		cameraScript = mainCamera.GetComponent<StackMenuCamera>();
		tilesArray = transform.parent.GetComponent<StackMenuTilesArray>();	
		levelNumber = GameObject.Find("LevelNumber").GetComponent<TextMesh>();
		medals = new Medal[5];
		medals[0] = GameObject.Find("Medal0").GetComponent<Medal>();
		medals[1] = GameObject.Find("Medal1").GetComponent<Medal>();
		medals[2] = GameObject.Find("Medal2").GetComponent<Medal>();
		medals[3] = GameObject.Find("Medal3").GetComponent<Medal>();
		medals[4] = GameObject.Find("Medal4").GetComponent<Medal>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseEnter() {
		if (!cameraScript.isZoomingIn && tilesArray.stackIsAvailable){
			levelNumber.text = "Level " + (System.Convert.ToInt32(tilesArray.stackLevel.Substring(5,2)) + 1).ToString();
			for (int i=0; i<5; i++)
				medals[i].showInStackMenu(tilesArray.stackLevel);
			tilesArray.mouseEntered();
		}
    }
	void OnMouseExit(){
		if (!cameraScript.isZoomingIn && tilesArray.stackIsAvailable){
			levelNumber.text = "";
			for (int i=0; i<5; i++)
				medals[i].hideInStackMenu();
			tilesArray.mouseExited();
		}
	}
	void OnMouseUpAsButton(){
		if (!cameraScript.isZoomingIn && tilesArray.stackIsAvailable)
			cameraScript.zoomIn(tilesArray.transform);
	}
}
