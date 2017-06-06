using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class SaveData : MonoBehaviour {


	public static UserContainer userContainer = new UserContainer();

	public delegate void SerializeAction ();

	public static event SerializeAction OnLoaded;
	public static event SerializeAction OnBeforeSave;

	public static void Load(string path) {
		userContainer = LoadUsers (path);

		foreach (UserData data in userContainer.users) {
			
			GameController.CreateUser (data, GameController.playerPath, 
				new Vector3 (BoardManager.Squares [data.currentPos].transform.position.x, 
							BoardManager.Squares [data.currentPos].transform.position.x, 0f),
				Quaternion.identity);

		}

		OnLoaded ();
	}

	public static void Save(string path, UserContainer users) {

		OnBeforeSave ();
		SaveUsers (path, users);

		ClearUsers ();

	}

	public static void AddUserToData(UserData data) {
		userContainer.users.Add (data);
	}


	public static void ClearUsers() {
		userContainer.users.Clear ();
	}

	private static UserContainer LoadUsers(string path) {
		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));


		FileStream stream = new FileStream (path, FileMode.Open);
	
		UserContainer users = serializer.Deserialize (stream) as UserContainer;

		stream.Close();
		return users;

	}

	private static void SaveUsers(string path, UserContainer users) {
	
		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));


		FileStream stream = new FileStream (path, FileMode.Truncate);
		serializer.Serialize(stream, users);

		stream.Close ();

	}
}
