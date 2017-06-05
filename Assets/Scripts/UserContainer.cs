using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;


[XmlRoot("UserCollection")]
public class UserContainer {

	[XmlArray("Users")]
	[XmlArrayItem("User")]
	public List<User> users = new List<User>();


	public static UserContainer Load(string path){
		TextAsset _xml = Resources.Load<TextAsset> (path);
		XmlSerializer serializer = new XmlSerializer (typeof(UserContainer));
		StringReader reader = new StringReader (_xml.text);
		UserContainer users = serializer.Deserialize (reader) as UserContainer;
		reader.Close();
		return users;

	}
}
