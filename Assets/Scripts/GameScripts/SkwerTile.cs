using UnityEngine;
using System.Collections;

public class SkwerTile : MonoBehaviour {
	public bool paused;
	public int state;
	public int realIndexi;
	public int realIndexj;
	public float[,] rgbState;
	public bool isPreRotating;
	public bool isRotating;
	public bool isStopingRotating;
	public bool isStateChanging;
	public bool isShaking;
	public bool isSuspended;
	public bool isRebuildingStack;
	public float rebuildingStackCounter;
	public float rebuildingStackPeriod;
	public float shakeCounter = 0;
	public float shakePeriod = 16;
	public float stateChangeCounter;
	public float stateChangePeriod;
	
	protected float preRotationCounter;
	protected float preRotationPeriod = 60;
	
	protected float plane0PreviewRotationCounter;
	protected float plane1PreviewRotationCounter;
	protected float plane0PreviewRotationPeriod = 240;
	protected float plane1PreviewRotationPeriod = 160;
	
	protected float plane0StopPreviewRotationCounter;
	protected float plane1StopPreviewRotationCounter;
	protected float plane0StopPreviewRotationPeriod = 15;
	protected float plane1StopPreviewRotationPeriod = 10;
	
	protected float plane0ForwardRotationCounter;
	protected float plane1ForwardRotationCounter;
	protected float plane0ForwardRotationPeriod;
	protected float plane1ForwardRotationPeriod;
	
	public Quaternion plane0suspendedDeltaAngle;
	public Quaternion plane1suspendedDeltaAngle;
	
	public Color[] planeColor;
	public float[] planeColorFactor;
	
	public Quaternion plane0OriginRotation;
	public Quaternion plane1OriginRotation;
	public Quaternion plane0TargetRotation;
	public Quaternion plane1TargetRotation;
	
	public float plane0OriginRotationY;
	public float plane1OriginRotationY;
	public float plane0TargetRotationY;
	public float plane1TargetRotationY;
	
	public Transform plane0;
	public Transform plane1;
	
	public Quaternion plane0ReverseOriginalRotation;
	public Quaternion plane1ReverseOriginalRotation;
	public Vector3 plane0ReverseOriginalPosition;
	public Vector3 plane1ReverseOriginalPosition;
	public Vector3 plane0OriginalPosition;
	public Vector3 plane1OriginalPosition;
	
	public Transform pointlight;
	
	public Game game;
	
	public int beaconCount;
	
	public SkwerTile[,] tiles;
	
	public bool shouldGoBackAfterRebuild;
	
