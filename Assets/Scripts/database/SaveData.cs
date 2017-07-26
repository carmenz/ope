using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class SaveData : MonoBehaviour {


	public static UserContainer userContainer = new UserContainer();


	public delegate void SerializeAction ();

	// notify user we are done loaded
	public static event SerializeAction OnLoaded;

	// tell user we are about to save
	public static event SerializeAction OnBeforeSave;


	public static NewPlayer player;
	private static TextAsset textAsset;

	public static void Save(string path, GameManager current) {
		SaveUser(path, current);
	}

	public static void Load(string path, GameManager current) {
		LoadUser(path, current);
	}


	public static void Update(string path, GameManager current) {
		UpdateUser(path, current);
	}
		


	private static void SaveUser(string path, GameManager current) {
		
		current.GetInputData ();
	
		bool userNotExist = true;
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
						userNotExist = false;
						//TODO: put the info into info box
						GameObject.Find("InfoBox").GetComponent<Text>().text = "Username already exist, please load saved game.";
					}
				}
			}
			stream.Close ();
		}

		// create a user element if user does not exist
		if (userNotExist) {

			// validations
			if (current.data.username == "" || current.data.password == "") {
				GameObject.Find ("InfoBox").GetComponent<Text> ().text = "Username and Passworkd cannot be empty.";
			} else {
				XDocument doc = XDocument.Load (path);

				XElement user = new XElement ("User");
				user.Add (new XElement ("Username", current.data.username));
				user.Add (new XElement ("Password", current.data.password));
				user.Add (new XElement ("CurrentPosX", current.data.currentPosX));
				user.Add (new XElement ("CurrentPosY", current.data.currentPosY));
				user.Add (new XElement ("CurrentIsland", current.data.currentIsland));
				user.Add (new XElement ("TotalScore", current.data.totalScore));

				doc.Root.Element ("Users").Add (user);
				doc.Save (path);

				SceneManager.LoadScene ("Main");
				GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
				gm.Username = current.data.username;
			}
		} 
	}
		

	private static void LoadUser(string path, GameManager current) {

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
							print ("heheheheheehe");
							gm.Username = current.data.username;

						} 
					} else {
						GameObject.Find ("InfoBox").GetComponent<Text> ().text = "Username and Password does not match, please try again.";
					}
				}
			}
			stream.Close ();
		}
	}

	private static void UpdateUser(string path, GameManager current) {
	
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
}
