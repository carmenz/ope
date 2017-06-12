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
	
	public int CurrentPosition{
		get {
			return _curPos;
		}
		set {
			_curPos = value;
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
