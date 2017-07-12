using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;
using DG.Tweening;

public class FillInTheBlankManager : MonoBehaviour {
	
	private GameManager gm;
	private FillInTheBlankManager fibm;
	public GameObject panel;
	public GameObject ten;
	private static string userpath = string.Empty;
	private int currentScore = 0;
	private int scorePlaceholder = 0;

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
	Vector2 startPos;

	// Use this for initialization
	IEnumerator Start () {

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankManager>();


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
				if (subIndex - 1 == 7) {
					// break once the last question is shown
					print("break");
					break;
				}
				questionText.text = questionText.text + indexNode.SelectSingleNode ("//Question" + subIndex).InnerText;

				// move to next question
				if (QNode.NextSibling.Name == "Q") {
					QNode = indexNode.NextSibling;
					print (QNode.Name);
					subIndex++;
					yield return new WaitForSeconds(2000);
					//StartCoroutine(AddListener());
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

		print (currentScore);
		currentScore = currentScore + 10;
		print (currentScore);
		panelScore.text = currentScore.ToString ();

		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}
		XmlNode userNode = usernameNode.ParentNode;

		// Update user.xml with the score
		XmlNode fillInTheBlankIndexNode = userNode.SelectSingleNode (".//FIB//Index");

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
		userNode.SelectSingleNode ("TotalScore").InnerText = 
			(int.Parse(userNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();

		xmlUserDoc.Save (userpath);
	}

	private static Vector2 temp;
	public void blankToChange(int subIndex, Button option, int optionNumber) {

		Text blankText = GameObject.Find ("Blank" + subIndex).GetComponent<Text> ();

		XmlDocument xmlFillInTheBlankDoc = new XmlDocument ();
		xmlFillInTheBlankDoc.Load (gm.Path);
		XmlNode indexNode = xmlFillInTheBlankDoc.SelectSingleNode ("//Index");

		int subIndexForInfo = fibm.subIndex - 1;

		// check if user answer is correct
		if(indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//correct").InnerText == "T") {
			blankText.color = Color.green;
			blankText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//value").InnerText;
			clickCounter = 2;
		} else {
			blankText.color = Color.red;
			clickCounter--;
		}

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

			// update info text on screen
			// OLD VERSION
			// infoText.text = option.GetComponentInChildren<Text>().text + " is not correct, try again!";
			// New version july11
			infoText.text = "Ooh, that's incorrect. Try again!";

		} else {

			// if user got it correct in either chance, score + 10
			if (indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + optionNumber + "//correct").InnerText == "T") {
				var info = indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + optionNumber + "//info").InnerXml;
				infoText.text = "That's correct! " + info;

				currentScore = currentScore + 10;
				InvokeRepeating ("AddScore", 0.0f, 0.07f);
	
				StartCoroutine (FadeTextInAndOut (0.6f));

//				var y = ten.transform.localPosition.y;
//				ten.SetActive(true);
//				ten.transform.DOLocalMoveY(-182, 2f);
//				ten.GetComponent<Text>().DOFade(0, 2f).OnComplete(() => {
//					ten.SetActive(false);
//					ten.transform.DOLocalMoveY(y, 0f);
//					ten.GetComponent<Text>().DOFade(1, 2f);

			} else {
				int check = 1;
				while (indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + check + "//correct").InnerText != "T") {
					check++;
				}
				XmlNode correctAnsNode = indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + check + "//value");
				var info = indexNode.SelectSingleNode ("//Blank" + subIndexForInfo + "//Option" + optionNumber + "//info").InnerXml;
				// OLD version
				// infoText.text = option.GetComponentInChildren<Text> ().text + " is also not correct." + correctAnsNode.InnerText + " is the correct answer.";
				// New version july11
				infoText.text = "That's still incorrect. " + info;
				blankText.text = correctAnsNode.InnerText;
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

			if (fibm.subIndex <= 7) {
				option1.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + fibm.subIndex + "//Option1//value").InnerText;
				option2.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + fibm.subIndex + "//Option2//value").InnerText;
				option3.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + fibm.subIndex + "//Option3//value").InnerText;
				option4.GetComponentInChildren<Text> ().text = nextBlankNode.SelectSingleNode ("//Blank" + fibm.subIndex + "//Option4//value").InnerText;
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

		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question7").InnerText)) {
			blankToChange (7, optionButton, optionNumber);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
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


	public void AddScore () {
		scorePlaceholder++;
		Text score = GameObject.Find("Score").GetComponent<Text>();
		score.text = scorePlaceholder.ToString ();
		if (scorePlaceholder % 10 == 0) {
			CancelInvoke ();
		}
	}

	IEnumerator Wait() {

		print ("hehhheheheehhee");
		yield return new WaitForSeconds(40f);
	}



	public IEnumerator FadeTextInAndOut(float t){
		Text i = GameObject.Find ("Tennnn").GetComponent<Text> ();
		Color tempColor = i.color;
		i.color = new Color(0, 255, 0, 1);
		Vector2 target = new Vector2 (i.transform.position.x, i.transform.position.y + 0.2f);
		Vector2 temp = new Vector2 (i.transform.position.x, i.transform.position.y);

		while (i.color.a > 0.3f)
		{
			i.color = Color.Lerp (i.color, Color.clear, 0.9f * Time.deltaTime);
			//Transform target;
			float step = 0.2f * Time.deltaTime;
			i.transform.position = Vector2.MoveTowards (i.transform.position, target, step);
			
			yield return null;
		}
		i.transform.position = Vector2.MoveTowards (i.transform.position, temp, 1);
		i.color = tempColor;

	}



}
