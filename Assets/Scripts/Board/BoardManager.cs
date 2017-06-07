using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
	
	List<GameObject> _squares = new List<GameObject>();

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
