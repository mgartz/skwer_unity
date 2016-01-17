using UnityEngine;
using System.Collections;
using System.IO;

public class StackMenuTilesArray : MonoBehaviour {
	
	public StackMenuTile[,] tiles;
	public Puzzle puzzle;
	public bool puzzleSet = false;
	public string stackLevel;
	public bool stackIsAvailable;
	
	// Use this for initialization
	void Start () {
		
		/*if (stackLevel == "stack00")
			stackIsAvailable = true;
		else
			stackIsAvailable = true;*/
		if (stackLevel == "stack00")
			stackIsAvailable = true;
		else{
			if (PlayerPrefs.HasKey("stack" + ((System.Convert.ToInt32(stackLevel.Substring(5,2))-1).ToString("D2")))
				|| PlayerPrefs.HasKey(stackLevel))
				stackIsAvailable = true;
			else
				stackIsAvailable = false;
		}
		readStack();
		
		
		tiles = new StackMenuTile[9,8];
		tiles[2,2] = transform.FindChild("Tile00").GetComponent<StackMenuTile>();
		tiles[3,2] = transform.FindChild("Tile10").GetComponent<StackMenuTile>();
		tiles[4,2] = transform.FindChild("Tile20").GetComponent<StackMenuTile>();
		tiles[5,2] = transform.FindChild("Tile30").GetComponent<StackMenuTile>();
		tiles[6,2] = transform.FindChild("Tile40").GetComponent<StackMenuTile>();
		tiles[2,3] = transform.FindChild("Tile01").GetComponent<StackMenuTile>();
		tiles[3,3] = transform.FindChild("Tile11").GetComponent<StackMenuTile>();
		tiles[4,3] = transform.FindChild("Tile21").GetComponent<StackMenuTile>();
		tiles[5,3] = transform.FindChild("Tile31").GetComponent<StackMenuTile>();
		tiles[6,3] = transform.FindChild("Tile41").GetComponent<StackMenuTile>();
		tiles[2,4] = transform.FindChild("Tile02").GetComponent<StackMenuTile>();
		tiles[3,4] = transform.FindChild("Tile12").GetComponent<StackMenuTile>();
		tiles[4,4] = transform.FindChild("Tile22").GetComponent<StackMenuTile>();
		tiles[5,4] = transform.FindChild("Tile32").GetComponent<StackMenuTile>();
		tiles[6,4] = transform.FindChild("Tile42").GetComponent<StackMenuTile>();
		tiles[2,5] = transform.FindChild("Tile03").GetComponent<StackMenuTile>();
		tiles[3,5] = transform.FindChild("Tile13").GetComponent<StackMenuTile>();
		tiles[4,5] = transform.FindChild("Tile23").GetComponent<StackMenuTile>();
		tiles[5,5] = transform.FindChild("Tile33").GetComponent<StackMenuTile>();
		tiles[6,5] = transform.FindChild("Tile43").GetComponent<StackMenuTile>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!puzzleSet)
			setPuzzle();
	}
	
	public void readStack(){
		TextAsset asset = Resources.Load(stackLevel) as TextAsset;
		Stream s = new MemoryStream(asset.bytes);
		using (BinaryReader b = new BinaryReader(s)){
			int pBase = b.ReadInt32();
			int size = b.ReadInt32();
			puzzle = new Puzzle(size);
			puzzle.baseState = pBase;
			puzzle.isZoomedIn = b.ReadBoolean();
			puzzle.isStackOrder = b.ReadBoolean();
			for (int j=0; j<size; j++)
				puzzle.tiles[j] = b.ReadInt32();
		}
	}
	
	public void setPuzzle(){
		puzzleSet = true;
		if (!stackIsAvailable)
			for (int i=2; i<7; i++)
				for (int j=2; j<6; j++)
					tiles[i,j].markUnavailable();
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++){
				tiles[i,j].baseState = puzzle.baseState;
				tiles[i,j].stateChange(false);
				while (tiles[i,j].state != puzzle.baseState)
					tiles[i,j].stateChange(false);
			}
		
		for (int i=0; i<puzzle.size; i++)
			tiles[puzzle.tiles[i]/4+2,puzzle.tiles[i]%4+2].tileAction(false);
	}
	
	public void mouseEntered(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].highLight();
	}
	public void mouseExited(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].highLighted = false;
	}
}