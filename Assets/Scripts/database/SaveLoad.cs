//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic; 
//using System.Runtime.Serialization.Formatters.Binary; 
//using System.IO;
//
//public static class SaveLoad {
//	public static UserContainer userContainer = new UserContainer();
//}

//public static void Save() {
//	savedUsers.Add(User.current);
//	BinaryFormatter bf = new BinaryFormatter();
//	FileStream file = File.Create (Application.persistentDataPath + "/savedUsers.xml");
//	bf.Serialize(file, SaveLoad.savedUsers);
//	file.Close();
//}
//
//public static void Load() {
//	if(File.Exists(Application.persistentDataPath + "/savedUsers.xml")) {
//		BinaryFormatter bf = new BinaryFormatter();
//		FileStream file = File.Open(Application.persistentDataPath + "/savedUsers.xml", FileMode.Open);
//		SaveLoad.savedGames = (List<User>)bf.Deserialize(file);
//		file.Close();
//	}
//}