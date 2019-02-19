using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public GameManager gameManager;
	public AttackInfoManager attackInfo;
	public TileScript tileScript;
	public CharControl_Battle charControl;

	public int atkInPoolWithMostTargets;
	public int highestTargetAmount;

	public int currentAttack;
	public Vector2 attacksTargetMargin;
	public Vector2 targetOrigin;

    // Start is called before the first frame update
    void Start()
    {
		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		attackInfo = gameManager.GetComponent<AttackInfoManager>();
		tileScript = gameManager.GetComponentInChildren<TileScript>();
		charControl = GetComponent<CharControl_Battle>();

		attacksTargetMargin = attackInfo.attackRangeSize[currentAttack];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	

	public void EnemyAILogic()
	{
		int[] storedAtk = new int[charControl.attacksLearned.Length]; //max amount of attacks, some maybe empty.
		int attacksStoredCounter = 0;

		atkInPoolWithMostTargets = -1;
		highestTargetAmount = 0;
		Vector2 optimalTileOrigin = Vector2.zero;

		attacksTargetMargin = attackInfo.attackRangeSize[currentAttack];

		int[] victimAmount;

		//Enough MP to Use?
		for (int i = 0; i < charControl.attacksLearned.Length; i++)
		{
			if (attackInfo.attackMpCosts[charControl.attacksLearned[i]] <= charControl.currentMp) //If character has enough MP left to use this attack
			{
				//Store this attack
				storedAtk[attacksStoredCounter] = attackInfo.attackID[charControl.attacksLearned[i]];
				attacksStoredCounter++;
			}
		}

		if (attacksStoredCounter == 0)
		{
			//NO MP
			Debug.Log(charControl.charId + "has not enough Mp to use any of its moves");
		}

		//Initialize an array of length of how many attacks have been stored for the amount of characters that will be hit by each stored attack. 
		victimAmount = new int[attacksStoredCounter];

		//Go through each of the attacks with enough mp.
		for (int attack = 0; attack < attacksStoredCounter; attack++)
		{
			//Left off investigation here
			gameManager.CalculateTargetAmount();

			for (int tileX = 3; tileX < tileScript.xTiles - (attacksTargetMargin.x); tileX++)
			{
				for (int tileY = 0; tileY < tileScript.yTiles - (attacksTargetMargin.y); tileY++)
				{
					targetOrigin.x = tileX;
					targetOrigin.y = tileY;

					gameManager.TargetPlacement();


					//see not just whether it has the most targets, but whether the places on which the targets lay are occupied
					for (int e = 0; e < gameManager.selectedTargets.Length; e++)
					{
						if (tileScript.tiles[gameManager.selectedTargets[e]].isOccupied)
						{
							victimAmount[attack]++;
						}
					}

					if (victimAmount[attack] != 0)
					{
						if (victimAmount[attack] > highestTargetAmount)
						{
							atkInPoolWithMostTargets = attack;
							highestTargetAmount = victimAmount[attack];
							optimalTileOrigin = new Vector2(tileX, tileY);
						}
						else if (victimAmount[attack] == highestTargetAmount)
						{
							// does "i" have more power than "atkInPoolWithMostTargets"?
							if (attackInfo.attackStrengths[storedAtk[attack]] > attackInfo.attackStrengths[storedAtk[atkInPoolWithMostTargets]])
							{
								atkInPoolWithMostTargets = attack;
							}
							else if (attackInfo.attackStrengths[storedAtk[attack]] == attackInfo.attackStrengths[storedAtk[atkInPoolWithMostTargets]])
							{
								if (Random.value == 0) atkInPoolWithMostTargets = attack;
							}
							highestTargetAmount = victimAmount[attack];
							optimalTileOrigin = new Vector2(tileX, tileY);
						}
					}
				}
			}
		}
		//Get the selected Targets based on what has just been decided as the best course;

		gameManager.targetOrigin = optimalTileOrigin;
		//currentAttack = storedAtk[atkInPoolWithMostTargets];

		gameManager.CalculateTargetAmount();
		//Debug.Log("attack with most targets " + atkInPoolWithMostTargets + " current attack " + currentAttack + " with an origin at " + targetOrigin + "and the target amount is " + targetAmount);
		gameManager.TargetPlacement();

		gameManager.LaunchAttack();



		//How much combined damage will it do

	}
}
