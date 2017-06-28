using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class QuizManager : MonoBehaviour {
	
	private GameManager gm;
	private QuizManager qm;
	public GameObject panel;


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
		
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();

		XmlDocument xmlQuizDoc = new XmlDocument ();

		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		bool finishLoading = false;
		XmlNode QNode = indexNode.NextSibling;

		if (o1Active && o2Active && o3Active && o4Active) {
			option1 = GameObject.Find ("Option1").GetComponent<Button> ();
			option2 = GameObject.Find ("Option2").GetComponent<Button> ();
			option3 = GameObject.Find ("Option3").GetComponent<Button> ();
			option4 = GameObject.Find ("Option4").GetComponent<Button> ();
		}

		if (clickCounter == 3) {
			option1.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option1//value").InnerText;
					option2.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode ("//Option2//value").InnerText;
					option3.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode ("//Option3//value").InnerText;
					option4.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode ("//Option4//value").InnerText;
			clickCounter = 2;
			
		}

		while (!finishLoading) {
			if (indexNode.InnerText == gm.Index.ToString()) {
	
				Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

				while (indexNode != null) {
		
					if (subIndex - 1 == 6) {
						// break once the last question is printed
						break;
					}

					if(o1Active && o2Active && o3Active && o4Active) {
						questionText.text = questionText.text + indexNode.SelectSingleNode ("//Question" + subIndex).InnerText;

						// move to next question in the quiz
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

				finishLoading = true;

				// Disable buttons
				option1.GetComponent<Button>().interactable = false; 
				option2.GetComponent<Button>().interactable = false; 
				option3.GetComponent<Button>().interactable = false; 
				option4.GetComponent<Button>().interactable = false; 
			

				panel.SetActive(true);

			} else {
				//move to next quiz
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}
	}


	public void blankToChange(int subIndex, Button option, int optionNumber) {

		Text blankText = GameObject.Find ("Blank" + subIndex).GetComponent<Text> ();

		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		int subIndexForInfo = qm.subIndex - 1;

		if(indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//correct").InnerText == "T") {
			print ("you got it correct");
			blankText.color = Color.green;
			clickCounter = 2;
		} else {
			
			blankText.color = Color.red;
			print ("wrongggggg");

			clickCounter--;

			print (clickCounter);
		}
			
		blankText.text = option.GetComponentInChildren<Text> ().text;


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
		} else {
			Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
			infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option"+ optionNumber +"//info").InnerText;

			o1Active = true;
			o2Active = true;
			o3Active = true;
			o4Active = true;

			// set all buttons active
			option1.gameObject.SetActive (true);
			option2.gameObject.SetActive (true);
			option3.gameObject.SetActive (true); 
			option4.gameObject.SetActive (true);

			// update button text to show options for next quiz
			if (qm.subIndex <= 6) {
				option1.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.NextSibling.SelectSingleNode ("//Blank" + qm.subIndex + "//Option1//value").InnerText;
				option2.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.NextSibling.SelectSingleNode ("//Blank" + qm.subIndex + "//Option2//value").InnerText;
				option3.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.NextSibling.SelectSingleNode ("//Blank" + qm.subIndex + "//Option3//value").InnerText;
				option4.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.NextSibling.SelectSingleNode ("//Blank" + qm.subIndex + "//Option4//value").InnerText;
			}
			clickCounter = 2;
		}
		Start ().MoveNext();
	}

	public void Task1OnClick()
	{
		Debug.Log("option1 is clicked");
		Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();


		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
			blankToChange (6, option1, 1);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option1, 1);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option1, 1);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option1, 1);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option1, 1);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option1, 1);
		}
	}
	public void Task2OnClick()
	{
		Debug.Log("option2 is clicked");
		Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();


		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		  
		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
			blankToChange (6, option2, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
				blankToChange (5, option2, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
				blankToChange (4, option2, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
				blankToChange (3, option2, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
				blankToChange (2, option2, 2);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
				blankToChange (1, option2, 2);
		}
	}
	public void Task3OnClick()
	{
		Debug.Log("option3 is clicked");
		Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();


		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
			blankToChange (6, option3, 3);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option3, 3);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option3, 3);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option3, 3);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option3, 3);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option3, 3);
		}
	}
	public void Task4OnClick()
	{
		Debug.Log("option4 is clicked");
		Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();

		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		Text questionText = GameObject.Find ("Question").GetComponent<Text> ();

		if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question6").InnerText)) {
			blankToChange (6, option4, 4);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option4, 4);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option4, 4);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option4, 4);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option4, 4);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option4, 4);
		}
	}

}
