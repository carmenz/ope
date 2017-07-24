using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System;

public class SquareController : MonoBehaviour {

	public string type;
	public int index;
	public string location;

	public void ShowCompleted() {
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
	}

}
