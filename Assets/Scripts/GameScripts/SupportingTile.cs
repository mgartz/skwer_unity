using UnityEngine;
using System.Collections;

public class SupportingTile : SkwerTile {
	float baseColorFactor = 0.1f;
	public int stateCount;
	
	bool isShowingError;
	int showingErrorCounter;
	int showingErrorPeriod = 32;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		plane0 = transform.FindChild("Plane0");
		plane1 = transform.FindChild("Plane1");
		game = GameObject.Find("Game").GetComponent<Game>();
		stateCount = 0;
		planeColorFactor[0] = 0.7f;
		planeColorFactor[1] = 0.4f;
	}
	
	public override void FixedUpdate ()	{
		base.FixedUpdate ();
		if (isShowingError && !isSuspended){
			if (showingErrorCounter++ >= showingErrorPeriod) {
				isShowingError = false;
				plane0.localPosition = new Vector3(0,7.4166f,0);
				plane1.localPosition = new Vector3(0,0,0);
			}
			else {
				float rand1 = Random.Range(-1.0f,1.0f);
				float rand2 = Random.Range(-1.0f,1.0f);
					
				plane0.localPosition = new Vector3(rand1,7.4166f,rand2);
				plane1.localPosition = new Vector3(rand1,0,rand2);
				
				if (showingErrorCounter == showingErrorPeriod/4){
					turnLightOn();
				}
				else if (showingErrorCounter == showingErrorPeriod/2){
					turnLightOff();
				}
				else if (showingErrorCounter == 3*showingErrorPeriod/4){
					turnLightOn();
				}
				else if (showingErrorCounter == showingErrorPeriod){
					turnLightOff();
				}
			}
		}
	}
	
	public override void previewRotate(){
		base.previewRotate();
		plane0.localScale = new Vector3(5,5,5);
		//plane1.localScale = new Vector3(8,8,8);
	}
	public override void stopPreviewRotate(){
		base.stopPreviewRotate();
		if (stateCount == 0){
			plane0.localScale = Vector3.zero;
			plane1.localScale = Vector3.zero;
		}

	}
	
	public override void resetTile (){
		base.resetTile();
		stateCount = 0;
	}
	
	public override void stateChange (bool forward){
		base.stateChange (forward);
		if (forward)
			stateCount--;
		else
			stateCount++;
	}
	
	public override void stateChangingStep(){
		base.stateChangingStep();
		if (stateCount == 0){
			planeColor[0] = new Color(rgbState[(state+1)%3,0]*planeColorFactor[0],rgbState[(state+1)%3,1]*planeColorFactor[0],rgbState[(state+1)%3,2]*planeColorFactor[0]);
			planeColor[1] = new Color(rgbState[(state+1)%3,0]*planeColorFactor[1],rgbState[(state+1)%3,1]*planeColorFactor[1],rgbState[(state+1)%3,2]*planeColorFactor[1]);
			plane0.renderer.material.color = Color.Lerp(planeColor[0],Color.black,stateChangeCounter/stateChangePeriod*(1-baseColorFactor));
			plane1.renderer.material.color = Color.Lerp(planeColor[1],Color.black,stateChangeCounter/stateChangePeriod*(1-baseColorFactor));
			plane0.localScale = Vector3.zero;
			plane1.localScale = Vector3.zero;
		}
		else if (stateCount < 0){
			planeColor[0] = new Color(rgbState[state,0]*planeColorFactor[0],rgbState[state,1]*planeColorFactor[0],rgbState[state,2]*planeColorFactor[0]);
			planeColor[1] = new Color(rgbState[state,0]*planeColorFactor[1],rgbState[state,1]*planeColorFactor[1],rgbState[state,2]*planeColorFactor[1]);
			plane0.renderer.material.color = planeColor[0];
			plane1.renderer.material.color = planeColor[1];
			plane0.localScale = new Vector3(5,5,5);
			plane1.localScale = new Vector3(8,8,8);
		}
		else{
			planeColor[0] = new Color(rgbState[game.puzzleBaseState,0]*planeColorFactor[0],rgbState[game.puzzleBaseState,1]*planeColorFactor[0],rgbState[game.puzzleBaseState,2]*planeColorFactor[0]);
			planeColor[1] = new Color(rgbState[game.puzzleBaseState,0]*planeColorFactor[1],rgbState[game.puzzleBaseState,1]*planeColorFactor[1],rgbState[game.puzzleBaseState,2]*planeColorFactor[1]);
			plane0.renderer.material.color = planeColor[0];
			plane1.renderer.material.color = planeColor[1];			
			plane0.localScale = new Vector3(5,5,5);
			plane1.localScale = new Vector3(8,8,8);
		}
	}
	
	public bool showError(bool withShake){
		isShowingError = true;
		showingErrorCounter = 0;
		state = (game.activeTilesArray.puzzle.baseState+1)%3;
		planeColor[0] = new Color(rgbState[state,0]*planeColorFactor[0],rgbState[state,1]*planeColorFactor[0],rgbState[state,2]*planeColorFactor[0]);
		planeColor[1] = new Color(rgbState[state,0]*planeColorFactor[1],rgbState[state,1]*planeColorFactor[1],rgbState[state,2]*planeColorFactor[1]);
		plane0.renderer.material.color = planeColor[0];
		plane1.renderer.material.color = planeColor[1];
		return false;
	}
	
	public bool zoomCheckPossible(){
		if (stateCount < 0)
			return showError(false);
		else if (stateCount > 0){
			if (realIndexi < 2){
				if (realIndexj == 0 && ((SupportingTile)tiles[realIndexi+1,1]).stateCount <= 0)
					return showError(true);
				else if (realIndexj == 7 && ((SupportingTile)tiles[realIndexi+1,6]).stateCount <= 0)
					return showError(true);
			}
			else if (realIndexi < 4){
				if (realIndexj == 0 && ((SupportingTile)tiles[realIndexi,1]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi+1,1]).stateCount <= 0)
					return showError(true);
				else if (realIndexj == 7 && ((SupportingTile)tiles[realIndexi,6]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi+1,6]).stateCount <= 0)
					return showError(true);
			}
			else if (realIndexi == 4){
				if (realIndexj == 0 && ((SupportingTile)tiles[realIndexi,1]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi+1,1]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi-1,1]).stateCount <= 0)
					return showError(true);
				else if (realIndexj == 7 && ((SupportingTile)tiles[realIndexi,6]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi+1,6]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi-1,6]).stateCount <= 0)
					return showError(true);
			}
			else if (realIndexi < 7){
				if (realIndexj == 0 && ((SupportingTile)tiles[realIndexi,1]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi-1,1]).stateCount <= 0)
					return showError(true);
				else if (realIndexj == 7 && ((SupportingTile)tiles[realIndexi,6]).stateCount <= 0 && ((SupportingTile)tiles[realIndexi-1,6]).stateCount <= 0)
					return showError(true);
			}
			else {
				if (realIndexj == 0 && ((SupportingTile)tiles[realIndexi-1,1]).stateCount <= 0)
					return showError(true);
				else if (realIndexj == 7 && ((SupportingTile)tiles[realIndexi-1,6]).stateCount <= 0)
					return showError(true);
			}

			if (realIndexj < 2){
				if (realIndexi == 0 && ((SupportingTile)tiles[1,realIndexj+1]).stateCount <= 0)
					return showError(true);
				else if (realIndexi == 8 && ((SupportingTile)tiles[7,realIndexj+1]).stateCount <= 0)
					return showError(true);
			}
			else if (realIndexj < 4){
				if (realIndexi == 0 && ((SupportingTile)tiles[1,realIndexj]).stateCount <= 0 && ((SupportingTile)tiles[1,realIndexj+1]).stateCount <= 0)
					return showError(true);
				else if (realIndexi == 8 && ((SupportingTile)tiles[7,realIndexj]).stateCount <= 0 && ((SupportingTile)tiles[7,realIndexj+1]).stateCount <= 0)
					return showError(true);
			}
			else if (realIndexj < 6){
				if (realIndexi == 0 && ((SupportingTile)tiles[1,realIndexj]).stateCount <= 0 && ((SupportingTile)tiles[1,realIndexj-1]).stateCount <= 0)
					return showError(true);
				else if (realIndexi == 8 && ((SupportingTile)tiles[7,realIndexj]).stateCount <= 0 && ((SupportingTile)tiles[7,realIndexj-1]).stateCount <= 0)
					return showError(true);
			}
			else {
				if (realIndexi == 0 && ((SupportingTile)tiles[1,realIndexj-1]).stateCount <= 0)
					return showError(true);
				else if (realIndexi == 8 && ((SupportingTile)tiles[7,realIndexj-1]).stateCount <= 0)
					return showError(true);
			}
		}
		return true;
	}
}
