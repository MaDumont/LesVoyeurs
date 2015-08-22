using UnityEngine;
using System.Collections;

public class scoreLabel : MonoBehaviour {

	string scoreString;

	void Start()
	{
		GameManager.getInstance ().addListener (this);
		scoreString = GameManager.getInstance().getPoints().ToString() + " points";
	}

	void OnGUI()
	{
		GUI.Box(new Rect(0,0,100,100),scoreString);
	}

	void updateScore(string points)
	{
		scoreString = points + " points";
	}
}
