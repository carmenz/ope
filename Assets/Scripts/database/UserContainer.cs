using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;



[XmlRoot("UserCollection")]
public class UserContainer
{
	[XmlArray("Users"),XmlArrayItem("User")]
	public List<UserData> users = new List<UserData>();


	public void Save(string path)
	{
		Debug.Log ("userContainer");
		
		var serializer = new XmlSerializer(typeof(UserContainer));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}

	}

	public static UserContainer Load(string path)
	{
		var serializer = new XmlSerializer(typeof(UserContainer));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as UserContainer;
		}
	}
}

