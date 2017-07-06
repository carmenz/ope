using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;

public class FillInTheBlankManager : MonoBehaviour {
	
	private GameManager gm;
	private FillInTheBlankManager qm;
	public GameObject panel;
	private static string userpath = string.Empty;
	private int currentScore = 0;

	private int clickCounter = 3;
	bool o1Active = true;
	bool o2Active = true;
	bool o3Active = true;
	bool o4Active = true;


	private Button option1;
	private Button option2;
	private Button option3;
	private Button option4;

	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		qm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankManager>();


		XmlDocument xmlFillInTheBlankDoc = new XmlDocument ();
		xmlFillInTheBlankDoc.Load (gm.Path);
		XmlNode indexNode = xmlFillInTheBlankDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		XmlNode QNode = indexNode.NextSibling;

		if (o1Active && o2Active && o3Active && o4Active) {
			option1 = GameObject.Find ("Option1").GetComponent<Button> ();
			option2 = GameObject.Find ("Option2").GetComponent<Button> ();
			option3 = GameObject.Find ("Option3").GetComponent<Button> ();
			option4 = GameObject.Find ("Option4").GetComponent<Button> ();
		}

		// show options for question 1 -- this only execute the first time
		if (clickCounter == 3) {
			option1.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option1//value").InnerText;
			option2.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode ("//Option2//value").InnerText;
			option3.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode ("//Option3//value").InnerText;
			option4.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode ("//Option4//value").InnerText;
			clickCounter = 2;	
		}


		while (indexNode.InnerText != gm.Index.ToString ()) {
			//move to next FIB
			indexNode = indexNode.ParentNode.NextSibling.FirstChild;
		}
			
		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		while (indexNode != null) {

			if(o1Active && o2Active && o3Active && o4Active) {
				if (subIndex - 1 == 6) {
					// break once the last question is correct
					break;
				}
				questionText.text = questionText.text + indexNode.SelectSingleNode ("//Question" + subIndex).InnerText;

				// move to next question
				if (QNode.NextSibling.Name == "Q") {
					//print ("found Q");
					QNode = indexNode.NextSibling;
					print (QNode.Name);
					subIndex++;
					yield return new WaitForSeconds(2000);
				} 
			}



			yield return new WaitForSeconds(2000);
		}

		// Disable buttons
		option1.GetComponent<Button>().interactable = false; 
		option2.GetComponent<Button>().interactable = false; 
		option3.GetComponent<Button>().interactable = false; 
		option4.GetComponent<Button>().interactable = false; 


		panel.SetActive(true);
		// display score on panel
		Text panelScore = GameObject.Find("PanelScore").GetComponent<Text>();
		currentScore = currentScore + 10;
		panelScore.text = currentScore.ToString ();

		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		// Update user.xml with the score
		XmlNode fillInTheBlankIndexNode = usernameNode.ParentNode.SelectSingleNode (".//FIB//Index");

		// find the matching game index
		while (gm.Index.ToString() != fillInTheBlankIndexNode.InnerText) {
			fillInTheBlankIndexNode = fillInTheBlankIndexNode.ParentNode.NextSibling.FirstChild;
		}

		// update node if <Score> node already exist
		if (fillInTheBlankIndexNode.ParentNode.ChildNodes.Count == 2) {
			fillInTheBlankIndexNode.ParentNode.SelectSingleNode("//Score").InnerText = currentScore.ToString();
		} else {
			// create new <Score> node
			XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
			scoreIndex.InnerText = currentScore.ToString();
			fillInTheBlankIndexNode.ParentNode.AppendChild (scoreIndex);
		}


		// Find user and update <TotalScore>
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 
		usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText = 
			(int.Parse(usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();

		xmlUserDoc.Save (userpath);
	}


	public void blankToChange(int subIndex, Button option, int optionNumber) {

		Text blankText = GameObject.Find ("Blank" + subIndex).GetComponent<Text> ();

		XmlDocument xmlFillInTheBlankDoc = new XmlDocument ();
		xmlFillInTheBlankDoc.Load (gm.Path);
		XmlNode indexNode = xmlFillInTheBlankDoc.SelectSingleNode ("//Index");

		int subIndexForInfo = qm.subIndex - 1;

		// check if user answer is correct
		if(indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//correct").InnerText == "T") {
			blankText.color = Color.green;
			clickCounter = 2;
		} else {
			blankText.color = Color.red;
			clickCounter--;
		}
			
		blankText.text = option.GetComponentInChildren<Text> ().text;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();

		// user got the first chance wrong
		if (clickCounter == 1) {
			
			Button clicked = GameObject.Find ("Option" + optionNumber).GetComponent<Button> ();
			clicked.gameObject.SetActive (false);

			if (optionNumber == 1) {
				o1Active = false;
			} else if (optionNumber == 2) {
				o2Active = false;
			} else if (optionNumber == 3) {
				o3Active = false;
			} else if (optionNumber == 4) {
				o4Active = false;
			}

			// clear info text if wrong
			infoText.text = "";

		} else {

			// update info text 
			infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//info").InnerText;


			// if user got it correct in either chance, score + 10
			if (indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + optionNumber + "//correct").InnerText == "T") {
				InvokeRepeating ("AddScore", 0.0f, 0.1f);
			}

			o1Active = true;
			o2Active = true;
			o3Active = true;
			o4Active = true;

			// set all buttons active
			option1.gameObject.SetActive (true);
			option2.gameObject.SetActive (true);
			option3.gameObject.SetActive (true); 
			option4.gameObject.SetActive (true);

			// update button text to show options for next blank
			XmlNode nextBlankNode = indexNode.NextSibling.NextSibling.FirstChild.NextSibling;

			if (qm.subIndex <= 6) {
				option1.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + qm.subIndex + "//Option1//value").InnerText;
				option2.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + qm.subIndex + "//Option2//value").InnerText;
				option3.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + qm.subIndex + "//Option3//value").InnerText;
				option4.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + qm.subIndex + "//Option4//value").InnerText;
			}
			clickCounter = 2;
		}
		Start ().MoveNext();
	}

	public void Task1OnClick()
	{
		Debug.Log("option1 is clicked");
		Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
		MacthQuestionWithOption (option1, 1);
	}

	public void Task2OnClick()
	{
		Debug.Log("option2 is clicked");
		Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
		MacthQuestionWithOption (option2, 2);
	}

	public void Task3OnClick()
	{
		Debug.Log("option3 is clicked");
		Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
		MacthQuestionWithOption (option3, 3);
	}

	public void Task4OnClick()
	{
		Debug.Log("option4 is clicked");
		Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();
		MacthQuestionWithOption (option4, 4);
	}

	private void MacthQuestionWithOption(Button optionButton, int optionNumber) 
	{
		XmlDocument xmlFillInTheBlankDoc = new XmlDocument ();
		xmlFillInTheBlankDoc.Load (gm.Path);
		XmlNode indexNode = xmlFillInTheBlankDoc.SelectSingleNode ("//Index");

		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
			blankToChange (6, optionButton, optionNumber);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, optionButton, optionNumber);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, optionButton, optionNumber);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, optionButton, optionNumber);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, optionButton, optionNumber);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, optionButton, optionNumber);
		}
	}


	void AddScore () {
		currentScore++;
		Text score = GameObject.Find("Score").GetComponent<Text>();
		score.text = currentScore.ToString ();

		if (currentScore % 10 == 0) {
			CancelInvoke ();
		}
	}



}
