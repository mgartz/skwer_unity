using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public bool isAppearing;
	public bool isDisappearing;
	
	int disappearingCounter;
	float disappearingPeriod = 20.0f;
	
	int appearingCounter;
	float appearingPeriod = 60.0f;
	
	Transform mainLight;
	Transform menu;
	
	public string nextScreen;
	
	// Use this for initialization
	void Start () {
	//	PlayerPrefs.DeleteAll();
		mainLight = transform.FindChild("Light");
		menu = GameObject.Find("Menu").transform;
		mainLight.GetComponent<Light>().intensity = 0;
		appear();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAppearing){
			appearingCounter++;
			mainLight.GetComponent<Light>().intensity = appearingCounter/appearingPeriod * appearingCounter/appearingPeriod;
			menu.localPosition = new Vector3(0,600-600*appearingCounter/appearingPeriod,0);
			if (appearingCounter == appearingPeriod)
				isAppearing = false;
		}
		if (isDisappearing){
			disappearingCounter++;
			menu.localPosition = new Vector3(0,800*disappearingCounter/disappearingPeriod,0);
			mainLight.GetComponent<Light>().intensity = 1 - disappearingCounter/disappearingPeriod;
			if (disappearingCounter == disappearingPeriod)
				Application.LoadLevel(nextScreen);
		}
	}
	
	public void appear(){
		isAppearing = true;
		appearingCounter = 0;
		if (!PlayerPrefs.HasKey("tutorial")){
			GameObject.Find("QuickPlayButton").transform.localScale = Vector3.zero;
			GameObject.Find("TutorialButton").transform.localScale = Vector3.zero;
		}
	}
	public void disappear(string nextScreeni){
		isAppearing = false;
//		menu.localPosition = new Vector3(0,600,0);
		nextScreen = nextScreeni;
		isDisappearing = true;
		disappearingCounter = 0;
	}
}
