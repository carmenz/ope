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

public class WordGame : MonoBehaviour {


	private string island;
	private static string path = string.Empty;
	private static string userpath = string.Empty;

	public void GetData() {
		bool firstChallengeOnIsland = true;
		bool firstWordGameOnIsland = true;

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = gm.Index;
		print ("pos is" + pos);
		// choose quiz file according to island

		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/wordGamesA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesD.xml");
			print ("we are on island D");
		}

		gm.Path = path;

		if (File.Exists (path)) {

			var dox = new XmlDocument ();
			dox.Load (path);

			//check if user already have that quiz on file

			FileStream wordGameStream = new FileStream (path, FileMode.Open);

			XmlTextReader xmlWordGameReader = new XmlTextReader (wordGameStream);

			FileStream userStream = new FileStream (userpath, FileMode.Open);
			XmlTextReader xmlUserReader = new XmlTextReader (userStream);

			xmlWordGameReader.Read ();

			while (xmlUserReader.Read ()) {

				if (xmlUserReader.Name == "Username") {
					// find user from user.xml
					if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
						xmlUserReader.Name.Contains ("User");
						if (xmlUserReader.ReadToNextSibling(island)) {
							print ("user already have the island");
							firstChallengeOnIsland = false;

							if (xmlUserReader.ReadToDescendant ("WordGames")) {
								firstWordGameOnIsland = false;
								print ("user already did a word game on the island");

							} else {
								print ("first word game for user on the island");
							}
						} else {
							print ("user not yet have island");
						}
					}
				}
			}
			wordGameStream.Close ();
			userStream.Close ();
		}


		if (firstWordGameOnIsland) {

			XmlDocument xmlUserDoc = new XmlDocument ();
			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


			while (firstWordGameOnIsland) {

				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//CurrentScore");

				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
				XmlNode xmlWordGames = xmlUserDoc.CreateNode (XmlNodeType.Element, "WordGames", null);
				XmlNode xmlWordGame = xmlUserDoc.CreateNode (XmlNodeType.Element, "Game", null);
				XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

				// find the matching user
				if (firstChallengeOnIsland) {
					print (gm.Username);
					if (usernameNode.InnerText == gm.Username) {
						XmlNode user = usernameNode.ParentNode;



						xmlIndex.InnerText = "1";

						xmlIsland.AppendChild (xmlWordGames);
						xmlWordGames.AppendChild (xmlWordGame);
						xmlWordGame.AppendChild (xmlIndex);

						user.InsertAfter (xmlIsland, nodeBefore);
						firstWordGameOnIsland = false;

						gm.Index = 1;


					} else {
						usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
					}
				} else {
					print ("not the first challenge but the first quiz");
					XmlNode islandNode = xmlUserDoc.SelectSingleNode ("//" + island);

					gm.Index = 1;
					xmlIndex.InnerText = "1";

					islandNode.AppendChild (xmlWordGames);
					xmlWordGames.AppendChild (xmlWordGame);
					xmlWordGame.AppendChild (xmlIndex);

					firstWordGameOnIsland = false;
				}
			}
			xmlUserDoc.Save (userpath);
		} else {
			// not the first quiz
			bool finishAdding = false;
			print("not the first quiz");
			XmlDocument xmlUserDoc = new XmlDocument ();

			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

			XmlDocument xmlWordGameDoc = new XmlDocument ();
			xmlWordGameDoc.Load (path);
			XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Game//Index");
			int numOfGamesInDB = xmlWordGameDoc.SelectNodes ("//Game//Index").Count;

			while (!finishAdding) {
				if (usernameNode.InnerText == gm.Username) {
					XmlNode wordGameNode = xmlUserDoc.SelectSingleNode ("//WordGames");

					print(wordGameNode.ChildNodes.Count);
					print(indexNode.InnerText);

					while (wordGameNode.ChildNodes.Count.ToString() == indexNode.InnerText) {
						
						indexNode = indexNode.ParentNode.NextSibling.FirstChild;

					}

					if (wordGameNode.ChildNodes.Count == numOfGamesInDB) {
						// user have finished all the games we have
						print ("user have finished all the games we have");
						print (wordGameNode.LastChild.FirstChild.InnerText);
						print (indexNode.InnerText);
						if (wordGameNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
							indexNode = indexNode.ParentNode.NextSibling.FirstChild;
						}

						print ("update db");
						//TODO: update db
						finishAdding = true;

					} else {



						XmlNode newWordGameNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Game", null);
						XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);


						xmlIndex.InnerText = indexNode.InnerText;

						newWordGameNode.AppendChild (xmlIndex);

						wordGameNode.InsertAfter (newWordGameNode, wordGameNode.LastChild);
						finishAdding = true;

						gm.Index = int.Parse(indexNode.InnerText);
					}

				} else {
					usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
				}
			}
			xmlUserDoc.Save (userpath);
		}
	}
}

