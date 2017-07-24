using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class Story : MonoBehaviour {

	private string island;
	private static string userpath = string.Empty;

	private int pos;

	public void GetData() {
		bool firstChallengeOnIsland = true;
		bool firstStoryOnIsland = true;

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = gm.Index;
		string currentIsland = gm.currentIsland;

		if (currentIsland == "A") {
			island = "IslandA";
		} else if (currentIsland == "B") {
			island = "IslandB";
		} else if (currentIsland == "C") {
			island = "IslandC";
		} else if (currentIsland == "D") {
			island = "IslandD";
		}

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		XmlNode userNode = HistoryCheck.scanAndGetUser (usernameNode, gm);

		if (userNode.SelectSingleNode(".//" + island) == null) {
			print ("user not yet have the island");
			HistoryCheck.AddIsland(xmlUserDoc, userNode, island);
			xmlUserDoc.Save (userpath);
		} else {
			print ("user already have the island");
		}

		XmlNode islandNode = userNode.SelectSingleNode (".//" + island);
		if (islandNode.SelectSingleNode (".//Stories") != null) {
			print("not the first story");

			if (usernameNode.InnerText == gm.Username) {
				XmlNode storyNode = xmlUserDoc.SelectSingleNode (".//Story");

				XmlNode newStoryNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Story", null);

				newStoryNode.InnerText = gm.typeCode.ToString();

				storyNode.InsertAfter (newStoryNode, storyNode);
			}
			xmlUserDoc.Save (userpath);
		} else {
			print ("first time access this mini challenge");
			HistoryCheck.AddMiniChallengeType (xmlUserDoc, userNode, "Stories", island);
			HistoryCheck.AddMiniChallenge (xmlUserDoc, userNode, "Story", "Stories", "1", true);	
		}
		xmlUserDoc.Save (userpath);
	}
}




