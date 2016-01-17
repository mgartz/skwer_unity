using UnityEngine;
using System.Collections;

public class GameTile : SkwerTile {
	bool inputUnblocked = false;
	
	public bool dropped;
	public int indexi;
	public int indexj;
	
	bool isBlinking;
	bool isSelected;
	bool isReversingDrop;
	
	float reverseDropCounter;
	float reverseDropPeriod = 1.0f;
	float blinkCounter;
	float blinkPeriod = 16;

	public Transform plane2;
	
	Transform tilesArray;	
	TilesArray tilesArrayComp;
	
	bool activeTile = false;

	bool wasBeaconOn = false;
	
	Quaternion plane2ReverseOriginalRotation;
	Vector3 plane2ReverseOriginalPosition;
	Vector3 plane2OriginalPosition;
	

	public override void Start () {
		base.Start();
		realIndexi = indexi+2;
		realIndexj = indexj+2;
		tilesArray = transform.parent;
		
		tiles[2,2] = tilesArray.FindChild("Tile00").GetComponent<GameTile>();
		tiles[3,2] = tilesArray.FindChild("Tile10").GetComponent<GameTile>();
		tiles[4,2] = tilesArray.FindChild("Tile20").GetComponent<GameTile>();
		tiles[5,2] = tilesArray.FindChild("Tile30").GetComponent<GameTile>();
		tiles[6,2] = tilesArray.FindChild("Tile40").GetComponent<GameTile>();
		tiles[2,3] = tilesArray.FindChild("Tile01").GetComponent<GameTile>();
		tiles[3,3] = tilesArray.FindChild("Tile11").GetComponent<GameTile>();
		tiles[4,3] = tilesArray.FindChild("Tile21").GetComponent<GameTile>();
		tiles[5,3] = tilesArray.FindChild("Tile31").GetComponent<GameTile>();
		tiles[6,3] = tilesArray.FindChild("Tile41").GetComponent<GameTile>();
		tiles[2,4] = tilesArray.FindChild("Tile02").GetComponent<GameTile>();
		tiles[3,4] = tilesArray.FindChild("Tile12").GetComponent<GameTile>();
		tiles[4,4] = tilesArray.FindChild("Tile22").GetComponent<GameTile>();
		tiles[5,4] = tilesArray.FindChild("Tile32").GetComponent<GameTile>();
		tiles[6,4] = tilesArray.FindChild("Tile42").GetComponent<GameTile>();
		tiles[2,5] = tilesArray.FindChild("Tile03").GetComponent<GameTile>();
		tiles[3,5] = tilesArray.FindChild("Tile13").GetComponent<GameTile>();
		tiles[4,5] = tilesArray.FindChild("Tile23").GetComponent<GameTile>();
		tiles[5,5] = tilesArray.FindChild("Tile33").GetComponent<GameTile>();
		tiles[6,5] = tilesArray.FindChild("Tile43").GetComponent<GameTile>();
		
		tilesArrayComp = tilesArray.GetComponent<TilesArray>();
		
		plane2 = transform.FindChild("Plane2");
		planeColorFactor[2] = 0.6f;
		plane2OriginalPosition = plane2.localPosition;
	}
	
