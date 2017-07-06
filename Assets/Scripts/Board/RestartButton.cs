using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {

	GameManager gm;

	void Awake () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void RestartGame() {
		// Reset state
		gm.Coordinate = new Vector2 (0,0);

		// Reload main scene
		SceneManager.LoadScene("Main");
	}
}
