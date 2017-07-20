using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;
using DG.Tweening;

public class Option : MonoBehaviour {

	private FillInTheBlankNewManager fibm;

	[SerializeField]
	int optionIndex;
	string _value;
	bool _isCorrect;
	string _info;

	void Start() {
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankNewManager>();
	}

	public void Init(string value, string info, bool isCorrect) {
		_value = value;
		_info = info;
		_isCorrect = isCorrect;
		Render();
	}

	public void Render() {
		gameObject.transform.Find("Text").GetComponent<Text>().text = _value;
	}

	public void OnClick() {
		if (_isCorrect) {
			// got correct answer
			// TODO: add score
			// TODO: check chance
			//fibm.AddScore(第一次就正确的分数or第二次正确的分数);
			fibm.RenderAnswerIntoGameView(_value, _isCorrect);
			fibm.RenderInfo("That's correct! " + _info);
			fibm.RenderNext();
		} else {
			fibm.Chance--;
			// check chance if chance - 1 > 0
			if (fibm.Chance > 0) {
				// alert first wrong
				fibm.RenderInfo("Ooh, that's incorrect. Try again!");
				// disable this button
				gameObject.SetActive(false);
			} else {
				// alert correct answer
				// TODO: add score
				//fibm.AddScore(不正确加不加分？);
				fibm.RenderAnswerIntoGameView(_value, _isCorrect);
				fibm.RenderInfo("That's still incorrect. " + _info);
				fibm.RenderNext();
			}
		}
	}
}
