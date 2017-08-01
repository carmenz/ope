using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// Serialize for monitor
	[SerializeField]
	Vector2 _coordinate;
	[SerializeField]
	string _tokenName = "First";
	[SerializeField]
	public List<Mission> missionListA = new List<Mission>();
	public List<Mission> missionListB = new List<Mission>();
	[SerializeField]
	public int score;
	public string currentIsland;

	public int typeCode;
	string _uname = "TestUser";

	[SerializeField]
	int _index = 1;

	[SerializeField]
	string _path = "path";


		
	public string Username{
		get {
			return _uname;
		}
		set {
			_uname = value;
		}
	}

	public string TokenName {
		get {
			return _tokenName;
		}
		set {
			_tokenName = value;
		}
	}

	public int Index{
		get {
			return _index;
		}
		set {
			_index = value;
		}
	}

	public string Path{
		get {
			return _path;
		}
		set {
			_path = value;
		}
	}

	public Vector2 Coordinate {
		get {
			return _coordinate;
		}
		set {
			_coordinate = value;
		}
	}
	
	void Awake() {
		// init
		DontDestroyOnLoad(this);
  
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	void onDisable() {
		SaveData.OnBeforeSave += delegate {
			GetInputData();
		};
	}

	public void updateDBTotalScore(int currentScore) {

		string userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		// Find user and update <TotalScore>
		while (usernameNode.InnerText != Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 
		usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText = 
			(int.Parse(usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();

		xmlUserDoc.Save (userpath);
	}


	public UserData data = new UserData ();

	public void GetInputData() {
		Scene currentScene = SceneManager.GetActiveScene ();
		string sceneName = currentScene.name;

		if (sceneName == "Login") {
			string nameInput = GameObject.FindGameObjectWithTag ("UsernameField").GetComponent<InputField> ().text;
			data.username = nameInput;

			string passwordInput = GameObject.FindGameObjectWithTag ("PasswordField").GetComponent<InputField> ().text;
			data.password = Login.Encrypt (passwordInput);

		} else {
			data.username = Username;
		}
		data.currentPosX = -1300;
		data.currentPosY = -200;
		data.currentIsland = "A";
		data.totalScore = 0;

	}

}



public class UserData{

	[XmlElement("Username")]
	public string username;

	[XmlElement("Password")]
	public string password;

	[XmlElement("CurrentposX")]
	public float currentPosX;

	[XmlElement("CurrentposY")]
	public float currentPosY;

	[XmlElement("CurrentIsland")]
	public string currentIsland;

	[XmlElement("TotalScore")]
	public int totalScore;


}
