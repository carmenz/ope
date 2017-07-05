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

public class SaveData : MonoBehaviour {

	// xml loading occur

	public static UserContainer userContainer = new UserContainer();
	//public static User user = new User();


	public delegate void SerializeAction ();

	// notify user we are done loaded
	public static event SerializeAction OnLoaded;

	// tell user we are about to save
	public static event SerializeAction OnBeforeSave;


	public static NewPlayer player;
	private static TextAsset textAsset;

	public static void Save(string path, User current) {

		print ("saveData Save");
//		OnBeforeSave ();
		SaveUser(path, current);
	}

	public static void Load(string path, User current) {

		print ("saveData Load");
		//		OnBeforeSave ();
		LoadUser(path, current);
	}


	public static void Update(string path, User current) {

		print ("saveData Update");
		//		OnBeforeSave ();
		UpdateUser(path, current);
	}
		


	private static void SaveUser(string path, User current) {
		
		current.GetInputData ();
	
		bool userNotExist = true;
		// check if user.xml exist
		if (File.Exists (path)) {
			var dox = new XmlDocument ();
				dox.Load (path);

			print ("pathhhhh");
			FileStream stream = new FileStream (path, FileMode.Open);
			XmlTextReader xmlReader = new XmlTextReader (stream);


//		
//			XmlDocument xmlReader = new XmlDocument();
//
//			xmlReader.Load (path);
//			textAsset = (TextAsset) Resources.Load("users.xml");
			//xmlReader.LoadXml(textAsset.text);

			// read file, check if user exist
			while (xmlReader.Read ()) {
				if (xmlReader.Name == "Username") {
					// user exists
					if (xmlReader.ReadElementContentAsString ().Equals (current.data.username)) {
						userNotExist = false;
						print ("username already exist, please load saved game!!!!");
					}
				}
			}
//			XmlNode usernameNode = xmlReader.SelectSingleNode("//Username");
//			while(usernameNode != null) {
//				if (usernameNode.InnerText == current.data.username) {
//					userNotExist = false;
//					print ("username already exist, please load saved game!!!!");
//					break;
//				} else {
//					usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
//				}
//			}
			stream.Close ();

		}

		// create a user element if user does not exist
		if (userNotExist) {

			XDocument doc = XDocument.Load (path);

//			XmlDocument doc = new XmlDocument ();
//			doc.Load (path);

			XElement user = new XElement ("User");
			user.Add (new XElement ("Username", current.data.username));
			user.Add (new XElement ("Password", current.data.password));
			user.Add (new XElement ("CurrentPosX", current.data.currentPosX));
			user.Add (new XElement ("CurrentPosY", current.data.currentPosY));
			user.Add (new XElement ("TotalScore", current.data.totalScore));

			doc.Root.Element ("Users").Add (user);
			doc.Save (path);


//			XmlNode newUserNode = doc.CreateNode (XmlNodeType.Element, "User", null);
//			XmlNode usernameNode = doc.CreateNode (XmlNodeType.Element, "UserName", null);
//			XmlNode passwordNode = doc.CreateNode (XmlNodeType.Element, "Password", null);
//			XmlNode currentPosXNode = doc.CreateNode (XmlNodeType.Element, "CurrentPosX", null);
//			XmlNode currentPosYNode = doc.CreateNode (XmlNodeType.Element, "CurrentPosY", null);
//			XmlNode totalScoreNode = doc.CreateNode (XmlNodeType.Element, "TotalScore", null);
//
//			usernameNode.InnerText = current.data.username;
//			passwordNode.InnerText = current.data.password;
//			currentPosXNode.InnerText = current.data.currentPosX.ToString();
//			currentPosYNode.InnerText = current.data.currentPosY.ToString();
//			totalScoreNode.InnerText = current.data.totalScore.ToString();
//
//			newUserNode.AppendChild (usernameNode);
//			newUserNode.AppendChild (passwordNode);
//			newUserNode.AppendChild (currentPosXNode);
//			newUserNode.AppendChild (currentPosYNode);
//			newUserNode.AppendChild (totalScoreNode);
//
//			XmlNode usersNode = doc.SelectSingleNode ("//Users");
//			usersNode.InsertAfter (newUserNode, usersNode.LastChild);
//
//			doc.Save (path);

			SceneManager.LoadScene ("Main");
			GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
			gm.Username = current.data.username;
		} 
	}
		

	private static void LoadUser(string path, User current) {

		current.GetInputData ();

		// check if user.xml exist
		if (File.Exists (path)) {
			var dox = new XmlDocument ();
			dox.Load (path);


			FileStream stream = new FileStream (path, FileMode.Open);
			XmlTextReader xmlReader = new XmlTextReader (stream);

			// read file, check if user exist
			while (xmlReader.Read ()) {
				if (xmlReader.Name == "Username") {
					
					// user exists
					if (xmlReader.ReadElementContentAsString ().Equals (current.data.username)) {
						xmlReader.ReadToNextSibling ("Password");

						// match username and password
						if (xmlReader.ReadElementContentAsString ().Equals (current.data.password)) {

							xmlReader.ReadToNextSibling ("CurrentPos");

							//load player position

							SceneManager.LoadScene ("Main");
							GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
							//gm.CurrentPosition = xmlReader.ReadElementContentAsInt ();
							gm.Username = current.data.username;

						} else {
							print ("Username and Password does not match, please try again");
						}
					}
				} 
			}
			stream.Close ();
		}
	}

	private static void UpdateUser(string path, User current) {


	
		// check if user.xml exist
		if (File.Exists (path)) {
			var dox = new XmlDocument ();
			dox.Load (path);
			print ("file");

			FileStream stream = new FileStream (path, FileMode.Open);
			XmlTextReader xmlReader = new XmlTextReader (stream);

			// read file, check if user exist
			while (xmlReader.Read ()) {
				if (xmlReader.Name == "Username") {
					// user exists
					if (xmlReader.ReadElementContentAsString ().Equals (current.data.username)) {
						xmlReader.ReadToNextSibling ("CurrentPos");

						GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

						XDocument doc = XDocument.Load (path);
						XElement currentPos = new XElement ("CurrentPos");
						currentPos.SetValue (gm.Coordinate);
						//print ("gm current" + gm.CurrentPosition);
						doc.Save (path);

						print ("done updating");
					}
					//user has to exist
					else {
						print ("BUG!!!! user does not exist in db");
					}
				}
			}
			stream.Close ();
		}
	}









	//	private static UserContainer LoadUsers(string path) {
	//		
	//		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));
	//
	//		FileStream stream = new FileStream (path, FileMode.Open);
	//		UserContainer users = serializer.Deserialize (stream) as UserContainer;
	//
	//		stream.Close();
	//		return users;
	//	}

	//	private static void SaveUsers(string path, UserContainer users) {
	//
	//
	//		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));
	//		// tell program to open file in the path, if there's anything in it, append it 
	//		FileStream stream = new FileStream (path, FileMode.Append);
	//		serializer.Serialize(stream, users);
	//		print ("SaveData SaveUsers");
	//
	//		stream.Close ();
	//	}
}
