﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Option4 : MonoBehaviour {

	private FillInTheBlankManager fibm;

	void Start() {
		fibm = GameObject.Find("FillInTheBlankManager").GetComponent<FillInTheBlankManager>();
	}

	public void onClick() {
		fibm.TaskOnClick(4);
	}
}
