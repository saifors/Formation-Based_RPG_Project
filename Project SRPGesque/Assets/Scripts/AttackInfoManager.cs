using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoManager : MonoBehaviour 
{
	public int attacks;
    public int[] attackID;
	public string[] attackNames;
	public string[] attackDescriptions;
    public int[] attackStrengths;
	public int[] attackMpCosts;
    public Vector2[] attackRangeSize; //Attack Row(X) and Column(Y) size
    public int[][] attackRangeActive; 

    
	//And an attackRange or Area of effect storing.

	// Use this for initialization
	void Start () 
	{
        attacks = 7;
        attackID = new int[attacks];
		attackNames = new string[attacks];
		attackDescriptions = new string[attacks];
        attackStrengths = new int[attacks];
		attackMpCosts = new int[attacks];
        attackRangeSize = new Vector2[attacks];
        attackRangeActive = new int[attacks][];

        //Attack 1
        SetAttack(0, "Fire Bolt",20, 10);
        SetAttackDescription(0, true, "Summons an arrow of fire from above to pierce through the foe's body.");
        SetRange(0, new Vector2(3,2), new int[] {1,0,1,0,1,0});
        //Attack 2
        SetAttack(1, "Ice Pike", 23, 12);
        SetAttackDescription(1, true, "Frozen pikes rize from the ground catching the enemy off guard.");
        SetRange(1, new Vector2(1,2), new int[] {1,1});
        //Attack 3
        SetAttack(2, "Dab", 7, 5);
        SetAttackDescription(2, false, "Kills off the user's brain cells to leave the enemy in pain");
        SetRange(2, new Vector2(1,1), new int[] {1});
        //Attack 4
        SetAttack(3, "II", 20, 10);
        SetAttackDescription(3, true, "idk2");
        SetRange(3, new Vector2(3,2), new int[] {1,1,1,0,1,0});
        //Attack 5
        SetAttack(4, "III", 20, 10);
        SetAttackDescription(4, true, "idk3");
        SetRange(5, new Vector2(3,2), new int[] {1,1,1,0,1,0});
        //Attack 6
        SetAttack(5, "IV", 20, 10);
        SetAttackDescription(5, true, "idk4");
        SetRange(6, new Vector2(3,2), new int[] {1,1,1,0,1,0});
        //Attack 7
        SetAttack(6, "V", 20, 10);
        SetAttackDescription(6, true, "idk5");
        SetRange(7, new Vector2(3,2), new int[] {1,1,1,0,1,0});
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetAttack(int atkId, string name, int strength, int mp)
	{
        attackID[atkId] = atkId;
        attackStrengths[atkId] = strength;
        attackNames[atkId] = name;
        attackMpCosts[atkId] = mp;
	}
	public void SetAttackDescription(int atkId, bool isMagic, string description)
	{
		if(isMagic) attackDescriptions[atkId] = "Magic: " + description;
		else if(!isMagic) attackDescriptions[atkId] = "Physical: " + description;
	}

    public void SetRange(int atkId, Vector2 rangeSize, int[] affected)//Range Size should never surpass 8,6
    {
        if(affected.Length != rangeSize.x*rangeSize.y) Debug.LogError("The size of the array of attack Id:" + atkId + " doesn't coincide with it´s given Column and/or row size");
        attackRangeSize[atkId] = rangeSize;
        //
        attackRangeActive[atkId] = affected;
        

    }
}
