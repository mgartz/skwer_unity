using UnityEngine;
using System.Collections;
using System.IO;

public class Game : MonoBehaviour
{
	public bool isQuickGame;
	public bool isTutorial;
	public bool isGUIOn;
	public bool isBlockingGUIInput;
	bool isStartingAnimation;
	bool isGoingToMenu;
	bool isGoingToFirstLevel;
	bool started;
	bool isOpeningMenu;
	bool isAdvancingLevel;
	bool isHidingGUI;
	bool isShowingGUI;
	bool isShowingGameSummary;
	bool isHidingGameSummary;
	bool isShowingGameEndedScreen;
	bool isHidingGameEndedScreen;
	bool isRebuildingStack;
	bool isShowingPauseScreen;
	bool isFlashing;
	bool isZoomingIn;
	bool isZoomingOut;
	public bool isBeaconOn;
	
	public int puzzleNumMoves;
	public int puzzleBaseState;
	public int[] puzzleStack;
	
	int startAnimationCounter;
	int goingToMenuCounter;
	int showingPauseScreenCounter;
	int isBlockingGUIInputCounter;
	int rebuildingStackCounter;
	int showingGameSummaryCounter;
	int hidingGameSummaryCounter;
	int showingGameEndedScreenCounter;
	int hidingGameEndedScreenCounter;
	int hidingGUICounter;
	int showingGUICounter;
	int advancingLevelCounter;
	int flashCounter;
	int zoomCounter;
	float openMenuCounter;
	public float beaconCounter;
	
	float startAnimationPeriod = 150.0f;
	float goingToMenuPeriod = 30.0f;
	float showingPauseScreenPeriod = 30.0f;	
	float rebuildingStackPeriod;
	float showingGameSummaryPeriod = 20.0f;
	float hidingGameSummaryPeriod = 20.0f;
	float showingGameEndedScreenPeriod = 20.0f;
	float hidingGameEndedScreenPeriod = 20.0f;
	float hidingGUIPeriod = 20.0f;
	float showingGUIPeriod = 20.0f;
	float flashPeriod = 200f;
	float zoomPeriod = 60;
	public float beaconPeriod = 128;
	int openMenuPeriod = 30;
	int isBlockingGUIInputPeriod = 60;
	int advancingLevelPeriod = 25;
	
	Transform skipButton;
	Transform resetButton;
	Transform tutorialText1;
	Transform tutorialText2;
	Transform stack0;
	Transform stack1;
	Transform stack2;
	Transform stacks;
	Transform stackOrder;
	Transform upperLight;
	LivesArray livesArray;
	Transform[] stackOrderTiles;
	Transform[] levelIndicators;
	public Transform[] summaryLevelIndicators;
	public Transform blueParticle;
	public Transform greenParticle;
	public Transform redParticle;
	Transform[] tilesArray;
	public TilesArray activeTilesArray;
	public Transform supportingTilesArray;
	
	GameObject quickGameScoreNumber;
	GameObject cameraArray;
	GameObject guiLeftPanel;
	GameObject guiRightPanel;
	GameObject pauseScreen;
	GameObject gameSummary;
	GameObject gameEndedScreen;
	GameObject gameSummaryQuickGameScoreNumber;
	GameObject gameSummaryNewRecord;
	GameObject effectsLightArray;
	
	public int numPuzzlesSkipped = 0;
	public int levelStars;	
	public int quickGameScore = 0;
	public int level = 0;
	public int livesLeft;
	
	float rebuildingStackPeriodPerLevel = 10.0f;
	Vector3 rebuildingStartingLocation;
	Vector3 rebuildingTargetLocation;	
	
	public bool isZoomedIn = true;
	public bool isStackOrder = false;
	
	public float beaconAmp = 0.4f;
	public bool puzzleGeneratorMode;
	public float[,] rgbState;
	
