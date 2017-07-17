using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// Serialize for monitor
	[SerializeField]
	Vector2 _coordinate;
	[SerializeField]
	string _tokenName = "First";
	[SerializeField]
	public List<Mission> missionList = new List<Mission>();
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
