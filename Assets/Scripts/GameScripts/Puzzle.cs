using UnityEngine;
using System.Collections;

public class Puzzle {
	public int baseState;
	public int[] tiles;
	public int size;
	public bool isZoomedIn;
	public bool isStackOrder;
	
	public Puzzle(int numMoves){
		tiles = new int[numMoves];
		size = numMoves;
	}
}