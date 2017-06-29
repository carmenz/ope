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
		gm.missions.fillInTheBlankRequired.Remove(gm.typeCode);
		gm.typeCode = 0;
		SceneManager.LoadScene(mainSceneName);
	}

	public void QuitWordGame() {
		gm.missions.gameRequired.Remove(gm.typeCode);
		gm.typeCode = 0;
		SceneManager.LoadScene(mainSceneName);
	}

	public void QuitStory() {
		gm.missions.storyRequired.Remove(gm.typeCode);
		gm.typeCode = 0;
		SceneManager.LoadScene(mainSceneName);
	}

}
