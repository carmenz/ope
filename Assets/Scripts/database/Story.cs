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
	private static string path = string.Empty;
	private static string userpath = string.Empty;

	public void GetData() {
		bool firstChallengeOnIsland = true;
		bool firstStoryOnIsland = true;

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = gm.typeCode;
		print ("pos is" + pos);

		if (pos < 25) {
			island = "IslandA";
		} else if (pos < 30) {
			island = "IslandB";
		} else if (pos < 40) {
			island = "IslandC";
		} else if (pos < 50) {
			island = "IslandD";
		}

		//check if user already have that story on file
		FileStream userStream = new FileStream (userpath, FileMode.Open);
		XmlTextReader xmlUserReader = new XmlTextReader (userStream);

		while (xmlUserReader.Read ()) {
			
			if (xmlUserReader.Name == "Username") {
				// find user from user.xml
				if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
					
					xmlUserReader.Name.Contains ("User");
					if (xmlUserReader.ReadToNextSibling(island)) {
						print ("user already have the island");
						firstChallengeOnIsland = false;

						if (xmlUserReader.ReadToDescendant ("Stories")) {
							firstStoryOnIsland = false;
							print ("user already did a story on the island");

						} else {
							print ("first story for user on the island");
						}
					} else {
						print ("user not yet have island");
					}
				}
			}
		}
		userStream.Close ();

		if (firstStoryOnIsland) {
			
			XmlDocument xmlUserDoc = new XmlDocument ();
			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


			while (firstStoryOnIsland) {

				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//CurrentScore");

				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
				XmlNode xmlStories = xmlUserDoc.CreateNode (XmlNodeType.Element, "Stories", null);
				XmlNode xmlStory = xmlUserDoc.CreateNode (XmlNodeType.Element, "Story", null);


				// find the matching user
				if (firstChallengeOnIsland) {
					
					if (usernameNode.InnerText == gm.Username) {
						XmlNode user = usernameNode.ParentNode;

						xmlStory.InnerText = "1";

						xmlIsland.AppendChild (xmlStories);
						xmlStories.AppendChild (xmlStory);
					

						user.InsertAfter (xmlIsland, nodeBefore);
						firstStoryOnIsland = false;

						gm.Index = "1";


					} else {
						usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
					}
				} else {
					print ("not the first challenge but the first story");
					XmlNode islandNode = xmlUserDoc.SelectSingleNode ("//" + island);

					gm.Index = "1";
			
					xmlStory.InnerText = "1";

					islandNode.AppendChild (xmlStories);
					xmlStories.AppendChild (xmlStory);

					firstStoryOnIsland = false;
				}
			}
			xmlUserDoc.Save (userpath);
		} else {
			// not the first story

			print("not the first story");
			XmlDocument xmlUserDoc = new XmlDocument ();

			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

			if (usernameNode.InnerText == gm.Username) {
				XmlNode storyNode = xmlUserDoc.SelectSingleNode ("//Story");

				XmlNode newStoryNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Story", null);

				newStoryNode.InnerText = gm.typeCode.ToString();

				storyNode.InsertAfter (newStoryNode, storyNode);
			}
			xmlUserDoc.Save (userpath);
		}
	}
}




