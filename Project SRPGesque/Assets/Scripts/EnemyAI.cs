using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public GameManager gameManager;
	//public AttackInfoManager attackInfo;
	public TileScript tileScript;
	public CharControl_Battle charControl;

	public int atkInPoolWithMostTargets;
	public int highestTargetAmount;

	int attacksStoredCounter;

	public int currentAttack;
	public Vector2 attacksTargetMargin;
	public Vector2 targetOrigin;
	Vector2[] optimalTileOrigins;

	// Start is called before the first frame update
	void Start()
    {
		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		//attackInfo = gameManager.GetComponent<AttackInfoManager>();
		tileScript = gameManager.GetComponentInChildren<TileScript>();
		charControl = GetComponent<CharControl_Battle>();

		attacksTargetMargin = gameManager.gameData.AttackList[currentAttack].rangeSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	

	public void EnemyAILogic()
	{

		int[] storedAtk = new int[charControl.attacksLearned.Length]; //max amount of attacks, some maybe empty.
		attacksStoredCounter = 0;

		atkInPoolWithMostTargets = -1;
		highestTargetAmount = 0;

		//Debug.Log(charControl.attacksLearned.Length);
		

		int[] victimAmount;

		//Enough MP to Use?
		for (int i = 0; i < charControl.attacksLearned.Length; i++)
		{
			//Debug.Log("mpCost" + gameManager.gameData.AttackList[charControl.attacksLearned[i]].mpCost);
			if (gameManager.gameData.AttackList[charControl.attacksLearned[i]].mpCost <= charControl.currentMp) //If character has enough MP left to use this attack
			{
				//Store this attack
				storedAtk[attacksStoredCounter] = gameManager.gameData.AttackList[charControl.attacksLearned[i]].id;
				attacksStoredCounter++;
			}
		}

		//Debug.Log(charControl.charId + " stored " + attacksStoredCounter + " attacks");
		

		if (attacksStoredCounter == 0)
		{
			//NO MP
			//Debug.Log(charControl.charId + "has not enough Mp to use any of its moves");
			StartCoroutine(WaitForMPNotif());
			Debug.Log("Not enough MP");
			return;
		}
		//Debug.Log(charControl.charId + " stored " + storedAtk[0]);

		//Initialize an array of length of how many attacks have been stored for the amount of characters that will be hit by each stored attack. 
		victimAmount = new int[attacksStoredCounter];
		optimalTileOrigins = new Vector2[attacksStoredCounter];

		//Go through each of the attacks with enough mp. And get their best tile placement
		for (int attack = 0; attack < attacksStoredCounter; attack++)
		{
			//Left off investigation here
			//
			victimAmount[attack] = 0;
			attacksTargetMargin = gameManager.gameData.AttackList[storedAtk[attack]].rangeSize;
			//Possible confusions: X and Y getting mixed up.
			for (int tileX = 3; tileX < tileScript.xTiles - (attacksTargetMargin.x); tileX++)
			{
				for (int tileY = 0; tileY < tileScript.yTiles - (attacksTargetMargin.y); tileY++)
				{
					gameManager.targetOrigin.x = tileX;
					gameManager.targetOrigin.y = tileY;

					gameManager.CalculateTargetAmount();
					gameManager.TargetPlacement();

					int onTileVictimAmount = 0;

					//see not just whether it has the most targets, but whether the places on which the targets lay are occupied
					for (int e = 0; e < gameManager.selectedTargets.Length; e++)
					{
						if (tileScript.tiles[gameManager.selectedTargets[e]].isOccupied)
						{
							onTileVictimAmount++;
						}
					}

					if(onTileVictimAmount > victimAmount[attack])//In the case of the amount of victims in this tile combination being higher than the previous highest for this attack.
					{
						victimAmount[attack] = onTileVictimAmount;
						optimalTileOrigins[attack] = new Vector2(tileX, tileY);
					}
					//End of Double For that check for the optimal target tiles of this attack
				}
			}
			//Debug.Log("attack " + attack + "'s optimal origin is " + optimalTileOrigins[attack]);
			if (victimAmount[attack] == 0)
			{
				//This attacks pattern makes it incapable of reaching anyone.
			}
		}
		
		//Now this one is different from the previous one, this one serves the purpose of comparing the attacks to each other to decide which one to choose.
		for(int attack = 0; attack < attacksStoredCounter; attack++)
		{
			if(attack == 0)// For the First iteration just put it as default best attack 
			{
				highestTargetAmount = victimAmount[attack];
				atkInPoolWithMostTargets = attack;
			}
			else //Everything else 
			{
				if(victimAmount[attack] > highestTargetAmount)
				{
					highestTargetAmount = victimAmount[attack];
					atkInPoolWithMostTargets = attack;
				}
				else if(victimAmount[attack] == highestTargetAmount) // In case it has as many victims as the highest
				{
					//Does it do more damage than the attack with most victims?
					if (gameManager.gameData.AttackList[storedAtk[attack]].strength > gameManager.gameData.AttackList[storedAtk[atkInPoolWithMostTargets]].strength)
					{
						highestTargetAmount = victimAmount[attack];
						atkInPoolWithMostTargets = attack;
					}
					else if(gameManager.gameData.AttackList[storedAtk[attack]].strength == gameManager.gameData.AttackList[storedAtk[atkInPoolWithMostTargets]].strength)
					{
						//By some coincidence both attacks have equal; amount of victims and strength
						//which one wastes less mp?
						if(gameManager.gameData.AttackList[storedAtk[attack]].mpCost < gameManager.gameData.AttackList[storedAtk[atkInPoolWithMostTargets]].mpCost)
						{
							highestTargetAmount = victimAmount[attack];
							atkInPoolWithMostTargets = attack;
						}
						else if(gameManager.gameData.AttackList[storedAtk[attack]].mpCost == gameManager.gameData.AttackList[storedAtk[atkInPoolWithMostTargets]].mpCost)
						{
							// By some godforsaken reason these attacks are way too fucking similar. Just leave it to chance.
							if(Random.value == 0)
							{
								highestTargetAmount = victimAmount[attack];
								atkInPoolWithMostTargets = attack;
							}
						}
					}
				}
			}
		}
		//Debug.Log(atkInPoolWithMostTargets + "or moreso known as " + storedAtk[atkInPoolWithMostTargets] + "shall be used by " + charControl.charId);

		currentAttack = storedAtk[atkInPoolWithMostTargets];
		gameManager.targetOrigin = optimalTileOrigins[atkInPoolWithMostTargets];
		//Debug.Log(optimalTileOrigins[atkInPoolWithMostTargets]);
		attacksTargetMargin = gameManager.gameData.AttackList[currentAttack].rangeSize;

		gameManager.targetMargin = attacksTargetMargin;

		gameManager.CalculateTargetAmount();
		gameManager.TargetPlacement();
		gameManager.EnemyTargetVisuals();

		gameManager.LaunchAttack();

	}
	IEnumerator WaitForMPNotif()
	{
		gameManager.battleUI.ChangeNotifText(charControl.name + " lacks enough MP to use an attack!");
		yield return new WaitForSeconds(2);
		Debug.Log("End turn");
		gameManager.EndTurn();
	}
}
