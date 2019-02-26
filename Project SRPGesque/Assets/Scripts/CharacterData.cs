using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class CharacterData
{
	[XmlAttribute("name")]
	public string name;
	public int id;
	public int level;
	public int hp;
	public int mp;
	public int attack;
	public int defense;
	public int resistance;
	public int speed;

}
