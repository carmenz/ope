using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class WordGameManager : MonoBehaviour {

	private GameManager gm;
	private WordGameManager wgm;
	public GameObject missionCompletePanel;
	public GameObject oopsPanel;
	private static string userpath = string.Empty;
	private int currentScore = 0;
	//public static bool finishAddingToDB = false;


	private int crossCount = 0;
	private int multiplexerCount = 0;
	public GameObject cross1;
	public GameObject cross2;
	public GameObject cross3;

	private Text multiplexer;
//	private Text panelScore;

	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
		//multiplexer = GameObject.Find("Multiplexer").GetComponent<Text>();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");


		print("this is game" + gm.Index.ToString());

		while (indexNode.InnerText != gm.Index.ToString ()) {
			//move to next word
			indexNode = indexNode.ParentNode.NextSibling.FirstChild;
		}

		Text wordText = GameObject.Find ("Word").GetComponent<Text> ();
		XmlNode wordNode = indexNode.NextSibling;
		while (indexNode != null) {

			if (subIndex - 1 == 8) {
				print ("nullll");
				break;
			}
			wordText.text = indexNode.SelectSingleNode("//Word" + subIndex + "//value").InnerText;

			// move to next question in the quiz
			if (wordNode.NextSibling != null) {
				wordNode = indexNode.NextSibling;

				subIndex++;
				yield return new WaitForSeconds(2000);
			} 
		}
	
		showPanel ("MissionComplete");
		AudioSource audio = GameObject.Find("AudioComplete").GetComponent<AudioSource>();
		audio.Play();
		updateDBScore ();
		gm.updateDBTotalScore (currentScore);

	}

	public void showPanel(string type) {
		if (type == "MissionComplete") {
			missionCompletePanel.SetActive (true);
		} else {
			oopsPanel.SetActive(true);
		}
		// display score on panel
		Text panelScore = GameObject.Find("PanelScore").GetComponent<Text>();
		panelScore.text = currentScore.ToString();
	}


	public void YesOnClick()
	{
		Debug.Log("yes is clicked");
		//finishAddingToDB = false;

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		int subIndexForInfo = wgm.subIndex - 1;

		// Disable now to use new texts
		// Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		// infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;

		// update user.xml to store user answers
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}


		//XmlNode islandNode = usernameNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
		XmlNode islandNode = usernameNode.ParentNode.SelectSingleNode(".//TotalScore").NextSibling;
		XmlNode gameIndexNode = islandNode.SelectSingleNode (".//Game//Index");

		// find the matching game index
		while (gm.Index.ToString() != gameIndexNode.InnerText) {
			gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
		}

		// update node if <word#> node already exist
		if (gameIndexNode.ParentNode.ChildNodes.Count > subIndexForInfo + 1) {
			gameIndexNode.ParentNode.SelectSingleNode (".//Word" + subIndexForInfo).InnerText = "T";
			//finishAddingToDB = true;


		} else {
			// create new <word#> node
			XmlNode wordNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
			wordNode.InnerText = "T";

			if (gameIndexNode.ParentNode.LastChild.Name == "Score") {
				print ("heheheh");
				int prevIndex = subIndexForInfo - 1;
				XmlNode prevNode = gameIndexNode.ParentNode.SelectSingleNode (".//Word" + prevIndex);
				gameIndexNode.ParentNode.InsertAfter (wordNode, prevNode);

			} else {
				gameIndexNode.ParentNode.AppendChild (wordNode);
			}
		
		}
		xmlUserDoc.Save (userpath);

		verifyAnswer ("T");

		Start ().MoveNext();
	}

	public void NoOnClick()
	{
		Debug.Log("No is clicked");
		//finishAddingToDB = false;

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		int subIndexForInfo = wgm.subIndex - 1;

		//Disable now to use new texts
		// Text infoText = GameObject.Find ("Info").GetComponent<Text> ();
		// infoText.text = indexNode.SelectSingleNode ("//Word"+ subIndexForInfo + "//info").InnerText;



		// update user.xml to store user answers
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		//XmlNode islandNode = usernameNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
		XmlNode islandNode = usernameNode.ParentNode.SelectSingleNode(".//TotalScore").NextSibling;
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

			XmlNode wordNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
			wordNode.InnerText = "F";

			if (gameIndexNode.ParentNode.LastChild.Name == "Score") {
				int prevIndex = subIndexForInfo - 1;
				XmlNode prevNode = gameIndexNode.ParentNode.SelectSingleNode (".//Word" + prevIndex);
				gameIndexNode.ParentNode.InsertAfter (wordNode, prevNode);

			} else {
				gameIndexNode.ParentNode.AppendChild (wordNode);
			}
		


		}
		xmlUserDoc.Save (userpath);

		verifyAnswer ("F");
		Start ().MoveNext();
	}


	public void verifyAnswer(string booleanValue) {
		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");
		int subIndexForInfo = wgm.subIndex - 1;
		Text infoText = GameObject.Find ("Info").GetComponent<Text> ();

		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == booleanValue) {
			AudioSource audio = GameObject.Find("AudioCorrect").GetComponent<AudioSource>();
			audio.Play();

			infoText.text = "Yes! Correct answer!";
			multiplexerCount++;
			crossCount = 0;

			multiplexerCheck (multiplexerCount);

			updateCurrentScore ();

			Text score = GameObject.Find("Score").GetComponent<Text>();
			score.text = currentScore.ToString();

			cross1.gameObject.SetActive (false);
			cross2.gameObject.SetActive (false);
			cross3.gameObject.SetActive (false);
		} else {
			infoText.text = "Oops sorry. Wrong answer!";
			multiplexerCount = 0;
			crossCount++;
			multiplexerCheck (multiplexerCount);

			if (crossCount == 1) {
				cross1.gameObject.SetActive (true);
			} else if (crossCount == 2) {
				cross2.gameObject.SetActive (true);
			} else {
				cross3.gameObject.SetActive (true);
			
				if (subIndex == 9) {
					showPanel ("MissionComplete");
					AudioSource audio = GameObject.Find("AudioComplete").GetComponent<AudioSource>();
					audio.Play();
				} else {
					showPanel ("Oops");
				}
				updateDBScore ();
			}
		}
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
		//if (gameIndexNode.ParentNode.ChildNodes.Count == 10) {
		if(gameIndexNode.ParentNode.LastChild.Name == "Score") {
			gameIndexNode.ParentNode.SelectSingleNode("//Score").InnerText = currentScore.ToString();
			//finishAddingToDB = true;
		} else {
			// create new <Score> node
			XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
			scoreIndex.InnerText = currentScore.ToString();
			gameIndexNode.ParentNode.AppendChild (scoreIndex);
		}
		xmlUserDoc.Save (userpath);
	}


