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
	string _correctValue;

	void Start() {
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankNewManager>();
	}

	public void Init(string value, string info, bool isCorrect, string correctValue) {
		_value = value;
		_info = info;
		_isCorrect = isCorrect;
		_correctValue = correctValue;
		Render();
	}

	public void Render() {
		gameObject.transform.Find("Text").GetComponent<Text>().text = _value;
	}

	public void OnClick() {
		if (_isCorrect) {
			// got correct answer
			// TODO: check chance
			fibm.RenderAnswerIntoGameView(_value, _isCorrect);
			fibm.RenderInfo(_info);
			fibm.AddScore ();
			StartCoroutine(fibm.ReadInfo());
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
				fibm.RenderAnswerIntoGameView(_correctValue, _isCorrect);
				fibm.RenderInfo(_info);
				StartCoroutine(fibm.ReadInfo());
			}
		}
	}

	public void Disable() {
		gameObject.GetComponent<Button> ().interactable = false;
	}


	public void Enable() {
		gameObject.GetComponent<Button> ().interactable = true;
	}


}
