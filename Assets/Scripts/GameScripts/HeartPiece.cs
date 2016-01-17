using UnityEngine;
using System.Collections;

public class HeartPiece : MonoBehaviour {
	bool isAppearing;
	bool isGoingToCorner;
	
	int appearCounter;
	int goToCornerCounter;
	
	float appearPeriod = 30;
	float goToCornerPeriod = 30;
	
	Vector3 cornerEndPosition;
	Vector3 cornerStartPosition;
	Vector3 cornerEndSize;
	Vector3 cornerStartSize;
	
	Vector3 originalPosition;
	
	GameSummary gameSummary;
	
	bool shouldSummaryShowArray;
	
	// Use this for initialization
	void Start () {
		originalPosition = transform.localPosition;
		isAppearing = false;
		shouldSummaryShowArray = false;
		gameSummary = GameObject.Find("GameSummary").GetComponent<GameSummary>();
	}
	
	void FixedUpdate(){
		if (isAppearing){
			float size = (float) (7.5 - 7.5*Mathf.Cos(appearCounter/appearPeriod*2/3*2*3.141592f));
			transform.localScale = new Vector3(size,0,size);
			if (++appearCounter > appearPeriod){
				isAppearing = false;
				if (shouldSummaryShowArray)
					gameSummary.showLivesArray();
				shouldSummaryShowArray = false;
			}
		}
		if (isGoingToCorner){
			if (goToCornerCounter >= 0){
				transform.localPosition = Vector3.Lerp(cornerStartPosition,cornerEndPosition,goToCornerCounter/goToCornerPeriod);
				transform.localScale = Vector3.Lerp(cornerStartSize,cornerEndSize,goToCornerCounter/goToCornerPeriod);
			}
			if (++goToCornerCounter > goToCornerPeriod){
				isGoingToCorner = false;
				gameSummary.hideLivesArray();
				transform.localScale = Vector3.zero;
			}
		}
	}
	

	public void hide(){
		transform.localScale = new Vector3(0,0,0);
	}
	public void appear(){
		transform.localPosition = originalPosition;
		isAppearing = true;
		appearCounter = 0;
		if (PlayerPrefs.GetInt("HeartPieces") % 4 == 0){
			renderer.material.mainTextureOffset = new Vector2(0.5f,0);
			shouldSummaryShowArray = true;
		}
		else if (PlayerPrefs.GetInt("HeartPieces") % 4 == 1)
			renderer.material.mainTextureOffset = new Vector2(0,0.5f);
		else if (PlayerPrefs.GetInt("HeartPieces") % 4 == 2)
			renderer.material.mainTextureOffset = new Vector2(0.5f,0.5f);
		else if (PlayerPrefs.GetInt("HeartPieces") % 4 == 3)
			renderer.material.mainTextureOffset = new Vector2(0,0);
	}
	public void addToArray(Vector3 position){
		isGoingToCorner = true;
		cornerStartPosition = transform.localPosition;
		cornerEndPosition = position;
		
		cornerStartSize = transform.localScale;
		cornerEndSize = new Vector3(3,0,3);
		goToCornerCounter = -60;
	}
}
