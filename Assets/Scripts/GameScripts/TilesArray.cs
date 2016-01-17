using UnityEngine;
using System.Collections;

public class TilesArray : MonoBehaviour {
	public SkwerTile[,] tiles;
	public bool isDrawingPuzzle;
	public bool isRestartingPuzzle;
	bool isEndingPuzzle;
	bool isPostDroppingTiles;
	bool isPuzzleEnded;
	bool isBlinkingTutorial;
	bool blinkState;
	bool isBlinkingOuter;
	
	int postDroppingTilesCounter;
	float postDroppingTilesPeriod = 60.0f;
	int endingPuzzleCounter;
	int endingPuzzlePeriod = 40;
	int drawingPuzzleCounter;
	int drawingPuzzlePeriod = 20;
	float restartingCounter;
	float restartingPeriod = 60;
	float blinkingTutorialCounter;
	float blinkingTutorialPeriod = 15;
	Game game;
	public bool activeTilesArray;
	
	bool started;
	public int[] stack;
	public int[] stackOrder;
	bool unblockAfterDraw;
	public StackOrderElement[] stackOrderElements;
	public StackOrderElement firstStackOrderElement;
	public StackOrderElement lastStackOrderElement;
	
	public int livesUsed = 0;
	public Puzzle puzzle;
	
	Vector3 originalPosition;
	Vector3 newPosition;
	
