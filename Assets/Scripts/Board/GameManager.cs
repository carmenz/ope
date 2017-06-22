using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// Serialize for monitor
	[SerializeField]
	int _curPos = 0;
	[SerializeField]
	string _tokenName = "First";
	[SerializeField]
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

	public string TokenName{
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
