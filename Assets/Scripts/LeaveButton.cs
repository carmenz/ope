using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;

public class LeaveButton : MonoBehaviour {

	private static string userpath = string.Empty;
	private GameManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void LeaveTheGame() {

		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);

		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 

		usernameNode.ParentNode.SelectSingleNode ("CurrentPosX").InnerText = gm.Coordinate.x.ToString();
		usernameNode.ParentNode.SelectSingleNode ("CurrentPosY").InnerText = gm.Coordinate.y.ToString();

		xmlUserDoc.Save (userpath);

		gm.Username = "";


		SceneManager.LoadScene ("Login");

	}
}