	public virtual void Start () {
		plane0OriginRotation = Quaternion.Euler(0,0,0);
		plane1OriginRotation = Quaternion.Euler(0,135,0);
		plane0TargetRotation = Quaternion.Euler(0,90,0);
		plane1TargetRotation = Quaternion.Euler(0,45,0);
	
		plane0OriginRotationY = 0;
		plane0TargetRotationY = 360;
		plane1OriginRotationY = 405;
		plane1TargetRotationY = 45;
		
		plane0suspendedDeltaAngle = Quaternion.Euler(Random.Range(0,3.0f),Random.Range(0,3.0f),Random.Range(0,3.0f));
		plane1suspendedDeltaAngle = Quaternion.Euler(Random.Range(0,3.0f),Random.Range(0,3.0f),Random.Range(0,3.0f));
		tiles = new SkwerTile[9,8];
		tiles[0,0] = GameObject.Find("SupportingTile00").GetComponent<SupportingTile>();
		tiles[0,1] = GameObject.Find("SupportingTile01").GetComponent<SupportingTile>();
		tiles[0,2] = GameObject.Find("SupportingTile02").GetComponent<SupportingTile>();
		tiles[0,3] = GameObject.Find("SupportingTile03").GetComponent<SupportingTile>();
		tiles[0,4] = GameObject.Find("SupportingTile04").GetComponent<SupportingTile>();
		tiles[0,5] = GameObject.Find("SupportingTile05").GetComponent<SupportingTile>();
		tiles[0,6] = GameObject.Find("SupportingTile06").GetComponent<SupportingTile>();
		tiles[0,7] = GameObject.Find("SupportingTile07").GetComponent<SupportingTile>();
		tiles[1,0] = GameObject.Find("SupportingTile10").GetComponent<SupportingTile>();
		tiles[1,1] = GameObject.Find("SupportingTile11").GetComponent<SupportingTile>();
		tiles[1,2] = GameObject.Find("SupportingTile12").GetComponent<SupportingTile>();
		tiles[1,3] = GameObject.Find("SupportingTile13").GetComponent<SupportingTile>();
		tiles[1,4] = GameObject.Find("SupportingTile14").GetComponent<SupportingTile>();
		tiles[1,5] = GameObject.Find("SupportingTile15").GetComponent<SupportingTile>();
		tiles[1,6] = GameObject.Find("SupportingTile16").GetComponent<SupportingTile>();
		tiles[1,7] = GameObject.Find("SupportingTile17").GetComponent<SupportingTile>();
		tiles[2,0] = GameObject.Find("SupportingTile20").GetComponent<SupportingTile>();
		tiles[2,1] = GameObject.Find("SupportingTile21").GetComponent<SupportingTile>();
		tiles[2,6] = GameObject.Find("SupportingTile26").GetComponent<SupportingTile>();
		tiles[2,7] = GameObject.Find("SupportingTile27").GetComponent<SupportingTile>();
		tiles[3,0] = GameObject.Find("SupportingTile30").GetComponent<SupportingTile>();
		tiles[3,1] = GameObject.Find("SupportingTile31").GetComponent<SupportingTile>();
		tiles[3,6] = GameObject.Find("SupportingTile36").GetComponent<SupportingTile>();
		tiles[3,7] = GameObject.Find("SupportingTile37").GetComponent<SupportingTile>();
		tiles[4,0] = GameObject.Find("SupportingTile40").GetComponent<SupportingTile>();
		tiles[4,1] = GameObject.Find("SupportingTile41").GetComponent<SupportingTile>();
		tiles[4,6] = GameObject.Find("SupportingTile46").GetComponent<SupportingTile>();
		tiles[4,7] = GameObject.Find("SupportingTile47").GetComponent<SupportingTile>();
		tiles[5,0] = GameObject.Find("SupportingTile50").GetComponent<SupportingTile>();
		tiles[5,1] = GameObject.Find("SupportingTile51").GetComponent<SupportingTile>();
		tiles[5,6] = GameObject.Find("SupportingTile56").GetComponent<SupportingTile>();
		tiles[5,7] = GameObject.Find("SupportingTile57").GetComponent<SupportingTile>();
		tiles[6,0] = GameObject.Find("SupportingTile60").GetComponent<SupportingTile>();
		tiles[6,1] = GameObject.Find("SupportingTile61").GetComponent<SupportingTile>();
		tiles[6,6] = GameObject.Find("SupportingTile66").GetComponent<SupportingTile>();
		tiles[6,7] = GameObject.Find("SupportingTile67").GetComponent<SupportingTile>();
		tiles[7,0] = GameObject.Find("SupportingTile70").GetComponent<SupportingTile>();
		tiles[7,1] = GameObject.Find("SupportingTile71").GetComponent<SupportingTile>();
		tiles[7,2] = GameObject.Find("SupportingTile72").GetComponent<SupportingTile>();
		tiles[7,3] = GameObject.Find("SupportingTile73").GetComponent<SupportingTile>();
		tiles[7,4] = GameObject.Find("SupportingTile74").GetComponent<SupportingTile>();
		tiles[7,5] = GameObject.Find("SupportingTile75").GetComponent<SupportingTile>();
		tiles[7,6] = GameObject.Find("SupportingTile76").GetComponent<SupportingTile>();
		tiles[7,7] = GameObject.Find("SupportingTile77").GetComponent<SupportingTile>();
		tiles[8,0] = GameObject.Find("SupportingTile80").GetComponent<SupportingTile>();
		tiles[8,1] = GameObject.Find("SupportingTile81").GetComponent<SupportingTile>();
		tiles[8,2] = GameObject.Find("SupportingTile82").GetComponent<SupportingTile>();
		tiles[8,3] = GameObject.Find("SupportingTile83").GetComponent<SupportingTile>();
		tiles[8,4] = GameObject.Find("SupportingTile84").GetComponent<SupportingTile>();
		tiles[8,5] = GameObject.Find("SupportingTile85").GetComponent<SupportingTile>();
		tiles[8,6] = GameObject.Find("SupportingTile86").GetComponent<SupportingTile>();
		tiles[8,7] = GameObject.Find("SupportingTile87").GetComponent<SupportingTile>();
		
		rgbState = new float[3,3];
		rgbState[0,0] = 1.0f;
		rgbState[0,1] = 0.0f;
		rgbState[0,2] = 0.5f;
		
		rgbState[1,0] = 0.0f;
		rgbState[1,1] = 0.9f;
		rgbState[1,2] = 0.18f;
		
		rgbState[2,0] = 0.0f;
		rgbState[2,1] = 0.5f;
		rgbState[2,2] = 1.0f;
		
		state = 0;
		
		game = GameObject.Find("Game").GetComponent<Game>();
		
		planeColorFactor = new float[3];
		planeColorFactor[0] = 1.0f;
		planeColorFactor[1] = 0.7f;
		
		plane0 = transform.FindChild("Plane0");
		plane1 = transform.FindChild("Plane1");
		
		planeColor = new Color[3];
		planeColor[0] = new Color(rgbState[state,0]*planeColorFactor[0],rgbState[state,1]*planeColorFactor[0],rgbState[state,2]*planeColorFactor[0],1);
		planeColor[1] = new Color(rgbState[state,0]*planeColorFactor[1],rgbState[state,1]*planeColorFactor[1],rgbState[state,2]*planeColorFactor[1],1);
		
		plane0.renderer.material.color = planeColor[0];
		plane1.renderer.material.color = planeColor[1];
		
		stateChangeCounter = 0;
		stateChangePeriod = 16;
		
		pointlight = transform.FindChild("Light");
		pointlight.GetComponent<Light>().range = 0;
		
		plane0OriginalPosition = plane0.localPosition;
		plane1OriginalPosition = plane1.localPosition;
	}
	

