using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public static List<GameObject> Squares = new List<GameObject>();

	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform)
		{
			if (child.tag == "Square")
			{
				Squares.Add(child.gameObject);
			}
		}

		PrintList ();
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
