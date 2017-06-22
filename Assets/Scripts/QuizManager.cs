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
