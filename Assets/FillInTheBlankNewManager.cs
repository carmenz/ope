using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;
using DG.Tweening;
using System.Text.RegularExpressions;

public class FillInTheBlankNewManager : MonoBehaviour {

	static string userpath = string.Empty;
	public int Chance;

	int NUM_OF_OPTIONS = 4;
	string CORRECT_COLOR = "#008000ff";
	string WRONG_COLOR = "#ff0000ff";
	int CHANCE = 2;

	GameManager gm;
	XmlDocument xmlFillInTheBlankDoc;
	Text questionTextBox;
	List<Option> options = new List<Option>();
	Text infoTextBox;
	Text scoreBox;

	XmlNode fib;
	string _fibIndex;
	XmlNodeList _questions;
	int _curQuestionIndex = 0;
	string _lastAnswer;

	// Use this for initialization
	void Start () {
		// Load data
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		
		xmlFillInTheBlankDoc = new XmlDocument ();
		xmlFillInTheBlankDoc.Load (gm.Path);
		fib = xmlFillInTheBlankDoc.SelectSingleNode("//FIB");
		_fibIndex = xmlFillInTheBlankDoc.SelectSingleNode("//FIB/@index").Value;
		_questions = fib.ChildNodes;
		
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		
		// TODO: set up boxes
		questionTextBox = GameObject.Find ("Question").GetComponent<Text> ();
		for (var i=1;i<=NUM_OF_OPTIONS;i++) {
			options.Add(GameObject.Find ("Option" + i.ToString()).GetComponent<Option> ());
		}
		infoTextBox = GameObject.Find ("Info").GetComponent<Text> ();
		scoreBox = GameObject.Find("Score").GetComponent<Text>();

		// init first line
		RenderNext();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RenderNext () {
		// reset
		Chance = CHANCE;

		XmlNode questionNode = _questions.Item(_curQuestionIndex);

		//check if there is a next line
		if (questionNode == null)
			return;

		// TODO: push next line into text box
		var questionText = questionNode.SelectSingleNode(".//Value").InnerText;
		questionTextBox.text = questionTextBox.text + questionText;

		// Init the options
		var optionsNode = questionNode.SelectSingleNode(".//Options").ChildNodes;
		for (var i=0; i< optionsNode.Count;i++) {
			options[i].gameObject.SetActive(true);
			var optionRoot = optionsNode.Item(i);
			var value = optionRoot.SelectSingleNode(".//Value").InnerText;
			var info = optionRoot.SelectSingleNode(".//Info").InnerText;
			var correctAnswer = optionRoot.SelectSingleNode(".//Correct").InnerText == "T" ? true : false;
			options[i].Init(value, info, correctAnswer);
		}

		_curQuestionIndex++;
	}

	public void RenderAnswerIntoGameView(string answer, bool isCorrect) {
		var text = questionTextBox.text;
		var color = isCorrect ? CORRECT_COLOR : WRONG_COLOR;

		var reg = new Regex(@"________________");

		if (reg.IsMatch(text)) {
			questionTextBox.text = reg.Replace(text, "<color=" + color + ">" + answer + "</color>");
		}

	}

	public void RenderInfo(string info) {
		infoTextBox.text = info;
	}

	public void AddScore() {
		// TODO: add score
		// TODO: animation of +10
		// TODO: animation of scoreTextBox
	}

	public void ShowPanel() {
		// TODO: set score text box
		// TODO: set active
	}

}
