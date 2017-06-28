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

public class Quiz : MonoBehaviour {

	private string island;
	private static string path = string.Empty;
	private static string userpath = string.Empty;



	public void GetData() {
		bool firstChallengeOnIsland = true;
		bool firstQuizOnIsland = true;

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = gm.Index;
		print ("pos is" + pos);
		// choose quiz file according to island

		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/quizzesA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesD.xml");
			print ("we are on island D");
		}

		gm.Path = path;

		if (File.Exists (path)) {

			var dox = new XmlDocument ();
			dox.Load (path);

			//check if user already have that quiz on file

			FileStream quizStream = new FileStream (path, FileMode.Open);
		
			XmlTextReader xmlQuizReader = new XmlTextReader (quizStream);

			FileStream userStream = new FileStream (userpath, FileMode.Open);
			XmlTextReader xmlUserReader = new XmlTextReader (userStream);
		
			xmlQuizReader.Read ();

			while (xmlUserReader.Read ()) {

				if (xmlUserReader.Name == "Username") {
					// find user from user.xml
					if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
						xmlUserReader.Name.Contains ("User");
						if (xmlUserReader.ReadToNextSibling(island)) {
							print ("user already have the island");
							firstChallengeOnIsland = false;

							if (xmlUserReader.ReadToDescendant ("Quizzes")) {
								firstQuizOnIsland = false;
								print ("user already did a quiz on the island");
							
							} else {
								print ("first quiz for user on the island");
							}
						} else {
							print ("user not yet have island");
						}
					}
				}
			}
			quizStream.Close ();
			userStream.Close ();
		}


		if (firstQuizOnIsland) {
			
			XmlDocument xmlUserDoc = new XmlDocument ();
			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


			while (firstQuizOnIsland) {

				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//CurrentScore");

				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
				XmlNode xmlQuizzes = xmlUserDoc.CreateNode (XmlNodeType.Element, "Quizzes", null);
				XmlNode xmlQuiz = xmlUserDoc.CreateNode (XmlNodeType.Element, "Quiz", null);
				XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

				// found the matching user
				if (firstChallengeOnIsland) {

					print (gm.Username);
					if (usernameNode.InnerText == gm.Username) {
						XmlNode user = usernameNode.ParentNode;



						xmlIndex.InnerText = "1";

						xmlIsland.AppendChild (xmlQuizzes);
						xmlQuizzes.AppendChild (xmlQuiz);
						xmlQuiz.AppendChild (xmlIndex);

						user.InsertAfter (xmlIsland, nodeBefore);
						firstQuizOnIsland = false;

						gm.Index = 1;


					} else {
						usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
					}
				} else {
					print ("not the first challenge but the first quiz");
					XmlNode islandNode = xmlUserDoc.SelectSingleNode ("//" + island);

					gm.Index = 1;
					xmlIndex.InnerText = "1";

					islandNode.AppendChild (xmlQuizzes);
					xmlQuizzes.AppendChild (xmlQuiz);
					xmlQuiz.AppendChild (xmlIndex);

					firstQuizOnIsland = false;
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

			XmlDocument xmlQuizDoc = new XmlDocument ();
			xmlQuizDoc.Load (path);
			XmlNode indexNode = xmlQuizDoc.SelectSingleNode ("//Quiz//Index");
			int numOfQuizzesInDB = xmlQuizDoc.SelectNodes ("//Quiz//Index").Count;

			while (!finishAdding) {
				if (usernameNode.InnerText == gm.Username) {
					XmlNode quizNode = xmlUserDoc.SelectSingleNode ("//Quizzes");

					print(quizNode.ChildNodes.Count);
					print(indexNode.InnerText);
					while (quizNode.ChildNodes.Count.ToString() == indexNode.InnerText) {
						indexNode = indexNode.ParentNode.NextSibling.FirstChild;
					}

					if (quizNode.ChildNodes.Count == numOfQuizzesInDB) {
						// user have finished all the quizzes we have
						print ("user have finished all the games we have");
						print (quizNode.LastChild.FirstChild.InnerText);
						print (indexNode.InnerText);
						if (quizNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
							indexNode = indexNode.ParentNode.NextSibling.FirstChild;
						}

						//TODO: update db
						print("update db");
						finishAdding = true;

					} else {
						XmlNode newQuizNode = xmlUserDoc.CreateNode (XmlNodeType.Element, "Quiz", null);
						XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

						xmlIndex.InnerText = indexNode.InnerText;

						newQuizNode.AppendChild (xmlIndex);

						print (quizNode.InnerText);
						quizNode.InsertAfter (newQuizNode, quizNode.LastChild);
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

public class QuizData{

	[XmlElement("Index")]
	public string index;

}


