using UnityEngine;
using System.Collections;

public class GameSummary : MonoBehaviour {
	Medal[] medals;
	Light gameSummaryLight;
	Transform title;
	Transform hintText;
	Transform nextButton;
	HeartPiece heartPiece;
	ExtraChanceText extraChanceText;
	Game game;
	int levelStars;
	LivesArray livesArray;
	
	public bool isAnimatingIndicators;
	int animatingIndicatorsCounter;
	float animatingIndicatorsPeriod = 110.0f;
	
	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game").GetComponent<Game>();
		
		if (!game.isTutorial){
			medals = new Medal[5];
			medals[0] = transform.FindChild("MedalsArray").FindChild("Medal0").GetComponent<Medal>();
			medals[1] = transform.FindChild("MedalsArray").FindChild("Medal1").GetComponent<Medal>();
			medals[2] = transform.FindChild("MedalsArray").FindChild("Medal2").GetComponent<Medal>();
			medals[3] = transform.FindChild("MedalsArray").FindChild("Medal3").GetComponent<Medal>();
			medals[4] = transform.FindChild("MedalsArray").FindChild("Medal4").GetComponent<Medal>();
			hintText = transform.FindChild("HintText");
			if (!game.isQuickGame){
				heartPiece = transform.FindChild("HeartPieceCube").GetComponent<HeartPiece>();
				heartPiece.hide();
				extraChanceText = transform.FindChild("ExtraChanceText").GetComponent<ExtraChanceText>();
				extraChanceText.hide();
				livesArray = transform.FindChild("GameSummaryLivesArray").GetComponent<LivesArray>();
			}
		}
		
		gameSummaryLight = transform.FindChild("GameSummaryLight").GetComponent<Light>();
		gameSummaryLight.range = 0;
		
