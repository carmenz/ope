using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;

public class QuizManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		bool finishLoading = false;


		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");


		while (!finishLoading) {

			if (indexNode.InnerText == gm.Index) {
				print ("this is the quiz to be display");
				Text questionText = GameObject.Find("Question").GetComponent<Text> ();
				questionText.text = indexNode.NextSibling.FirstChild.InnerText;

				Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
				Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
				Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
				Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();


				option1.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.FirstChild.SelectSingleNode("//Option1//value").InnerText;
				option2.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (1).SelectSingleNode("//Option2//value").InnerText;
				option3.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (2).SelectSingleNode("//Option3//value").InnerText;
				option4.GetComponentInChildren<Text> ().text = indexNode.NextSibling.FirstChild.NextSibling.ChildNodes.Item (3).SelectSingleNode("//Option4//value").InnerText;


				finishLoading = true;
			} else {
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

}
