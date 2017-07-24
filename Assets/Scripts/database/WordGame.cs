using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGame : MonoBehaviour {

	private string island;
	private static string path = string.Empty;
	private GameManager gm;
	private int pos;


	public void GetData() {

		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		string currentIsland = gm.currentIsland;

		pos = gm.Index;

		// choose game file according to island
		if (currentIsland == "A") {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/wordGamesA.xml");
		} else if (currentIsland == "B") {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/wordGamesB.xml");
		} else if (currentIsland == "C") {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/wordGamesC.xml");
		} else if (currentIsland == "D") {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/wordGamesD.xml");
		}

		gm.Path = path;
		HistoryCheck.FirstTimeCheck (path, island, "WordGames", "Game");
	}
}