		title = transform.FindChild("Title");
		nextButton = transform.FindChild("NextStackButton");
		
	}
	
	void FixedUpdate(){
		if (isAnimatingIndicators){
			int periodBetweenIndicators = 10;
			for (int i=0; i<10; i++){
				if (animatingIndicatorsCounter - 2*periodBetweenIndicators == periodBetweenIndicators*i){
					game.summaryLevelIndicators[i].localScale = new Vector3(0.5f,0.5f,0.5f);
					if (game.summaryLevelIndicators[i].GetComponent<LevelIndicator>().state == 1){
						levelStars++;
						if (levelStars == 6){
							title.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
							title.localPosition = new Vector3(0,0,13);
							title.GetComponent<TextMesh>().text = "success";
							if (Global.stackLevel != "stack39"){
								nextButton.renderer.material.color = new Color(1,1,1,1);
								nextButton.GetComponent<GameSummaryNextButton>().isAvailable = true;
							}
						}
						else if (levelStars == 7)
							title.GetComponent<TextMesh>().text = "good";
						else if (levelStars == 8)
							title.GetComponent<TextMesh>().text = "great";
						else if (levelStars == 9)
							title.GetComponent<TextMesh>().text = "awesome";
						else if (levelStars == 10){
							title.GetComponent<TextMesh>().text = "perfect";
						}
						if (levelStars >= 6)
							for (int j=0; j<5; j++)
								medals[j].showInGameSummary(levelStars);
					}
				}
			}
			for (int i=0; i<10; i++)
				if (animatingIndicatorsCounter - 2*periodBetweenIndicators == i*2){
					if (levelStars < 6){
						title.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
						title.localPosition = new Vector3(-36,0,13);
						if (title.GetComponent<TextMesh>().text == "")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "c";
						else if (title.GetComponent<TextMesh>().text == "c")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "o";
						else if (title.GetComponent<TextMesh>().text == "co")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "m";
						else if (title.GetComponent<TextMesh>().text == "com")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "p";
						else if (title.GetComponent<TextMesh>().text == "comp")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "u";
						else if (title.GetComponent<TextMesh>().text == "compu")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "t";
						else if (title.GetComponent<TextMesh>().text == "comput")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "i";
						else if (title.GetComponent<TextMesh>().text == "computi")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "n";
						else if (title.GetComponent<TextMesh>().text == "computin")
							title.GetComponent<TextMesh>().text = title.GetComponent<TextMesh>().text + "g";
					}
				}
			if (++animatingIndicatorsCounter > animatingIndicatorsPeriod){
				isAnimatingIndicators = false;
				game.checkGameSummary();
				if (levelStars < 6){
					title.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
					title.localPosition = new Vector3(0,0,13);
					title.GetComponent<TextMesh>().text = "failure";
					if (!PlayerPrefs.HasKey(Global.stackLevel))
						hintText.localScale = new Vector3(0.6f,0.6f,0.6f);
				}
			}
		}
	}
	
	public void showGameSummary(){
		if (!game.isQuickGame && !game.isTutorial)
			livesArray.setLives(0,0);
		levelStars = 0;
		if (!game.isTutorial && !game.isQuickGame){
			heartPiece.hide();
			extraChanceText.hide();
		}
		if (!game.isTutorial){
			isAnimatingIndicators = true;
			hintText.localScale = new Vector3(0,0,0);
			
			animatingIndicatorsCounter = 0;
			title.GetComponent<TextMesh>().text = "";
			
			for (int i=0; i<5; i++)
			medals[i].transform.localScale = Vector3.zero;
			
			for (int i=0; i<10; i++)
				game.summaryLevelIndicators[i].localScale = Vector3.zero;
		}
		else {
			PlayerPrefs.SetInt("tutorial",1);
			PlayerPrefs.Save();
		}

		if (PlayerPrefs.HasKey(Global.stackLevel) && Global.stackLevel != "stack39"){
			nextButton.renderer.material.color = new Color(1,1,1,1);
			nextButton.GetComponent<GameSummaryNextButton>().isAvailable = true;
		}
		else {
			nextButton.renderer.material.color = new Color(1,1,1,0.3f);
			nextButton.GetComponent<GameSummaryNextButton>().isAvailable = false;			
		}

		gameSummaryLight.range = 150;
	}
	public void hideGameSummary(){
		gameSummaryLight.range = 0;
		if (!game.isTutorial)
			for (int i=0; i<5; i++)
				medals[i].hideInGameSummary();
		if (!game.isTutorial && !game.isQuickGame){
			extraChanceText.hide();
			heartPiece.hide();
		}
	}
	public void quickShowGameSummary(){
		if (!game.isTutorial && !game.isQuickGame)
			livesArray.setLives(0,0);
		isAnimatingIndicators = false;
		if (Global.stackLevel != "stack39" && (PlayerPrefs.HasKey(Global.stackLevel) || game.levelStars > 5)){
			nextButton.renderer.material.color = new Color(1,1,1,1);
			nextButton.GetComponent<GameSummaryNextButton>().isAvailable = true;
		}
		else {
			nextButton.renderer.material.color = new Color(1,1,1,0.3f);
			nextButton.GetComponent<GameSummaryNextButton>().isAvailable = false;			
		}
		
		if (!game.isTutorial){
			for (int i=0; i<10; i++)
				game.summaryLevelIndicators[i].localScale = new Vector3(0.5f,0.5f,0.5f);
			
			for (int i=0; i<5; i++)
				medals[i].transform.localScale = Vector3.zero;
	
			title.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
			title.localPosition = new Vector3(0,0,13);
			if (game.levelStars >= 6){
				if (game.levelStars == 6)
					title.GetComponent<TextMesh>().text = "success";
				else if (game.levelStars == 7)
					title.GetComponent<TextMesh>().text = "good";
				else if (game.levelStars == 8)
					title.GetComponent<TextMesh>().text = "great";
				else if (game.levelStars == 9)
					title.GetComponent<TextMesh>().text = "awesome";
				else if (game.levelStars == 10){
					title.GetComponent<TextMesh>().text = "perfect";
				}
				if (Global.stackLevel != "stack39"){
					nextButton.renderer.material.color = new Color(1,1,1,1);
					nextButton.GetComponent<GameSummaryNextButton>().isAvailable = true;
				}
				for (int j=0; j<5; j++)
					medals[j].showInGameSummary(game.levelStars);
				hintText.localScale = new Vector3(0,0,0);
			}
			
			else{
				title.GetComponent<TextMesh>().text = "failure";
				if (!PlayerPrefs.HasKey(Global.stackLevel))
					hintText.localScale = new Vector3(0.6f,0.6f,0.6f);
			}
		}
		else{
			PlayerPrefs.SetInt("tutorial",1);
			PlayerPrefs.Save();
		}
		game.checkGameSummary();
		gameSummaryLight.range = 150;
	}

	public void addHeartPiece(){
		heartPiece.appear();
	}
	public void addExtraChance(){
		extraChanceText.appear();
	}
	
	public void showLivesArray(){
		livesArray.blendInAll();
		heartPiece.addToArray(0.18f*livesArray.lives[Global.getGameLives()-1].localPosition + livesArray.transform.localPosition);
	}
	public void hideLivesArray(){
		livesArray.blendOutAll();
	}
}