	public virtual void FixedUpdate () {
		if (!paused){
			if (isStateChanging){
				stateChangingStep();
				stateChangeCounter++;
			}
			else {
				if (isPreRotating){
					preRotationCounter++;
					if (preRotationCounter == preRotationPeriod){
						isRotating = true;
						isPreRotating = false;
						plane0PreviewRotationCounter = 0;
						plane1PreviewRotationCounter = 0;
					}
				}
				if (isRotating){
					if (plane0PreviewRotationCounter >= 0)
						plane0.transform.localRotation = Quaternion.Euler(0,
							plane0OriginRotationY*(1-plane0PreviewRotationCounter/plane0PreviewRotationPeriod) + plane0TargetRotationY*(plane0PreviewRotationCounter/plane0PreviewRotationPeriod),0);
					if (plane1PreviewRotationCounter >= 0)
						plane1.transform.localRotation = Quaternion.Euler(0,
							plane1OriginRotationY*(1-plane1PreviewRotationCounter/plane1PreviewRotationPeriod) + plane1TargetRotationY*(plane1PreviewRotationCounter/plane1PreviewRotationPeriod),0);
					
					if (plane0PreviewRotationCounter++ == plane0PreviewRotationPeriod)
						plane0PreviewRotationCounter = 0;
					if (plane1PreviewRotationCounter++ == plane1PreviewRotationPeriod)
						plane1PreviewRotationCounter = 0;
				}
				if (isStopingRotating) {
					plane0StopPreviewRotationCounter += 1;
					plane1StopPreviewRotationCounter += 1;
					
					if (plane0StopPreviewRotationCounter <= plane0StopPreviewRotationPeriod && plane0StopPreviewRotationCounter >= 0)
						plane0.transform.localRotation = Quaternion.Slerp(plane0OriginRotation, plane0TargetRotation, plane0StopPreviewRotationCounter/plane0StopPreviewRotationPeriod);
					else
						plane0StopPreviewRotationCounter = plane0StopPreviewRotationPeriod;
					
					if (plane1StopPreviewRotationCounter <= plane1StopPreviewRotationPeriod && plane1StopPreviewRotationCounter >= 0)
						plane1.transform.localRotation = Quaternion.Slerp(plane1OriginRotation, plane1TargetRotation, plane1StopPreviewRotationCounter/plane1StopPreviewRotationPeriod);
					else
						plane1StopPreviewRotationCounter = plane1StopPreviewRotationPeriod;
					
					if (plane0StopPreviewRotationCounter >= plane0StopPreviewRotationPeriod)
						plane0StopPreviewRotationCounter = plane0StopPreviewRotationPeriod;
					if (plane1StopPreviewRotationCounter >= plane1StopPreviewRotationPeriod)
						plane1StopPreviewRotationCounter = plane1StopPreviewRotationPeriod;
					if (plane0StopPreviewRotationCounter == plane0StopPreviewRotationPeriod && plane1StopPreviewRotationCounter == plane1StopPreviewRotationPeriod)
						isStopingRotating = false;
				}
				if (isSuspended){
					plane0.localRotation = plane0.localRotation * plane0suspendedDeltaAngle;
					plane1.localRotation = plane1.localRotation * plane1suspendedDeltaAngle;
				}
			}
			if (isRebuildingStack){
				plane0.localPosition = Vector3.Lerp(plane0ReverseOriginalPosition,plane0OriginalPosition,rebuildingStackCounter/rebuildingStackPeriod);
				plane1.localPosition = Vector3.Lerp(plane1ReverseOriginalPosition,plane1OriginalPosition,rebuildingStackCounter/rebuildingStackPeriod);
				
				plane0.localRotation = Quaternion.Lerp(plane0ReverseOriginalRotation, plane0TargetRotation, rebuildingStackCounter/rebuildingStackPeriod);
				plane1.localRotation = Quaternion.Lerp(plane1ReverseOriginalRotation, plane1TargetRotation, rebuildingStackCounter/rebuildingStackPeriod);
				
				if (++rebuildingStackCounter > rebuildingStackPeriod) {
					isRebuildingStack = false;
					isPreRotating = false;
					isRotating = false;
					isStopingRotating = false;
					plane0.localRotation = plane0OriginRotation;
					plane1.localRotation = plane1OriginRotation;
				}
			}
		}
	}
	
