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

public class FillInTheBlank : MonoBehaviour {

	private string island;
	private static string path = string.Empty;
	private static string userpath = string.Empty;


	public void GetData() {
		bool firstChallengeOnIsland = true;
		bool firstFillInTheBlankOnIsland = true;

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = gm.Index;
		print ("pos is" + pos);
		// choose FillInTheBlank file according to island

		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/blanksB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/blanksC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/blanksD.xml");
			print ("we are on island D");
		}

		gm.Path = path;

		if (File.Exists (path)) {

			var dox = new XmlDocument ();
			dox.Load (path);

			//check if user already have that FillInTheBlank on file

			FileStream fillInTheBlankStream = new FileStream (path, FileMode.Open);
		
			XmlTextReader xmlFillInTheBlankReader = new XmlTextReader (fillInTheBlankStream);

			FileStream userStream = new FileStream (userpath, FileMode.Open);
			XmlTextReader xmlUserReader = new XmlTextReader (userStream);
		
			xmlFillInTheBlankReader.Read ();

			while (xmlUserReader.Read ()) {

				if (xmlUserReader.Name == "Username") {
					// find user from user.xml
					if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
						xmlUserReader.Name.Contains ("User");
						if (xmlUserReader.ReadToNextSibling(island)) {
							print ("user already have the island");
							firstChallengeOnIsland = false;

							if (xmlUserReader.ReadToDescendant ("FillInTheBlanks")) {
								firstFillInTheBlankOnIsland = false;
								print ("user already did a FillInTheBlank on the island");
							
							} else {
								print ("first FillInTheBlank for user on the island");
							}
						} else {
							print ("user not yet have island");
						}
					}
				}
			}
			fillInTheBlankStream.Close ();
			userStream.Close ();
		}


		if (firstFillInTheBlankOnIsland) {
			
			XmlDocument xmlUserDoc = new XmlDocument ();
			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


			while (firstFillInTheBlankOnIsland) {

				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//TotalScore");

				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
				XmlNode xmlFillInTheBlanks = xmlUserDoc.CreateNode (XmlNodeType.Element, "FillInTheBlanks", null);
				XmlNode xmlFillInTheBlank = xmlUserDoc.CreateNode (XmlNodeType.Element, "FillInTheBlank", null);
				XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

				// found the matching user
				if (firstChallengeOnIsland) {


					if (usernameNode.InnerText == gm.Username) {
						XmlNode user = usernameNode.ParentNode;



						xmlIndex.InnerText = "1";

						xmlIsland.AppendChild (xmlFillInTheBlanks);
						xmlFillInTheBlanks.AppendChild (xmlFillInTheBlank);
						xmlFillInTheBlank.AppendChild (xmlIndex);

						user.InsertAfter (xmlIsland, nodeBefore);
						firstFillInTheBlankOnIsland = false;

						gm.Index = 1;


					} else {
						usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
					}
				} else {
					print ("not the first challenge but the first FillInTheBlank");
					XmlNode islandNode = xmlUserDoc.SelectSingleNode ("//" + island);

					gm.Index = 1;
					xmlIndex.InnerText = "1";

					islandNode.AppendChild (xmlFillInTheBlanks);
					xmlFillInTheBlanks.AppendChild (xmlFillInTheBlank);
					xmlFillInTheBlank.AppendChild (xmlIndex);

					firstFillInTheBlankOnIsland = false;
				}
			}
			xmlUserDoc.Save (userpath);
		} else {
			// not the first FillInTheBlank
			bool finishAdding = false;
			print("not the first FillInTheBlank");
			XmlDocument xmlUserDoc = new XmlDocument ();

			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

			XmlDocument xmlFillInTheBlankDoc = new XmlDocument ();
			xmlFillInTheBlankDoc.Load (path);
			XmlNode indexNode = xmlFillInTheBlankDoc.SelectSingleNode ("//FillInTheBlank//Index");
			int numOfFillInTheBlanksInDB = xmlFillInTheBlankDoc.SelectNodes ("//FillInTheBlank//Index").Count;

			while (!finishAdding) {
				if (usernameNode.InnerText == gm.Username) {
					XmlNode fillInTheBlankNode = xmlUserDoc.SelectSingleNode ("//FillInTheBlanks");

					while (fillInTheBlankNode.ChildNodes.Count.ToString() == indexNode.InnerText) {
						indexNode = indexNode.ParentNode.NextSibling.FirstChild;
					}

					if (fillInTheBlankNode.ChildNodes.Count == numOfFillInTheBlanksInDB) {
						// user have finished all the FillInTheBlanks we have
						print ("user have finished all the games we have");
						if (fillInTheBlankNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
							indexNode = indexNode.ParentNode.NextSibling.FirstChild;
						}

						//TODO: update db
						print("update db");
						finishAdding = true;

					} else {
						XmlNode newFillInTheBlankNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "FillInTheBlank", null);
						XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

						xmlIndex.InnerText = indexNode.InnerText;

						newFillInTheBlankNode.AppendChild (xmlIndex);

						fillInTheBlankNode.InsertAfter (newFillInTheBlankNode, fillInTheBlankNode.LastChild);
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

public class FillInTheBlankData{

	[XmlElement("Index")]
	public string index;

}


