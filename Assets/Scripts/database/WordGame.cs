using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class WordGame : MonoBehaviour {


	private string island;
	private static string path = string.Empty;

	public void GetData() {

		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		int pos = gm.Index;
		print ("pos is" + pos);

		// choose quiz file according to island
		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/wordGamesD.xml");
			print ("we are on island D");
		}

		gm.Path = path;
		HistoryCheck.FirstTimeCheck (path, island, "WordGames", "Game");
	}
}

