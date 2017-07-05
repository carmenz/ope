using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;

public class GameController : MonoBehaviour {

	public static Button saveButton;
	public static Button loadButton;

	private User currentUser = new User();
	private static string datapath;

	void Awake() {
		if (Application.platform == RuntimePlatform.WindowsPlayer) {
			datapath = System.IO.Path.Combine (Application.persistentDataPath, "Resources/users.xml");
		} else {
		datapath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");

		}
		saveButton = GameObject.Find ("SaveButton").GetComponent<Button> ();
		loadButton = GameObject.Find ("LoadButton").GetComponent<Button> ();
	}


	void OnEnable() {

		// switch to main menu after saveButton on Login is clicked
		saveButton.onClick.AddListener (delegate {
			print("savebutton clicked OnEnable");
			//SaveData.Save (datapath, SaveData.userContainer);
			SaveData.Save(datapath, currentUser);
		});
		 loadButton.onClick.AddListener(delegate {
			print("loadbutton clicked OnEnable");
			SaveData.Load(datapath, currentUser);
		 });
	}

//	void OnDisable() {
//		saveButton.onClick.RemoveListener (delegate {
//			//print("savebutton clicked OnDisable");
//			//SaveData.Save (datapath, SaveData.userContainer);
//			SaveData.Save(datapath, currentUser);
//		});
//		loadButton.onClick.RemoveListener(delegate {
//			print("loadbutton clicked OnDisable");
//			SaveData.Load(datapath, currentUser);
//		});
//	}

}
