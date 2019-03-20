using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour
{
	public int charId;
	public CharacterStats.Alliance alliance;
	public Transform trans;

	public string name;
	public int classID;
	public int modelID;

	public int level;
	public int exp;
	public int currentHp;
	public int currentMp;
	public int hp;
	public int mp;
	public int atk;
	public int def;
	public int res;
	public int spd;

	public bool isDefending = false;

	public int[] attacksLearned;
	public int attacksAmount; //How many attacks does this character have.
	public int maxAttacks = 6;
	//public AttackInfoManager attackInfo;

	public GameManager gameManager;
	public GameData gameData;

	public bool isDead;

	public Vector2 tile;
	public int tileID;
	public int rowSize;

	public EnemyAI ai;

	private Vector3 playerRot = new Vector3(0,90,0);
	private Vector3 enemyRot = new Vector3(0,-90,0);

	private Animator anim;
	private bool lacksAnimator = true;

	public void MyTurn()
	{
		isDefending = false;
		if(alliance == CharacterStats.Alliance.Enemy)
		{
			ai.EnemyAILogic();
		}
	}

	public void Init(int charID, bool isPlayer)
	{
		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		gameData = gameManager.gameData;

		trans = transform;
		try
		{
			anim = GetComponentInChildren<Animator>();
			anim.Play("Idle");
		}
		catch
		{
			lacksAnimator = false;
		}
		

		if (isPlayer)
        {
            alliance = CharacterStats.Alliance.Player;
        }
        else
        {
            alliance = CharacterStats.Alliance.Enemy;
			ai = gameObject.AddComponent<EnemyAI>();
        }

		charId = charID;

		if(isPlayer)
		{
			classID = gameData.FullFormationsCollection[0].formation[charID].classID;
			//tile formation from game data here pls
			tile = gameData.FullFormationsCollection[0].formation[charID].tiles;

			//Debug.Log(gameData.Party[charID].level);
			name = gameData.Party[charID].name;
			level = gameData.Party[charID].level;

			hp = gameData.Party[charID].hp;
			mp = gameData.Party[charID].mp;
			atk = gameData.Party[charID].attack;
			def = gameData.Party[charID].defense;
			res = gameData.Party[charID].resistance;
			spd = gameData.Party[charID].speed;

			currentHp = gameData.Party[charID].currentHp;
			currentMp = gameData.Party[charID].currentMp;
			if (currentHp > hp) currentHp = hp;
			if (currentMp > mp) currentMp = mp;

			modelID = gameData.Party[charID].modelId;
			
		}
		else
		{
			int enemyID;
			enemyID = charID - gameManager.partyMembers; //PLACEHOLDER UNTIL I HAVE SHIT THAT ACTUALLY WORKS
			//Debug.Log(charId + "Is enemy" + enemyID);

			//Debug.Log(enemyID);
			classID = gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].classID;
			//Debug.Log(charID + "class is " + classID);
			tile = gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].tiles;
			

			name = gameData.EnemyCollection[enemyID].name;

			level = gameData.EnemyCollection[enemyID].level;

			hp = gameData.EnemyCollection[enemyID].hp;
			mp = gameData.EnemyCollection[enemyID].mp;
			atk = gameData.EnemyCollection[enemyID].attack;
			def = gameData.EnemyCollection[enemyID].defense;
			res = gameData.EnemyCollection[enemyID].resistance;
			spd = gameData.EnemyCollection[enemyID].speed;

			currentHp = hp;
			currentMp = mp;

			modelID = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].classID].modelId;

		}

		gameManager.assigner.Assign(this, modelID);
		if (isPlayer) trans.eulerAngles = playerRot;
		else trans.eulerAngles = enemyRot;

		//attackInfo = GameObject.FindGameObjectWithTag("Manager").GetComponent<AttackInfoManager>();
		CalculateAttackNumber();

        /*tile.x = PlayerPrefs.GetFloat(charID + "_TileX");
		tile.y = PlayerPrefs.GetFloat(charID + "_TileY");*/
        tileID = Mathf.FloorToInt(tile.y + tile.x* rowSize);
		//Debug.Log(charId + " on " + tile.x + "X" + tile.y + "Y" + " on tile " + tileID + "Class ID" + classID);
        //transform.position = 
    }
    public void UpdateTileID()
    {
		if(alliance == CharacterStats.Alliance.Player)
		{
			tile = gameData.FullFormationsCollection[0].formation[charId].tiles;
		}
		else
		{
			int enemyID;
			enemyID = charId - gameManager.partyMembers;
			tile = gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].tiles;
		}

        tileID = Mathf.FloorToInt(tile.y + tile.x * rowSize);
    }
    public void CalculateAttackNumber()
    {
        maxAttacks = 6;
		//attacksAmount = PlayerPrefs.GetInt(charID + "AtkNum", 1);
		if (alliance == CharacterStats.Alliance.Player)
		{
			attacksAmount = gameData.Party[charId].attackAmount;
			if (attacksAmount > maxAttacks)
			{
				attacksAmount = maxAttacks;
				gameData.Party[charId].attackAmount = maxAttacks;
				PlayerPrefs.SetInt(charId + "AtkNum", maxAttacks);
			}
			attacksLearned = new int[attacksAmount];
			for (int i = 0; i < attacksAmount; i++)
			{
				attacksLearned[i] = gameData.Party[charId].attacksLearned[i];
			}
		}
		else
		{
			int enemyID;
			enemyID = charId - gameManager.partyMembers;
			//Need to fina a way to get their charID in scene
			attacksAmount = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].classID].attackAmount; // insert long ass code here to indicate their actual charID in current scene and relate it to their id in the monster array].attackAmount;
			
			if (attacksAmount > maxAttacks)
			{
				attacksAmount = maxAttacks;

				

				gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].classID].attackAmount = maxAttacks;
			}
			attacksLearned = new int[attacksAmount];
			for (int i = 0; i < attacksAmount; i++)
			{
				attacksLearned[i] = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formation[enemyID].classID].attacksLearned[i];
			}
		}
    }

    public void UseMp(int mpAmount)
    {
		//Debug.Log(charId + "used" + mpAmount + "mp");
		if (currentMp - mpAmount < 0)Debug.Log(charId + "used more MP than it has");
		currentMp -= mpAmount;
		gameManager.battleUI.UpdateLifeBars(gameManager.activeCharacter);
        if(alliance == CharacterStats.Alliance.Player) gameData.Party[charId].currentMp = currentMp;
	}

    public void Damage(int attackPower, int attackerStrength)
    {
		int totalDamage;
		

		if (isDefending)
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - (def * 0.25f) );
		}
		else
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - def*0.5f );
		}
		if (totalDamage < 0) totalDamage = 0;
		currentHp -= totalDamage;

		if (currentHp < 0) currentHp = 0;
		gameManager.battleUI.UpdateLifeBars(charId);
		if (alliance == CharacterStats.Alliance.Player) gameData.Party[charId].currentHp = currentHp;

		//Debug.Log(charId + " has been hit for " + totalDamage + " by combining " + attackPower + "and" + attackerStrength + " leaving it at " + currentHp + " HP");

        if (currentHp <= 0) Die();
        
    }

    public void Die()
    {
        currentHp = 0;
        isDead = true;
		
        gameManager.tileScript.tiles[tileID].isOccupied = false;
        gameObject.SetActive(false);
        if(alliance == CharacterStats.Alliance.Enemy)//if enemy
        {
            //Debug.Log(charId + "died");
			gameManager.battleUI.enemyInfoPopUp[charId - gameManager.partyMembers].gameObject.SetActive(false);
            gameManager.enemyDefeated++;
			/*for (int i = 0; i < gameManager.enemyAmount; i++)
            {
                if(gameManager.charControl[i].isDead == true)
                {
                    
                }
            }*/
			
        }
		
    }
}
