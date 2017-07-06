using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

	public string mainSceneName = "Main";
	GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void QuitFillInTheBlank() {
		SceneManager.LoadScene(mainSceneName);
	}

	public void QuitWordGame() {
		SceneManager.LoadScene(mainSceneName);
	}

	public void QuitStory() {
		SceneManager.LoadScene(mainSceneName);
	}

	public void QuitQuiz() {
		SceneManager.LoadScene(mainSceneName);
	}

}
