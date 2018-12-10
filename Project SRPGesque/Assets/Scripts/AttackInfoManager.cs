using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoManager : MonoBehaviour 
{
	public int attacks;
	public string[] attackNames;
	public string[] attackDescriptions;
    public int[] attackStrengths;
	public int[] attackMpCosts;
	//And an attackRange or Area of effect storing.

	// Use this for initialization
	void Start () 
	{
        attacks = 1;
		attackNames = new string[attacks];
		attackDescriptions = new string[attacks];
        attackStrengths = new int[attacks];
		attackMpCosts = new int[attacks];
        SetAttack(0, "Fire Arrow I",20, 10);
        SetAttackDescription(0, true, "Unleashes a single powerful fireball to one foe");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetAttack(int atkId, string name, int strength, int mp)
	{
        attackStrengths[atkId] = strength;
        attackNames[atkId] = name;
        attackMpCosts[atkId] = mp;
	}
	public void SetAttackDescription(int atkId, bool isMagic, string description)
	{
		if(isMagic) attackDescriptions[atkId] = "Magic: " + description;
		else if(!isMagic) attackDescriptions[atkId] = "Physical: " + description;
	}
}
