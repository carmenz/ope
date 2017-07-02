using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour {

	private string island;
	private static string path = string.Empty;

	public void GetData() {
		GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		int pos = gm.Index;
		print ("pos is" + pos);

		// choose FillInTheBlank file according to island
		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/quizzesA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.persistentDataPath, "Resources/quizzesD.xml");
			print ("we are on island D");
		}

		gm.Path = path;

		//HistoryCheck.FirstTimeCheck (path, island, "Quizzes", "Quiz");
	}
}
