using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {

	GameManager gm;

	void Awake () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestartGame() {
		// Reset state
		gm.CurrentPosition = 0;

		// Reload main scene
		SceneManager.LoadScene("Main");
	}
}
