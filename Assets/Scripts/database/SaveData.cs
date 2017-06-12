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




	public static void Load(string path) {
		userContainer = LoadUsers (path);

		//foreach (UserData data in userContainer.users) {


//			GameController.CreateUser (data, GameController.playerPath, 
//				new Vector3 (GameObject.Find("Board").GetComponent<BoardManager>().Squares [data.currentPos].transform.position.x, 
//					GameObject.Find("Board").GetComponent<BoardManager>().Squares [data.currentPos].transform.position.x, 0f),
//				Quaternion.identity);

		//}

		OnLoaded ();
	}

	public static void Save(string path, User current) {

		print ("saveData Save");
//		OnBeforeSave ();
		SaveUser(path, current);
	}


	private static UserContainer LoadUsers(string path) {
		
		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));

		FileStream stream = new FileStream (path, FileMode.Open);
		UserContainer users = serializer.Deserialize (stream) as UserContainer;

		stream.Close();
		return users;
	}

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


	private static void SaveUser(string path, User current) {

		print ("SaveData saveuser");

		current.StoreData ();
		bool userNotExist = true;


		// check if user.xml exist
		if (File.Exists (path)) {
			var dox = new XmlDocument ();
				dox.Load (path);


			FileStream stream = new FileStream (path, FileMode.Open);
			XmlTextReader xmlReader = new XmlTextReader (stream);



			// read file, check if user exist
			while (xmlReader.Read ()) {
				if (xmlReader.Name == "Username") {;
					// user exists
					if (xmlReader.ReadElementContentAsString ().Equals (current.data.username)) {
						userNotExist = false;
						print ("username already exist");
						xmlReader.ReadToNextSibling ("Password");

						// match username and password
						if (xmlReader.ReadElementContentAsString ().Equals (current.data.password)) {
							
							xmlReader.ReadToNextSibling ("CurrentPos");

							//load player position

//							print (xmlReader.ReadElementContentAsInt ());
//							GameManager gm = new GameManager();
//							gm.CurrentPosition = xmlReader.ReadElementContentAsInt ();
//							print (gm.CurrentPosition);
							SceneManager.LoadScene ("Main");




						} else {
							print ("Username and Password does not match");
						}
					}
				}
			}
			stream.Close ();
		}

		// create a user element if user does not exist
		if (userNotExist) {
			XDocument doc = XDocument.Load (path);

			XElement user = new XElement ("User");
			user.Add (new XElement ("Username", current.data.username));
			user.Add (new XElement ("Password", current.data.password));
			user.Add (new XElement ("CurrentPos", current.data.currentPos));
			user.Add (new XElement ("CurrentScore", current.data.currentScore));

			doc.Root.Element ("Users").Add (user);
			doc.Save (path);
		} 
	}
}
