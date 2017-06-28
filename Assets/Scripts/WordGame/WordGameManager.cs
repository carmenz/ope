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
	private int currentScore = 70;
	public static bool finishAddingToDB = false;

	public int subIndex = 1;

	// Use this for initialization
	IEnumerator Start () {

		print ("scorrrrrrr" + currentScore);
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();
		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");


		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);

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

				Text score = GameObject.Find("Score").GetComponent<Text>();
				score.text = currentScore.ToString();


				// Update user.xml with the score

				XmlNode gameIndexNode = xmlUserDoc.SelectSingleNode ("//Game//Index");

				// find the matching game index
				while (gm.Index.ToString() != gameIndexNode.InnerText) {
					gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
				}
					
				// update node if <Score> node already exist
				if (gameIndexNode.ParentNode.ChildNodes.Count == 8) {
					gameIndexNode.ParentNode.SelectSingleNode("//Score").InnerText = currentScore.ToString();
					finishAddingToDB = true;
				} else {
					// create new <Score> node
					XmlNode scoreIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Score", null);
					scoreIndex.InnerText = currentScore.ToString();
					gameIndexNode.ParentNode.AppendChild (scoreIndex);

				}



			} else {
				//move to next quiz
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}
		}
		xmlUserDoc.Save (userpath);
	}


	public void ThisPillarOnClick()
	{
		Debug.Log("thisPillar is clicked");
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

		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//thisPillar").InnerText == "T") {
			print ("user got the correct answer");
		} else {
			currentScore = currentScore - 10;
		}
			
		// update user.xml to store user answers
		while (!finishAddingToDB) {
			if (usernameNode.InnerText == gm.Username) {
				XmlNode gameIndexNode = xmlUserDoc.SelectSingleNode ("//Game//Index");

				// find the matching game index
				while (gm.Index.ToString() != gameIndexNode.InnerText) {
					gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
				}

				print ("this is new gameIndex in userdb" + gameIndexNode.InnerText);
				// update node if <word#> node already exist
				if (gameIndexNode.ParentNode.ChildNodes.Count > subIndexForInfo+1) {
					gameIndexNode.ParentNode.SelectSingleNode("//Word" + subIndexForInfo).InnerText = "T";
					finishAddingToDB = true;
				} else {
					// create new <word#> node
					XmlNode wordIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
					wordIndex.InnerText = "T";

					gameIndexNode.ParentNode.AppendChild (wordIndex);
					finishAddingToDB = true;

				}
			} else {
				usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
			}
		}
		xmlUserDoc.Save (userpath);
		Start ().MoveNext();

	}
	public void OtherPillarOnClick()
	{
		Debug.Log("otherPillar is clicked");
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

		if (indexNode.SelectSingleNode ("//Word" + subIndexForInfo + "//thisPillar").InnerText == "F") {
			print ("user got the correct answer");
		} else {
			currentScore = currentScore - 10;
		}

		// update user.xml to store user answers
		while (!finishAddingToDB) {
			if (usernameNode.InnerText == gm.Username) {
				XmlNode gameIndexNode = xmlUserDoc.SelectSingleNode ("//Game//Index");


				print ("this is gameIndex" + gameIndexNode.InnerText);
				// find the matching game index
				while (gm.Index.ToString() != gameIndexNode.InnerText) {
					gameIndexNode = gameIndexNode.ParentNode.NextSibling.FirstChild;
				}

				print ("this is new gameIndex" + gameIndexNode.InnerText);
				print ("subbbb" + subIndexForInfo);

				// update node if <word#> node already exist
				if (gameIndexNode.ParentNode.ChildNodes.Count > subIndexForInfo+1) {
					gameIndexNode.ParentNode.SelectSingleNode("//Word" + subIndexForInfo).InnerText = "F";
					finishAddingToDB = true;
				} else {
					// create new <word#> node
					XmlNode wordIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Word" + subIndexForInfo, null);
					wordIndex.InnerText = "F";

					gameIndexNode.ParentNode.AppendChild (wordIndex);
					finishAddingToDB = true;
				}
			} else {
				usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
			}

		}
		xmlUserDoc.Save (userpath);

		Start ().MoveNext();

	}

	// Update is called once per frame
	void Update () {
		
	}
}


