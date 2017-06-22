using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// Serialize for monitor
	[SerializeField]
	int _curPos;
	[SerializeField]
	Vector2 _coordinate;
	[SerializeField]
	string _tokenName = "First";
	[SerializeField]
	// public List<int> quizRequired = new List<int>();
	// public string quizDesc;
	public Missions missions;

	public int typeCode;
	string _uname = "TestUser";

	[SerializeField]
	string _index = "index";

	[SerializeField]
	string _path = "path";

	
	public int CurrentPosition{
		get {
			return _curPos;
		}
		set {
			_curPos = value;
		}
	}
		
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

	public string Index{
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
