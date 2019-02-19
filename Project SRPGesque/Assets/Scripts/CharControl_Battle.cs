using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour 
{
	public int charId;
    public CharacterStats.Alliance alliance;

    public int level;
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
    public AttackInfoManager attackInfo;

    public GameManager gameManager;

    public bool isDead;

    public Vector2 tile;
	public int tileID;
    public int rowSize;

	public EnemyAI ai;

	// Use this for initialization
	void Start () 
	{
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        
        
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

        level = PlayerPrefs.GetInt(charID + "Level");

        hp = PlayerPrefs.GetInt(charID + "Hp");
		mp = PlayerPrefs.GetInt(charID + "Mp");
		atk = PlayerPrefs.GetInt(charID + "Attack");
		def = PlayerPrefs.GetInt(charID + "Defense");
		res = PlayerPrefs.GetInt(charID + "Resistance");
		spd = PlayerPrefs.GetInt(charID + "Speed");

        currentHp = PlayerPrefs.GetInt(charID + "Current Hp");
        currentMp = PlayerPrefs.GetInt(charID + "Current Mp");
        if (currentHp > hp) currentHp = hp;
        if (currentMp > mp) currentMp = mp;

        attackInfo = GameObject.FindGameObjectWithTag("Manager").GetComponent<AttackInfoManager>();
        CalculateAttackNumber(charID);

        tile.x = PlayerPrefs.GetFloat(charID + "_TileX");
		tile.y = PlayerPrefs.GetFloat(charID + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x* rowSize);
        //transform.position = 
    }
    public void UpdateTileID()
    {
		
		tile.x = PlayerPrefs.GetFloat(charId + "_TileX");
        tile.y = PlayerPrefs.GetFloat(charId + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x * rowSize);
    }
    public void CalculateAttackNumber(int charID)
    {
        maxAttacks = 6;
        //attacksAmount = PlayerPrefs.GetInt(charID + "AtkNum", 1);
		attacksAmount = 3; //PLACEHOLDER
        if (attacksAmount > maxAttacks) attacksAmount = maxAttacks; PlayerPrefs.SetInt(charID + "AtkNum", maxAttacks);
        attacksLearned = new int[attacksAmount];
        attacksLearned[0] = 0;//Placheolders
        attacksLearned[1] = 1;
        attacksLearned[2] = 2;
    }

    public void UseMp(int mpAmount)
    {
        currentMp -= mpAmount;
		gameManager.battleUI.UpdateLifeBars(gameManager.activeCharacter);
        //if(alliance == 0) gameManager.battleUI.UpdatePlayerBars(charId);
    }

    public void Damage(int attackPower, int attackerStrength)
    {
		int totalDamage;
		Debug.Log(charId + "ow");

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
            if(gameManager.enemyDefeated >= gameManager.enemyAmount)
            {
                gameManager.Victory();
            }
        }
		else
		{
			gameManager.GameOverCheck();
		}

    }
}
