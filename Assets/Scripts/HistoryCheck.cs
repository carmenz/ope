using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class HistoryCheck : MonoBehaviour {

	public static void FirstTimeCheck(string path, string island, string miniChallengeName, string miniChallengeNameSingular) {
		
		var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		XmlNode userNode = scanAndGetUser (usernameNode, gm);
		XmlNode islandNode = userNode.SelectSingleNode (".//" + island);

		if (!usernameNode.ParentNode.InnerXml.Contains (island)) {
			print ("user not yet have the island");
			AddIsland(xmlUserDoc, userNode, island);
		} else {
			print ("user already have the island");
		}

		if (islandNode.SelectSingleNode (".//" + miniChallengeName) != null) {
			print ("user already did such a mini challenge on the island");

			XmlDocument xmlMiniChallengeDoc = new XmlDocument ();
			xmlMiniChallengeDoc.Load (path);
			XmlNode indexNode = xmlMiniChallengeDoc.SelectSingleNode (".//" + miniChallengeNameSingular + "//Index");
			int numOfSuchMiniChallengeInDB = xmlMiniChallengeDoc.SelectNodes (".//" + miniChallengeNameSingular + "//Index").Count;


			XmlNode miniChallengeNode = userNode.SelectSingleNode (".//" + miniChallengeName);

			while (miniChallengeNode.ChildNodes.Count.ToString() == indexNode.InnerText) {
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}

			if (miniChallengeNode.ChildNodes.Count == numOfSuchMiniChallengeInDB) {
				print ("user have finished all the games we have");
				if (miniChallengeNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
					indexNode = indexNode.ParentNode.NextSibling.FirstChild;
				}

				//TODO: update db
				print("update db");


			} else {
				print ("second time access this mini challenge");
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
		XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//TotalScore");
		XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
		userNode.InsertAfter (xmlIsland, nodeBefore);
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
}






