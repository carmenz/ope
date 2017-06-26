using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class Option4 : MonoBehaviour {

	private QuizManager qm;
	private GameManager gm;

	// Use this for initialization
	void Start() {
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public void onClick() {
		XmlDocument xmlQuizDoc = new XmlDocument ();
		xmlQuizDoc.Load (gm.Path);
		XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Index");

		Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
		Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
		Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
		Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();

		qm.Task4OnClick();
	
		int subIndex = qm.subIndex - 1;

		option1.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.SelectSingleNode ("//Blank"+ subIndex +"//Option1//value").InnerText;
		option2.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.SelectSingleNode ("//Blank"+ subIndex +"//Option2//value").InnerText;
		option3.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.SelectSingleNode ("//Blank"+ subIndex +"//Option3//value").InnerText;
		option4.GetComponentInChildren<Text> ().text = indexNode.NextSibling.NextSibling.FirstChild.SelectSingleNode ("//Blank"+ subIndex +"//Option4//value").InnerText;

	}

}
