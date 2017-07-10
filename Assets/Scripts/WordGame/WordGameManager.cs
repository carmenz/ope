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
	public static bool finishAddingToDB = false;

	private int crossCount = 0;
	private int multiplexerCount = 0;
	public GameObject cross1;
	public GameObject cross2;
	public GameObject cross3;

	private Text multiplexer;
	private Text panelScore;

	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {
		multiplexer = GameObject.Find("Multiplexer").GetComponent<Text>();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

//		XmlDocument xmlUserDoc = new XmlDocument ();
//		xmlUserDoc.Load (userpath);

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
	
		missionCompletePanel.SetActive(true);

		// display score on panel
		panelScore = GameObject.Find("MissionCompletePanelScore").GetComponent<Text>();
		panelScore.text = currentScore.ToString();

		updateDBScore ();
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
			XmlNode wordNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
			wordNode.InnerText = "T";

			if (gameIndexNode.ParentNode.LastChild.Name == "Score") {
				int prevIndex = subIndexForInfo - 1;
				XmlNode prevNode = gameIndexNode.ParentNode.SelectSingleNode (".//Word" + prevIndex);
				gameIndexNode.ParentNode.InsertAfter (wordNode, prevNode);

			} else {
				gameIndexNode.ParentNode.AppendChild (wordNode);
			}
		
		}
		xmlUserDoc.Save (userpath);

		verifyAnswer ("T");
		// check if user got the correct answer
//		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == "T") {
//			print ("user got the correct answer");
//			multiplexerCount++;
//			crossCount = 0;
//
//			multiplexerCheck (multiplexerCount);
//
//			currentScore = currentScore + 10 * multiplexerCount;
//
//			Text score = GameObject.Find("Score").GetComponent<Text>();
//			score.text = currentScore.ToString();
//
//			cross1.gameObject.SetActive (false);
//			cross2.gameObject.SetActive (false);
//			cross3.gameObject.SetActive (false);
//
//		} else {
//			multiplexerCount = 0;
//			crossCount++;
//			multiplexerCheck (multiplexerCount);
//
//			if (crossCount == 1) {
//				cross1.gameObject.SetActive (true);
//			} else if (crossCount == 2) {
//				cross2.gameObject.SetActive (true);
//			} else {
//				cross3.gameObject.SetActive (true);
//
//				oopsPanel.SetActive(true);
//
//				// display score on panel
//				panelScore = GameObject.Find("OopsPanelScore").GetComponent<Text>();
//				panelScore.text = currentScore.ToString();
//
//				updateDBScore ();
//			}
//		}

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
		// check if user got the correct answer
//		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == "F") {
//			print ("user got the correct answer");
//			multiplexerCount++;
//			crossCount = 0;
//
//			multiplexerCheck (multiplexerCount);
//
//			currentScore = currentScore + 10 * multiplexerCount;
//
//			Text score = GameObject.Find("Score").GetComponent<Text>();
//			score.text = currentScore.ToString();
//
//
//			cross1.gameObject.SetActive (false);
//			cross2.gameObject.SetActive (false);
//			cross3.gameObject.SetActive (false);
//		} else {
//			multiplexerCount = 0;
//			crossCount++;
//			multiplexerCheck (multiplexerCount);
//
//
//			if (crossCount == 1) {
//				cross1.gameObject.SetActive (true);
//			} else if (crossCount == 2) {
//				cross2.gameObject.SetActive (true);
//			} else {
//				cross3.gameObject.SetActive (true);
//				oopsPanel.SetActive(true);
//
//				// display score on panel
//				panelScore = GameObject.Find("OopsPanelScore").GetComponent<Text>();
//				panelScore.text = currentScore.ToString();
//
//				updateDBScore ();
//			}
//		}
		Start ().MoveNext();
	}


	public void verifyAnswer(string booleanValue) {
		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");
		int subIndexForInfo = wgm.subIndex - 1;

		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == booleanValue) {
			print ("user got the correct answer");
			multiplexerCount++;
			crossCount = 0;

			multiplexerCheck (multiplexerCount);

			currentScore = currentScore + 10 * multiplexerCount;

			Text score = GameObject.Find("Score").GetComponent<Text>();
			score.text = currentScore.ToString();

			cross1.gameObject.SetActive (false);
			cross2.gameObject.SetActive (false);
			cross3.gameObject.SetActive (false);
		} else {
			multiplexerCount = 0;
			crossCount++;
			multiplexerCheck (multiplexerCount);

			if (crossCount == 1) {
				cross1.gameObject.SetActive (true);
			} else if (crossCount == 2) {
				cross2.gameObject.SetActive (true);
			} else {
				cross3.gameObject.SetActive (true);

				if (subIndex == 8) {
					missionCompletePanel.SetActive(true);

					// display score on panel
					panelScore = GameObject.Find("MissionCompletePanelScore").GetComponent<Text>();
					panelScore.text = currentScore.ToString();


				} else {
					oopsPanel.SetActive(true);


					// display score on panel
					panelScore = GameObject.Find("OopsPanelScore").GetComponent<Text>();
					panelScore.text = currentScore.ToString();
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
		if (gameIndexNode.ParentNode.ChildNodes.Count == 10) {
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

	public void multiplexerCheck(int multiplexerCount) {
		multiplexer = GameObject.Find("Multiplexer").GetComponent<Text>();
		if (multiplexerCount <= 2) {
			multiplexer.text = "x1";
		} else if (multiplexerCount <= 5) {
			multiplexer.text = "x2";
		} else {
			multiplexer.text = "x3";
		}

	}

}


