using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FillInTheBlank : MonoBehaviour {

	private string island;
	private static string path = string.Empty;
	private GameManager gm;
	private int pos;

	public void GetData() {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		string currentIsland = gm.currentIsland;
		pos = gm.Index;

		// choose FillInTheBlank file according to island
		if (currentIsland == "A") {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksA.xml");
		
		} else if (currentIsland == "B") {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksB.xml");
		
		} else if (currentIsland == "C") {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksC.xml");
	
		} else if (currentIsland == "D") {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksD.xml");

		}

		gm.Path = path;
		HistoryCheck.FirstTimeCheck (path, island, "FillInTheBlanks", "FIB");
	}
}


