using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;

public class QuizManager : MonoBehaviour {
	
	private GameManager gm;
	private bool optionChosen = false;


	// Use this for initialization
	IEnumerator Start () {
		
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		XmlDocument xmlQuizDoc = new XmlDocument ();

		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		bool finishLoading = false;
		while (!finishLoading) {

			if (indexNode.InnerText == gm.Index) {
				print ("this is the quiz to be display");

				while (indexNode != null) {

					Text questionText = GameObject.Find ("Question").GetComponent<Text> ();
					questionText.text = questionText.text + indexNode.NextSibling.FirstChild.InnerText;



					Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
					Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
					Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
					Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();


					option1.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option1//value").InnerText;
					option2.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode ("//Option2//value").InnerText;
					option3.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode ("//Option3//value").InnerText;
					option4.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode ("//Option4//value").InnerText;

					yield break;
					// move to next question in the quiz
					if (indexNode.ParentNode.NextSibling.FirstChild != null) {
						indexNode = indexNode.ParentNode.NextSibling.FirstChild;
						yield return null;
					}

				}

				finishLoading = true;
			} else {
				//move to next quiz
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}

	}



	public void Task1OnClick()
	{
		Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
		Debug.Log("option1 is chosed");
		Text blank1Text1 = GameObject.Find ("Blank1").GetComponent<Text> ();

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");
		if (indexNode.NextSibling.FirstChild.NextSibling.FirstChild.SelectSingleNode ("//Option1//correct").InnerText == "T") {
			blank1Text1.color = Color.green;
		} else {
			blank1Text1.color = Color.red;
		}
		blank1Text1.text = option1.GetComponentInChildren<Text> ().text;
		optionChosen = true;
		Start ().MoveNext();

	}
	public void Task2OnClick()
	{
		Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
		Debug.Log("option2 is chosed");
		Text blank2Text1 = GameObject.Find ("Blank1").GetComponent<Text> ();

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");
		if (indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode ("//Option2//correct").InnerText == "T") {
			blank2Text1.color = Color.green;
		} else {
			blank2Text1.color = Color.red;
		}
		blank2Text1.text = option2.GetComponentInChildren<Text> ().text;
		optionChosen = true;

	}
	public void Task3OnClick()
	{
		Button option3 = GameObject.Find ("Option2").GetComponent<Button> ();
		Debug.Log("option3 is chosed");
		Text blank3Text1 = GameObject.Find ("Blank1").GetComponent<Text> ();

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");
		if (indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode ("//Option3//correct").InnerText == "T") {
			blank3Text1.color = Color.green;
		} else {
			blank3Text1.color = Color.red;
		}
		blank3Text1.text = option3.GetComponentInChildren<Text> ().text;
		optionChosen = true;

	}
	public void Task4OnClick()
	{
		Button option4 = GameObject.Find ("Option2").GetComponent<Button> ();
		Debug.Log("option4 is chosed");
		Text blank4Text1 = GameObject.Find ("Blank1").GetComponent<Text> ();

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");
		if (indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode ("//Option4//correct").InnerText == "T") {
			blank4Text1.color = Color.green;
		} else {
			blank4Text1.color = Color.red;
		}
		blank4Text1.text = option4.GetComponentInChildren<Text> ().text;
		optionChosen = true;

	}
		

}
