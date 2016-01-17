using UnityEngine;
using System.Collections;

public static class Global{

	public static string stackLevel = "stack00";
	public static int quickGameLives = 10;
	public static bool isGameActive = false;
	public static bool isGamePaused = false;
	public static int extraChances = PlayerPrefs.GetInt("ExtraChances",0);
	
	public static int getGameLives(){
		if (stackLevel == "tutorial")
			return 15;
		else
			return (10 + PlayerPrefs.GetInt("HeartPieces",0)/4);
	}
}

