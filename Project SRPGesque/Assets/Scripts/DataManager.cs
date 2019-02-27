using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //For Directory
using System.Xml.Serialization;

public static class DataManager //Rmember static will be during entire game so no static variables preferably.
{
    //We´re going to make this independent so it can apply to any project, at least by the end
	//This script is EXCLUSIVELY for data

		//XML

	//T is to save type of object the same as with GetComponent<x>(); This is to make it universal so you can just put in the script and never look at it.
	public static void SaveToXML<T>(object data, string fileName, string path) //could replace void with bool so it returns a bool depending on if Save succeeded.
	{
		Debug.Log("[DataManager] Save " + Time.time); //Can show you how fast it took to take the action
		

		//Ask: Does path exist
		if(!Directory.Exists(path))
		{
			Debug.Log("[DataManager] create new directory");
			Directory.CreateDirectory(path);
		}

		XmlSerializer serializer = new XmlSerializer(typeof(T)); //In charge of serializingand deserializing.
		using (FileStream stream = new FileStream(path + "/" + fileName, FileMode.Create)) //Using this file Serializae data.
		{
			serializer.Serialize(stream, data);
		}
		Debug.Log("[DataManager] Game saved " + Time.time);
	}

	public static object LoadFromXML<T>(string fileName, string path)
	{
		Debug.Log("[DataManager] Load " + Time.time);
		object data; // Coul also use T

		XmlSerializer serializer = new XmlSerializer(typeof(T)); 
		using (FileStream stream = new FileStream(path + "/" + fileName, FileMode.Open)) 
		{
			data = serializer.Deserialize(stream); //would need " (T) " before serializer if T was used for data instead of object
		}
		Debug.Log("[DataManager] Game loaded " + Time.time);

		return data;
	}

	
}
