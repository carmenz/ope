using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class QuizManager : MonoBehaviour {
	
	private GameManager gm;
	private QuizManager qm;
	public GameObject panel;

	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
		
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();

		XmlDocument xmlQuizDoc = new XmlDocument ();

		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		bool finishLoading = false;

		while (!finishLoading) {

			if (indexNode.InnerText == gm.Index.ToString()) {
	
				Text questionText = GameObject.Find ("Question").GetComponent<Text> ();
				XmlNode QNode = indexNode.NextSibling;
				while (indexNode != null) {
		
					if (subIndex - 1 == 6) {
						print ("nullll");

						break;
					}
					questionText.text = questionText.text + indexNode.SelectSingleNode("//Question"+subIndex).InnerText;


					Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
					Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
					Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
					Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();


					option1.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option1//value").InnerText;
					option2.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode ("//Option2//value").InnerText;
					option3.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode ("//Option3//value").InnerText;
					option4.GetComponentInChildren<Text> ().text = QNode.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode ("//Option4//value").InnerText;

				


					// move to next question in the quiz
					if (QNode.NextSibling.Name == "Q") {
						//print ("found Q");
						QNode = indexNode.NextSibling;
						print (QNode.Name);
						subIndex++;
						yield return new WaitForSeconds(2000);
					} 

				}


				finishLoading = true;

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


		if (indexNode.NextSibling.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option"+optionNumber+"//correct").InnerText == "T") {
			blankText.color = Color.green;
		} else {
			blankText.color = Color.red;
		}
		blankText.text = option.GetComponentInChildren<Text> ().text;
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
			blankToChange (6, option1, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option1, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option1, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option1, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option1, 2);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option1, 2);
		}

		int subIndexForInfo = qm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option1//info").InnerText;

		Start ().MoveNext();

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
			
		int subIndexForInfo = qm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option2//info").InnerText;

		Start ().MoveNext();

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
			blankToChange (6, option3, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option3, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option3, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option3, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option3, 2);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option3, 2);
		}

		int subIndexForInfo = qm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option3//info").InnerText;

		Start ().MoveNext();
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
			blankToChange (6, option4, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question5").InnerText)) {
			blankToChange (5, option4, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question4").InnerText)) {
			blankToChange (4, option4, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question3").InnerText)) {
			blankToChange (3, option4, 2);
		} else if (questionText.text.Contains(indexNode.SelectSingleNode ("//Question2").InnerText)) {
			blankToChange (2, option4, 2);
		} else if (questionText.text == indexNode.SelectSingleNode ("//Question1").InnerText) {
			blankToChange (1, option4, 2);
		}

		int subIndexForInfo = qm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Blank"+ subIndexForInfo +"//Option4//info").InnerText;


		Start ().MoveNext();
	}


}
