using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //For Directory
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System;

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

	// SAVE TEXT
	public static void SaveToText<T>(object data, string fileName, string path)
	{
		Debug.Log("[DataManager] Save to text");
		//Change data to string
		string textData = SerializeToString<T>(data);
		Debug.Log("[DataManager] TextData: " + textData);
		//Encrypt data
		textData = Encrypt(textData);
		Debug.Log("[DataManager] Encrypt: " + textData);

		//Save using a file
		CreateDirectory(path);

		StreamWriter writer = new StreamWriter(path + "/" + fileName);
		writer.Write(textData);
		writer.Close(); //end of string. Otherwise it stays open and will have trouble when this function is called again.

		//Save using player prefs (Unity)
		//PlayerPrefs.SetString("Data", textData); //Only inconvenience: It´s exclusive to Unity if it weren´t for this part everything would be C#


		Debug.Log("[DataManager] Text saved");
	}
	public static object LoadFromText<T>(string fileName, string path)
	{
		//Read a file´s text (load)
		string textData = "";
		
		StreamReader reader = new StreamReader(path + "/" + fileName);
		//reader.ReadLine() //For only a line
		
		textData = reader.ReadToEnd();
		reader.Close();

		//Load string through playerpref
		/*
		if(PlayerPrefs.HasKey(fileName))//Check whether it exists
		{
			textData = PlayerPrefs.GetString(fileName);
		}
		*/
		//Decrypt
		textData = Decrypt(textData);
		Debug.Log(textData);
		

		//Convert String to my data
		object data = DeserializeFromString<T>(textData);

		return data;
	}

	//String Serializer
	public static string SerializeToString<T>(object data) //This is so we can enrypt the data.
	{
		Debug.Log("[DataManager] Serialize to string");
		
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		using (StringWriter writer = new StringWriter())
		{
			serializer.Serialize(writer, data);
			return writer.ToString(); //receive the serialized data witten in form of a string
		}
	}
	public static object DeserializeFromString<T>(string text)
	{
		Debug.Log("[DataManager] Deserialize from string");
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		return serializer.Deserialize(new StringReader(text)); //HERE IS WHERE MY NIGHTMARES EXIST
	}
	//ENCRYPT
	private static string Encrypt(string text)
	{
		try
		{
			string key = "D3$$e7";

			byte[] keyArray;

			byte[] byteText = UTF8Encoding.UTF8.GetBytes(text);

			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
			hashmd5.Clear();

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] result = cTransform.TransformFinalBlock(byteText, 0, byteText.Length);
			tdes.Clear();

			text = Convert.ToBase64String(result, 0, result.Length);
		}
		catch (Exception)
		{

		}

		return text;
	}
	private static string Decrypt(string text)
	{
		try
		{
			string key = "D3$$e7";

			byte[] keyArray;

			byte[] byteText = Convert.FromBase64String(text);

			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
			hashmd5.Clear();

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] result = cTransform.TransformFinalBlock(byteText, 0, byteText.Length);
			tdes.Clear();

			text = UTF8Encoding.UTF8.GetString(result);
		}
		catch (Exception e)
		{
			Debug.LogError(e.GetBaseException());
		}
		return text;
	}
	//UTILS
	private static void CreateDirectory(string path)
	{
		//Ask: Does path exist
		if (!Directory.Exists(path))
		{
			Debug.Log("[DataManager] create new directory");
			Directory.CreateDirectory(path);
		}
	}
	// Utils
	public static string LoadTextFromFile(string pathFile)
	{
		TextAsset asset = Resources.Load<TextAsset>(pathFile); //Resources.Load lets you load anything in the Resources folder of your project
		return asset.text;
	}

	public static string[] ReadLinesFromString(string text)
	{
		StringReader strReader = new StringReader(text);
		List<string> linesList = new List<string>();

		while (true)
		{
			string line = strReader.ReadLine();
			if (line != null) linesList.Add(line);
			else break;
		}

		return linesList.ToArray(); //Change the List to an array as it no longer needs to be modified.
	}
}
