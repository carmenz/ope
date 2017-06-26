using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class WordGameManager : MonoBehaviour {

	private GameManager gm;
	private WordGameManager wgm;
	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();

		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		bool finishLoading = false;

		while (!finishLoading) {

			if (indexNode.InnerText == gm.Index) {

				Text wordText = GameObject.Find ("Word").GetComponent<Text> ();
				XmlNode wordNode = indexNode.NextSibling;
				while (indexNode != null) {

					if (subIndex - 1 == 6) {
						print ("nullll");
						break;
					}
					wordText.text = indexNode.SelectSingleNode("//Word" + subIndex + "//value").InnerText;


					Button thisPillar = GameObject.Find ("This").GetComponent<Button> ();
					Button otherPillar = GameObject.Find ("Other").GetComponent<Button> ();




					// move to next question in the quiz
					if (wordNode.NextSibling != null) {
						//print ("found Q");
						wordNode = indexNode.NextSibling;
						print (wordNode.Name);
						subIndex++;
						yield return new WaitForSeconds(2000);
					} 

				}


				finishLoading = true;
			} else {


				//move to next quiz
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}
	}


	public void ThisPillarOnClick()
	{
		Debug.Log("thisPillar is clicked");


		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");



		int subIndexForInfo = wgm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;

		Start ().MoveNext();


	}
	public void OtherPillarOnClick()
	{
		Debug.Log("otherPillar is clicked");


		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");



		int subIndexForInfo = wgm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;

		Start ().MoveNext();

	}

	// Update is called once per frame
	void Update () {
		
	}
}


