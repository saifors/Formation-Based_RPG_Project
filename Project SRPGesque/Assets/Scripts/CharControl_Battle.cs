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

	public Animator anim;
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
		

		if (isPlayer)
		{
			classID = gameData.FullFormationsCollection[0].formations[charID].classID;
			//tile formation from game data here pls
			tile = gameData.FullFormationsCollection[0].formations[charID].tiles;

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
			classID = gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].classID;
			//Debug.Log(charID + "class is " + classID);
			tile = gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].tiles;
			

			name = gameData.EnemyCollection[classID].name;

			level = gameData.EnemyCollection[classID].level;

			

			hp = gameData.EnemyCollection[classID].hp;
			mp = gameData.EnemyCollection[classID].mp;
			atk = gameData.EnemyCollection[classID].attack;
			def = gameData.EnemyCollection[classID].defense;
			res = gameData.EnemyCollection[classID].resistance;
			spd = gameData.EnemyCollection[classID].speed;

			currentHp = hp;
			currentMp = mp;
			

			modelID = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].classID].modelId;

		}

		

		gameManager.assigner.Assign(this, modelID);
		try
		{
			anim = GetComponentInChildren<Animator>();
			if(isPlayer)anim.Play("BattleIdle");
			//Debug.Log("Success");
		}
		catch
		{
			lacksAnimator = false;
		}
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
			tile = gameData.FullFormationsCollection[0].formations[charId].tiles;
		}
		else
		{
			int enemyID;
			enemyID = charId - gameManager.partyMembers;
			tile = gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].tiles;
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
			attacksAmount = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].classID].attackAmount; // insert long ass code here to indicate their actual charID in current scene and relate it to their id in the monster array].attackAmount;
			
			if (attacksAmount > maxAttacks)
			{
				attacksAmount = maxAttacks;

				

				gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].classID].attackAmount = maxAttacks;
			}
			attacksLearned = new int[attacksAmount];
			for (int i = 0; i < attacksAmount; i++)
			{
				attacksLearned[i] = gameData.EnemyCollection[gameData.FullFormationsCollection[gameManager.enemyGroupID].formations[enemyID].classID].attacksLearned[i];
			}
		}
    }

    public void UseMp(int mpAmount)
    {
		//Debug.Log(charId + "used" + mpAmount + "mp");
		if (currentMp - mpAmount < 0)Debug.Log(charId + "used more MP than it has");
		currentMp -= mpAmount;
		gameManager.battleUI.UpdateMPBar(gameManager.activeCharacter);
        if(alliance == CharacterStats.Alliance.Player) gameData.Party[charId].currentMp = currentMp;
	}

    public void Damage(int attackPower, int attackerStrength)
    {
		int totalDamage;

		int defResStat;
		if (!gameManager.gameData.AttackList[gameManager.currentAttack].isMagic)
		{
			defResStat = def;
		}
		else
		{
			defResStat = res;
		}

			if (isDefending)
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - (defResStat * 0.25f) );
		}
		else
		{
			totalDamage = Mathf.FloorToInt( (attackPower + attackerStrength)/2 - defResStat * 0.5f );
		}
		if (totalDamage < 0) totalDamage = 0;
		currentHp -= totalDamage;

		if (currentHp < 0) currentHp = 0;
		gameManager.battleUI.UpdateHPBar(charId);
		if (alliance == CharacterStats.Alliance.Player) gameData.Party[charId].currentHp = currentHp;
		HurtAnim();

		//Debug.Log(charId + " has been hit for " + totalDamage + " by combining " + attackPower + "and" + attackerStrength + " leaving it at " + currentHp + " HP");
		
        
    }

	public void Heal(Recovery.RecoveryType type, int healValue)
	{
		if ( ( (type == Recovery.RecoveryType.FixedHeal || type == Recovery.RecoveryType.PercentHeal) && currentHp < hp) || ((type == Recovery.RecoveryType.FixedRecover || type == Recovery.RecoveryType.PercentRecover) && currentMp < mp) )
		{

			Debug.Log(charId + " healed to " + currentHp + healValue);
			gameManager.battleUI.itemMenu.SetActive(false);
			gameManager.battleUI.partyInfo.SetActive(true);
			gameManager.soundPlayer.PlaySound(0, true);

			GameObject particle = Instantiate(gameManager.battleAnim.particleAnim[1]);
			SpellAnim partSpell = particle.GetComponent<SpellAnim>();
			partSpell.Init();
			partSpell.trans.position = trans.position;
			Destroy(particle, partSpell.particles[0].startLifetime + partSpell.particles[0].duration + 0.1f);

			Recovery.Recover(type, healValue, this);
			gameManager.battleUI.UpdateLifeBars(charId);
			if (alliance == CharacterStats.Alliance.Player) gameData.Party[charId].currentHp = currentHp;

			if(gameManager.selecting == GameManager.SelectingMenu.selectingItem) gameManager.EndItem();
			gameManager.selecting = GameManager.SelectingMenu.waiting;
			gameManager.soundPlayer.PlaySound(6, true);
		}
		else
		{
			//That's a bad idea lads.
			gameManager.soundPlayer.PlaySound(2, true);
		}

		
	}

	public void DeathCheck()
	{
		if (currentHp <= 0)
		{
			Die();
			/*try
			{
				anim.Play("Fade");
				StartCoroutine(WaitForFade());
			}
			catch
			{
				Die();
			}*/
		}
		gameManager.battleUI.finishedHPBarAnim = true;
	}

    public void Die()
    {
        
		currentHp = 0;
        isDead = true;
		
        gameManager.tileScript.tiles[tileID].isOccupied = false;
        gameObject.SetActive(false);
        if(alliance == CharacterStats.Alliance.Enemy)//if enemy
        {
            Debug.Log(charId + "died");
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

	public Animator CastAnim()
	{
		

		try
		{
			if (alliance == CharacterStats.Alliance.Player)
			{
				if (gameData.AttackList[gameManager.currentAttack].isMagic) anim.Play("Cast");
				else anim.Play("Attack");
				return anim;
			}
			else return null;
		}
		catch
		{

			return null;
		}
	}
	public void HurtAnim()
	{
		/*try
		{
			anim.Play("Hurt");
			StartCoroutine(WaitForAnim());
		}
		catch
		{ */
			//Debug.Log("No hurt anim found");
			Invoke("EndHurtAnim", 0.2f);
		/*}*/
	}

	public void EndHurtAnim()
	{
		gameManager.FinishHitAttack();
	}

	IEnumerator WaitForAnim()
	{
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 0.1f);
		EndHurtAnim();
	}
	IEnumerator WaitForFade()
	{
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 0.25f);
		Die();
	}
}