	public Medal[] medals;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (isQuickGame) {
				if (Global.isGameActive)
					togglePauseScreen ();
			} else {
				if (isStartingAnimation)
					quickStart ();
				else if (isRebuildingStack)
					quickRebuild ();
				else if (Global.isGameActive)
					togglePauseScreen ();
			}
		}
	}
	void OnGUI (){
		if ((Event.current.Equals (Event.KeyboardEvent ("space")) || Event.current.Equals (Event.KeyboardEvent ("return")) || Input.GetMouseButtonDown (0)) && !isQuickGame) {
			if (isStartingAnimation)
				quickStart ();
			else if (isRebuildingStack)
				quickRebuild ();
			else if (isShowingGameEndedScreen || isHidingGameEndedScreen || isShowingGameSummary
				|| gameEndedScreen.GetComponent<GameEndedScreen> ().isOnScreen
				|| gameSummary.GetComponent<GameSummary> ().isAnimatingIndicators)
				quickShowGameSummary ();
		}
		if (Event.current.Equals (Event.KeyboardEvent ("1"))) {
		//	activeTilesArray.dropTiles (true);
		//	nextLevel (true);
		}
	}
	
	void Start (){
		if (!isTutorial) {
			resetButton = GameObject.Find ("ResetButton").transform;
			skipButton = GameObject.Find ("SkipButton").transform;
		}
		if (isTutorial) {
			toggleBeacon ();
			tutorialText1 = GameObject.Find ("TutorialText1").transform;
			tutorialText2 = GameObject.Find ("TutorialText2").transform;
		}
		
		redParticle = transform.FindChild ("RedPart");
		greenParticle = transform.FindChild ("GreenPart");
		blueParticle = transform.FindChild ("BluePart");
		
		rgbState = new float[3, 3];
		rgbState [0, 0] = 1.0f;
		rgbState [0, 1] = 0.0f;
		rgbState [0, 2] = 0.5f;
		
		rgbState [1, 0] = 0.0f;
		rgbState [1, 1] = 0.9f;
		rgbState [1, 2] = 0.18f;
		
		rgbState [2, 0] = 0.0f;
		rgbState [2, 1] = 0.5f;
		rgbState [2, 2] = 1.0f;
		
		livesArray = GameObject.Find("LivesArray").GetComponent<LivesArray>();
		
		if (isQuickGame) {
			tilesArray = new Transform[2];
			livesLeft = Global.quickGameLives;
		} else {
			tilesArray = new Transform[10];
			livesLeft = Global.getGameLives();
		}
		
		tilesArray [0] = transform.Find ("TilesArrayA");
		tilesArray [1] = transform.Find ("TilesArrayB");
		if (!isQuickGame) {
			tilesArray [2] = transform.Find ("TilesArrayC");
			tilesArray [3] = transform.Find ("TilesArrayD");
			tilesArray [4] = transform.Find ("TilesArrayE");
			tilesArray [5] = transform.Find ("TilesArrayF");
			tilesArray [6] = transform.Find ("TilesArrayG");
			tilesArray [7] = transform.Find ("TilesArrayH");
			tilesArray [8] = transform.Find ("TilesArrayI");
			tilesArray [9] = transform.Find ("TilesArrayJ");
		}
		
		activeTilesArray = tilesArray [0].GetComponent<TilesArray> ();
		
		stack0 = GameObject.Find ("Stack0").transform.FindChild ("Text");
		stack1 = GameObject.Find ("Stack1").transform.FindChild ("Text");
		stack2 = GameObject.Find ("Stack2").transform.FindChild ("Text");
		
		stackOrderTiles = new Transform[10];
		stackOrderTiles [0] = GameObject.Find ("StackOrder0").transform;
		stackOrderTiles [1] = GameObject.Find ("StackOrder1").transform;
		stackOrderTiles [2] = GameObject.Find ("StackOrder2").transform;
		stackOrderTiles [3] = GameObject.Find ("StackOrder3").transform;
		stackOrderTiles [4] = GameObject.Find ("StackOrder4").transform;
		stackOrderTiles [5] = GameObject.Find ("StackOrder5").transform;
		stackOrderTiles [6] = GameObject.Find ("StackOrder6").transform;
		stackOrderTiles [7] = GameObject.Find ("StackOrder7").transform;
		stackOrderTiles [8] = GameObject.Find ("StackOrder8").transform;
		stackOrderTiles [9] = GameObject.Find ("StackOrder9").transform;
		
		if (isQuickGame) {
			levelIndicators = new Transform[5];
			levelIndicators [0] = GameObject.Find ("LevelIndicator0").transform;
			levelIndicators [1] = GameObject.Find ("LevelIndicator1").transform;
			levelIndicators [2] = GameObject.Find ("LevelIndicator2").transform;
			levelIndicators [3] = GameObject.Find ("LevelIndicator3").transform;
			levelIndicators [4] = GameObject.Find ("LevelIndicator4").transform;
			levelIndicators [0].localPosition = new Vector3 (0, 0, 32);
			levelIndicators [1].localPosition = new Vector3 (0, 0, 16);
			levelIndicators [2].localPosition = new Vector3 (0, 0, 0);
			levelIndicators [3].localPosition = new Vector3 (0, 0, -16);
			levelIndicators [4].localPosition = new Vector3 (0, 0, -32);
			GameObject.Find ("LevelIndicator5").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("LevelIndicator6").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("LevelIndicator7").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("LevelIndicator8").transform.localScale = new Vector3 (0, 0, 0);
			GameObject.Find ("LevelIndicator9").transform.localScale = new Vector3 (0, 0, 0);
		} else {
			levelIndicators = new Transform[10];
			levelStars = 0;
			levelIndicators [0] = GameObject.Find ("LevelIndicator0").transform;
			levelIndicators [1] = GameObject.Find ("LevelIndicator1").transform;
			levelIndicators [2] = GameObject.Find ("LevelIndicator2").transform;
			levelIndicators [3] = GameObject.Find ("LevelIndicator3").transform;
			levelIndicators [4] = GameObject.Find ("LevelIndicator4").transform;
			levelIndicators [5] = GameObject.Find ("LevelIndicator5").transform;
			levelIndicators [6] = GameObject.Find ("LevelIndicator6").transform;
			levelIndicators [7] = GameObject.Find ("LevelIndicator7").transform;
			levelIndicators [8] = GameObject.Find ("LevelIndicator8").transform;
			levelIndicators [9] = GameObject.Find ("LevelIndicator9").transform;
			
			if (!isTutorial){
				summaryLevelIndicators = new Transform[10];
				summaryLevelIndicators [0] = GameObject.Find ("StateLevelIndicator0").transform;
				summaryLevelIndicators [1] = GameObject.Find ("StateLevelIndicator1").transform;
				summaryLevelIndicators [2] = GameObject.Find ("StateLevelIndicator2").transform;
				summaryLevelIndicators [3] = GameObject.Find ("StateLevelIndicator3").transform;
				summaryLevelIndicators [4] = GameObject.Find ("StateLevelIndicator4").transform;
				summaryLevelIndicators [5] = GameObject.Find ("StateLevelIndicator5").transform;
				summaryLevelIndicators [6] = GameObject.Find ("StateLevelIndicator6").transform;
				summaryLevelIndicators [7] = GameObject.Find ("StateLevelIndicator7").transform;
				summaryLevelIndicators [8] = GameObject.Find ("StateLevelIndicator8").transform;
				summaryLevelIndicators [9] = GameObject.Find ("StateLevelIndicator9").transform;
			}
		}
		
		stacks = GameObject.Find ("Stacks").transform;
		stackOrder = GameObject.Find ("StackOrder").transform;
		
		cameraArray = GameObject.Find ("CameraArray");
		upperLight = cameraArray.transform.FindChild ("UpperLight");
		supportingTilesArray = cameraArray.transform.FindChild ("SupportingTilesArray");
		guiLeftPanel = GameObject.Find ("GUILeftPanel");
		guiRightPanel = GameObject.Find ("GUIRightPanel");
		
		gameSummary = GameObject.Find ("GameSummary");
		pauseScreen = GameObject.Find ("PauseScreen");
		gameEndedScreen = GameObject.Find ("GameEndedScreen");
		
		if (isQuickGame) {
			quickGameScoreNumber = GameObject.Find ("QuickGameScore");
			gameSummaryQuickGameScoreNumber = GameObject.Find ("GameSummaryQuickGameScore");
			quickGameScoreNumber.GetComponent<TextMesh> ().text = quickGameScore.ToString ();
			gameSummaryNewRecord = GameObject.Find ("NewRecord");
		} else {
			if (!isTutorial && !isQuickGame){
				medals = new Medal[5];
				medals[0] = guiLeftPanel.transform.FindChild("MedalsArray").FindChild("Medal0").GetComponent<Medal>();
				medals[1] = guiLeftPanel.transform.FindChild("MedalsArray").FindChild("Medal1").GetComponent<Medal>();
				medals[2] = guiLeftPanel.transform.FindChild("MedalsArray").FindChild("Medal2").GetComponent<Medal>();
				medals[3] = guiLeftPanel.transform.FindChild("MedalsArray").FindChild("Medal3").GetComponent<Medal>();
				medals[4] = guiLeftPanel.transform.FindChild("MedalsArray").FindChild("Medal4").GetComponent<Medal>();
			}	
		}
		
		puzzleStack = new int[3];
		
		effectsLightArray = GameObject.Find ("EffectsLightArray");
	}
	
	void FixedUpdate (){
		if (isBlockingGUIInput) {
			if (++isBlockingGUIInputCounter == isBlockingGUIInputPeriod)
				isBlockingGUIInput = false;
		}
		beaconCounter++;
		if (beaconCounter >= beaconPeriod)
			beaconCounter = 0;
		if (!started) {
			livesArray.setLives(livesLeft);
			if (!isQuickGame) {
				readStack ();
				for (int i=0; i<10; i++)
					tilesArray [i].GetComponent<TilesArray> ().loadPuzzle ();
				started = true;
				isStartingAnimation = true;
				startAnimationCounter = 0;
				levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
				if (!isTutorial)
					summaryLevelIndicators [0].GetComponent<LevelIndicator> ().setSkipped ();
				Global.isGameActive = true;
				toggleStackOrder ();
			} else {
				isStartingAnimation = true;
				started = true;
				tilesArray [0].GetComponent<TilesArray> ().newRandomPuzzle (3);
				tilesArray [0].GetComponent<TilesArray> ().loadPuzzle ();
				tilesArray [1].GetComponent<TilesArray> ().newRandomPuzzle (3);
				tilesArray [1].GetComponent<TilesArray> ().loadPuzzle ();
				toggleZoom ();
				levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
				
				Global.isGameActive = true;
				showGUI ();

				activeTilesArray.bringForward ();
				activeTilesArray.resetPuzzle ();
				toggleStackOrder ();
				checkStack ();
			}
		}
		if (isStartingAnimation) {
			startAnimationCounter++;
			if (!isQuickGame) {
				cameraArray.transform.position = new Vector3 (0, -600 + 950 * startAnimationCounter / startAnimationPeriod, 0);
				if (startAnimationCounter == startAnimationPeriod) {
					isStartingAnimation = false;
					isFlashing = true;
					if ((!isZoomedIn && activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn) ||
						(isZoomedIn && !activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn))
						toggleZoom ();
					if (isTutorial) {
						tutorialText1.GetComponent<TextMesh>().text = "clicking on red affects the color of the surrounding tiles";
						tutorialText1.GetComponent<TutorialText>().appear ();
						activeTilesArray.setTutorialHints();
					}
				}
				
				if (startAnimationCounter < startAnimationPeriod * 0.9f) {
					guiRightPanel.transform.localPosition = new Vector3 (0, 180, 0);
					guiLeftPanel.transform.localPosition = new Vector3 (0, 180, 0);
				} else {
					if (!isShowingGUI) {
						showGUI ();
					}
				}
				for (int i=0; i<10; i++)
					if (startAnimationCounter == 10 * (i + 1) - 9) {
						tilesArray [9 - i].GetComponent<TilesArray> ().drawPuzzle ();
						if (i == 9) {
							activeTilesArray.bringForward ();
							activeTilesArray.resetPuzzle ();
							puzzleNumMoves = activeTilesArray.puzzle.size;
							puzzleStack [0] = activeTilesArray.stack [0];
							puzzleStack [1] = activeTilesArray.stack [1];
							puzzleStack [2] = activeTilesArray.stack [2];
							checkStack ();
						}
					}
			} else {
				if (startAnimationCounter == startAnimationPeriod)
					isStartingAnimation = false;
				upperLight.GetComponent<Light> ().intensity = startAnimationCounter / startAnimationPeriod * 5.5f;
			}
		}
		if (isGoingToMenu) {
			cameraArray.transform.localPosition = new Vector3 (cameraArray.transform.localPosition.x, cameraArray.transform.localPosition.y + 20, cameraArray.transform.localPosition.z);
			supportingTilesArray.transform.localPosition = new Vector3 (0, 350 - 1000 * (1 - goingToMenuCounter / goingToMenuPeriod), 0);

			if (++goingToMenuCounter == goingToMenuPeriod) {
				if (isGoingToFirstLevel)
					Application.LoadLevel ("GameScene");
				else if (isQuickGame || isTutorial)
					Application.LoadLevel ("MenuScene");
				else
					Application.LoadLevel ("StackMenuScene");
				isGoingToMenu = false;
				isGoingToFirstLevel = false;				
			}
				
		}
		if (isRebuildingStack) {
			cameraArray.transform.position = Vector3.Lerp (rebuildingStartingLocation, rebuildingTargetLocation, rebuildingStackCounter / rebuildingStackPeriod);
			if (rebuildingStackCounter % rebuildingStackPeriodPerLevel == 0) {
				if (level > 0) {
					level--;
					tilesArray [level].GetComponent<TilesArray> ().rebuildStack ();
				}
			}
			rebuildingStackCounter++;
			if (rebuildingStackCounter > rebuildingStackPeriod) {
				livesLeft = Global.getGameLives();
				livesArray.setLives(livesLeft);
				isRebuildingStack = false;
				showGUI ();
				activeTilesArray = tilesArray [0].GetComponent<TilesArray> ();
				activeTilesArray.resetPuzzleFromScratch ();
				activeTilesArray.bringForward ();
				for (int i=level+1; i<10; i++)
					tilesArray [i].GetComponent<TilesArray> ().setBack2 (activeTilesArray.puzzle.baseState);
				
				checkStack ();
				levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
				for (int i=1; i<10; i++)
					levelIndicators [i].GetComponent<LevelIndicator> ().setNull ();
			}
		}
		if (isShowingGUI) {
			showingGUICounter++;
			guiRightPanel.transform.localPosition = new Vector3 (0, 180 - 180 * showingGUICounter / showingGUIPeriod, 0);
			guiLeftPanel.transform.localPosition = new Vector3 (0, 180 - 180 * showingGUICounter / showingGUIPeriod, 0);
			if (showingGUICounter == showingGUIPeriod)
				isShowingGUI = false;
		}
		if (isHidingGUI) {
			hidingGUICounter++;
			guiRightPanel.transform.localPosition = new Vector3 (0, 180 * hidingGUICounter / hidingGUIPeriod, 0);
			guiLeftPanel.transform.localPosition = new Vector3 (0, 180 * hidingGUICounter / hidingGUIPeriod, 0);
			if (hidingGUICounter == hidingGUIPeriod)
				isHidingGUI = false;
		}
		if (isShowingGameSummary) {
			showingGameSummaryCounter++;
			gameSummary.transform.localPosition = new Vector3 (0, 10 - 195 * showingGameSummaryCounter / showingGameSummaryPeriod, 0);
			if (showingGameSummaryCounter == showingGameSummaryPeriod)
				isShowingGameSummary = false;
		}
		if (isHidingGameSummary) {
			hidingGameSummaryCounter++;
			gameSummary.transform.localPosition = new Vector3 (0, -185 + 195 * hidingGameSummaryCounter / hidingGameSummaryPeriod, 0);
			if (hidingGameSummaryCounter == hidingGameSummaryPeriod) {
				isHidingGameSummary = false;
				if (!isQuickGame) {
					gameSummary.GetComponent<GameSummary> ().hideGameSummary ();
					if (!isTutorial){
						summaryLevelIndicators [0].GetComponent<LevelIndicator> ().setSkipped ();
						for (int i=1; i<10; i++)
							summaryLevelIndicators [i].GetComponent<LevelIndicator> ().setNull ();
					}
				}
			}
		}
		if (isShowingGameEndedScreen) {
			showingGameEndedScreenCounter++;
			gameEndedScreen.transform.localPosition = new Vector3 (0, 10 - 195 * showingGameEndedScreenCounter / showingGameEndedScreenPeriod, 0);
			if (showingGameEndedScreenCounter == showingGameEndedScreenPeriod)
				isShowingGameEndedScreen = false;
		}
		if (isHidingGameEndedScreen) {
			hidingGameEndedScreenCounter++;
			gameEndedScreen.transform.localPosition = new Vector3 (0, -185 + 195 * hidingGameEndedScreenCounter / hidingGameEndedScreenPeriod, 0);
			if (hidingGameEndedScreenCounter == hidingGameEndedScreenPeriod)
				isHidingGameEndedScreen = false;
		}
		if (isShowingPauseScreen) {
			showingPauseScreenCounter++;
			pauseScreen.transform.localPosition = new Vector3 (0, -175 * showingPauseScreenCounter / showingPauseScreenPeriod, 0);
			if (showingPauseScreenCounter == showingPauseScreenPeriod)
				isShowingPauseScreen = false;
		}
		if (isFlashing) {
			effectsLightArray.transform.position = new Vector3 (0, -600 + 3000 * flashCounter / flashPeriod, 0);
			flashCounter++;
			if (flashCounter == flashPeriod)
				flashCounter = 0;
		}
		if (isZoomingIn) {
			zoomCounter++;
			cameraArray.transform.localPosition = new Vector3 (cameraArray.transform.localPosition.x, 350 - level * 50 + 300 * (1 - zoomCounter / zoomPeriod), cameraArray.transform.localPosition.z);
			upperLight.transform.localPosition = new Vector3 (0, -100 - 300 * (1 - zoomCounter / zoomPeriod), 0);
			supportingTilesArray.transform.localPosition = new Vector3 (0, 350 - 1000 * (1 - zoomCounter / zoomPeriod), 0);
			if (zoomCounter == zoomPeriod) {
				isZoomingIn = false;
				isZoomedIn = true;
			}
		}
		if (isZoomingOut) {
			zoomCounter++;
			cameraArray.transform.localPosition = new Vector3 (cameraArray.transform.localPosition.x, 350 - level * 50 + 300 * (zoomCounter / zoomPeriod), cameraArray.transform.localPosition.z);
			upperLight.transform.localPosition = new Vector3 (0, -100 - 300 * (zoomCounter / zoomPeriod), 0);
			supportingTilesArray.transform.localPosition = new Vector3 (0, 350 - 1000 * (zoomCounter / zoomPeriod), 0);
			if (zoomCounter == zoomPeriod)
				isZoomingOut = false;
		}
		if (isOpeningMenu) {
			openMenuCounter++;
			if (openMenuCounter > openMenuPeriod)
				isOpeningMenu = false;
		}
		if (isAdvancingLevel) {
			advancingLevelCounter++;
			cameraArray.transform.localPosition = new Vector3 (cameraArray.transform.localPosition.x, cameraArray.transform.localPosition.y - 2, cameraArray.transform.localPosition.z);
			if (advancingLevelCounter == advancingLevelPeriod) {
				isAdvancingLevel = false;
			}
		}
	}

	public void resetPuzzle (){
		if (!activeTilesArray.isDrawingPuzzle) {
			activeTilesArray.livesUsed++;
			
			livesLeft--;
			livesArray.setLives(livesLeft);
			if (livesLeft == 0) {
				dropAllStacks ();
			} else {
				activeTilesArray.resetPuzzle ();
				int chances = 3;
				if (!isQuickGame)
					chances += Global.extraChances;
				if (activeTilesArray.livesUsed == chances) {
					if (isQuickGame) {
						activeTilesArray.dropTiles (true);
						nextLevel (false);
					}
					else {
						if (numPuzzlesSkipped == 4) {
							dropAllStacks ();
						} else {
							if (level == 9)
								activeTilesArray.dropTiles (false);
							else
								activeTilesArray.dropTiles (true);
							nextLevel (false);
						}
					}
				}					
				if (activeTilesArray.livesUsed == 2) {
					if (!isBeaconOn)
						toggleBeacon ();
				}
				else if (activeTilesArray.livesUsed == 1){
					if (isTutorial && level == 6)
						tutorialText1.GetComponent<TextMesh> ().text = "after two mistakes the solution tiles will blink";
				}

				checkStack ();
			}
			if (!isTutorial && (livesLeft == 1 || (activeTilesArray.livesUsed == (3 + Global.extraChances - 1)) || (isQuickGame && activeTilesArray.livesUsed == 2))){
				resetButton.GetComponent<ResetButton> ().makeUnavailable ();
			}
		}
	}

	public void toggleZoom (){
		if (isZoomedIn) {
			zoomOut ();
		} else {
			zoomIn ();
		}
	}
	public void toggleBeacon (){
		isBeaconOn = !isBeaconOn;
	}
	public void toggleStackOrder (){
		isStackOrder = !isStackOrder;
		if (isStackOrder) {
			hideStacks ();
			showStackOrder ();
		} else {
			hideStackOrder ();
			showStacks ();
		}
	}
	
	public void zoomIn (){
		isZoomingIn = true;
		zoomCounter = 0;
	}
	public void zoomOut (){
		isZoomingOut = true;
		isZoomedIn = false;
		zoomCounter = 0;
	}

	public void removePuzzleStack (int state){
		puzzleStack [state]--;
		
		StackOrderElement element = activeTilesArray.lastStackOrderElement;
		while (element.state != state)
			element = element.lastElement;
		if (element == activeTilesArray.firstStackOrderElement)
			activeTilesArray.firstStackOrderElement = element.nextElement;
		if (element == activeTilesArray.lastStackOrderElement)
			activeTilesArray.lastStackOrderElement = element.lastElement;
		element.removeFromList ();
		
		puzzleNumMoves--;
		checkStack ();
	}
	
	public void openMenu (){
		openMenuCounter = 0;
		isOpeningMenu = true;
	}
	
	public void checkStack (){
		if (!isTutorial && !isQuickGame)
			for (int i=0; i<5; i++)
				medals[i].showInStackMenu(Global.stackLevel);
		
		stack0.GetComponent<TextMesh> ().text = puzzleStack [0].ToString ();
		stack1.GetComponent<TextMesh> ().text = puzzleStack [1].ToString ();
		stack2.GetComponent<TextMesh> ().text = puzzleStack [2].ToString ();
		
		int currentLevel;
		if (!isQuickGame && !isTutorial){
			currentLevel = System.Convert.ToInt32(Global.stackLevel.Substring(5,2));
			GameObject.Find("LevelNumber").GetComponent<TextMesh>().text =  "Level " + (currentLevel+1) + " - " + (level+1);
		}
		
		StackOrderElement element = activeTilesArray.lastStackOrderElement;
		for (int i=0; i<10; i++) {
			if (i < puzzleNumMoves) {
				stackOrderTiles[i].localScale = new Vector3(20,20,20);
				stackOrderTiles [i].renderer.material.color = new Color (rgbState [element.state, 0], rgbState [element.state, 1], rgbState [element.state, 2]);
				element = element.lastElement;
			} else{
				stackOrderTiles[i].localScale = new Vector3(0,0,0);
				stackOrderTiles[i].renderer.material.color = Color.black;
			}
		}
		stackOrder.localPosition = new Vector3 (stackOrder.localPosition.x, stackOrder.localPosition.y, -70 + 7.25f * puzzleNumMoves);
	}
	public void checkGameSummary (){
		if (isQuickGame)
			gameSummaryQuickGameScoreNumber.GetComponent<TextMesh> ().text = quickGameScore.ToString ();
		checkSaveState ();
	}
	public void checkSaveState (){
		if (isQuickGame) {
			if (PlayerPrefs.HasKey ("quickGameRecord")) {
				int record = PlayerPrefs.GetInt ("quickGameRecord");
				if (record < quickGameScore) {
					PlayerPrefs.SetInt ("quickGameRecord", quickGameScore);
					PlayerPrefs.Save ();
					gameSummaryNewRecord.GetComponent<TextMesh> ().text = "*new record";
				} else
					gameSummaryNewRecord.GetComponent<TextMesh> ().text = "*record " + record.ToString ();
			} else {
				PlayerPrefs.SetInt ("quickGameRecord", quickGameScore);
				PlayerPrefs.Save ();
				gameSummaryNewRecord.GetComponent<TextMesh> ().text = "*new record";
			}
		} else {
			if (PlayerPrefs.HasKey (Global.stackLevel)) {
				int record = PlayerPrefs.GetInt (Global.stackLevel);
				if (record < levelStars) {
					PlayerPrefs.SetInt (Global.stackLevel, levelStars);
					PlayerPrefs.Save ();
					if (levelStars == 10 && !isTutorial){
						PlayerPrefs.SetInt("HeartPieces",PlayerPrefs.GetInt("HeartPieces",0)+1);
						gameSummary.GetComponent<GameSummary>().addHeartPiece();
					}
				}
			} else {
				if (levelStars > 5) {
					if (Global.stackLevel == "stack15" || Global.stackLevel == "stack31"){
						PlayerPrefs.SetInt("ExtraChances",PlayerPrefs.GetInt("ExtraChances",0)+1);
						Global.extraChances++;
						gameSummary.GetComponent<GameSummary>().addExtraChance();
					}
					PlayerPrefs.SetInt (Global.stackLevel, levelStars);
					PlayerPrefs.Save ();
					if (levelStars == 10 && !isTutorial){
						PlayerPrefs.SetInt("HeartPieces",PlayerPrefs.GetInt("HeartPieces",0)+1);
						gameSummary.GetComponent<GameSummary>().addHeartPiece();
					}
				}
			}
		}
	}
	
	public void dropAllStacks (){
		activeTilesArray.GetComponent<TilesArray>().dropTiles (false);
		hideGUI ();
		if (isQuickGame)
			showGameSummary ();
		else
			showGameEndedScreen ();
	}

	public void retryWholeStack (){
		numPuzzlesSkipped = 0;
		if (!isTutorial){
			skipButton.GetComponent<SkipButton> ().makeAvailable ();
			resetButton.GetComponent<ResetButton>().makeAvailable ();
		}
		else {
			tutorialText1.GetComponent<TextMesh>().text = "clicking on red affects the color of the surrounding tiles";
			tutorialText1.GetComponent<TutorialText>().appear();
			tutorialText2.GetComponent<TutorialText>().disappear();
		}
		activeTilesArray.GetComponent<TilesArray> ().rebuildStack ();
		if (!isQuickGame) {
			levelStars = 0;
			rebuildingStartingLocation = cameraArray.transform.position;
			rebuildingTargetLocation = new Vector3 (0, 650, 0);
			isRebuildingStack = true;
			rebuildingStackPeriod = (level + 3) * rebuildingStackPeriodPerLevel;
			rebuildingStackCounter = 0;
			if (isBeaconOn)
				toggleBeacon();
		} else {
			quickGameScore = 0;
			quickGameScoreNumber.GetComponent<TextMesh> ().text = quickGameScore.ToString ();
			level = 0;
			livesLeft = Global.quickGameLives;
			livesArray.setLives(livesLeft);
			Global.isGameActive = true;
			tilesArray [0].GetComponent<TilesArray> ().newRandomPuzzle (3);
			tilesArray [0].GetComponent<TilesArray> ().loadPuzzle ();
			tilesArray [1].GetComponent<TilesArray> ().newRandomPuzzle (3);
			tilesArray [1].GetComponent<TilesArray> ().loadPuzzle ();
			levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
			levelIndicators [1].GetComponent<LevelIndicator> ().setNull ();
			levelIndicators [2].GetComponent<LevelIndicator> ().setNull ();
			levelIndicators [3].GetComponent<LevelIndicator> ().setNull ();
			levelIndicators [4].GetComponent<LevelIndicator> ().setNull ();
			
			activeTilesArray.resetPuzzleFromScratch ();
			activeTilesArray.bringForward ();
			checkStack ();
		}
		Global.isGameActive = true;
	}
	
	public void nextLevel (bool passed){
		if (isQuickGame) {
			if (isBeaconOn)
				toggleBeacon ();
			
			if (passed) {
				quickGameScore += (3 - activeTilesArray.livesUsed) * activeTilesArray.puzzle.size * (3 - activeTilesArray.livesUsed) * activeTilesArray.puzzle.size;
				quickGameScoreNumber.GetComponent<TextMesh> ().text = quickGameScore.ToString ();
				level++;
			} else {
				if (level > 10) 
					level = 10;
				else if (level > 6)
					level = 6;
				else if (level > 3)
					level = 3;
				else if (level > 1)
					level = 1;
			}
			
			if (livesLeft > 1)
				resetButton.GetComponent<ResetButton>().makeAvailable ();
			
			isAdvancingLevel = true;
			advancingLevelCounter = 0;
			if (activeTilesArray == tilesArray [0].GetComponent<TilesArray> ())
				activeTilesArray = tilesArray [1].GetComponent<TilesArray> ();
			else
				activeTilesArray = tilesArray [0].GetComponent<TilesArray> ();
			activeTilesArray.bringForward ();
			
			for (int i=0; i<5; i++)
				levelIndicators [i].GetComponent<LevelIndicator> ().setNull ();
			if (level < 2)
				levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
			else if (level < 4)
				levelIndicators [1].GetComponent<LevelIndicator> ().setCurrent ();
			else if (level < 7)
				levelIndicators [2].GetComponent<LevelIndicator> ().setCurrent ();
			else if (level < 11)
				levelIndicators [3].GetComponent<LevelIndicator> ().setCurrent ();
			else
				levelIndicators [4].GetComponent<LevelIndicator> ().setCurrent ();
			activeTilesArray.newQuickPlayPuzzle ();
			activeTilesArray.resetPuzzle ();

			puzzleNumMoves = activeTilesArray.puzzle.size;
			puzzleStack [0] = activeTilesArray.stack [0];
			puzzleStack [1] = activeTilesArray.stack [1];
			puzzleStack [2] = activeTilesArray.stack [2];
			checkStack ();
		} else {
			if (isTutorial) {
				if (level == 0) {
					tutorialText2.GetComponent<TextMesh> ().text = "clicking green affects tiles along its row and column";
					tutorialText1.GetComponent<TutorialText> ().disappear ();
					tutorialText2.GetComponent<TutorialText> ().appear ();
				} else if (level == 1) {
					tutorialText1.GetComponent<TextMesh> ().text = "clicking blue affects tiles diagonally";
					tutorialText2.GetComponent<TutorialText> ().disappear ();
					tutorialText1.GetComponent<TutorialText> ().appear ();
				} else if (level == 2) {
					tutorialText2.GetComponent<TextMesh> ().text = "to solve the puzzle you need to restore the base color to all tiles";
					tutorialText1.GetComponent<TutorialText> ().disappear ();
					tutorialText2.GetComponent<TutorialText> ().appear ();
				} else if (level == 3) {
					tutorialText1.GetComponent<TextMesh> ().text = "you can see the available moves and suggested order on the left";
					tutorialText2.GetComponent<TutorialText> ().disappear ();
					tutorialText1.GetComponent<TutorialText> ().appear ();
					GameObject.Find("StackOrder0").GetComponent<StackOrderImage>().blinkTutorial();
					GameObject.Find("StackOrder1").GetComponent<StackOrderImage>().blinkTutorial();
					GameObject.Find("TutorialArrow").transform.localRotation = Quaternion.Euler(0,0,0);
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(-230,-700,4);
				} else if (level == 4) {
					tutorialText2.GetComponent<TextMesh> ().text = "look for patterns in the outer tiles to help you find the solution";
					tutorialText1.GetComponent<TutorialText> ().disappear ();
					tutorialText2.GetComponent<TutorialText> ().appear ();
					GameObject.Find("StackOrder0").GetComponent<StackOrderImage>().isBlinkingTutorial = false;
					GameObject.Find("StackOrder1").GetComponent<StackOrderImage>().isBlinkingTutorial = false;
					GameObject.Find("TutorialArrow").transform.localRotation = Quaternion.Euler(0,0,180);
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(-200,-700,100);
				} else if (level == 5) {
					if (!isBeaconOn)
						toggleBeacon ();
					tutorialText1.GetComponent<TextMesh> ().text = "failing a puzzle will cost you a life and restart the puzzle";
					tutorialText2.GetComponent<TutorialText> ().disappear ();
					tutorialText1.GetComponent<TutorialText> ().appear ();
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(185,-700,138);
				} else if (level == 6) {
					tutorialText2.GetComponent<TextMesh> ().text = "move the mouse over a tile to see a preview of your movement";
					tutorialText1.GetComponent<TutorialText> ().disappear ();
					tutorialText2.GetComponent<TutorialText> ().appear ();
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(-230,0,4);
				} else if (level == 7) {
					tutorialText1.GetComponent<TextMesh> ().text = "each level consists of 10 puzzles";					
					tutorialText2.GetComponent<TutorialText> ().disappear ();
					tutorialText1.GetComponent<TutorialText> ().appear ();
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(220,-700,4);
				} else if (level == 8) {
					tutorialText2.GetComponent<TextMesh> ().text = "solve 6 or more to pass the level";
					tutorialText1.GetComponent<TutorialText> ().disappear ();
					tutorialText2.GetComponent<TutorialText> ().appear ();
				} else if (level == 9) {
					tutorialText1.GetComponent<TextMesh> ().text = "";
					tutorialText2.GetComponent<TutorialText> ().disappear ();
					GameObject.Find("TutorialArrow").transform.localPosition = new Vector3(220,0,4);
				}
				
			}
			
			if (!isTutorial || level > 5)
				if (isBeaconOn)
					toggleBeacon ();
			if (livesLeft > 1 && !isTutorial)
				resetButton.GetComponent<ResetButton> ().makeAvailable ();
			if (passed)
				levelStars++;
			else
				numPuzzlesSkipped++;
			if (passed) {
				levelIndicators [level].GetComponent<LevelIndicator> ().setPassed ();
				if (!isTutorial)
					summaryLevelIndicators [level].GetComponent<LevelIndicator> ().setPassed ();
			} else {
				levelIndicators [level].GetComponent<LevelIndicator> ().setSkipped ();
				if (!isTutorial)
					summaryLevelIndicators [level].GetComponent<LevelIndicator> ().setSkipped ();
			}

			level++;
			if (level == 10) {
				level--;
				hideGUI ();
				showGameEndedScreen ();
			} else {
				levelIndicators [level].GetComponent<LevelIndicator> ().setCurrent ();
				if (!isTutorial)
					summaryLevelIndicators [level].GetComponent<LevelIndicator> ().setSkipped ();
				isAdvancingLevel = true;
				advancingLevelCounter = 0;
				activeTilesArray = tilesArray [level].GetComponent<TilesArray> ();
				activeTilesArray.bringForward ();
				activeTilesArray.resetPuzzle ();
				for (int i=level+1; i<10; i++)
					tilesArray [i].GetComponent<TilesArray> ().setBack ();
				puzzleNumMoves = activeTilesArray.puzzle.size;
				puzzleStack [0] = activeTilesArray.stack [0];
				puzzleStack [1] = activeTilesArray.stack [1];
				puzzleStack [2] = activeTilesArray.stack [2];
				checkStack ();
				if ((!isZoomedIn && activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn) ||
					(isZoomedIn && !activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn))
					toggleZoom ();
			}
			if (isTutorial && level < 3)
				activeTilesArray.setTutorialHints();
			if (isTutorial && level == 5)
				activeTilesArray.setTutorialHintsOuter();
			if (isTutorial && level == 6){
				activeTilesArray.tiles[2,2].beaconCount++;
				activeTilesArray.tiles[4,2].beaconCount--;
				activeTilesArray.tiles[4,3].beaconCount--;
			}
		}
	}
	
	public void skipLevel (){
		if (!isAdvancingLevel) {
			if (isQuickGame) {
				livesLeft--;
				if (livesLeft == 1){
					resetButton.GetComponent<ResetButton> ().makeUnavailable ();
					skipButton.GetComponent<SkipButton> ().makeUnavailable ();
				}
				livesArray.setLives(livesLeft);
				if (livesLeft == 0)
					dropAllStacks ();
				else {
					activeTilesArray.dropTiles (true);
					nextLevel (false);
				}
			} else {
				if (level == 9)
					activeTilesArray.dropTiles (false);
				else
					activeTilesArray.dropTiles (true);
				nextLevel (false);
				if (numPuzzlesSkipped > 3)
					skipButton.GetComponent<SkipButton> ().makeUnavailable ();
			}
		}
	}

	public void showStacks (){
		stacks.localPosition = new Vector3 (-260, stacks.localPosition.y, stacks.localPosition.z);
	}
	public void hideStacks (){
		stacks.localPosition = new Vector3 (-300, stacks.localPosition.y, stacks.localPosition.z);
	}
	
	public void showStackOrder (){
		stackOrder.localPosition = new Vector3 (-260, stacks.localPosition.y, -70 + 7.25f * puzzleNumMoves);
	}
	public void hideStackOrder (){
		stackOrder.localPosition = new Vector3 (-300, stacks.localPosition.y, stacks.localPosition.z);
	}

	public void readStack (){
		TextAsset asset = Resources.Load (Global.stackLevel) as TextAsset;
		Stream s = new MemoryStream (asset.bytes);
		using (BinaryReader b = new BinaryReader(s)) {
			for (int i=0; i<10; i++) {
				int pBase = b.ReadInt32 ();
				int size = b.ReadInt32 ();
				Puzzle puzzle = new Puzzle (size);
				puzzle.baseState = pBase;
				puzzle.isZoomedIn = b.ReadBoolean ();
				puzzle.isStackOrder = b.ReadBoolean ();
				for (int j=0; j<size; j++)
					puzzle.tiles [j] = b.ReadInt32 ();
				tilesArray [i].GetComponent<TilesArray> ().puzzle = puzzle;
			}
		}
	}

	public void showGameEndedScreen (){
		Global.isGameActive = false;
		isShowingGameEndedScreen = true;
		showingGameEndedScreenCounter = 0;
		gameEndedScreen.GetComponent<GameEndedScreen> ().showGameEndedScreen ();
	}
	public void hideGameEndedScreen (){
		isHidingGameEndedScreen = true;
		hidingGameEndedScreenCounter = 0;
	}
	
	public void showGameSummary (){
		Global.isGameActive = false;
		isShowingGameSummary = true;
		showingGameSummaryCounter = 0;
		if (!isQuickGame)
			gameSummary.GetComponent<GameSummary> ().showGameSummary ();
		else
			checkGameSummary ();
	}
	public void hideGameSummary (){
		isHidingGameSummary = true;
		hidingGameSummaryCounter = 0;
	}
	
	public void togglePauseScreen (){
		Global.isGamePaused = !Global.isGamePaused;
		if (Global.isGamePaused) {
			isShowingPauseScreen = true;
			showingPauseScreenCounter = 0;
			if (isTutorial){
				GameObject.Find("TutorialText1").renderer.enabled = false;
				GameObject.Find("TutorialText2").renderer.enabled = false;
			}
		} else {
			isShowingPauseScreen = false;
			pauseScreen.transform.localPosition = new Vector3 (0, 0, 0);
			if (isTutorial){
				GameObject.Find("TutorialText1").renderer.enabled = true;
				GameObject.Find("TutorialText2").renderer.enabled = true;
			}
		}
		
	}
	
	public void showGUI (){
		if (!isGUIOn) {
			if (!isTutorial && !isQuickGame)
				guiLeftPanel.transform.FindChild("MedalsArray").localScale = new Vector3(0.8f,0.8f,0.8f);
			isGUIOn = true;
			isShowingGUI = true;
			showingGUICounter = 0;
		}
	}
	public void hideGUI (){
		if (isGUIOn) {
			if (!isTutorial && !isQuickGame)
				guiLeftPanel.transform.FindChild("MedalsArray").localScale = Vector3.zero;
			isGUIOn = false;
			isHidingGUI = true;
			hidingGUICounter = 0;
			
		}
	}
	
	public void blockGUIInput (){
		isBlockingGUIInput = true;
		isBlockingGUIInputCounter = 0;
	}

	public void quickStart (){
		if (isTutorial) {
			tutorialText1.GetComponent<TextMesh> ().text = "clicking red affects the color of the surrounding tiles";
			tutorialText1.GetComponent<TutorialText> ().appear ();
			activeTilesArray.setTutorialHints();
		}
		isStartingAnimation = false;
		isFlashing = true;
		if ((!isZoomedIn && activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn) ||
			(isZoomedIn && !activeTilesArray.GetComponent<TilesArray> ().puzzle.isZoomedIn))
			toggleZoom ();
		
		if (!isShowingGUI) {
			showGUI ();
		}
		for (int i=0; i<10; i++) {
			tilesArray [9 - i].GetComponent<TilesArray> ().drawPuzzle ();
			if (i == 9) {
				activeTilesArray.bringForward ();
				activeTilesArray.resetPuzzle ();
				puzzleNumMoves = activeTilesArray.puzzle.size;
				puzzleStack [0] = activeTilesArray.stack [0];
				puzzleStack [1] = activeTilesArray.stack [1];
				puzzleStack [2] = activeTilesArray.stack [2];
				checkStack ();
			}
		}
	}
	public void quickRebuild (){
		level = 0;
		cameraArray.transform.position = rebuildingTargetLocation;
		isRebuildingStack = false;
		for (int i=0; i<10; i++)
			tilesArray [i].GetComponent<TilesArray> ().quickRebuildStack ();
		for (int i=1; i<10; i++)
			tilesArray [i].GetComponent<TilesArray> ().setBack2 (tilesArray[0].GetComponent<TilesArray>().puzzle.baseState);

		livesLeft = Global.getGameLives();
		livesArray.setLives(livesLeft);

		showGUI ();
		activeTilesArray = tilesArray [0].GetComponent<TilesArray> ();
		activeTilesArray.resetPuzzleFromScratch ();
		activeTilesArray.bringForward ();
		
		checkStack ();
		levelIndicators [0].GetComponent<LevelIndicator> ().setCurrent ();
		for (int i=1; i<10; i++)
			levelIndicators [i].GetComponent<LevelIndicator> ().setNull ();
	}
	public void quickShowGameSummary (){
		isHidingGameEndedScreen = false;
		isShowingGameEndedScreen = false;
		gameEndedScreen.transform.localPosition = new Vector3 (0, 10, 0);
		gameEndedScreen.GetComponent<GameEndedScreen> ().isOnScreen = false;
		
		isShowingGameSummary = false;
		isHidingGameSummary = false;
		gameSummary.transform.localPosition = new Vector3 (0, -185, 0);
		gameSummary.GetComponent<GameSummary> ().quickShowGameSummary ();
	}
	
	public void goToMenu (){
		isGoingToMenu = true;
		goingToMenuCounter = 0;
		if (!isQuickGame)
			for (int i=0; i<level; i++)
				Destroy (tilesArray [i].gameObject);
	}
	public void goToFirstLevel (){
		isGoingToFirstLevel = true;
		isGoingToMenu = true;
		Global.stackLevel = "stack00";
		goingToMenuCounter = 0;
		for (int i=0; i<level; i++)
			Destroy (tilesArray [i].gameObject);
	}
}