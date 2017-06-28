using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class BoardManager : MonoBehaviour {
	
	List<GameObject> _squares = new List<GameObject>();
	private static string userpath = string.Empty;

	public List<GameObject> Squares{
		get {
			return _squares;
		}
	}

	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform)
		{
			if (child.tag == "Square")
			{
				_squares.Add(child.gameObject);
			}
		}

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		Text scoreText = GameObject.Find ("Score").GetComponent<Text> ();

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		FileStream userStream = new FileStream (userpath, FileMode.Open);
		XmlTextReader xmlUserReader = new XmlTextReader (userStream);

		while (xmlUserReader.Read ()) {

			if (xmlUserReader.Name == "Username") {
				// find user from user.xml and get TotalScore
				if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
					xmlUserReader.ReadToFollowing ("TotalScore").ToString ();
					scoreText.text = xmlUserReader.ReadElementContentAsString ();

				}
			}
		}

		userStream.Close ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// determine if user needs to slide down
	void AddSlide() {

	}
		


	//for test purpose
	void Find() {

	}

	void Display() {

	}

	void PrintList() {
		foreach (GameObject square in Squares) {
			print (square.name);
		}
	}

}
