using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesButton : MonoBehaviour {

	private WordGameManager wgm;

	void Start() {
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();
	}

	public void onClick() {
		wgm.YesOnClick();
	}
}