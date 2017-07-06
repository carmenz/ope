using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoButton : MonoBehaviour {

	private WordGameManager wgm;

	void Start() {
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();
	}

	public void onClick() {
		wgm.NoOnClick();
	}
}
