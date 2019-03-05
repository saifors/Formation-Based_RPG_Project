using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour 
{
	public int charId;
    public CharacterStats.Alliance alliance;

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

    public bool isDead;

    public Vector2 tile;
	public int tileID;
    public int rowSize;

	public EnemyAI ai;

	// Use this for initialization
	void Start () 
	{
        
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
			//Debug.Log(gameManager.gameData.Party[charID].level);
			level = gameManager.gameData.Party[charID].level;

			hp = gameManager.gameData.Party[charID].hp;
			mp = gameManager.gameData.Party[charID].mp;
			atk = gameManager.gameData.Party[charID].attack;
			def = gameManager.gameData.Party[charID].defense;
			res = gameManager.gameData.Party[charID].resistance;
			spd = gameManager.gameData.Party[charID].speed;

			currentHp = gameManager.gameData.Party[charID].currentHp;
			currentMp = gameManager.gameData.Party[charID].currentMp;
			if (currentHp > hp) currentHp = hp;
			if (currentMp > mp) currentMp = mp;

			//tile formation from game data here pls
		}
		else
		{
			level = gameManager.gameData.EnemyCollection[charID].level;

			hp = gameManager.gameData.EnemyCollection[charID].hp;
			mp = gameManager.gameData.EnemyCollection[charID].mp;
			atk = gameManager.gameData.EnemyCollection[charID].attack;
			def = gameManager.gameData.EnemyCollection[charID].defense;
			res = gameManager.gameData.EnemyCollection[charID].resistance;
			spd = gameManager.gameData.EnemyCollection[charID].speed;

			currentHp = gameManager.gameData.Party[charID].currentHp;
			currentMp = gameManager.gameData.Party[charID].currentMp;
			if (currentHp > hp) currentHp = hp;
			if (currentMp > mp) currentMp = mp;
		}

        //attackInfo = GameObject.FindGameObjectWithTag("Manager").GetComponent<AttackInfoManager>();
        CalculateAttackNumber();

        tile.x = PlayerPrefs.GetFloat(charID + "_TileX");
		tile.y = PlayerPrefs.GetFloat(charID + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x* rowSize);
        //transform.position = 
    }
    public void UpdateTileID()
    {
		if(alliance == CharacterStats.Alliance.Player)
		{
			tile = gameManager.gameData.Formation[charId].tiles;
		}
		else
		{
			tile.x = PlayerPrefs.GetFloat(charId + "_TileX");
			tile.y = PlayerPrefs.GetFloat(charId + "_TileY");
		}

        tileID = Mathf.FloorToInt(tile.y + tile.x * rowSize);
    }
    public void CalculateAttackNumber()
    {
        maxAttacks = 6;
		//attacksAmount = PlayerPrefs.GetInt(charID + "AtkNum", 1);
		if (alliance == CharacterStats.Alliance.Player)
		{
			attacksAmount = gameManager.gameData.Party[charId].attackAmount;
			if (attacksAmount > maxAttacks) attacksAmount = maxAttacks; PlayerPrefs.SetInt(charId + "AtkNum", maxAttacks);
			attacksLearned = new int[attacksAmount];
			for (int i = 0; i < attacksAmount; i++)
			{
				attacksLearned[i] = gameManager.gameData.Party[charId].attacksLearned[i];
			}
		}
		else
		{
			//Need to fina a way to get their charID in scene
			/*attacksAmount = gameManager.gameData.EnemyCollection[ insert long ass code here to indicate their actual charID in current scene and relate it to their id in the monster array].attackAmount;
			if (attacksAmount > maxAttacks) attacksAmount = maxAttacks; PlayerPrefs.SetInt(charId + "AtkNum", maxAttacks);
			attacksLearned = new int[attacksAmount];
			for (int i = 0; i < attacksAmount; i++)
			{
				attacksLearned[i] = gameManager.gameData.EnemyCollection[charId].attacksLearned[i];
			}*/
		}
    }

    public void UseMp(int mpAmount)
    {
		Debug.Log(charId + "used" + mpAmount + "mp");
		if (currentMp - mpAmount < 0)Debug.Log(charId + "used more MP than it has");
		currentMp -= mpAmount;
		gameManager.battleUI.UpdateLifeBars(gameManager.activeCharacter);
        //if(alliance == 0) gameManager.battleUI.UpdatePlayerBars(charId);
    }

    public void Damage(int attackPower, int attackerStrength)
    {
		int totalDamage;
		

		if (isDefending)
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - (def * 1.5f) );
		}
		else
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - def );
		}
		currentHp -= totalDamage;

		if (currentHp < 0) currentHp = 0;
		gameManager.battleUI.UpdateLifeBars(charId);

		Debug.Log(charId + " has been hit for " + totalDamage + " leaving it at " + currentHp + " HP");

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
            gameManager.battleUI.enemyInfoPopUp[charId - gameManager.partyMembers].gameObject.SetActive(false);
            for (int i = 0; i < gameManager.enemyAmount; i++)
            {
                if(gameManager.charControl[i].isDead == true)
                {
                    gameManager.enemyDefeated++;
                }
            }
        }

    }
}
