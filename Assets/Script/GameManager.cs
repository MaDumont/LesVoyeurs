using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;
	private int points;
	public float Timer = 10f;

	private int stealthBonus = 0;
	private int highestSinglePoints = 0;
	private int detections = 0;
	private int nbPhotos = 0;

	private float timeRemaining;
	private static int winCondition;
	private GameState gamestate;
    public SoundManager soundManager;
	private ArrayList guardListeners;
	private bool gameOver = false;
	private bool win = false;
	private Vector3 playerPos;
	private GameObject player;
	private string endOfGameString;
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

	public void SetPlayer(GameObject go)
	{
		player = go;
	}

	// Use this for initialization
	void Start () {
        		
	}

	public void Win()
	{
		timeRemaining = 0.0f;
		endOfGameString = "Congratulations!\nYou escaped with " + nbPhotos + " pictures!";
		win = true;
		Time.timeScale = 0;
	}
	
	public void GameOver(){
		timeRemaining = 0.0f;
		gameOver = true;
		Debug.Log ("GameOver");
		Invoke ("ExitToMenu", 3);
		Time.timeScale = 0;
	}

	void ExitToMenu(){
		Debug.Log ("What");
		Application.LoadLevel (1);
	}

	public void TryToCatchPlayer(Vector3 pos)
	{
		float distance = Vector3.Distance (pos, player.transform.position);

		if (distance < 0.5f && !gameOver && !win) {
			endOfGameString = "You have been caught!!";
			GameOver();
		}
	}

	// Update is called once per frame
	void Update () {
		if(!gameOver)
			timeRemaining -= Time.deltaTime;

		if (!gameOver && !win && timeRemaining <= 0) {
			endOfGameString = "You ran out of time!!";
			GameOver ();
		}
	}

	public void updatePoints(int delta)
	{
		points += delta;

		if (delta > 100)
			nbPhotos++;

		if (delta > 100 && delta > highestSinglePoints)
			highestSinglePoints = delta;
		if (gamestate == GameState.Stealth)
			stealthBonus += delta / 2;
	}

	void OnGUI()
	{
		string sidePanelString = "State : " + GameManager.getInstance().getGameState() + "\nPoints : " + points
			+ "\nBest Photo : " + highestSinglePoints + "\nStealth Bonus : " + stealthBonus + "\nTimes Seen : " + detections;
		GUI.Box(new Rect(0,0,125,80),sidePanelString);

		if (win) {
			var x = Screen.width / 2 - 75;
			var y = Screen.height / 2 - 50;
			GUI.Box (new Rect (x, y, 200, 55), "You Won!\n" + endOfGameString);
		} else if (gameOver) {
			var x = Screen.width / 2 - 75;
			var y = Screen.height / 2 - 50;
			GUI.Box (new Rect (x, y, 150, 40), "Game Over!\n" + endOfGameString);
		} else {
			var x = Screen.width/2 - 50;
			GUI.Box(new Rect(x,0,100,25),timeRemaining.ToString("#.00"));
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
		detections++;
		if (gamestate != GameState.Detected) {
			gamestate++;
			foreach (MonoBehaviour listener in guardListeners) 
			{
				listener.SendMessage("gameStateChanged", gamestate);
			}
		}
	}
}
