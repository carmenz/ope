using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class WordGameManager : MonoBehaviour {

	private GameManager gm;
	private WordGameManager wgm;
	public GameObject panel;
	private static string userpath = string.Empty;
	private int currentScore = 0;
	public static bool finishAddingToDB = false;

	private int crossCount = 0;
	public GameObject cross1;
	public GameObject cross2;
	public GameObject cross3;


	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
	

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		bool finishLoading = false;

		print("this is game" + gm.Index.ToString());

		while (!finishLoading) {

			if (indexNode.InnerText == gm.Index.ToString()) {

				Text wordText = GameObject.Find ("Word").GetComponent<Text> ();
				XmlNode wordNode = indexNode.NextSibling;
				while (indexNode != null) {

					if (subIndex - 1 == 6) {
						print ("nullll");
						break;
					}
					wordText.text = indexNode.SelectSingleNode("//Word" + subIndex + "//value").InnerText;

					// move to next question in the quiz
					if (wordNode.NextSibling != null) {
						//print ("found Q");
						wordNode = indexNode.NextSibling;

						subIndex++;
						yield return new WaitForSeconds(2000);
					} 
				}
				finishLoading = true;
				panel.SetActive(true);

				// display score on panel
				Text score = GameObject.Find("Score").GetComponent<Text>();
				score.text = currentScore.ToString();

				updateDBScore ();
				print ("hehehhehe");

				
			} else {
				//move to next word
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}

		updateDBTotalScore ();

	}


	public void YesOnClick()
	{
		Debug.Log("yes is clicked");
		finishAddingToDB = false;

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		int subIndexForInfo = wgm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;

		// update user.xml to store user answers
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		XmlNode islandNode = usernameNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
		XmlNode gameIndexNode = islandNode.SelectSingleNode (".//Game//Index");

		// find the matching game index
		while (gm.Index.ToString() != gameIndexNode.InnerText) {
			gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
		}

		// update node if <word#> node already exist
		if (gameIndexNode.ParentNode.ChildNodes.Count > subIndexForInfo + 1) {
			gameIndexNode.ParentNode.SelectSingleNode (".//Word" + subIndexForInfo).InnerText = "T";
			finishAddingToDB = true;


		} else {
			// create new <word#> node
			XmlNode wordIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
			wordIndex.InnerText = "T";

			gameIndexNode.ParentNode.AppendChild (wordIndex);
		
		}
		xmlUserDoc.Save (userpath);

		// check if user got the correct answer
		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == "T") {
			print ("user got the correct answer");
			currentScore = currentScore + 10;
			cross1.gameObject.SetActive (false);
			cross2.gameObject.SetActive (false);
			cross3.gameObject.SetActive (false);
			crossCount = 0;
		} else {
			crossCount++;
			if (crossCount == 1) {
				cross1.gameObject.SetActive (true);
			} else if (crossCount == 2) {
				cross2.gameObject.SetActive (true);
			} else {
				cross3.gameObject.SetActive (true);

				panel.SetActive(true);

				// display score on panel
				Text score = GameObject.Find("Score").GetComponent<Text>();
				score.text = currentScore.ToString();

				updateDBScore ();
			}
		}

		Start ().MoveNext();
	}

	public void NoOnClick()
	{
		Debug.Log("No is clicked");
		finishAddingToDB = false;

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		int subIndexForInfo = wgm.subIndex - 1;

		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;



		// update user.xml to store user answers
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		XmlNode islandNode = usernameNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
		XmlNode gameIndexNode = islandNode.SelectSingleNode (".//Game//Index");

		// find the matching game index
		while (gm.Index.ToString() != gameIndexNode.InnerText) {
			gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
		}

		// update node if <word#> node already exist
		if (gameIndexNode.ParentNode.ChildNodes.Count > subIndexForInfo+1) {
			gameIndexNode.ParentNode.SelectSingleNode(".//Word" + subIndexForInfo).InnerText = "F";
			
		} else {
			// create new <word#> node
			XmlNode wordIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
			wordIndex.InnerText = "F";

			gameIndexNode.ParentNode.AppendChild (wordIndex);
		}
		xmlUserDoc.Save (userpath);


		// check if user got the correct answer
		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == "F") {
			print ("user got the correct answer");
			currentScore = currentScore + 10;
			crossCount = 0;
			cross1.gameObject.SetActive (false);
			cross2.gameObject.SetActive (false);
			cross3.gameObject.SetActive (false);
		} else {
			crossCount++;
			if (crossCount == 1) {
				cross1.gameObject.SetActive (true);
			} else if (crossCount == 2) {
				cross2.gameObject.SetActive (true);
			} else {
				cross3.gameObject.SetActive (true);
				panel.SetActive(true);

				// display score on panel
				Text score = GameObject.Find("Score").GetComponent<Text>();
				score.text = currentScore.ToString();

				updateDBScore ();
			}
		}
		Start ().MoveNext();
	}

	public void updateDBScore() {

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		while (usernameNode.InnerText != gm.Username) {

			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}
		// Update user.xml with the score
		XmlNode gameIndexNode = usernameNode.ParentNode.SelectSingleNode (".//Game//Index");

		// find the matching game index
		while (gm.Index.ToString() != gameIndexNode.InnerText) {
			gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
		}

		// update node if <Score> node already exist
		if (gameIndexNode.ParentNode.ChildNodes.Count == 8) {
			gameIndexNode.ParentNode.SelectSingleNode("//Score").InnerText = currentScore.ToString();
			//finishAddingToDB = true;
		} else {
			// create new <Score> node
			XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
			scoreIndex.InnerText = currentScore.ToString();
			print (gameIndexNode.ParentNode.InnerXml);
			gameIndexNode.ParentNode.AppendChild (scoreIndex);
			print (gameIndexNode.ParentNode.InnerXml);
		}
		xmlUserDoc.Save (userpath);
	}


	public void updateDBTotalScore() {
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		// Find user and update <TotalScore>
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 
		usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText = 
			(int.Parse(usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();

		xmlUserDoc.Save (userpath);
	}

}