	public virtual void stateChangingStep(){
		plane0.transform.localRotation = Quaternion.Slerp(plane0OriginRotation, plane0TargetRotation, stateChangeCounter/stateChangePeriod);
		plane1.transform.localRotation = Quaternion.Slerp(plane1OriginRotation, plane1TargetRotation, stateChangeCounter/stateChangePeriod);
		
		Color newColor0, newColor1;
		Color oldColor0, oldColor1;
		newColor0 = new Color(rgbState[state,0]*planeColorFactor[0],rgbState[state,1]*planeColorFactor[0],rgbState[state,2]*planeColorFactor[0],1);
		oldColor0 = new Color(rgbState[(state+2)%3,0]*planeColorFactor[0],rgbState[(state+2)%3,1]*planeColorFactor[0],rgbState[(state+2)%3,2]*planeColorFactor[0],1);
		newColor1 = new Color(rgbState[state,0]*planeColorFactor[1],rgbState[state,1]*planeColorFactor[1],rgbState[state,2]*planeColorFactor[1],1);
		oldColor1 = new Color(rgbState[(state+2)%3,0]*planeColorFactor[1],rgbState[(state+2)%3,1]*planeColorFactor[1],rgbState[(state+2)%3,2]*planeColorFactor[1],1);

		if (stateChangeCounter <= stateChangePeriod/2)
			planeColor[0] = Color.Lerp(oldColor0,newColor0,stateChangeCounter/stateChangePeriod*2);
		if (stateChangeCounter <= 3*stateChangePeriod/4 && stateChangeCounter > stateChangePeriod/4)
			planeColor[1] = Color.Lerp(oldColor1,newColor1,(stateChangeCounter-stateChangePeriod/4)/stateChangePeriod*2);
		
		if (!isPreRotating)
			pointlight.GetComponent<Light>().range = 20-20*stateChangeCounter/stateChangePeriod;
		
		if (!paused){
			plane0.renderer.material.color = planeColor[0];
			plane1.renderer.material.color = planeColor[1];
		}
		else{
			plane0.renderer.material.color = Color.Lerp(Color.black, planeColor[0],0.3f);
			plane1.renderer.material.color = Color.Lerp(Color.black, planeColor[1],0.3f);
		}
		
		if (stateChangeCounter == stateChangePeriod){
			isStateChanging = false;
		}
	}
	
