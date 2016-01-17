using UnityEngine;
using System.Collections;

public class StackMenuTile : MonoBehaviour {
	public int realIndexi;
	public int realIndexj;
	public int state;
	public float[,] rgbState;
	
	public Color[] planeColor;
	public float[] planeColorFactor;
	
	public Transform plane0;
	public Transform plane1;
	public Transform plane2;
	
	Transform tilesArray;
	
	public int baseState;
	
	public float plane2RotationPeriod = 20;
	public int plane2RotationCounter;
	
	public StackMenuTile[,] tiles;
	
	public bool highLighted;
	public bool wasHighLighted;
	public bool isAvailable;
	
	public int rotatingStage;
	
	// Use this for initialization
	void Start () {
		isAvailable = true;
		//plane2RotationCounter = Random.Range(0,60);
		realIndexi+=2;
		realIndexj+=2;
		tilesArray = transform.parent;
		plane2 = transform.FindChild("Plane2");
		plane2.localRotation = Quaternion.Euler(180,0,0);
				
		tiles = new StackMenuTile[9,8];
		tiles[2,2] = tilesArray.FindChild("Tile00").GetComponent<StackMenuTile>();
		tiles[3,2] = tilesArray.FindChild("Tile10").GetComponent<StackMenuTile>();
		tiles[4,2] = tilesArray.FindChild("Tile20").GetComponent<StackMenuTile>();
		tiles[5,2] = tilesArray.FindChild("Tile30").GetComponent<StackMenuTile>();
		tiles[6,2] = tilesArray.FindChild("Tile40").GetComponent<StackMenuTile>();
		tiles[2,3] = tilesArray.FindChild("Tile01").GetComponent<StackMenuTile>();
		tiles[3,3] = tilesArray.FindChild("Tile11").GetComponent<StackMenuTile>();
		tiles[4,3] = tilesArray.FindChild("Tile21").GetComponent<StackMenuTile>();
		tiles[5,3] = tilesArray.FindChild("Tile31").GetComponent<StackMenuTile>();
		tiles[6,3] = tilesArray.FindChild("Tile41").GetComponent<StackMenuTile>();
		tiles[2,4] = tilesArray.FindChild("Tile02").GetComponent<StackMenuTile>();
		tiles[3,4] = tilesArray.FindChild("Tile12").GetComponent<StackMenuTile>();
		tiles[4,4] = tilesArray.FindChild("Tile22").GetComponent<StackMenuTile>();
		tiles[5,4] = tilesArray.FindChild("Tile32").GetComponent<StackMenuTile>();
		tiles[6,4] = tilesArray.FindChild("Tile42").GetComponent<StackMenuTile>();
		tiles[2,5] = tilesArray.FindChild("Tile03").GetComponent<StackMenuTile>();
		tiles[3,5] = tilesArray.FindChild("Tile13").GetComponent<StackMenuTile>();
		tiles[4,5] = tilesArray.FindChild("Tile23").GetComponent<StackMenuTile>();
		tiles[5,5] = tilesArray.FindChild("Tile33").GetComponent<StackMenuTile>();
		tiles[6,5] = tilesArray.FindChild("Tile43").GetComponent<StackMenuTile>();
		
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
		
		planeColorFactor = new float[3];
		planeColorFactor[2] = 1;
		
		planeColor = new Color[3];
		
		Light light = transform.FindChild("Light").GetComponent<Light>();
		light.transform.localPosition = new Vector3(0,2.5f,0);
		light.range = 20;
	}
	
	void FixedUpdate () {
		if (highLighted){
			wasHighLighted = true;
			if (rotatingStage == 0)
				plane2.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(180,0,0), Quaternion.Euler(180,180,0), plane2RotationCounter/plane2RotationPeriod);
			else
				plane2.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(180,180,0), Quaternion.Euler(180,360,0), plane2RotationCounter/plane2RotationPeriod);
			if (plane2RotationCounter++ == plane2RotationPeriod){
				plane2RotationCounter = 0;
				rotatingStage = 1 - rotatingStage;
			}
			transform.FindChild("Light").GetComponent<Light>().enabled = true;
		}
		else if (wasHighLighted){
			wasHighLighted = false;
			if (isAvailable){
				plane2.transform.localRotation = Quaternion.Euler(180,0,0);
				transform.FindChild("Light").GetComponent<Light>().enabled = false;
			}
		}
	}
		
	
	public void stateChange(bool forward){
		if (forward)
			state = (state+1)%3;
		else
			state = (state+2)%3;
		planeColor[2] = new Color(rgbState[state,0]*planeColorFactor[2],rgbState[state,1]*planeColorFactor[2],rgbState[state,2]*planeColorFactor[2],1);
		
		if (!isAvailable)
			plane2.renderer.material.color = Color.Lerp(Color.white,planeColor[2],0.9f);
		else
			plane2.renderer.material.color = planeColor[2];
	}
	
	public void tileAction(bool forward){
		if (state == 0){
			if (realIndexi > 2){
				if (realIndexj > 2)
					tiles[realIndexi-1,realIndexj-1].stateChange(forward);
				tiles[realIndexi-1,realIndexj].stateChange(forward);
				if (realIndexj < 5)
					tiles[realIndexi-1,realIndexj+1].stateChange(forward);
			}
			if (realIndexi < 6){
				if (realIndexj > 2)
					tiles[realIndexi+1,realIndexj-1].stateChange(forward);
				tiles[realIndexi+1,realIndexj].stateChange(forward);
				if (realIndexj < 5)
					tiles[realIndexi+1,realIndexj+1].stateChange(forward);
			}
			if (realIndexj > 2)
				tiles[realIndexi,realIndexj-1].stateChange(forward);
			if (realIndexj < 5)
				tiles[realIndexi,realIndexj+1].stateChange(forward);
		}
		else if (state == 1){
			for (int i=2; i<realIndexi; i++)
				tiles[i,realIndexj].stateChange(forward);
			for (int i=realIndexi+1; i<7; i++)
				tiles[i,realIndexj].stateChange(forward);
			for (int j=2; j<realIndexj; j++)
				tiles[realIndexi,j].stateChange(forward);
			for (int j=realIndexj+1; j<6; j++)
				tiles[realIndexi,j].stateChange(forward);
		}
		else {
			for (int i=1; realIndexi-i>=2 && realIndexj-i>=2; i++)
				tiles[realIndexi-i,realIndexj-i].stateChange(forward);
			for (int i=1; realIndexi+i<7 && realIndexj-i>=2; i++)
				tiles[realIndexi+i,realIndexj-i].stateChange(forward);
			for (int i=1; realIndexi-i>=2 && realIndexj+i<6; i++)
				tiles[realIndexi-i,realIndexj+i].stateChange(forward);
			for (int i=1; realIndexi+i<7 && realIndexj+i<6; i++)
				tiles[realIndexi+i,realIndexj+i].stateChange(forward);
		}
	}
	
	public void markUnavailable(){
		isAvailable = false;
		plane2.localRotation = Quaternion.Euler(0,0,0);
	}
	public void highLight(){
		highLighted = true;
		rotatingStage = 0;
	}
}
