using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestQuiz : MonoBehaviour {

	public string mainSceneName = "Main";
	GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void QuitQuiz() {
		gm.missions.quizRequired.Remove(gm.typeCode);
		gm.typeCode = 0;
		SceneManager.LoadScene(mainSceneName);
	}
}
