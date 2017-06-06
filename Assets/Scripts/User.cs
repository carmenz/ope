using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;


public class User : MonoBehaviour {

	public UserData data = new UserData ();
	public InputField username_field;
	public int password = 000;
	public int currentPos = 0;
	public int currentScore = 0;


	public void StoreData() {
		

		data.username = username_field.text.ToString();


		data.pwd = password;
		data.currentPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentPosition;
		data.currentScore = currentScore;
		Vector3 pos = transform.position;
	}


	public void LoadData() {
		name = data.username;
		currentPos = data.currentPos;
		currentScore = data.currentScore;
		transform.position = new Vector2 (BoardManager.Squares [currentPos].transform.position.x, BoardManager.Squares [currentPos].transform.position.y);
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
