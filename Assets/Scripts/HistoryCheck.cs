using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Xml;

public class HistoryCheck : MonoBehaviour {

	public static void FirstTimeCheck(string path, string island, string miniChallengeName, string miniChallengeNameSingular) {
		
		var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		XmlNode userNode = scanAndGetUser (usernameNode, gm);



		if (userNode.SelectSingleNode(".//" + island) == null) {
			print ("user not yet have the island");
			AddIsland(xmlUserDoc, userNode, island);
			xmlUserDoc.Save (userpath);
		} else {
			print ("user already have the island");
		}
		XmlNode islandNode = userNode.SelectSingleNode (".//" + island);
		if (islandNode.SelectSingleNode (".//" + miniChallengeName) != null) {
			print ("user already did such a mini challenge on the island");

			XmlDocument xmlMiniChallengeDoc = new XmlDocument ();
			xmlMiniChallengeDoc.Load (path);
			XmlNode indexNode = xmlMiniChallengeDoc.SelectSingleNode (".//" + miniChallengeNameSingular + "//Index");
			int numOfSuchMiniChallengeInDB = xmlMiniChallengeDoc.SelectNodes (".//" + miniChallengeNameSingular + "//Index").Count;


			XmlNode miniChallengeNode = userNode.SelectSingleNode (".//" + miniChallengeName);

			// move to next indexed question if this is already


			print ("num" + numOfSuchMiniChallengeInDB);
			if (miniChallengeNode.ChildNodes.Count == numOfSuchMiniChallengeInDB) {
				print ("user have finished all the games we have");

				if (miniChallengeNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
					indexNode = indexNode.ParentNode.FirstChild;
					print ("hahahhaa");
				}

				print ("index" + indexNode.InnerText);


				//TODO: update db
				print("update db");


			} else {
				print ("second time access this mini challenge");
				while (miniChallengeNode.ChildNodes.Count.ToString() == indexNode.InnerText && indexNode.ParentNode.NextSibling != null) {
					indexNode = indexNode.ParentNode.NextSibling.FirstChild;
				}
				print ("index" + indexNode.InnerText);
				AddMiniChallenge (xmlUserDoc, userNode, miniChallengeNameSingular, miniChallengeName, indexNode.InnerText, false);	
				gm.Index = int.Parse(indexNode.InnerText);
			}

		} else {
			print ("first time access this mini challenge");
			AddMiniChallengeType (xmlUserDoc, userNode, miniChallengeName, island);
			AddMiniChallenge (xmlUserDoc, userNode, miniChallengeNameSingular, miniChallengeName, "1", true);		
			gm.Index = 1;
		}
		xmlUserDoc.Save (userpath);
	}


	public static XmlNode scanAndGetUser(XmlNode usernameNode, GameManager gm) {
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}
		return usernameNode.ParentNode;
	}


	public static void AddIsland(XmlDocument xmlUserDoc, XmlNode userNode, string island) {
		XmlNode nodeBefore = userNode.SelectSingleNode (".//TotalScore");
		XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
		//xmlIsland.InnerText = "";
		userNode.InsertAfter (xmlIsland, nodeBefore);
		print ("finish adding island");
	}

	public static void AddMiniChallengeType(XmlDocument xmlUserDoc, XmlNode userNode, string miniChallengeName, string island) {
		XmlNode xmlMiniChallengeType = xmlUserDoc.CreateNode (XmlNodeType.Element, miniChallengeName, null);
		XmlNode islandNode = userNode.SelectSingleNode (".//" + island);
		islandNode.AppendChild (xmlMiniChallengeType);
	}

	public static void AddMiniChallenge(XmlDocument xmlUserDoc, XmlNode userNode, string miniChallengeNameSingular, string miniChallengeName, string index, bool firstSuchMiniChallenge) {
		XmlNode xmlMiniChallengeSingular = xmlUserDoc.CreateNode (XmlNodeType.Element, miniChallengeNameSingular, null);
		XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);
		xmlIndex.InnerText = index;

		XmlNode miniChallengeTypeNode = userNode.SelectSingleNode (".//" + miniChallengeName);
		if (firstSuchMiniChallenge) {
			miniChallengeTypeNode.AppendChild (xmlMiniChallengeSingular);
			xmlMiniChallengeSingular.AppendChild (xmlIndex);
		} else {
			xmlMiniChallengeSingular.AppendChild (xmlIndex);
			miniChallengeTypeNode.InsertAfter (xmlMiniChallengeSingular, miniChallengeTypeNode.LastChild);
		}
	}

	public static void DisplayIslandName() {
		Text islandName = GameObject.Find ("IslandName").GetComponent<Text> ();
		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		if (gm.currentIsland == "A") {
			islandName.text = "Positive Play Island";
		} else if (gm.currentIsland == "B") {
			islandName.text = "Island Of Informed Decisions";
		} else if (gm.currentIsland == "C") {
			islandName.text = "island C";
		} else if (gm.currentIsland == "D") {
			islandName.text = "island D";
		}

	}
}






