using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoader : MonoBehaviour {

	public const string path = "Resources/users";

	void Start() {
		SaveData.Load (path);
		//foreach (UserData user in uc.username) {
			//print (user.username);
	//	}
	}
}
