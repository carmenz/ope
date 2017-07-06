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
	
		pos = gm.Index;

		// choose FillInTheBlank file according to island
		if (pos < 25) {
			island = "IslandA";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksA.xml");
			print ("we are on island A");
		} else if (pos < 30) {
			island = "IslandB";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksB.xml");
			print ("we are on island B");
		} else if (pos < 40) {
			island = "IslandC";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksC.xml");
			print ("we are on island C");
		} else if (pos < 50) {
			island = "IslandD";
			path = System.IO.Path.Combine (Application.dataPath, "Resources/blanksD.xml");
			print ("we are on island D");
		}

		gm.Path = path;
		HistoryCheck.FirstTimeCheck (path, island, "FillInTheBlanks", "FIB");
	}
}

//public class FillInTheBlankData{
//	[XmlElement("Index")]
//	public string index;
//}


