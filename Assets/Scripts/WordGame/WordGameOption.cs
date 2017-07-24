using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGameOption : MonoBehaviour {

	private WordGameManager wgm;
	bool _isTrue;

	void Start() {
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();
	}

//	public void onClick() {
//		wgm.NoOnClick();
//	}


	public void Init(int i) {
		if (i == 1) {

			_isTrue = true;
		} else if (i == 2) {

			_isTrue = false;
		}
	}

	public void onClick(bool isTrue) {

		if (_isTrue) {
			wgm.UpdateOrCreateWordNode ("T");

			wgm.VerifyAnswer ("T");
			wgm.ShowNextWord ();
		} else {
			wgm.UpdateOrCreateWordNode("F");

			wgm.VerifyAnswer("F");
			wgm.ShowNextWord();
		}


	}

}
