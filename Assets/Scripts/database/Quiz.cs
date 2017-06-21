using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using System;
using System.Runtime;

public class Quiz : MonoBehaviour {

	private string island;
	private static string path = string.Empty;
	private static string userpath = string.Empty;

	 public void LoadQuizData() {
	 	
	 }


	public void GetData(User current) {
		bool firstChallengeOnIsland = true;
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		int pos = current.currentPos;
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


		if (File.Exists (path)) {

			var dox = new XmlDocument ();
			dox.Load (path);


			//check if user already have that quiz on file

			FileStream quizStream = new FileStream (path, FileMode.Open);
		
			XmlTextReader xmlQuizReader = new XmlTextReader (quizStream);

			FileStream userStream = new FileStream (userpath, FileMode.Open);
			XmlTextReader xmlUserReader = new XmlTextReader (userStream);

		
			xmlQuizReader.Read ();

			// read file, check if user exist
			while (xmlUserReader.Read ()) {
		
				print ("whileeee read");

					// user did some challenges on the island
				if (xmlUserReader.Name == island) {
					firstChallengeOnIsland = false;
					// user already did some quiz challenges
					if (xmlUserReader.ReadToNextSibling ("Quiz")) {
						print ("found quiz");
						xmlUserReader.ReadToNextSibling ("Index");
						print ("found index");
						int index = xmlUserReader.ReadElementContentAsInt ();

						print ("index is " + index);
						// ignore quizes that the user already did
						if (xmlQuizReader.ReadElementContentAsInt ().Equals (index)) {
							print ("index matches");
							xmlQuizReader.ReadToNextSibling ("Index");
						}

					} else {

						print ("user not yet have challenge on the island");

					}
				} 
			}
			quizStream.Close();
			userStream.Close ();
		}
		// user did not have any challenges for the island yet


		if (firstChallengeOnIsland) {

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load (userpath);
			XmlNode username = xmlDoc.SelectSingleNode("//Username");

			while (firstChallengeOnIsland) {

				if (username.InnerText == current.username) {
					print ("herererereeeeeeeeeeeeeeeeeeee");
					XmlNode user = username.ParentNode;
					XmlNode nodeBefore = xmlDoc.SelectSingleNode ("//CurrentScore");


					XmlNode xmlIsland = xmlDoc.CreateNode (XmlNodeType.Element, island, null);
					XmlNode xmlQuizzes = xmlDoc.CreateNode (XmlNodeType.Element, "Quizzes", null);
					XmlNode xmlQuiz = xmlDoc.CreateNode (XmlNodeType.Element, "Quiz", null);
					XmlNode xmlIndex = xmlDoc.CreateNode (XmlNodeType.Element, "Index", null);
					// index go by position
					xmlIndex.InnerText = pos.ToString ();

					xmlIsland.AppendChild (xmlQuizzes);
					xmlQuizzes.AppendChild (xmlQuiz);
					xmlQuiz.AppendChild (xmlIndex);

					user.InsertAfter (xmlIsland, nodeBefore);
					firstChallengeOnIsland = false;
				} else {
					username = username.ParentNode.NextSibling.FirstChild;

				}

			}


			xmlDoc.Save(userpath);

		}



	}
		


}

public class QuizData{

	[XmlElement("Index")]
	public string index;


}




