using UnityEngine;
using System.Collections;

public class StackMenuCamera : MonoBehaviour {
	public bool isAppearing;
	public bool isZoomingIn;
	
	int zoomingInCounter;
	float zoomingInPeriod1 = 15.0f;
	float zoomingInPeriod2 = 60.0f;
	int appearingCounter;
	float appearingPeriod = 40.0f;
	float zoom1targetX;
	float zoom1targetZ;
	
	Transform tilesArray;
	Transform light1;
	Transform light2;
	
	float lightMovementPeriod = 600;
	int lightMovementCounter;
	// Use this for initialization
	void Start () {
		light1 = transform.FindChild("Light1");
		light2 = transform.FindChild("Light2");
		appear();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAppearing){
			appearingCounter++;
			float light2Intensity = 5f;
			if (appearingCounter <= appearingPeriod)
				light2.GetComponent<Light>().intensity = light2Intensity*appearingCounter/appearingPeriod;
			
			if (appearingCounter > appearingPeriod)				
				light1.GetComponent<Light>().intensity = 0.35f*(appearingCounter - appearingPeriod)/appearingPeriod * (appearingCounter - appearingPeriod)/appearingPeriod;
			if (appearingCounter == appearingPeriod*2)
				isAppearing = false;
		}
		else {
			lightMovementCounter++;
			if (lightMovementPeriod == lightMovementCounter)
				lightMovementCounter = 0;
			light2.localPosition = new Vector3(0,0,2000 - 100*Mathf.Sin(lightMovementCounter/lightMovementPeriod*2*Mathf.PI));
		}
		if (isZoomingIn){
			if (zoomingInCounter++ == zoomingInPeriod1+zoomingInPeriod2){
				isZoomingIn = false;
				Global.stackLevel = tilesArray.GetComponent<StackMenuTilesArray>().stackLevel;
				Application.LoadLevel("GameScene");
			}
			if (zoomingInCounter < zoomingInPeriod1)
				camera.transform.localPosition = new Vector3(360-(360-zoom1targetX)*zoomingInCounter/zoomingInPeriod1,1350-350*zoomingInCounter/zoomingInPeriod1,-300+(300+zoom1targetZ)*zoomingInCounter/zoomingInPeriod1);
			else
				camera.transform.localPosition = new Vector3(zoom1targetX,1000 - 2000*(zoomingInCounter-zoomingInPeriod1)/zoomingInPeriod2,zoom1targetZ);
		}
	}

	public void zoomIn(Transform tilesArrayi){
		if (!isZoomingIn){
			tilesArray = tilesArrayi;
			zoom1targetX = tilesArray.localPosition.x;
			zoom1targetZ = tilesArray.localPosition.z;
			isZoomingIn = true;
			zoomingInCounter = 0;
		}
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel("MenuScene");
    }
	
	void appear(){
		isAppearing = true;
		appearingCounter = 0;
	}
}
