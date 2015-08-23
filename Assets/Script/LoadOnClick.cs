using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public void LoadScene(int level){
		Application.LoadLevel (level);
	}

	public void Quit(){
		Application.Quit ();
	}

	public void LevelSelect(){
		GameObject.FindGameObjectWithTag ("Menu").SetActive (false);
		GameObject.FindGameObjectWithTag ("Levels").SetActive (true);
	}
}
