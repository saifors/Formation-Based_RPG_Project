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

        //Attack 1
        SetAttack(0, "Fire Bolt",20, 10);
        SetAttackDescription(0, true, "Summons an arrow of fire from above to pierce through the foe's body.");
        //Attack 2
        SetAttack(1, "Ice Pike", 20, 10);
        SetAttackDescription(1, true, "Frozen pikes rize from the ground catching the enemy off guard.");
        //Attack 3
        SetAttack(2, "I", 20, 10);
        SetAttackDescription(2, true, "idk");
        //Attack 4
        SetAttack(3, "II", 20, 10);
        SetAttackDescription(3, true, "idk2");
        //Attack 5
        SetAttack(4, "III", 20, 10);
        SetAttackDescription(4, true, "idk3");
        //Attack 6
        SetAttack(5, "IV", 20, 10);
        SetAttackDescription(5, true, "idk4");
        //Attack 7
        SetAttack(6, "V", 20, 10);
        SetAttackDescription(6, true, "idk5");
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
