using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class User : MonoBehaviour {

//	public UserData data = new UserData ();
//
//	public string username = "TestName";
//	public string  password = "000";
//	public float currentPosX = 0.0f;
//	public float currentPosY = 0.0f;
//	public string currentIsland = "A";
//	public int score = 0;
//


//	public void GetInputData() {
//
//		Scene currentScene = SceneManager.GetActiveScene ();
//		// Retrieve the name of this scene.
//		string sceneName = currentScene.name;
//
//
//		if (sceneName == "Login") {
//			string nameInput = GameObject.FindGameObjectWithTag ("UsernameField").GetComponent<InputField> ().text;
//			data.username = nameInput;
//
//			string passwordInput = GameObject.FindGameObjectWithTag ("PasswordField").GetComponent<InputField> ().text;
//			data.password = Login.Encrypt (passwordInput);
//
//		} else {
//			data.username = GameObject.Find ("GameManager").GetComponent<GameManager> ().name;
//		}
//		data.currentPosX = GameObject.Find ("GameManager").GetComponent<GameManager> ().Coordinate.x;
//		data.currentPosY = GameObject.Find ("GameManager").GetComponent<GameManager> ().Coordinate.y;
//		data.currentIsland = "A";
//		data.totalScore = score;
//
//	}



//	void onDisable() {
//
//		// SaveData.OnLoaded += delegate {
//		// 	LoadData();
//		// };
//		SaveData.OnBeforeSave += delegate {
//			GetInputData();
//		};
////		SaveData.OnBeforeSave += delegate {
////			SaveData.AddUserToData(data);
////		};
//	}

}

//public class UserData{
//
//	[XmlElement("Username")]
//		public string username;
//
//	[XmlElement("Password")]
//		public string password;
//
//	[XmlElement("CurrentposX")]
//	public float currentPosX;
//
//	[XmlElement("CurrentposY")]
//	public float currentPosY;
//
//	[XmlElement("CurrentIsland")]
//	public string currentIsland;
//
//	[XmlElement("TotalScore")]
//	public int totalScore;
//
//
//}
