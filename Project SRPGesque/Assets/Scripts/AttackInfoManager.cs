using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoManager : MonoBehaviour 
{
	public int attacks;
	public string[] attackDescriptions;
	public int[] attackStrengths;
	public int[] attackMpCosts;
	//And an attackRange or Area of effect storing.

	// Use this for initialization
	void Start () 
	{
		
		attackDescriptions = new string[attacks];
		attackStrengths = new int[attacks];
		attackMpCosts = new int[attacks];
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetAttack(int atkId, int strength, int mp)
	{
		attackStrengths[atkId] = strength;
		attackMpCosts[atkId] = mp;
	}
	public void SetAttackDescription(int atkId, bool isMagic, string description)
	{
		if(isMagic) attackDescriptions[atkId] = "Magic: " + description;
		if(!isMagic) attackDescriptions[atkId] = "Physical: " + description;
	}
}
