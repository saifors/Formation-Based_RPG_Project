using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControl_Battle : MonoBehaviour 
{
	public int charId;
    public int alliance;
    public string allianceString;
    public int currentHp;
    public int currentMp;
    public int hp;
	public int mp;
	public int atk;
	public int def;
	public int res;
	public int spd;

    public int[] attacksLearned;
    public int attacksAmount; //How many attacks does this character have.
    public int maxAttacks = 6;
    public AttackInfoManager attackInfo;

    public Vector2 tile;
	public int tileID;
    public int rowSize;

	// Use this for initialization
	void Start () 
	{
        
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(int charID, bool isPlayer)
	{
        if(isPlayer)
        {
            alliance = 0;
            allianceString = "Player";
        }
        else
        {
            alliance = 1;
            allianceString = "Enemy";
        }
        hp = PlayerPrefs.GetInt(allianceString + charID + "Hp");
		mp = PlayerPrefs.GetInt(allianceString + charID + "Mp");
		atk = PlayerPrefs.GetInt(allianceString + charID + "Attack");
		def = PlayerPrefs.GetInt(allianceString + charID + "Defense");
		res = PlayerPrefs.GetInt(allianceString + charID + "Resistance");
		spd = PlayerPrefs.GetInt(allianceString + charID + "Speed");

        currentHp = PlayerPrefs.GetInt(allianceString + charID + "Current Hp");
        currentMp = PlayerPrefs.GetInt(allianceString + charID + "Current Mp");
        if (currentHp > hp) currentHp = hp;
        if (currentMp > mp) currentMp = mp;

        attackInfo = GameObject.FindGameObjectWithTag("Manager").GetComponent<AttackInfoManager>();
        CalculateAttackNumber(charID);

        tile.x = PlayerPrefs.GetFloat(allianceString + charID + "_TileX");
		tile.y = PlayerPrefs.GetFloat(allianceString + charID + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x* rowSize);
        //transform.position = 
    }
    public void UpdateTileID(string allianceString)
    {
        tile.x = PlayerPrefs.GetFloat(allianceString + charId + "_TileX");
        tile.y = PlayerPrefs.GetFloat(allianceString + charId + "_TileY");
        tileID = Mathf.FloorToInt(tile.y + tile.x * rowSize);
    }
    public void CalculateAttackNumber(int charID)
    {
        maxAttacks = 6;
        //attacksAmount = PlayerPrefs.GetInt(charID + "AtkNum", 1);
		attacksAmount = 3; //PLACEHOLDER
        if (attacksAmount > maxAttacks) attacksAmount = maxAttacks; PlayerPrefs.SetInt(allianceString + charID + "AtkNum", maxAttacks);
        attacksLearned = new int[attacksAmount];
        attacksLearned[0] = 0;//Placheolders
        attacksLearned[1] = 1;
        attacksLearned[2] = 2;
    }
}
