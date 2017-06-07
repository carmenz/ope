using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;


public class User : MonoBehaviour {

	public UserData data = new UserData ();

	public int password = 000;
	public int currentPos = 0;
	public int currentScore = 0;



	public void StoreData() {

		string nameInput = GameObject.FindGameObjectWithTag ("UsernameField").GetComponent<InputField> ().text;

		data.username = nameInput;
		//data.username = username_field.text;
		//data.username = name;

		data.pwd = password;
		data.currentPos = GameObject.Find("GameManager").GetComponent<GameManager>().CurrentPosition;
		data.currentScore = currentScore;
		Vector3 pos = transform.position;
	}


	public void LoadData() {
		name = data.username;
		currentPos = data.currentPos;
		currentScore = data.currentScore;
		transform.position = new Vector2 (GameObject.Find("Board").GetComponent<BoardManager>().Squares [currentPos].transform.position.x, 
			GameObject.Find("Board").GetComponent<BoardManager>().Squares [currentPos].transform.position.y);
	}

	void OnEnable() {
		SaveData.OnLoaded += delegate {
			LoadData();
		};
		SaveData.OnBeforeSave += delegate {
			StoreData();
		};
		SaveData.OnBeforeSave += delegate {
			SaveData.AddUserToData(data);
		};

	}

	void onDisable() {

		SaveData.OnLoaded += delegate {
			LoadData();
		};
		SaveData.OnBeforeSave += delegate {
			StoreData();
		};
		SaveData.OnBeforeSave += delegate {
			SaveData.AddUserToData(data);
		};
	}

}

public class UserData{

	[XmlElement("Username")]
		public string username;

	[XmlElement("Password")]
		public int pwd;


	[XmlElement("Currentpos")]
	public int currentPos;

	[XmlElement("CurrentScore")]
	public int currentScore;

}
