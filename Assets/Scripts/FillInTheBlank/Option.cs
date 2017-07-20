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
	string value;
	bool correctAnswer;
	string info;
	XmlNode root;

	void Start() {
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankNewManager>();
	}

	public void Init(XmlNode optionNode) {
		root = optionNode;
		value = root.SelectSingleNode(".//Value").InnerText;
		info = root.SelectSingleNode(".//Info").InnerText;
		correctAnswer = root.SelectSingleNode(".//Correct").InnerText == "T" ? true : false;
		Render();
	}

	public void Render() {
		gameObject.transform.Find("Text").GetComponent<Text>().text = value;
	}

	public void OnClick() {
		if (correctAnswer) {
			// got correct answer
			// TODO: add score
			// TODO: check chance
			//fibm.AddScore(第一次就正确的分数or第二次正确的分数);
			fibm.RenderAnswerIntoGameView(value, correctAnswer);
			fibm.RenderInfo("That's correct! " + info);
			fibm.RenderNext();
		} else {
			fibm.CHANCE--;
			// check chance if chance - 1 > 0
			if (fibm.CHANCE > 0) {
				// alert first wrong
				fibm.RenderInfo("Ooh, that's incorrect. Try again!");
				// disable this button
				gameObject.SetActive(false);
			} else {
				// alert correct answer
				// TODO: add score
				//fibm.AddScore(不正确加不加分？);
				fibm.RenderAnswerIntoGameView(value, correctAnswer);
				fibm.RenderInfo("That's still incorrect. " + info);
				fibm.RenderNext();
			}
		}
	}
}
