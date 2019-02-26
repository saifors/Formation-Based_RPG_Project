using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class PartyData
{
	[XmlArray("Party")]
	[XmlArrayItem("Character")]
	public List<CharacterData> Party = new List<CharacterData>();
}