//	public void updateDBTotalScore() {
//		XmlDocument xmlUserDoc = new XmlDocument ();
//		xmlUserDoc.Load (userpath);
//		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
//		// Find user and update <TotalScore>
//		while (usernameNode.InnerText != gm.Username) {
//			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
//		} 
//		print ("hahaha");
//		usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText = 
//			(int.Parse(usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();
//
//		xmlUserDoc.Save (userpath);
//	}


	public void multiplexerCheck(int multiplexerCount) {
		multiplexer = GameObject.Find("Multiplexer").GetComponent<Text>();
		if (multiplexerCount <= 2) {
			multiplexer.text = "x1";

		} else if (multiplexerCount <= 5) {
			multiplexer.text = "x2";
			if (multiplexerCount == 3) {
				multiplexer.GetComponent<Animation>().Play();
				AudioSource audio = GameObject.Find("AudioMultiplier").GetComponent<AudioSource>();
				audio.Play();
			}
		} else {
			multiplexer.text = "x3";
			if (multiplexerCount == 6) {
				multiplexer.GetComponent<Animation>().Play();
				AudioSource audio = GameObject.Find("AudioMultiplier").GetComponent<AudioSource>();
				audio.Play();
			}
		}
	}


	public void updateCurrentScore() {
		if (multiplexer.text == "x1") {
			currentScore = currentScore + 10 * 1;
		} else if (multiplexer.text == "x2") {
			currentScore = currentScore + 10 * 2;
		} else {
			currentScore = currentScore + 10 * 3;
		}
	}
}


