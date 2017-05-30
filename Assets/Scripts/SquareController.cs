using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class SquareController : MonoBehaviour {

	public string type;
	public int index;
	// Use this for initialization
	void Awake () {
		index = Int32.Parse(Regex.Replace(this.gameObject.name, @"[^\d\d]", ""));
		//print (index);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
