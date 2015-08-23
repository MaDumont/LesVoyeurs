﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;
	private int points;
	public float Timer = 10f;
	private float timeRemaining;
	private static int winCondition;
	private GameState gamestate;
	private ArrayList pointsListeners;
    public SoundManager soundManager;
	private ArrayList guardListeners;
	private bool gameOver = false;
	private Vector3 playerPos;

	public static GameManager getInstance()
	{
		return instance;
	}

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			
			//if not, set instance to this
			instance = this;
		
		//If instance already exists and it's not this:
		else if (instance != this)
			
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Call the InitGame function to initialize the first level 
		InitGame();
        soundManager = SoundManager.getInstance();
	}
	
	//Initializes the game for each level.
	void InitGame()
	{
		timeRemaining = Timer;
		gamestate = GameState.Stealth;
	}

	private GameManager()
	{
		points = 0;
		guardListeners = new ArrayList ();
	}

	// Use this for initialization
	void Start () {
        		
	}

	void GameOver(){
		timeRemaining = 0.0f;
		gameOver = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!gameOver)
			timeRemaining -= Time.deltaTime;

		if (timeRemaining <= 0)
			GameOver ();
	}

	public void updatePoints(int delta)
	{
		points += delta;
	}

	void OnGUI()
	{
		string sidePanelString = "points : " + points + "\nstate : " + GameManager.getInstance().getGameState();
		GUI.Box(new Rect(0,0,100,100),sidePanelString);

		var x = Screen.width/2 - 50;
		GUI.Box(new Rect(x,0,100,25),timeRemaining.ToString("#.00"));

		if(gameOver){
			x = Screen.width/2 - 50;
			var y = Screen.height / 2 - 50;
			GUI.Box (new Rect (x, y, 100, 25), "Game Over!");
		}
	}

	public int getPoints()
	{
		return points;
	}

	public GameState getGameState()
	{
		return gamestate;
	}

	public void setGameState(GameState newGameState)
	{
		gamestate = newGameState;
	}

	public void addGuardListener(MonoBehaviour listner)
	{
		guardListeners.Add (listner);
	}
	
	public void removeGuardListener(MonoBehaviour listner)
	{
		guardListeners.Remove (listner);
	}

	public void setPlayerPos(Vector3 pos)
	{
		playerPos = pos;
		foreach (MonoBehaviour listener in guardListeners) 
		{
			listener.SendMessage("heardNoise", pos);
		}
	}

	public Vector3 getPlayerPos()
	{
		return playerPos;
	}

	public void stepUpGameState()
	{
		if (gamestate != GameState.Detected) {
			gamestate++;
			foreach (MonoBehaviour listener in guardListeners) 
			{
				listener.SendMessage("gameStateChanged", gamestate);
			}
		}
	}
}