	public virtual void previewRotate(){
		isPreRotating = true;
		isRotating = false;
		preRotationCounter = 0;
		pointlight.GetComponent<Light>().range = 25;
	}
	public virtual void stopPreviewRotate(){
		if (isRotating)
			isStopingRotating = true;
		plane0StopPreviewRotationCounter = 0;
		plane1StopPreviewRotationCounter = 0;
		isRotating = false;
		isPreRotating = false;
		pointlight.GetComponent<Light>().range = 0;
	}
	public virtual void stateChange(bool forward){
		if (forward)
			state = (state+1)%3;
		else
			state = (state+2)%3;
		isStateChanging = true;
		stateChangeCounter = 0;
		if (forward){
			isRotating = false;
			isPreRotating = false;
			isStopingRotating = false;
			preRotationCounter = 0;
		}
	}
	public virtual void pause(){}
	public virtual void unpause(){}
	public virtual void dropTile(bool forward){
		pointlight.GetComponent<Light>().range = 0;
		if (!forward){
			isSuspended = true;
			isShaking = false;
		}
	}
	public virtual void freezeTile(){
	}
	public virtual void rebuildStack(bool isActiveTilesArray){
		isSuspended = false;
		
		if (!isActiveTilesArray){
			plane0ReverseOriginalPosition = new Vector3(plane0OriginalPosition.x,plane0.localPosition.y+50*Random.Range(0,30),plane0OriginalPosition.z);
			plane1ReverseOriginalPosition = new Vector3(plane1OriginalPosition.x,plane1.localPosition.y+50*Random.Range(0,30),plane1OriginalPosition.z);
			plane0ReverseOriginalRotation = plane0TargetRotation;
			plane1ReverseOriginalRotation = plane1TargetRotation;
			rebuildingStackPeriod = 60.0f;
		}
		else {
			plane0ReverseOriginalPosition = plane0.localPosition;
			plane1ReverseOriginalPosition = plane1.localPosition;
			plane0ReverseOriginalRotation = plane0.localRotation;
			plane1ReverseOriginalRotation = plane1.localRotation;
			rebuildingStackPeriod = 30.0f;
		}

		isRebuildingStack = true;
		rebuildingStackCounter = 0;
	}
	public virtual void quickRebuildStack(bool isActiveTilesArray){
		isSuspended = false;
		isRebuildingStack = false;
		isPreRotating = false;
		isRotating = false;
		isStopingRotating = false;
		
		plane0.localPosition = plane0OriginalPosition;
		plane1.localPosition = plane1OriginalPosition;
		plane0.localRotation = plane0OriginRotation;
		plane1.localRotation = plane1OriginRotation;
	}
	public virtual void reverseDropTile(){}
	public virtual void unblockInput(){}
	public virtual void blockInput(){}
	public virtual void addToBack(){}
	public virtual void addToBack2(int baseState){}
	public virtual void bringForward(){}
	public virtual void turnLightOn(){
		pointlight.GetComponent<Light>().range = 40;
	}
	public virtual void turnLightOff(){
		pointlight.GetComponent<Light>().range = 0;
	}
	public virtual void tileAction(bool forward){}
	public virtual void resetTile(){
		beaconCount = 0;
	}
	public virtual void shake(){
		isShaking = true;
		shakeCounter = 0;
	}

	public virtual void blinkTutorial(bool on){
		if (on)
			pointlight.GetComponent<Light>().range = 25;
		else
			pointlight.GetComponent<Light>().range = 0;
	}
}
