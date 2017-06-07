using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Save() {
		SaveData.Save (System.IO.Path.Combine (Application.dataPath, "Resources/users.xml"), SaveData.userContainer);
	}
}
