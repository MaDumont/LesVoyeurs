using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;
	private int points;
	private static int winCondition;
	private GameState gamestate;
	private ArrayList pointsListeners;
    public SoundManager soundManager;

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
	}
	
	//Initializes the game for each level.
	void InitGame()
	{

	}

	private GameManager()
	{
		points = 0;
		gamestate = GameState.Stealth;
		pointsListeners = new ArrayList ();
	}

	// Use this for initialization
	void Start () {
        		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void updatePoints(int delta)
	{
		points += delta;
		foreach (MonoBehaviour listener in pointsListeners) 
		{
			listener.SendMessage("updateScore", points.ToString());
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

	public void addListener(MonoBehaviour listner)
	{
		pointsListeners.Add (listner);
	}

	public void removeListener(MonoBehaviour listner)
	{
		pointsListeners.Remove (listner);
	}
}
