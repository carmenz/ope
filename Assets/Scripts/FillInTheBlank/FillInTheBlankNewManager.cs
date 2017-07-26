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
	int scorePlaceholder = 0;
	int currentScore = 0;
	public GameObject ten;
	public GameObject panel;

	XmlNode fib;
	//string _fibIndex;
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
		//_fibIndex = xmlFillInTheBlankDoc.SelectSingleNode("//FIB/@index").Value;
		_questions = fib.ChildNodes;
		

		
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


	public IEnumerator ReadInfo() {
		int j = 0;
		for(j = 0; j < options.Count; j++) {
				options[j].Disable();
		}

		yield return new WaitForSeconds (2);
		RenderNext();

		// reset info text
		var info = "";
		RenderInfo(info);
	}

	public void RenderNext () {
		// reset
		Chance = CHANCE;

		XmlNode questionNode = _questions.Item(_curQuestionIndex+1);

		//check if there is a next line
		// if there is no next line => open score panel!
		if (questionNode == null) {
			//StartCoroutine(RenderScorePanel ());
			RenderScorePanel();
			UpdateDBScore ();
			gm.updateDBTotalScore (currentScore);
			return;
		}

		// TODO: push next line into text box
		var questionText = questionNode.SelectSingleNode(".//Value").InnerText;
		questionTextBox.text = questionTextBox.text + questionText;

		// Init the options
		for(int j = 0; j < options.Count; j++) {
			options [j].Enable ();
		}
		var optionsNode = questionNode.SelectSingleNode(".//Options").ChildNodes;
		string correctValue = "";
		for(int j = 0; j < 4; j++) {
			if(optionsNode[j].SelectSingleNode(".//Correct").InnerText == "T") {
				correctValue = optionsNode [j].SelectSingleNode (".//Value").InnerText;
			}
		}


		for (var i=0; i< optionsNode.Count;i++) {
			options[i].gameObject.SetActive(true);
			var optionRoot = optionsNode.Item(i);
			var value = optionRoot.SelectSingleNode(".//Value").InnerText;
			var info = optionRoot.SelectSingleNode(".//Info").InnerText;
			var correctAnswer = optionRoot.SelectSingleNode(".//Correct").InnerText == "T" ? true : false;
			options[i].Init(value, info, correctAnswer, correctValue);
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

	public void RenderScorePanel() {
		//yield return new WaitForSeconds (2);
		AudioSource audio = GameObject.Find("AudioComplete").GetComponent<AudioSource>();
		audio.Play();

		panel.SetActive(true);
		// display score on panel
		Text panelScore = GameObject.Find("PanelScore").GetComponent<Text>();

		panelScore.text = currentScore.ToString ();

	}

	public void UpdateDBScore() {
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		// Update user.xml with the score
		XmlNode fillInTheBlankIndexNode = usernameNode.ParentNode.SelectSingleNode (".//FIB//Index");
		XmlNode fibNode = fillInTheBlankIndexNode.ParentNode;

		// find the matching game index
		while (gm.Index.ToString() != fillInTheBlankIndexNode.InnerText) {
			fillInTheBlankIndexNode = fibNode.NextSibling.FirstChild;
		}
			
		if (fibNode.ChildNodes.Count == 2) {
			fibNode.RemoveChild (fibNode.LastChild);
		} 

		// create new <Score> node
		XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
		scoreIndex.InnerText = currentScore.ToString();
		fillInTheBlankIndexNode.ParentNode.AppendChild (scoreIndex);

		xmlUserDoc.Save (userpath);
	}

	public void AddScore() {
		// add score
		InvokeRepeating ("AddToTen", 0.0f, 0.07f);
		currentScore = currentScore + 10;

		// animation of +10
		var y = ten.transform.localPosition.y;
		ten.SetActive(true);
		ten.transform.DOLocalMoveY(y+70, 2f, false);
		ten.GetComponent<Text> ().DOFade (0, 2f).OnComplete (() => {
			ten.SetActive (false);
			ten.transform.DOLocalMoveY (y, 0f);
			ten.GetComponent<Text> ().DOFade (1, 0f);
		});
	}

	public void AddToTen () {
		scorePlaceholder++;
		Text score = GameObject.Find("Score").GetComponent<Text>();
		score.text = scorePlaceholder.ToString ();
		if (scorePlaceholder % 10 == 0) {
			CancelInvoke ();
		}
	}



}
