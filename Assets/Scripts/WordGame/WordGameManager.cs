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

	private int crossCount = 0;
	private int multiplexerCount = 0;
	public GameObject cross1;
	public GameObject cross2;
	public GameObject cross3;

	private Text multiplexer;

	public int subIndex = 1;

	public GameObject info;
	public Button yes;
	public Button no;

	void Awake() {
		info.SetActive (false);
	}

	// Use this for initialization
	void Start () {

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");

		while (indexNode.InnerText != gm.Index.ToString ()) {
			//move to next index
			indexNode = indexNode.ParentNode.NextSibling.FirstChild;
		}
		Text wordText = GameObject.Find ("Word").GetComponent<Text> ();

		// get the first word
		XmlNode wordNode = indexNode.NextSibling;
		wordText.text = indexNode.SelectSingleNode("//Word" + subIndex + "//value").InnerText;
		subIndex++;
	}

	public void YesOnClick()
	{
		updateOrCreateWordNode("T");

		verifyAnswer("T");
		showNextWord();
	}

	public void NoOnClick()
	{
		updateOrCreateWordNode("F");

		verifyAnswer("F");
		showNextWord();
	}


	public void updateOrCreateWordNode(string value) {
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		int subIndexForInfo = wgm.subIndex - 1;

		// update user.xml to store user answers
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		XmlNode islandNode = usernameNode.ParentNode.SelectSingleNode(".//TotalScore").NextSibling;
		XmlNode gameIndexNode = islandNode.SelectSingleNode (".//Game//Index");

		// find the matching game index
		while (gm.Index.ToString() != gameIndexNode.InnerText) {
			gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
		}

		XmlNode gameNode = gameIndexNode.ParentNode;
		// clear history if <word#> node already exist
		if (gameNode.ChildNodes.Count > subIndexForInfo + 1) {
			while (gameNode.ChildNodes.Count > 1) {
				gameNode.RemoveChild (gameNode.LastChild);
			}
		} 
		// create new <word#> node
		XmlNode wordNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
		wordNode.InnerText = value;
		gameNode.AppendChild (wordNode);

		xmlUserDoc.Save (userpath);
	}



	public void showNextWord() {
		if (subIndex != 9) {
			XmlDocument xmlWordGameDoc = new XmlDocument ();
			xmlWordGameDoc.Load (gm.Path);
			XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");
			while (indexNode.InnerText != gm.Index.ToString ()) {
				//move to next word
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
			Text wordText = GameObject.Find ("Word").GetComponent<Text> ();
			wordText.text = indexNode.ParentNode.SelectSingleNode (".//Word" + subIndex + "//value").InnerText;
			subIndex++;
		} else {
			// add subIndex for the last word
			subIndex++;
		}
	}

	public void showPanel(string type) {
		if (type == "MissionComplete") {
			missionCompletePanel.SetActive (true);
			AudioSource audio = GameObject.Find ("AudioComplete").GetComponent<AudioSource> ();
			audio.Play ();
		} else {
			oopsPanel.SetActive(true);
		}

		// display score on panel
		Text panelScore = GameObject.Find("PanelScore").GetComponent<Text>();
		panelScore.text = currentScore.ToString();
	}



	public void verifyAnswer(string booleanValue) {
		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");
		int subIndexForInfo = wgm.subIndex - 1;

		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//Yes").InnerText == booleanValue) {
			AudioSource audio = GameObject.Find("AudioCorrect").GetComponent<AudioSource>();
			audio.Play();

			StartCoroutine(FadeResultInAndOut(0.6f,"Correct"));
			updateCurrentScore ();

			cross1.gameObject.SetActive (false);
			cross2.gameObject.SetActive (false);
			cross3.gameObject.SetActive (false);

		} else {
			updateCrosses ();
			StartCoroutine(FadeResultInAndOut(0.6f,"Incorrect"));
		}
	}


	public void updateCrosses() {
		multiplexerCount = 0;
		crossCount++;
		multiplexerCheck (multiplexerCount);

		if (crossCount == 1) {
			cross1.gameObject.SetActive (true);
		} else if (crossCount == 2) {
			cross2.gameObject.SetActive (true);
		} else {
			cross3.gameObject.SetActive (true);
		}
	}

	public IEnumerator FadeResultInAndOut(float t, string correctness){
		info.SetActive (true);
		Text infoText = GameObject.Find ("Info").GetComponentInChildren<Text>();
		GameObject textContainer = GameObject.Find ("Info");
		if (correctness == "Correct") {
			infoText.text = "Yes! Correct answer!";

			multiplexerCount++;
			crossCount = 0;
			multiplexerCheck (multiplexerCount);

		} else {
			infoText.text = "Oops sorry. Wrong answer!";
		}
		yes.gameObject.SetActive(false);
		no.gameObject.SetActive(false);

		yield return new WaitForSeconds (1);
		yes.gameObject.SetActive(true);
		no.gameObject.SetActive(true);
		info.SetActive (false);
		print (subIndex);
		if (crossCount == 3) {
			if (subIndex == 10) {
				showPanel ("MissionComplete");
			} else {
				showPanel ("Oops");
			}
			updateDBScore ();
			gm.updateDBTotalScore (currentScore);
		} else {
			if (subIndex == 10) {
				yes.gameObject.SetActive (false);
				no.gameObject.SetActive (false);
				showPanel ("MissionComplete");
				updateDBScore ();
				gm.updateDBTotalScore (currentScore);
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

		// create new <Score> node
		XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
		scoreIndex.InnerText = currentScore.ToString();
		gameIndexNode.ParentNode.AppendChild (scoreIndex);

		xmlUserDoc.Save (userpath);
	}

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
		Text score = GameObject.Find("Score").GetComponent<Text>();
		score.text = currentScore.ToString();
	}


}


