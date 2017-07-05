using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisPillar : MonoBehaviour {


	private WordGameManager wgm;
	private GameManager gm;

	// Use this for initialization
	void Start() {
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void onClick() {

		wgm.YesOnClick();

	}


}