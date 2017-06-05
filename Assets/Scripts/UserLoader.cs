using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLoader : MonoBehaviour {
	
	public const string path = "user";
	// Use this for initialization
	void Start () {
		UserContainer uc = UserContainer.Load (path);
		foreach (User user in uc.users) {
			print (user.username);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