	public override void FixedUpdate () {
		if (!paused){
			if (game.isBeaconOn){
				if (beaconCount > 0)
					plane2.renderer.material.color = Color.Lerp(planeColor[2], Color.white, game.beaconAmp + game.beaconAmp*Mathf.Sin(game.beaconCounter/game.beaconPeriod*6.2832f));
				else
					plane2.renderer.material.color = Color.Lerp(planeColor[2], Color.black, 0.5f);
				wasBeaconOn = true;
			}
			if (!game.isBeaconOn && wasBeaconOn){
				wasBeaconOn = false;
				plane2.renderer.material.color = planeColor[2];
			}
			if (isBlinking){
				plane0.renderer.material.color = Color.Lerp(Color.white, planeColor[0],blinkCounter/blinkPeriod);
				plane1.renderer.material.color = Color.Lerp(Color.white, planeColor[1],blinkCounter/blinkPeriod);
				plane2.renderer.material.color = Color.Lerp(Color.white, planeColor[2],blinkCounter/blinkPeriod);
				if (blinkCounter++ >= blinkPeriod){
					isBlinking = false;
					if (isSelected && (game.puzzleStack[state] > 0) && (!game.isBeaconOn || beaconCount>0))
						checkPreviewRotate();
				}
			}
			if (isShaking){
				shakeCounter++;
				float rand1 = Random.Range(-1.0f,1.0f);
				float rand2 = Random.Range(-1.0f,1.0f);
				
				plane0.localPosition = new Vector3(rand1,7.4166f,rand2);
				plane1.localPosition = new Vector3(rand1,0,rand2);
				if (shakeCounter >= shakePeriod){
					plane0.localPosition = new Vector3(0,7.4166f,0);
					plane1.localPosition = new Vector3(0,0,0);
					isShaking = false;
				}
			}
			if (isReversingDrop){
				if (++reverseDropCounter == reverseDropPeriod)
					isReversingDrop = false;
				
				plane0.localPosition = Vector3.Lerp(plane0ReverseOriginalPosition,plane0OriginalPosition,reverseDropCounter/reverseDropPeriod);
				plane1.localPosition = Vector3.Lerp(plane1ReverseOriginalPosition,plane1OriginalPosition,reverseDropCounter/reverseDropPeriod);
				plane2.localPosition = Vector3.Lerp(plane2ReverseOriginalPosition,plane2OriginalPosition,reverseDropCounter/reverseDropPeriod);
				
				plane0.localRotation = Quaternion.Lerp(plane0ReverseOriginalRotation, plane0TargetRotation, reverseDropCounter/reverseDropPeriod);
				plane1.localRotation = Quaternion.Lerp(plane1ReverseOriginalRotation, plane1TargetRotation, reverseDropCounter/reverseDropPeriod);
				plane2.localRotation = Quaternion.Lerp(plane2ReverseOriginalRotation, plane0TargetRotation, reverseDropCounter/reverseDropPeriod);
				
				if (reverseDropCounter == reverseDropPeriod){
					isBlinking = false;
					isPreRotating = false;
					isRotating = false;
					isStateChanging = false;
					isStopingRotating = false;
					plane0.localRotation = plane0OriginRotation;
					plane1.localRotation = plane1OriginRotation;
					plane2.localRotation = plane0OriginRotation;
				}
			}
			if (isRebuildingStack){
				plane2.localPosition = Vector3.Lerp(plane2ReverseOriginalPosition,plane2OriginalPosition,rebuildingStackCounter/rebuildingStackPeriod);
				plane2.localRotation = Quaternion.Lerp(plane2ReverseOriginalRotation, plane0TargetRotation, rebuildingStackCounter/rebuildingStackPeriod);
				
				if (rebuildingStackCounter == rebuildingStackPeriod) {
					isBlinking = false;
					plane2.localRotation = plane0OriginRotation;
				}
			}
		}
		base.FixedUpdate();
	}
	
	
	void OnMouseEnter() {
		isSelected = true;
		
		if (!Global.isGamePaused && Global.isGameActive && inputUnblocked && (game.puzzleStack[state] > 0) && (!game.isBeaconOn || beaconCount>0))
			checkPreviewRotate();
    }
	void OnMouseExit(){
		isSelected = false;
		
		if (inputUnblocked && Global.isGameActive){
			plane0.renderer.material.color = planeColor[0];
			plane1.renderer.material.color = planeColor[1];
			plane2.renderer.material.color = planeColor[2];
			if (state == 0){
				tiles[realIndexi-1,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi-1,realIndexj].stopPreviewRotate();
				tiles[realIndexi-1,realIndexj+1].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj+1].stopPreviewRotate();
				tiles[realIndexi,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi,realIndexj+1].stopPreviewRotate();
			}
			else if (state == 1){
				for (int i=0; i<realIndexi; i++)
					tiles[i,realIndexj].stopPreviewRotate();
				for (int i=realIndexi+1; i<9; i++)
					tiles[i,realIndexj].stopPreviewRotate();
				for (int j=0; j<realIndexj; j++)
					tiles[realIndexi,j].stopPreviewRotate();
				for (int j=realIndexj+1; j<8; j++)
					tiles[realIndexi,j].stopPreviewRotate();
			}
			else {
				for (int i=1; realIndexi-i>=0 && realIndexj-i>=0; i++)
					tiles[realIndexi-i,realIndexj-i].stopPreviewRotate();
				for (int i=1; realIndexi+i<9 && realIndexj-i>=0; i++)
					tiles[realIndexi+i,realIndexj-i].stopPreviewRotate();
				for (int i=1; realIndexi-i>=0 && realIndexj+i<8; i++)
					tiles[realIndexi-i,realIndexj+i].stopPreviewRotate();
				for (int i=1; realIndexi+i<9 && realIndexj+i<8; i++)
					tiles[realIndexi+i,realIndexj+i].stopPreviewRotate();
			}
		}
	}
	void OnMouseUpAsButton(){
		if (!Global.isGamePaused && Global.isGameActive && inputUnblocked && (game.puzzleStack[state] > 0) && (!game.isBeaconOn || beaconCount>0) && tilesArrayComp.activeTilesArray){
			if (!game.puzzleGeneratorMode)
				game.removePuzzleStack(state);
			tileAction(true);
			isBlinking = true;
			blinkCounter = 0;
		}
		else if (!dropped)
			shake();
	}
	
	void checkPreviewRotate(){
		plane0.renderer.material.color = Color.Lerp(planeColor[0], Color.black, 1);

		if (state == 0){
			tiles[realIndexi-1,realIndexj-1].previewRotate();
			tiles[realIndexi-1,realIndexj].previewRotate();
			tiles[realIndexi-1,realIndexj+1].previewRotate();
			tiles[realIndexi+1,realIndexj-1].previewRotate();
			tiles[realIndexi+1,realIndexj].previewRotate();
			tiles[realIndexi+1,realIndexj+1].previewRotate();
			tiles[realIndexi,realIndexj-1].previewRotate();
			tiles[realIndexi,realIndexj+1].previewRotate();
		}
		else if (state == 1){
			for (int i=0; i<realIndexi; i++)
				tiles[i,realIndexj].previewRotate();
			for (int i=realIndexi+1; i<9; i++)
				tiles[i,realIndexj].previewRotate();
			for (int j=0; j<realIndexj; j++)
				tiles[realIndexi,j].previewRotate();
			for (int j=realIndexj+1; j<8; j++)
				tiles[realIndexi,j].previewRotate();
		}
		else {
			for (int i=1; realIndexi-i>=0 && realIndexj-i>=0; i++)
				tiles[realIndexi-i,realIndexj-i].previewRotate();
			for (int i=1; realIndexi+i<9 && realIndexj-i>=0; i++)
				tiles[realIndexi+i,realIndexj-i].previewRotate();
			for (int i=1; realIndexi-i>=0 && realIndexj+i<8; i++)
				tiles[realIndexi-i,realIndexj+i].previewRotate();
			for (int i=1; realIndexi+i<9 && realIndexj+i<8; i++)
				tiles[realIndexi+i,realIndexj+i].previewRotate();
		}
	}
	
	public override void tileAction(bool forward){
		if (forward) {
			beaconCount--;
			if (state == 0){
				game.redParticle.position = transform.position + new Vector3(0,30,0);
				game.redParticle.GetComponent<ParticleSystem>().Play();
			}
			else if (state == 1){
				game.greenParticle.position = transform.position + new Vector3(0,30,0);
				game.greenParticle.GetComponent<ParticleSystem>().Play();
			}
			else if (state == 2){
				game.blueParticle.position = transform.position + new Vector3(0,30,0);
				game.blueParticle.GetComponent<ParticleSystem>().Play();
			}
		}
		else
			beaconCount++;
		if (state == 0){
			tiles[realIndexi-1,realIndexj-1].stateChange(forward);
			tiles[realIndexi-1,realIndexj].stateChange(forward);
			tiles[realIndexi-1,realIndexj+1].stateChange(forward);
			tiles[realIndexi+1,realIndexj-1].stateChange(forward);
			tiles[realIndexi+1,realIndexj].stateChange(forward);
			tiles[realIndexi+1,realIndexj+1].stateChange(forward);
			tiles[realIndexi,realIndexj-1].stateChange(forward);
			tiles[realIndexi,realIndexj+1].stateChange(forward);
		}
		else if (state == 1){
			for (int i=0; i<realIndexi; i++)
				tiles[i,realIndexj].stateChange(forward);
			for (int i=realIndexi+1; i<9; i++)
				tiles[i,realIndexj].stateChange(forward);
			for (int j=0; j<realIndexj; j++)
				tiles[realIndexi,j].stateChange(forward);
			for (int j=realIndexj+1; j<8; j++)
				tiles[realIndexi,j].stateChange(forward);
		}
		else {
			for (int i=1; realIndexi-i>=0 && realIndexj-i>=0; i++)
				tiles[realIndexi-i,realIndexj-i].stateChange(forward);
			for (int i=1; realIndexi+i<9 && realIndexj-i>=0; i++)
				tiles[realIndexi+i,realIndexj-i].stateChange(forward);
			for (int i=1; realIndexi-i>=0 && realIndexj+i<8; i++)
				tiles[realIndexi-i,realIndexj+i].stateChange(forward);
			for (int i=1; realIndexi+i<9 && realIndexj+i<8; i++)
				tiles[realIndexi+i,realIndexj+i].stateChange(forward);
		}
	}
	public override void pause(){
		plane0.renderer.material.color = planeColor[0];
		plane1.renderer.material.color = planeColor[1];
		plane2.renderer.material.color = planeColor[2];
		paused = true;
	}
	public override void unpause(){
		paused = false;
	}
	public override void blockInput(){
		inputUnblocked = false;
	}
	public override void unblockInput(){
		inputUnblocked = true;
		if (isSelected && (game.puzzleStack[state] > 0) && (!game.isBeaconOn || beaconCount>0))
			checkPreviewRotate();
	}
	
	public override void addToBack(){
		paused = true;
		blockInput();
		if (game.activeTilesArray.puzzle.baseState != state){
			plane0.renderer.material.color = Color.Lerp(Color.black, planeColor[0],0.1f);
			plane1.renderer.material.color = Color.Lerp(Color.black, planeColor[1],0.1f);	
		}
		else{
			plane0.renderer.material.color = Color.Lerp(Color.black, planeColor[0],0.9f);
			plane1.renderer.material.color = Color.Lerp(Color.black, planeColor[1],0.9f);
		}
		plane2.renderer.material.color = new Color(rgbState[game.activeTilesArray.puzzle.baseState,0]*planeColorFactor[2],rgbState[game.activeTilesArray.puzzle.baseState,1]*planeColorFactor[2],rgbState[game.activeTilesArray.puzzle.baseState,2]*planeColorFactor[2],1);
	}
	public override void addToBack2(int baseState){
		plane0.renderer.material.color = new Color(rgbState[baseState,0]*planeColorFactor[0],rgbState[baseState,1]*planeColorFactor[0],rgbState[baseState,2]*planeColorFactor[0],1);
		plane1.renderer.material.color = new Color(rgbState[baseState,0]*planeColorFactor[1],rgbState[baseState,1]*planeColorFactor[1],rgbState[baseState,2]*planeColorFactor[1],1);
		plane2.renderer.material.color = new Color(rgbState[baseState,0]*planeColorFactor[2],rgbState[baseState,1]*planeColorFactor[2],rgbState[baseState,2]*planeColorFactor[2],1);
	}
	public override void bringForward(){
		activeTile = true;
		paused = false;
		dropped = false;
		this.unblockInput();
		
		planeColor[0] = new Color(rgbState[state,0]*planeColorFactor[0],rgbState[state,1]*planeColorFactor[0],rgbState[state,2]*planeColorFactor[0],1);
		planeColor[1] = new Color(rgbState[state,0]*planeColorFactor[1],rgbState[state,1]*planeColorFactor[1],rgbState[state,2]*planeColorFactor[1],1);
		planeColor[2] = new Color(rgbState[tilesArrayComp.puzzle.baseState,0]*planeColorFactor[2],rgbState[tilesArrayComp.puzzle.baseState,1]*planeColorFactor[2],rgbState[tilesArrayComp.puzzle.baseState,2]*planeColorFactor[2],1);
		
		plane0.renderer.material.color = planeColor[0];
		plane1.renderer.material.color = planeColor[1];
		plane2.renderer.material.color = planeColor[2];
	}

	public override void dropTile(bool forward){
		if (isShaking)
			isShaking = false;
		dropped = true;
		base.dropTile(forward);
		if (isSelected){
			isSelected = false;
			plane0.renderer.material.color = planeColor[0];
			plane1.renderer.material.color = planeColor[1];
			plane2.renderer.material.color = planeColor[2];
			if (state == 0){
				tiles[realIndexi-1,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi-1,realIndexj].stopPreviewRotate();
				tiles[realIndexi-1,realIndexj+1].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj].stopPreviewRotate();
				tiles[realIndexi+1,realIndexj+1].stopPreviewRotate();
				tiles[realIndexi,realIndexj-1].stopPreviewRotate();
				tiles[realIndexi,realIndexj+1].stopPreviewRotate();
			}
			else if (state == 1){
				for (int i=0; i<realIndexi; i++)
					tiles[i,realIndexj].stopPreviewRotate();
				for (int i=realIndexi+1; i<9; i++)
					tiles[i,realIndexj].stopPreviewRotate();
				for (int j=0; j<realIndexj; j++)
					tiles[realIndexi,j].stopPreviewRotate();
				for (int j=realIndexj+1; j<8; j++)
					tiles[realIndexi,j].stopPreviewRotate();
			}
			else {
				for (int i=1; realIndexi-i>=0 && realIndexj-i>=0; i++)
					tiles[realIndexi-i,realIndexj-i].stopPreviewRotate();
				for (int i=1; realIndexi+i<9 && realIndexj-i>=0; i++)
					tiles[realIndexi+i,realIndexj-i].stopPreviewRotate();
				for (int i=1; realIndexi-i>=0 && realIndexj+i<8; i++)
					tiles[realIndexi-i,realIndexj+i].stopPreviewRotate();
				for (int i=1; realIndexi+i<9 && realIndexj+i<8; i++)
					tiles[realIndexi+i,realIndexj+i].stopPreviewRotate();
			}
		}
		
		blockInput();
		
		if (forward) {
			plane0.gameObject.AddComponent<Rigidbody>();
			plane1.gameObject.AddComponent<Rigidbody>();
			plane2.gameObject.AddComponent<Rigidbody>();
		
			if (game.isQuickGame){
				plane0.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
				plane1.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
				plane2.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
			}
			else{
				plane0.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
				plane1.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
				plane2.rigidbody.AddRelativeForce(Random.Range(-5000.0f,5000.0f),60000,Random.Range(-5000.0f,5000.0f));
			}
			plane0.rigidbody.angularVelocity = new Vector3(Random.Range(0,100.1f),Random.Range(0,100.1f),Random.Range(0,100.1f));
			plane1.rigidbody.angularVelocity = new Vector3(Random.Range(0,100.1f),Random.Range(0,100.1f),Random.Range(0,100.1f));
			plane2.rigidbody.angularVelocity = new Vector3(Random.Range(0,100.1f),Random.Range(0,100.1f),Random.Range(0,100.1f));
		}
		else
			game.isBeaconOn = false;
	}
	public override void freezeTile(){
		plane0.rigidbody.angularVelocity = Vector3.zero;
		plane0.rigidbody.velocity = Vector3.zero;
		plane1.rigidbody.angularVelocity = Vector3.zero;
		plane1.rigidbody.velocity = Vector3.zero;
		plane2.rigidbody.angularVelocity = Vector3.zero;
		plane2.rigidbody.velocity = Vector3.zero;
		Destroy(plane0.rigidbody);
		Destroy(plane1.rigidbody);
		Destroy(plane2.rigidbody);
		Destroy(this.GetComponent<BoxCollider>());
	}
	public override void rebuildStack(bool isActiveTilesArray){
		base.rebuildStack(isActiveTilesArray);
		
		if (!isActiveTilesArray){
			this.gameObject.AddComponent<BoxCollider>();
			this.GetComponent<BoxCollider>().size = new Vector3(10,10,10);

			plane2ReverseOriginalPosition = new Vector3(plane2OriginalPosition.x,plane2.localPosition.y+50*Random.Range(0,30),plane2OriginalPosition.z);
			plane2ReverseOriginalRotation = plane0TargetRotation;
		}
		else {
			plane2ReverseOriginalPosition = plane2.localPosition;
			plane2ReverseOriginalRotation = plane2.localRotation;	
		}		
	}
	public override void quickRebuildStack(bool isActiveTilesArray){
		base.quickRebuildStack(isActiveTilesArray);
		if (this.gameObject.GetComponent<BoxCollider>() == null){
			this.gameObject.AddComponent<BoxCollider>();
			this.GetComponent<BoxCollider>().size = new Vector3(10,10,10);
		}
		isBlinking = false;
		plane2.localPosition = plane2OriginalPosition;
		plane2.localRotation = plane0OriginRotation;
	}
	public override void reverseDropTile(){
		plane0.rigidbody.angularVelocity = Vector3.zero;
		plane0.rigidbody.velocity = Vector3.zero;
		plane1.rigidbody.angularVelocity = Vector3.zero;
		plane1.rigidbody.velocity = Vector3.zero;
		plane2.rigidbody.angularVelocity = Vector3.zero;
		plane2.rigidbody.velocity = Vector3.zero;
		Destroy(plane0.rigidbody);
		Destroy(plane1.rigidbody);
		Destroy(plane2.rigidbody);
		
		isBlinking = false;
		isPreRotating = false;
		isRotating = false;
		isStateChanging = false;
		isStopingRotating = false;
		plane0.localRotation = plane0OriginRotation;
		plane1.localRotation = plane1OriginRotation;
		plane2.localRotation = plane0OriginRotation;
		
		plane0.localPosition = plane0OriginalPosition;
		plane1.localPosition = plane1OriginalPosition;
		plane2.localPosition = plane2OriginalPosition;
	}
	public override void stateChangingStep(){
		base.stateChangingStep();
		if (stateChangeCounter > stateChangePeriod/2)
			planeColor[2] = new Color(rgbState[tilesArrayComp.puzzle.baseState,0]*planeColorFactor[2],rgbState[tilesArrayComp.puzzle.baseState,1]*planeColorFactor[2],rgbState[tilesArrayComp.puzzle.baseState,2]*planeColorFactor[2],1);
		
		if (!paused)
			plane2.renderer.material.color = planeColor[2];
		else
			plane2.renderer.material.color = Color.Lerp(Color.black, planeColor[2],0.3f);
		
		if (stateChangeCounter == stateChangePeriod){
			if (inputUnblocked && isSelected && (game.puzzleStack[state] > 0) && (!game.isBeaconOn || beaconCount>0))
				checkPreviewRotate();
			if (!activeTile)
				addToBack();
		}
	}
}