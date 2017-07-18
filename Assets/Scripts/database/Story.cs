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

		//check if user already have that story on file
//		FileStream userStream = new FileStream (userpath, FileMode.Open);
//		XmlTextReader xmlUserReader = new XmlTextReader (userStream);
//
//		while (xmlUserReader.Read ()) {
//			
//			if (xmlUserReader.Name == "Username") {
//				// find user from user.xml
//				if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
//					
//					xmlUserReader.Name.Contains ("User");
//					if (xmlUserReader.ReadToNextSibling(island)) {
//						print ("user already have the island");
//						firstChallengeOnIsland = false;
//
//						if (xmlUserReader.ReadToDescendant ("Stories")) {
//							firstStoryOnIsland = false;
//							print ("user already did a story on the island");
//
//						} else {
//							print ("first story for user on the island");
//						}
//					} else {
//						print ("user not yet have island");
//					}
//				}
//			}
//		}
//		userStream.Close ();


		//var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		//GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

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




		//if (firstStoryOnIsland) {
//			
//			XmlDocument xmlUserDoc = new XmlDocument ();
//			xmlUserDoc.Load (userpath);
//			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
//

			//while (firstStoryOnIsland) {

//				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode (".//TotalScore");
//
//				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
//				XmlNode xmlStories = xmlUserDoc.CreateNode (XmlNodeType.Element, "Stories", null);
//				XmlNode xmlStory = xmlUserDoc.CreateNode (XmlNodeType.Element, "Story", null);


				// find the matching user
//				if (firstChallengeOnIsland) {
//					
//					if (usernameNode.InnerText == gm.Username) {
//						XmlNode user = usernameNode.ParentNode;
//
//						xmlStory.InnerText = "1";
//
//						xmlIsland.AppendChild (xmlStories);
//						xmlStories.AppendChild (xmlStory);
//					
//
//						user.InsertAfter (xmlIsland, nodeBefore);
//						firstStoryOnIsland = false;
//
//						gm.Index = 1;
//
//
//					} else {
//						usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
//					}
//				} else {
//					print ("not the first challenge but the first story");
//					//XmlNode islandNode = xmlUserDoc.SelectSingleNode (".//" + island);
//
//					gm.Index = 1;
//			
//					xmlStory.InnerText = "1";
//
//					islandNode.AppendChild (xmlStories);
//					xmlStories.AppendChild (xmlStory);
//
//					firstStoryOnIsland = false;
//				}
//			}
//			xmlUserDoc.Save (userpath);
	//	} else {
			// not the first story

			//print("not the first story");
//			XmlDocument xmlUserDoc = new XmlDocument ();
//
//			xmlUserDoc.Load (userpath);
//			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

//			if (usernameNode.InnerText == gm.Username) {
//				XmlNode storyNode = xmlUserDoc.SelectSingleNode (".//Story");
//
//				XmlNode newStoryNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Story", null);
//
//				newStoryNode.InnerText = gm.typeCode.ToString();
//
//				storyNode.InsertAfter (newStoryNode, storyNode);
//			}
//			xmlUserDoc.Save (userpath);
		//}
	}
}




