using UnityEngine;
using System.Collections;

public class StackOrderElement{
	public StackOrderElement nextElement;
	public StackOrderElement lastElement;
	public int state;
	
	public StackOrderElement (){
		lastElement = null;
		nextElement = null;
		state = 0;
	}
	public void addToList(StackOrderElement last){
		lastElement = last;
		if (lastElement != null)
			lastElement.nextElement = this;
	}
	public void removeFromList(){
		if (lastElement != null)
			lastElement.nextElement = nextElement;
		if (nextElement != null)
			nextElement.lastElement = lastElement;
		lastElement = null;
		nextElement = null;
	}
}