	// Use this for initialization
	void Start () {
		originalPosition = transform.localPosition;
		tiles = new SkwerTile[9,8];
		stack = new int[3];
		stackOrder = new int[50];
		stackOrderElements = new StackOrderElement[50];
		for (int i=0; i<50; i++)
			stackOrderElements[i] = new StackOrderElement();
		tiles[2,2] = transform.FindChild("Tile00").GetComponent<GameTile>();
		tiles[3,2] = transform.FindChild("Tile10").GetComponent<GameTile>();
		tiles[4,2] = transform.FindChild("Tile20").GetComponent<GameTile>();
		tiles[5,2] = transform.FindChild("Tile30").GetComponent<GameTile>();
		tiles[6,2] = transform.FindChild("Tile40").GetComponent<GameTile>();
		tiles[2,3] = transform.FindChild("Tile01").GetComponent<GameTile>();
		tiles[3,3] = transform.FindChild("Tile11").GetComponent<GameTile>();
		tiles[4,3] = transform.FindChild("Tile21").GetComponent<GameTile>();
		tiles[5,3] = transform.FindChild("Tile31").GetComponent<GameTile>();
		tiles[6,3] = transform.FindChild("Tile41").GetComponent<GameTile>();
		tiles[2,4] = transform.FindChild("Tile02").GetComponent<GameTile>();
		tiles[3,4] = transform.FindChild("Tile12").GetComponent<GameTile>();
		tiles[4,4] = transform.FindChild("Tile22").GetComponent<GameTile>();
		tiles[5,4] = transform.FindChild("Tile32").GetComponent<GameTile>();
		tiles[6,4] = transform.FindChild("Tile42").GetComponent<GameTile>();
		tiles[2,5] = transform.FindChild("Tile03").GetComponent<GameTile>();
		tiles[3,5] = transform.FindChild("Tile13").GetComponent<GameTile>();
		tiles[4,5] = transform.FindChild("Tile23").GetComponent<GameTile>();
		tiles[5,5] = transform.FindChild("Tile33").GetComponent<GameTile>();
		tiles[6,5] = transform.FindChild("Tile43").GetComponent<GameTile>();
		
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
		
		game = GameObject.Find("Game").GetComponent<Game>();
		drawingPuzzleCounter = drawingPuzzlePeriod;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!started){
			started = true;
		}
		if (isDrawingPuzzle){
			if (drawingPuzzleCounter >= 0){
				tiles[drawingPuzzleCounter/4+2,drawingPuzzleCounter%4+2].unpause();
				if (unblockAfterDraw)
					tiles[drawingPuzzleCounter/4+2,drawingPuzzleCounter%4+2].unblockInput();
			}
			drawingPuzzleCounter++;
			if (drawingPuzzleCounter == drawingPuzzlePeriod){
				isDrawingPuzzle = false;
				unblockAfterDraw = false;
			}
		}
		if (isPostDroppingTiles){
			if (++postDroppingTilesCounter == postDroppingTilesPeriod){
				isPostDroppingTiles = false;
				if (game.isQuickGame && game.livesLeft > 0)
					reverseDropTiles();
				else
					freezeTiles();
			}
			transform.localPosition = Vector3.Lerp(originalPosition,newPosition,postDroppingTilesCounter/postDroppingTilesPeriod);
		}
		if (activeTilesArray){
			if (isAllState(puzzle.baseState) && !isEndingPuzzle && !isPuzzleEnded){
				isEndingPuzzle = true;
				endingPuzzleCounter = 0;
			}
			if (!isPuzzleEnded){
				if (isEndingPuzzle){
					endingPuzzleCounter++;
					if (endingPuzzleCounter == endingPuzzlePeriod){
						isPuzzleEnded = true;
						if (game.level == 9 && !game.isQuickGame)
							dropTiles(false);
						else
							dropTiles(true);
						game.nextLevel(true);
						isEndingPuzzle = false;
					}
				}
				else if (!isRestartingPuzzle){
					if (!checkAreMovesAvailable() && Global.isGameActive){
						isRestartingPuzzle = true;
						restartingCounter = 0;
					}
				}
				else {
					restartingCounter++;
					if (restartingCounter == restartingPeriod/2)
						for (int i=2; i<7; i++)
							for (int j=2; j<6; j++)
								((GameTile)tiles[i,j]).shake();
					if (restartingCounter >= restartingPeriod){
						game.resetPuzzle();
						isRestartingPuzzle = false;
					}
				}
			}
			if (isBlinkingTutorial){
				if (++blinkingTutorialCounter == blinkingTutorialPeriod){
					blinkingTutorialCounter = 0;
					blinkState = !blinkState;
					if (!isBlinkingOuter){
						for (int i=2; i<7; i++)
							for (int j=2; j<6; j++)
								if (tiles[i,j].state != puzzle.baseState)
									tiles[i,j].blinkTutorial(blinkState);
					}
					else{
						for (int i=0; i<9; i++)
							for (int j=0; j<8; j++)
								if ((i>6 || i<2 || j<2 || j>5) && ((SupportingTile) tiles[i,j]).stateCount != 0)
									tiles[i,j].blinkTutorial(blinkState);
					}
				}
			}
		}
	}
	
	public void drawPuzzle(){
		isDrawingPuzzle = true;
		drawingPuzzleCounter = 0;
	}
	
	public bool isAllState(int state){
		if (game.puzzleGeneratorMode)
			return false;
		
		bool allequal = true;
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				if (tiles[i,j].state != state)
					allequal = false;
		return allequal;
	}
	
	public void dropTiles(bool forward){
		if (activeTilesArray){
			game.blockGUIInput();
			if (forward) {
				isPostDroppingTiles = true;
				postDroppingTilesCounter = 0;
				activeTilesArray = false;
				if (game.isQuickGame)
					postDroppingTilesPeriod = 30.0f;
				originalPosition = transform.localPosition;
				newPosition = originalPosition + new Vector3(0,-100,0);
				for (int i=2; i<7; i++)
					for (int j=2; j<6; j++)
						tiles[i,j].dropTile(forward);
			}
			else {
				for (int i=0; i<9; i++)
					for (int j=0; j<8; j++)
						tiles[i,j].dropTile(forward);
			}
		}
	}
	public void deleteTiles(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				DestroyImmediate(tiles[i,j].gameObject);
	}
	public void reverseDropTiles(){
		for (int i=2; i<7; i++)
				for (int j=2; j<6; j++)
					tiles[i,j].reverseDropTile();
	}
	public void freezeTiles(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].freezeTile();
	}
	
	public void setBack(){
		for (int i=2; i<7; i++)
			for (int j=2; j<7; j++)
				tiles[i,j].addToBack();
	}
	public void setBack2(int baseState){
		for (int i=2; i<7; i++)
			for (int j=2; j<7; j++)
				tiles[i,j].addToBack2(baseState);
	}
	public void bringForward(){
		activeTilesArray = true;
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].bringForward();
	}
	
	public void newRandomPuzzle(int numMoves){
		puzzle = new Puzzle(numMoves);
		puzzle.baseState = Random.Range(0,3);
		puzzle.isStackOrder = true;
		for (int i=0; i<numMoves; i++)
			puzzle.tiles[i] = Random.Range(0,20);
		livesUsed = 0;
	}
	public void newQuickPlayPuzzle(){
		if (game.level < 2)
			newRandomPuzzle(3);
		else if (game.level < 4)
			newRandomPuzzle(4);
		else if (game.level < 7)
			newRandomPuzzle(5);
		else if (game.level < 11)
			newRandomPuzzle(6);
		else
			newRandomPuzzle(7);
	}
	
	public void loadPuzzle(){
		game.puzzleNumMoves = puzzle.size;
		game.puzzleBaseState = puzzle.baseState;
		setPuzzle();
	}
	
	public void resetPuzzle(){
		isDrawingPuzzle = true;
		isPuzzleEnded = false;
		isEndingPuzzle = false;
		unblockAfterDraw = true;
		drawingPuzzleCounter = 0;
		setPuzzle();
		Debug.Log("resetPuzzle" + transform.name);
	}
	public void resetPuzzleFromScratch(){
		resetPuzzle();
		livesUsed = 0;
	}
	
	public void setPuzzle(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].blockInput();
		
		stack[0] = 0;
		stack[1] = 0;
		stack[2] = 0;
		
		for (int i=0; i<9; i++)
			for (int j=0; j<8; j++){
				tiles[i,j].stateChange(false);
				while (tiles[i,j].state != puzzle.baseState)
					tiles[i,j].stateChange(false);
				tiles[i,j].resetTile();
				tiles[i,j].stopPreviewRotate();
				tiles[i,j].pause();
			}
		
		firstStackOrderElement = stackOrderElements[0];
		lastStackOrderElement = null;
		for (int i=0; i<puzzle.size; i++){
			tiles[puzzle.tiles[i]/4+2,puzzle.tiles[i]%4+2].tileAction(false);
			stackOrder[i] = tiles[puzzle.tiles[i]/4+2,puzzle.tiles[i]%4+2].state;
			stackOrderElements[i].state = stackOrder[i];
			stackOrderElements[i].addToList(lastStackOrderElement);
			lastStackOrderElement = stackOrderElements[i];
			stack[tiles[puzzle.tiles[i]/4+2,puzzle.tiles[i]%4+2].state]++;
		}
		
		game.puzzleBaseState = puzzle.baseState;
		game.puzzleStack[0] = stack[0];
		game.puzzleStack[1] = stack[1];
		game.puzzleStack[2] = stack[2];		
		game.puzzleNumMoves = puzzle.size;
	}
	
	public void rebuildStack(){
		livesUsed = 0;
		isPuzzleEnded = false;
		isEndingPuzzle = false;
		isPostDroppingTiles = false;
		isRestartingPuzzle = false;
		if (activeTilesArray)
			for (int i=0; i<9; i++)
				for (int j=0; j<8; j++)
					tiles[i,j].rebuildStack(activeTilesArray);
		else
			for (int i=2; i<7; i++)
				for (int j=2; j<6; j++)
					tiles[i,j].rebuildStack(activeTilesArray);
		activeTilesArray = false;
		if (!game.isQuickGame)
			transform.localPosition = originalPosition;
	}
	public void quickRebuildStack(){
		livesUsed = 0;
		isPuzzleEnded = false;
		isEndingPuzzle = false;
		isPostDroppingTiles = false;
		isRestartingPuzzle = false;
		if (activeTilesArray)
			for (int i=0; i<9; i++)
				for (int j=0; j<8; j++)
					tiles[i,j].quickRebuildStack(activeTilesArray);
		else{
			for (int i=2; i<7; i++)
				for (int j=2; j<6; j++)
					tiles[i,j].quickRebuildStack(activeTilesArray);
		}
		activeTilesArray = false;
		transform.localPosition = originalPosition;
	}
	
	public void turnLightsOn(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].turnLightOn();
	}
	public void turnLightsOff(){
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				tiles[i,j].turnLightOff();
	}

	public bool checkAreMovesAvailable(){
		if (game.puzzleGeneratorMode)
			return true;
		if (game.puzzleNumMoves == 0)
			return false;
		bool tileAvailable = false;
		
		if (!game.isZoomedIn){
			bool shouldReturnFalse = false;
			for (int i=0; i<9; i++)
				for (int j=0; j<2; j++)
					if (!((SupportingTile)tiles[i,j]).zoomCheckPossible())
						shouldReturnFalse = true;
			for (int i=0; i<9; i++)
				for (int j=6; j<8; j++)
					if (!((SupportingTile)tiles[i,j]).zoomCheckPossible())
						shouldReturnFalse = true;
			for (int i=0; i<2; i++)
				for (int j=2; j<6; j++)
					if (!((SupportingTile)tiles[i,j]).zoomCheckPossible())
						shouldReturnFalse = true;
			for (int i=7; i<9; i++)
				for (int j=2; j<6; j++)
					if (!((SupportingTile)tiles[i,j]).zoomCheckPossible())
						shouldReturnFalse = true;
			if (shouldReturnFalse)
				return false;
		}
		
		
		for (int i=2; i<7; i++)
			for (int j=2; j<6; j++)
				if (game.puzzleStack[tiles[i,j].state] > 0 && (!game.isBeaconOn || tiles[i,j].beaconCount > 0))
					tileAvailable = true;
		return tileAvailable;
	}

	public void setState(int state){
		puzzle.baseState = state;
		for (int i=0; i<9; i++)
			for (int j=0; j<8; j++){
				tiles[i,j].stateChange(false);
				while (tiles[i,j].state != puzzle.baseState)
					tiles[i,j].stateChange(false);
				tiles[i,j].resetTile();
			}
	}

	public void setTutorialHints(){
		isBlinkingTutorial = true;
		blinkState = false;
	}
	public void setTutorialHintsOuter(){
		isBlinkingOuter = true;
		isBlinkingTutorial = true;
		blinkState = false;
	}
}