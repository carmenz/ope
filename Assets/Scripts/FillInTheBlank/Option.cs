using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Option : MonoBehaviour {

	private FillInTheBlankManager fibm;

	void Start() {
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankManager>();
	}

	public void onClick1() {
		fibm.TaskOnClick(1);
	}

	public void onClick2() {
		fibm.TaskOnClick(2);
	}

	public void onClick3() {
		fibm.TaskOnClick(3);
	}

	public void onClick4() {
		fibm.TaskOnClick(4);
	}
}
