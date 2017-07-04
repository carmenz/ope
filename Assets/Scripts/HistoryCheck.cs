using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class HistoryCheck : MonoBehaviour {

	private static string userpath = string.Empty;


	public static void FirstTimeCheck(string path, string island, string miniChallengeName, string miniChallengeNameSingular) {
		
		userpath = System.IO.Path.Combine (Application.persistentDataPath, "Resources/users.xml");
		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();



		bool firstChallengeOnIsland = true;
		bool firstMiniChallengeOnIsland = true;


		if (File.Exists (path)) {

			var dox = new XmlDocument ();
			dox.Load (path);

			//check if user already have that FillInTheBlank on file

			FileStream miniChallengeStream = new FileStream (path, FileMode.Open);

			XmlTextReader xmlMiniChallengeReader = new XmlTextReader (miniChallengeStream);

			FileStream userStream = new FileStream (userpath, FileMode.Open);
			XmlTextReader xmlUserReader = new XmlTextReader (userStream);

			xmlMiniChallengeReader.Read ();

			while (xmlUserReader.Read ()) {

				if (xmlUserReader.Name == "Username") {
					// find user from user.xml
					if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
						xmlUserReader.Name.Contains ("User");
						if (xmlUserReader.ReadToNextSibling(island)) {
							print ("user already have the island");
							firstChallengeOnIsland = false;

							if (xmlUserReader.ReadToDescendant (miniChallengeName)) {
								firstMiniChallengeOnIsland = false;
								print ("user already did such a mini challenge on the island");

							} else {
								print ("first such mini challenge for user on the island");
							}
						} else {
							print ("user not yet have island");
						}
					}
				}
			}
			miniChallengeStream.Close ();
			userStream.Close ();
		}


		if (firstMiniChallengeOnIsland) {

			XmlDocument xmlUserDoc = new XmlDocument ();
			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


			while (firstMiniChallengeOnIsland) {

				XmlNode nodeBefore = xmlUserDoc.SelectSingleNode ("//TotalScore");

				XmlNode xmlIsland = xmlUserDoc.CreateNode (XmlNodeType.Element, island, null);
				XmlNode xmlMiniChallengeType = xmlUserDoc.CreateNode (XmlNodeType.Element, miniChallengeName, null);
				XmlNode xmlMiniChallengeSingular = xmlUserDoc.CreateNode (XmlNodeType.Element, miniChallengeNameSingular, null);
				XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

				// found the matching user


				print ("first challenge on island");
				if (usernameNode.InnerText == gm.Username) {
					XmlNode user = usernameNode.ParentNode;

					if (firstChallengeOnIsland) {
						xmlIndex.InnerText = "1";

						xmlIsland.AppendChild (xmlMiniChallengeType);
						xmlMiniChallengeType.AppendChild (xmlMiniChallengeSingular);
						xmlMiniChallengeSingular.AppendChild (xmlIndex);

						user.InsertAfter (xmlIsland, nodeBefore);
						firstMiniChallengeOnIsland = false;

						gm.Index = 1;
					} else {
							print ("not the first challenge but the first such mini challenge" + miniChallengeNameSingular);


						XmlNode islandNode = usernameNode.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
				
							gm.Index = 1;
							xmlIndex.InnerText = "1";

							islandNode.AppendChild (xmlMiniChallengeType);
							xmlMiniChallengeType.AppendChild (xmlMiniChallengeSingular);
							xmlMiniChallengeSingular.AppendChild (xmlIndex);

							firstMiniChallengeOnIsland = false;
					} 
				} else {
					usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
				} 
			}
			xmlUserDoc.Save (userpath);

		} else {
			// not the first such mini challenge
			print("not the first such mini challenge");
			XmlDocument xmlUserDoc = new XmlDocument ();

			xmlUserDoc.Load (userpath);
			XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

			XmlDocument xmlMiniChallengeDoc = new XmlDocument ();
			xmlMiniChallengeDoc.Load (path);
			XmlNode indexNode = xmlMiniChallengeDoc.SelectSingleNode (".//" + miniChallengeNameSingular + "//Index");
			int numOfSuchMiniChallengeInDB = xmlMiniChallengeDoc.SelectNodes (".//" + miniChallengeNameSingular + "//Index").Count;

		
			while (usernameNode.InnerText != gm.Username) {
				usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
			}
					
			XmlNode miniChallengeNode = usernameNode.ParentNode.SelectSingleNode (".//" + miniChallengeName);

			while (miniChallengeNode.ChildNodes.Count.ToString() == indexNode.InnerText) {
				indexNode = indexNode.ParentNode.NextSibling.FirstChild;
			}

			if (miniChallengeNode.ChildNodes.Count == numOfSuchMiniChallengeInDB) {
				// user have finished all such mini challenges we have
				print ("user have finished all the games we have");
				if (miniChallengeNode.LastChild.FirstChild.InnerText == indexNode.InnerText) {
					indexNode = indexNode.ParentNode.NextSibling.FirstChild;
				}

				//TODO: update db
				print("update db");
			

			} else {
				XmlNode newMiniChallengeNode = xmlUserDoc.CreateNode (XmlNodeType.Element, miniChallengeNameSingular, null);
				XmlNode xmlIndex = xmlUserDoc.CreateNode (XmlNodeType.Element, "Index", null);

				xmlIndex.InnerText = indexNode.InnerText;

				newMiniChallengeNode.AppendChild (xmlIndex);

				miniChallengeNode.InsertAfter (newMiniChallengeNode, miniChallengeNode.LastChild);


				gm.Index = int.Parse(indexNode.InnerText);

			}

			xmlUserDoc.Save (userpath);
		}
	}
}






