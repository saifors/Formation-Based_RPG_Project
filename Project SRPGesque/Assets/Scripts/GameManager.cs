using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	int tempBattleCounter = 0;

	//Debug
	public bool debug;
	public GameObject debugMenu;
	//Basics
	private float timeCounter;
	private Vector2 axis;
	private Transform cam_T;
	private OWPlayerController playerController;
	public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam }; public CameraSetting camSet;
	public Camera cam;
	public enum GameState { Overworld, Battle, GameMenu, Text, Event };
	public GameState gameState;
	public TransitionManager transition;
	public EventManager eventManager;

	//Encounters
	[Header("Encounters")]
	public bool randomEcountersOn;
	public float encounterMinimumPercent = 100;
	private EncounterMapID encounterSpecs;
	public int currentEncounterMap;

	public bool specifiedEncounter;
	public int specEncounterID;

	[Header("User Interface")]
	//Battle Interface (Canvas)    
	public GameObject battleMenu;
	public BattleUI battleUI;
	public enum SelectingMenu { selectingAction, selectingAttack, selectingTarget, selectingMove, selectingItem, victoryScreen, enemyTurn, attacking, waiting };
	public SelectingMenu selecting;
	public DialogueBox dialogueUI;
	private GameObject dialogueObj;
	public int levelUpScreenProgress;

	public NotificationBox notifBox;

	//[HideInInspector] public AttackInfoManager attackInfo;
	[Header("Characters & Enemies")]
	public GameObject battleCharacterPrefab;
	public CharControl_Battle[] charControl;
	//Playable Characters: Amount, Object and Controller Arrays, Active/Currently Selected, 
	public int partyMembers;
	public GameObject[] characters;

	public int activeCharacter;

	//Enemy Characters: Current Amount and start of battle Amount, Object and Controller Arrays, Check Amount alive.
	public int initialEnemyAmount;
	public int enemyAmount;
	public int enemyGroupID;
	public int enemyDefeated;
	public int[] possibleEncounters;

	[Header("Battle")]
	//Battle
	public ModelAssigner assigner;
	public BattleAnimation battleAnim;
	public Vector3 battleCam;

	public int totalGold;
	public int totalExp;
	public List<int> allItems;

	//Battle - Move
	public GameObject cursor; //Prefab
	private float tileSelectCooldownCounter;

	//Battle - Attack
	public GameObject targetCursor; //Prefab
	public int currentAttack;
	public Vector2 targetMargin;
	[Header("Tiles")]
	//Tiles
	[HideInInspector] public TileScript tileScript;
	public int tileAmount;
	public Vector2 tileVectorSize;
	public Vector2 tileSelection;
	public int selectedTile; // looks at tileSelection and gets the Id from those
	public Vector2[] selectionLimit = new Vector2[4]; //Start Limit and End Limit for Move and for Target.

	private Transform selectedTileIndicator;

	[Header("Attack Target")]
	//Target
	public int targetAmount; //How many Tiles are in the range of the attack
	public Vector2 targetOrigin;
	public int[] selectedTargets; // All tileIDs affected by current attack;
	public Vector2[] selectedTargetVector; //X and Y of all tiles in range
	public Transform[] selectedTargetsTransform;

	public int targetEnemies;
	public int targetsHit;

	public GameObject confirmTargetFX;

	[Header("Turn System")]
	//Turn System
	public int[] turnOrder; //Takes in Combined character Ids of both player and enemy, length will depend on playerAmount, enemy amount and the speed of each and every one.
	public int turnCounter; //Counter of turns inside a phase of turns. a turn phase keeps repeating whenever it ends. 
	private int turnAmount;
	private int[] characterSpeeds; //Store speed of each charID
	private int[] charactersWithExtraTurn;

	[Header("Miscelaneous")]
	// Pause Stuff
	public bool isPaused;
	public GameObject pausePanel;
	public PauseMenuScript pauseMenu;
	//Location of the where battles take place
	private Transform battlefield;

	//Audio
	public SoundPlayer soundPlayer;

	//Game Data 
	public GameData gameData;
	public string fileName;
	public string cacheName = "spelQuick.od";

	//Start
	// Use this for initialization
	void Start()
	{
		gameData = GameDataManager.Load(PlayerPrefs.GetString("CurrentFile"));

		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();

		LoadMiscData();
		playerController.transform.position = gameData.Misc.pos;
		playerController.transform.localRotation = gameData.Misc.rot;
		//Debug.Log("Start Event Trigger " + gameData.EventCollection[0].hasBeenTriggered);

		LanguageManager.LoadLanguage();


		gameState = GameState.Overworld;
		randomEcountersOn = true;//Depending on the area. Maybe a scene database indicating whether true or false?.

		partyMembers = 3; //PlayerPrefs.GetInt("PartyMembers", 3);

		cam = Camera.main;

		transition = GetComponent<TransitionManager>();
		transition.gameData = gameData;

		//Initialization of Objects
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();

		cam_T.position = playerController.trans.position;

		battleUI = GameObject.FindGameObjectWithTag("UI").GetComponent<BattleUI>();
		dialogueUI = battleUI.GetComponentInChildren<DialogueBox>();
		dialogueUI.Init(this);
		dialogueObj = dialogueUI.gameObject;
		dialogueObj.SetActive(false);
		notifBox = battleUI.GetComponentInChildren<NotificationBox>();
		notifBox.Init(this);

		
		tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
		tileScript.Init();
		battlefield = GameObject.FindGameObjectWithTag("Battlefield").GetComponent<Transform>();
		encounterSpecs = battlefield.GetComponent<EncounterMapID>();
		assigner = GetComponent<ModelAssigner>();
		battleAnim = GetComponent<BattleAnimation>();

		battleCam = battlefield.position + new Vector3(0.6f, 0, 0.4f);

		//Databases.
		currentEncounterMap = encounterSpecs.encounterID;
		possibleEncounters = gameData.MapEncounterCollection[currentEncounterMap].groupIDs;
		specifiedEncounter = false;

		soundPlayer = gameObject.GetComponent<SoundPlayer>();
		soundPlayer.Init(this, true);

		//Create a cursor for Formation Movement
		GameObject objCursor;
		objCursor = Instantiate(cursor);
		selectedTileIndicator = objCursor.GetComponent<Transform>();
		selectedTileIndicator.gameObject.SetActive(false);

		//For Movement Limit calculations
		tileVectorSize.x = tileScript.xTiles;
		tileVectorSize.y = tileScript.yTiles;
		tileAmount = tileScript.tileAmount - 1;
		selectionLimit[0] = new Vector2(tileVectorSize.x / 2, 0);
		selectionLimit[1] = new Vector2(tileVectorSize.x, tileVectorSize.y);
		selectionLimit[2] = new Vector2(0, 0);
		selectionLimit[3] = new Vector2(tileVectorSize.x / 2, tileVectorSize.y);

		eventManager = GetComponent<EventManager>();
		eventManager.Init(this, playerController);
		
		//MoveFormation(0, charControl[0].tile); //PLACEHOLDER
		battleUI.Init();

		

		pauseMenu = pausePanel.GetComponent<PauseMenuScript>();
		pauseMenu.Init(this);

		for (int i = 0; i < partyMembers; i++)
		{
			UpdateStats(i);
		}

		PlayOverworldMusic();
	}

	// Update
	void Update()
	{
		gameData.Misc.playtimeSeconds += Time.deltaTime;


		if (isPaused) return;
		

		if (gameState == GameState.Overworld)
		{

			{
				if (camSet == CameraSetting.OverworldCam)
				{
					if (playerController.isMoving)
					{
						cam_T.position = playerController.trans.position;
					}
				}
				if (randomEcountersOn)
				{
					if (playerController.isMoving)
					{
						timeCounter += Time.deltaTime;
						if (timeCounter >= 2.5f)
						{
							encounterMinimumPercent -= 2.5f;

							if (UnityEngine.Random.Range(0, 100) >= encounterMinimumPercent)
							{
								EncounterAnim();
								encounterMinimumPercent = 100;
							}
							timeCounter = 0;
						}


					}
				}
			}

			if (debug)
			{

			}
		}
		if (gameState == GameState.Battle)
		{
			if (selecting == SelectingMenu.selectingMove)
			{
				if (tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;
				//Controls for moving around the selected tiles.
				if (tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
				{

					FormationMovement();
				}
			}
			else if (selecting == SelectingMenu.selectingTarget)
			{
				if (tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;

				if (tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
				{
					AttackTargetMovement();

				}
			}
			else if (selecting == SelectingMenu.enemyTurn)
			{
				//Enemy AI
			}

			if (debug)
			{

			}
		}


	}
	
	

	public void AddMiscToGameData()
	{
		
		gameData.Misc.pos = playerController.trans.position;
		gameData.Misc.rot = playerController.trans.rotation;
	}

	public void LoadMiscData()
	{
		if (!gameData.Misc.newGame)
		{
			playerController.transform.position = gameData.Misc.pos;
			playerController.transform.localRotation = gameData.Misc.rot;
		}
		else
		{
			Debug.Log("started a new game");
			gameData.Misc.newGame = false;
		}
	}
	
	public void UpdateStats(int characterID)
	{
		int lvMultiplier = gameData.Party[characterID].level - 1;
		
		//Something is going wrong here, even when the result of growth and lvmultiplier should be higher.
		gameData.Party[characterID].hp = gameData.CharStats[characterID].hpBase + Mathf.FloorToInt(( (float)gameData.CharStats[characterID].hpGrowth / 100f) * lvMultiplier);
		gameData.Party[characterID].mp = gameData.CharStats[characterID].mpBase + Mathf.FloorToInt(( (float)gameData.CharStats[characterID].mpGrowth / 100) * lvMultiplier);
		gameData.Party[characterID].attack = gameData.CharStats[characterID].attackBase + Mathf.FloorToInt(((float)gameData.CharStats[characterID].attackGrowth / 100f) * lvMultiplier);
		gameData.Party[characterID].defense = gameData.CharStats[characterID].defenseBase + Mathf.FloorToInt(((float)gameData.CharStats[characterID].defenseGrowth / 100f) * lvMultiplier);
		gameData.Party[characterID].resistance = gameData.CharStats[characterID].resistanceBase + Mathf.FloorToInt(((float)gameData.CharStats[characterID].resistanceGrowth / 100f) * lvMultiplier);
		gameData.Party[characterID].speed = gameData.CharStats[characterID].speedBase + Mathf.FloorToInt(((float)gameData.CharStats[characterID].speedGrowth / 100f) * lvMultiplier);

	}
	

	//Input
	public void ConfirmSelectedCommand()
	{
		soundPlayer.PlaySound(0, true);
		if (battleUI.command == BattleUI.CommandSelection.Attack)
		{
			battleUI.InitiateAttackSelection();

		}
		else if (battleUI.command == BattleUI.CommandSelection.Defend)
		{
			//Go to Select defend
			Defend();
		}
		else if (battleUI.command == BattleUI.CommandSelection.Move)
		{
			//Go to move menu
			selecting = SelectingMenu.selectingMove;

			tileSelection = charControl[activeCharacter].tile; //0 is a placeholder for now Will be replaced with an Int indicating the active character when that's a thing            
			selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileVectorSize.y));
			selectedTileIndicator.gameObject.SetActive(true);
			selectedTileIndicator.position = tileScript.tileTransform[selectedTile].position;
			battleUI.actionMenu.SetActive(false);
			//gameManager.MoveFormation(0,5);
		}
		else if (battleUI.command == BattleUI.CommandSelection.Item)
		{
			//Go to Item Menu
			Item();
		}
		else if (battleUI.command == BattleUI.CommandSelection.Run)
		{
			//Execute Running away
			RunFromBattle();
		}
		else
		{
			//Debug.Log("Tried to confirm an Action command selection but nothing was selected");
		}
	}
	public void ReturnToCommandSelection()
	{
		soundPlayer.PlaySound(1, true);
		selecting = SelectingMenu.selectingAction;
		selectedTileIndicator.gameObject.SetActive(false);
		battleUI.attackMenu.SetActive(false);
		battleUI.itemMenu.SetActive(false);
		battleUI.partyInfo.SetActive(true);
		battleUI.actionMenu.SetActive(true);
	}
	public void ConfirmAttackSelection()
	{
		//This is all going to change once it's an array of course
		soundPlayer.PlaySound(0, true);
		GameObject[] objTarget;

		currentAttack = charControl[activeCharacter].attacksLearned[battleUI.attackOptionSelected];
		//Debug.Log("Attack " + currentAttack);

		CalculateTargetAmount();

		objTarget = new GameObject[targetAmount];
		selectedTargetsTransform = new Transform[targetAmount];

		for (int i = 0; i < targetAmount; i++)
		{
			objTarget[i] = Instantiate(targetCursor);
			objTarget[i].name = "TargetCursor_" + i;
			selectedTargetsTransform[i] = objTarget[i].GetComponent<Transform>();
		}
		targetOrigin.x = 0; //row
		targetOrigin.y = 0; //column

		TargetPlacement();
		TargetPlacementVisuals();


		//selectedTargetVector[0] = Vector2.zero;
		//selectedTarget[i] = selectedTargetVector[i].x + selectedTargetVector[i].y;
		//selectedTargetsTransform[i].gameObject.SetActive(true);
		//selectedTargetsTransform[i].position = gameManager.tileScript.tileTransform[selectedTarget[i]].position;
		battleUI.attackMenu.SetActive(false);
		selecting = SelectingMenu.selectingTarget;
	}

	public void ReturnToAttackSelect()
	{
		soundPlayer.PlaySound(1, true);

		for (int i = 0; i < targetAmount; i++) //Eliminate the target cursors.
		{
			Destroy(selectedTargetsTransform[i].gameObject);
		}
		selectedTargetsTransform = new Transform[0];

		battleUI.InitiateAttackSelection();
	}

	public void ConfirmItemSelection()
	{
		//Use the item selected
		UseItem();
	}

	public void SetAxis(Vector2 inputAxis)
	{
		axis = inputAxis;
	}

	//Attack		
	public void StartAttack()
	{
		if (charControl[activeCharacter].currentMp - gameData.AttackList[charControl[activeCharacter].attacksLearned[battleUI.attackOptionSelected]].mpCost < 0)
		{
			battleUI.ChangeNotifText("Not enough MP.");
			soundPlayer.PlaySound(2, true);

			return;
		}

		//Debug.Log("Start Attack");

		

		for (int i = 0; i < selectedTargets.Length; i++)
		{
			

			if (tileScript.tiles[selectedTargets[i]].isOccupied)
			{
				battleUI.partyInfo.SetActive(true);
				selecting = SelectingMenu.attacking;
				LaunchAttack();
				break;
			}
			else if (i >= selectedTargets.Length - 1) soundPlayer.PlaySound(2, true);
		}

		/*if(tileScript.tiles[selectedTarget].isOccupied)
        {
            soundPlayer.PlaySound(0,1, true);
            for (int i = 0; i < enemyAmount; i++) //4 is a placeholder for enemy amount
            {
                if(enemyControl[i].tileID == selectedTarget[x])
                {
                    enemyControl[i].Damage(attackInfo.attackStrengths[charControl[activeCharacter].attacksLearned[battleUI.attackOptionSelected]]);
                        
                }

            }
            //gameManager.charControl[gameManager.activeCharacter].UseMp(gameManager.attackInfo.attackMpCosts[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attackOptionSelected]]);
        }
        else soundPlayer.PlaySound(2,1, true);
        */
	}
	public void LaunchAttack()
	{
		//Debug.Log("Attack Launched");
		battleUI.actionMenu.SetActive(false);
		if (charControl[activeCharacter].alliance == CharacterStats.Alliance.Player)
		{
			charControl[activeCharacter].UseMp(gameData.AttackList[charControl[activeCharacter].attacksLearned[battleUI.attackOptionSelected]].mpCost);
			soundPlayer.PlaySound(0, true);
		}
		else if (charControl[activeCharacter].alliance == CharacterStats.Alliance.Enemy) charControl[activeCharacter].UseMp(gameData.AttackList[charControl[activeCharacter].ai.currentAttack].mpCost);

		soundPlayer.PlaySound(6, true, 0.75f);

		for (int i = 0; i < selectedTargets.Length; i++)
		{
			GameObject obj = Instantiate(confirmTargetFX);
			obj.transform.position = tileScript.tiles[selectedTargets[i]].transform.position;
			Destroy(obj, 1);
		}

		/*for (int i = 0; i < selectedTargets.Length; i++)
		{
			Debug.Log(activeCharacter + " launches attack on " + selectedTargets[]);
		}*/

		battleAnim.LaunchAttackAnim();
		//Character attack animation
		//charControl[activeCharacter].anim.Play(attackAnimation);
		//Play Animation on all targets
		//Instantiate(AttackEffect).Play
		//When animation has finished:
		//if(AttackEffect finished)

	}
	public void HitAttack() //For some reason won't work on enemy turn
	{
		
		targetEnemies = 0;
		targetsHit = 0;
		for (int target = 0; target < selectedTargets.Length; target++)
		{
			//Debug.Log(activeCharacter + "goes through target" + target + "which has tile ID of " + selectedTargets[target] + "or" + tileScript.tiles[selectedTargets[target]].tileID);
			if (tileScript.tiles[selectedTargets[target]].isOccupied)
			{

				//Debug.Log(activeCharacter + "goes through all the character on target tile" + tileScript.tiles[selectedTargets[target]].tileID);
				for (int chara = 0; chara < partyMembers + enemyAmount; chara++)
				{
					if (charControl[chara].tileID == tileScript.tiles[selectedTargets[target]].tileID)
					{
						//Debug.Log(charControl[chara].charId + "Has been hit");
						charControl[chara].Damage(gameData.AttackList[currentAttack].strength, charControl[activeCharacter].atk);

						targetEnemies++;
					}
					//else Debug.Log(tileScript.tiles[selectedTargets[target]].tileID + "is target but character" + chara + "is on" + charControl[chara].tileID);
				}
			}

		}
		//Debug.Log("HitAttack on " + targetEnemies + " targets");



		//battleAnim.HitAnim(animes[longest]);

	}

	public void FinishHitAttack()
	{
		targetsHit++;
		if(targetsHit >= targetEnemies)
		{
			//Debug.Log("All targets have Finished their hurt Animation");
			battleAnim.FinishHit();
			targetsHit = 0;
		}
	}



	//Defend
	public void Defend()
	{
		charControl[activeCharacter].isDefending = true;
		//Change To Defend idle?
		battleUI.ChangeNotifText(charControl[activeCharacter].name + " takes a defensive posture!");

		StartCoroutine(WaitForDefend());
	}
	IEnumerator WaitForDefend()
	{
		selecting = SelectingMenu.waiting;
		yield return new WaitForSeconds(1);
		EndTurn();
	}

	//Item
	public void Item()
	{
		selecting = SelectingMenu.selectingItem;
		battleUI.OpenItemMenu();
	}
	public void UseItem()
	{
		int currentItem = gameData.ItemInventory[battleUI.itemBox.selected].itemId;
		
		switch (gameData.ItemsCollection[currentItem].effect)
		{
			case ItemData.ItemEffect.Heal20_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedHeal, 20);
				break;
			case ItemData.ItemEffect.Heal50_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedHeal, 50);
				break;
			case ItemData.ItemEffect.Heal100_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedHeal, 100);
				break;
			case ItemData.ItemEffect.Heal200_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedHeal, 200);
				break;
			case ItemData.ItemEffect.Heal500_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedHeal, 500);
				break;
			case ItemData.ItemEffect.Heal50_P:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.PercentHeal, 50);
				break;
			case ItemData.ItemEffect.Heal100_P:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.PercentHeal, 100);
				break;
			case ItemData.ItemEffect.Recover50_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedRecover, 50);
				break;
			case ItemData.ItemEffect.Recover100_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedRecover, 100);
				break;
			case ItemData.ItemEffect.Recover300_F:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.FixedRecover, 300);
				break;
			case ItemData.ItemEffect.Recover50_P:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.PercentRecover, 50);
				break;
			case ItemData.ItemEffect.Recover100_P:
				charControl[activeCharacter].Heal(Recovery.RecoveryType.PercentRecover, 100);
				break;
			case ItemData.ItemEffect.CurePoison:
				break;
			default:
				break;
		}
	}
	public void EndItem()
	{
		List<InventoryData> ItemL = new List<InventoryData>(gameData.ItemInventory);

		ItemL[battleUI.itemBox.selected].amount--;
		if(ItemL[battleUI.itemBox.selected].amount <= 0)
		{
			ItemL.Remove(gameData.ItemInventory[battleUI.itemBox.selected]);
		}
		gameData.ItemInventory = ItemL.ToArray();
		battleUI.itemBox.DestroyItemText();
		Invoke("EndTurn", 2);
	}
	public void FullHeal(int charaID)
	{
		gameData.Party[charaID].currentHp = gameData.Party[charaID].hp;
		gameData.Party[charaID].currentMp = gameData.Party[charaID].mp;
	}
	//----------------------------------Turn System--------------------------------------------
	public void CalculateTurnOrderInPhase()
	{		
		turnAmount = partyMembers + enemyAmount;
		turnOrder = new int[turnAmount];

		characterSpeeds = new int[partyMembers + enemyAmount];
		for (int i = 0; i < characterSpeeds.Length; i++)
		{
			characterSpeeds[i] = charControl[i].spd;
		}

		//TO FIX: For some reason when the fastest character of all is the same as the local int fastestIncChache all turns become theirs.

		bool[] assigned = new bool[characterSpeeds.Length];
		for (int i = 0; i < assigned.Length; i++) assigned[i] = false;

		for (int turnI = 0; turnI < turnOrder.Length; turnI++) //Initial turn order calculation
		{
			int fastestInCache = -1;
			for (int i = 0; i < assigned.Length; i++)
			{
				if (!assigned[i]) // Prevents previously assigned characters to be put as the default fastestInCache
				{
					fastestInCache = i;
					break;
				}
			}

			for (int character = 1; character < characterSpeeds.Length; character++)//Is this the chracter that has this turn in turn order?
			{
				//Somewhere around here check whther current character is = fastestInCache?
				
				//is this character the fastest from the characters that haven´t been assigned to a previous turn?
				if(!assigned[character]) //if this characters hasn't been assigned to an earlier turn
				{
					if (characterSpeeds[character] > characterSpeeds[fastestInCache])
					{
						fastestInCache = character;	
					}
				}		
			}
			turnOrder[turnI] = fastestInCache;
			assigned[fastestInCache] = true;
		}		
	}

	public void NextTurn()
	{
		turnCounter++;
		if (turnCounter >= turnAmount) turnCounter = 0;

		if (!charControl[turnOrder[turnCounter]].isDead) StartTurn();
		else NextTurn();
	}

	public void StartTurn()
	{
		activeCharacter = turnOrder[turnCounter];

		battleUI.TurnActiveVisuals(activeCharacter);
		//Debug.Log(activeCharacter + " has started it's turn");
		selecting = SelectingMenu.waiting;
		if(charControl[activeCharacter].alliance == CharacterStats.Alliance.Enemy)
		{
			battleUI.EnemyTurnUIChange();
		}
		else
		{
			battleUI.PlayerTurnUIChange();
		}

	}
	public void EndTurn()
	{
		//Debug.Log(activeCharacter + " has ended their turn");
		for (int  i = 0; i < selectedTargetsTransform.Length; i++)
		{
			Destroy(selectedTargetsTransform[i].gameObject);
		}
		selectedTargetsTransform = new Transform[0];

		if (enemyDefeated >= enemyAmount);

		GameOverCheck();

		if(enemyDefeated >= enemyAmount)
		{
			Victory();
			return;
		}

		NextTurn();
	}
	public void FinishedTurnAnim()
	{
		charControl[activeCharacter].MyTurn();
		if (charControl[activeCharacter].alliance == CharacterStats.Alliance.Enemy)
		{
			selecting = SelectingMenu.enemyTurn;
		}
		else
		{
			selecting = SelectingMenu.selectingAction;
		}
	}

	//-------------------------Tile Selection & targets----------------------
	#region Tile and Target Movement
	public void FormationMovement()
    {
        if (axis.x > 0) //Right
        {
            if (axis.y > 0) //Upright
            {
                //tile - ytiles
                tileSelection.x--;

                if (tileSelection.x < selectionLimit[0].x) 
                {
                    tileSelection.x++;
                    soundPlayer.PlaySound(2, true);
                }

            }
            else
            if (axis.y < 0) //DownRight
            {
                //tile + 1
                //I don't even know anymore
                tileSelection.y++;

                if (tileSelection.y >= selectionLimit[1].y) 
                {
                    tileSelection.y--;
                    soundPlayer.PlaySound(2, true);
                }
            }
            else //Right
            {
                //tile - ytiles + 1
                tileSelection.x--;
                tileSelection.y++;
                if (tileSelection.x < selectionLimit[0].x && tileSelection.y >= selectionLimit[1].y) soundPlayer.PlaySound(2, true);
                if (tileSelection.x < selectionLimit[0].x) tileSelection.x++;
                if (tileSelection.y >= selectionLimit[1].y) tileSelection.y--;
            }
        }
        else if (axis.x < 0) //Left
        {
            if (axis.y > 0) //UpLeft
            {
                //tile - 1
                tileSelection.y--;

                 if (tileSelection.y < selectionLimit[0].y) 
                {
                    tileSelection.y++;
                    soundPlayer.PlaySound(2, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                tileSelection.x++;
                if (tileSelection.x >= selectionLimit[1].x) 
                {
                    tileSelection.x--;
                    soundPlayer.PlaySound(2, true);
                }

            }
            else //Left
            {
                //tile + ytiles - 1
                tileSelection.x++;
                tileSelection.y--;
                if (tileSelection.x >= selectionLimit[1].x && tileSelection.y < selectionLimit[0].y) soundPlayer.PlaySound(2, true);
                if(tileSelection.x >= selectionLimit[1].x) tileSelection.x--;
                if (tileSelection.y < selectionLimit[0].y) tileSelection.y++;

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            tileSelection.x--;
            tileSelection.y--;
            if (tileSelection.x < selectionLimit[0].x && tileSelection.y < selectionLimit[0].y) soundPlayer.PlaySound(2, true);
            if(tileSelection.x < selectionLimit[0].x) tileSelection.x++;
            if(tileSelection.y < selectionLimit[0].y) tileSelection.y++;

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            tileSelection.x++;
            tileSelection.y++;
            if (tileSelection.y >= selectionLimit[1].y && tileSelection.x >= selectionLimit[1].x) soundPlayer.PlaySound(2, true);

            if (tileSelection.y >= selectionLimit[1].y) tileSelection.y--;    
            if(tileSelection.x >= selectionLimit[1].x) tileSelection.x--;
            

        }

        if (axis.x != 0 || axis.y != 0)
        {
            selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileVectorSize.y));
            selectedTileIndicator.position = tileScript.tileTransform[selectedTile].position;
			soundPlayer.PlaySound(3, true);
			tileSelectCooldownCounter = 0;
            //gameManager.MoveFormation(0, selectedTile);
        }
    }
    public void AttackTargetMovement()
    {
        //attackInfo.attackRangeSize[currentattack].;
        
        if (axis.x > 0) //Right
        {
            if (axis.y > 0) //Upright
            {
                //tile - ytiles
                targetOrigin.x--;

                if (targetOrigin.x < selectionLimit[2].x)
                {
                    targetOrigin.x++;
                    soundPlayer.PlaySound(2,  true);
                }

            }
            else
            if (axis.y < 0) //DownRight
            {
                //tile + 1
                //I don't even know anymore
                targetOrigin.y++;

                if (targetOrigin.y + targetMargin.x > selectionLimit[3].y)
                {
                    targetOrigin.y--;
                    soundPlayer.PlaySound(2,  true);
                }
            }
            else //Right
            {
                //tile - ytiles + 1
                targetOrigin.x--;
                targetOrigin.y++;
                if (targetOrigin.x < selectionLimit[2].x && targetOrigin.y + targetMargin.x > selectionLimit[3].y) soundPlayer.PlaySound(2,  true);
                if (targetOrigin.x < selectionLimit[2].x) targetOrigin.x++;
                if (targetOrigin.y + targetMargin.x > selectionLimit[3].y) targetOrigin.y--;
            }
        }
        else if (axis.x < 0) //Left
        {
            if (axis.y > 0) //UpLeft
            {
                //tile - 1
                targetOrigin.y--;

                if (targetOrigin.y < selectionLimit[2].y)
                {
                    targetOrigin.y++;
                    soundPlayer.PlaySound(2, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                targetOrigin.x++;
                if (targetOrigin.x + targetMargin.y > selectionLimit[3].x)
                {
                    targetOrigin.x--;
                    soundPlayer.PlaySound(2, true);
                }

            }
            else //Left
            {
                //tile + ytiles - 1
                targetOrigin.x++;
                targetOrigin.y--;
                if (targetOrigin.x + targetMargin.y > selectionLimit[3].x && targetOrigin.y < selectionLimit[2].y) soundPlayer.PlaySound(2, true);
                if (targetOrigin.x + targetMargin.y > selectionLimit[3].x) targetOrigin.x--;
                if (targetOrigin.y < selectionLimit[2].y) targetOrigin.y++;

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            targetOrigin.x--;
            targetOrigin.y--;
            if (targetOrigin.x < selectionLimit[2].x && targetOrigin.y < selectionLimit[2].y) soundPlayer.PlaySound(2, true);
            if (targetOrigin.x < selectionLimit[2].x) targetOrigin.x++;
            if (targetOrigin.y < selectionLimit[2].y) targetOrigin.y++;

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            targetOrigin.x++;
            targetOrigin.y++;
            if (targetOrigin.y + targetMargin.x > selectionLimit[3].y && targetOrigin.x + targetMargin.y > selectionLimit[3].x) soundPlayer.PlaySound(2, true);

            if (targetOrigin.y + targetMargin.x > selectionLimit[3].y) targetOrigin.y--;
            if (targetOrigin.x + targetMargin.y > selectionLimit[3].x) targetOrigin.x--;


        }

        if (axis.x != 0 || axis.y != 0)
        {
            TargetPlacement();
			TargetPlacementVisuals();
			soundPlayer.PlaySound(3, true);
			tileSelectCooldownCounter = 0;
            //gameManager.MoveFormation(0, selectedTile);
        }

    }
	#endregion 
	public void TargetPlacement()
    {
        //Calculate which should be selected Targets using 
        int targetsCounter = 0;
		//Debug.Log(targetAmount);
        selectedTargets = new int[targetAmount];
        selectedTargetVector = new Vector2[targetAmount];        

        //Declaring values for row and column
        int whatColumn; //X                
        int whatRow; //Y 
        whatRow = 0;
        whatColumn = 0;

        for (int i = 0; i < gameData.AttackList[currentAttack].rangeActive.Length; i++) //For all of the attacks range size 
        {
            //go through all the possible rows until you find the one the current array iteration is on
            for(int n = 0; n <= targetMargin.y; n++)
            {
                if (i == targetMargin.x*n )
                {
                    whatRow = n;
                    break;
                }
            }
            
            if(gameData.AttackList[currentAttack].rangeActive[i] == 1) //Is the current tile being inspected a part of the range?
            {

                whatColumn = i - whatRow * Mathf.FloorToInt(targetMargin.x);
                
                //To Do: Make it look at the given rangeSize.x  to determine on which row it is
                selectedTargetVector[targetsCounter].x = whatColumn + targetOrigin.y;
                selectedTargetVector[targetsCounter].y = whatRow + targetOrigin.x;
                

                selectedTargets[targetsCounter] = Mathf.FloorToInt(selectedTargetVector[targetsCounter].x + (selectedTargetVector[targetsCounter].y * tileVectorSize.y)); //Store this as a tileID for Selected targets
                
                targetsCounter++;
				
            }   
        }
		/*Debug.Log("Target amount: " + targetsCounter);
		for (int i = 0; i < targetsCounter; i++)
		{
			Debug.Log("	" + selectedTargets[i]);
		}*/

		//Debug.Log("Char" + activeCharacter + " has " + targetsCounter + "targets");
    }
	public void EnemyTargetVisuals()
	{
		GameObject[] objTarget;

		CalculateTargetAmount();

		objTarget = new GameObject[targetAmount];
		selectedTargetsTransform = new Transform[targetAmount];

		for (int i = 0; i < targetAmount; i++)
		{
			objTarget[i] = Instantiate(targetCursor);
			objTarget[i].name = "TargetCursor_" + i;
			selectedTargetsTransform[i] = objTarget[i].GetComponent<Transform>();
		}

		TargetPlacementVisuals();
	}
	public void TargetPlacementVisuals()
	{
		for (int i = 0; i < targetAmount; i++)
		{
			selectedTargetsTransform[i].position = tileScript.tileTransform[selectedTargets[i]].position + new Vector3(0, 0.005f, 0);
		}
	}
	public void CalculateTargetAmount()
	{
		targetAmount = 0;
		targetMargin = gameData.AttackList[currentAttack].rangeSize;

		for (int i = 0; i < targetMargin.x * targetMargin.y; i++) //each of them is at least 1
		{
			if (gameData.AttackList[currentAttack].rangeActive[i]  == 1)
			{
				targetAmount++;
			}
		}
	}

	public void MoveFormation(int charID, Vector2 tiles)
    {
		gameData.FullFormationsCollection[0].formations[charID].tiles = tiles;
		//CharacterStats.SetTileOccupied(charID, tiles, tileScript.yTiles);
        
        PlaceCharacterOnTheirTile(charID);
    }
    public void PlaceCharacterOnTheirTile(int charID)
    {
		tileScript.tiles[charControl[charID].tileID].isOccupied = false;
		charControl[charID].UpdateTileID();
		charControl[charID].trans.position = tileScript.tileTransform[charControl[charID].tileID].position;
		tileScript.tiles[charControl[charID].tileID].isOccupied = true;
		//tileScript.tileTransform[gameData PlayerPrefs.GetInt(charID + "_TileID")].position;

	}

//-------------------------Start Battle-------------------------
    public void ToggleEncounterRate()
	{
		randomEcountersOn = !randomEcountersOn;
	}
	public void EncounterAnim()
	{
		specifiedEncounter = false;
		gameState = GameState.Event;
		playerController.isMoving = false;
		playerController.isRunning = false;
		battleUI.battleTransition.TransitionTo();
	}
	public void SpecifiedBattleEncounterAnim(int encounterIdentity)
	{
		specifiedEncounter = true;
		specEncounterID = encounterIdentity;

		battleUI.battleTransition.TransitionTo();
	}

    public void InitializeEncounter(bool isGroupSpecified)
    {
		if (gameState == GameState.Battle) return;
		playerController.isMoving = false;
        gameState = GameState.Battle;
        camSet = CameraSetting.BattleCam;
		selecting = SelectingMenu.selectingAction;
        cam_T.position = battleCam;
        
        enemyDefeated = 0;

		if (!isGroupSpecified)
		{
			int randEncounter;
			randEncounter = UnityEngine.Random.Range(0, possibleEncounters.Length);


			enemyGroupID = possibleEncounters[randEncounter];
		}
		//Debug.Log("enemy Group" + enemyGroupID);

		/*if (enemyGroupID == 1) enemyGroupID = 2; //This will later be random
		else enemyGroupID = 1;*/

		enemyAmount = gameData.FullFormationsCollection[enemyGroupID].formations.Length; //PLACHEOLDER

		partyMembers = 3; //PlayerPrefs.GetInt("PartyMembers", 3);
		
        
        characters = new GameObject[partyMembers + enemyAmount];
		charControl = new CharControl_Battle[partyMembers + enemyAmount];
		
        
        for (int i = 0; i < partyMembers + enemyAmount; i++)
        {
			characters[i] = Instantiate(battleCharacterPrefab);
            
            charControl[i] = characters[i].GetComponent<CharControl_Battle>();
            charControl[i].rowSize = tileScript.yTiles;
			if(i < partyMembers)characters[i].name = "Battle_Char_" + i;
			else characters[i].name = "Battle_Enemy_" + i;



			//Up to here is good.
			

			//gameData.FullFormationsCollection[].formation[]

			if (i < partyMembers)
			{
				charControl[i].Init(i, true);
			}
			else
			{
				charControl[i].Init(i, false);
			}
            PlaceCharacterOnTheirTile(i);
			
		}
		//Debug.Log(enemyGroupID + "test4");

		battleUI.InitializeInfoBoxes();
        battleUI.attackOptionAmount = charControl[activeCharacter].attacksAmount;
        battleMenu.SetActive(true);
        battleUI.ChangeNotifText("Encountered an enemy!");

		turnCounter = 0;
		CalculateTurnOrderInPhase();
		StartCoroutine(WaitForBattleStart());

		PlayBattleMusic(isGroupSpecified);
    }
	public void InitializeEncounter(bool isGroupSpecified, int fightGroupID)
	{
		enemyGroupID = fightGroupID;
		if (gameState == GameState.Battle) return;
		playerController.isMoving = false;
		gameState = GameState.Battle;
		camSet = CameraSetting.BattleCam;
		selecting = SelectingMenu.selectingAction;
		cam_T.position = battleCam;

		enemyDefeated = 0;

		
		//Debug.Log("enemy Group" + enemyGroupID);

		/*if (enemyGroupID == 1) enemyGroupID = 2; //This will later be random
		else enemyGroupID = 1;*/

		enemyAmount = gameData.FullFormationsCollection[enemyGroupID].formations.Length; //PLACHEOLDER

		partyMembers = 3; //PlayerPrefs.GetInt("PartyMembers", 3);


		characters = new GameObject[partyMembers + enemyAmount];
		charControl = new CharControl_Battle[partyMembers + enemyAmount];


		for (int i = 0; i < partyMembers + enemyAmount; i++)
		{
			characters[i] = Instantiate(battleCharacterPrefab);

			charControl[i] = characters[i].GetComponent<CharControl_Battle>();
			charControl[i].rowSize = tileScript.yTiles;
			if (i < partyMembers) characters[i].name = "Battle_Char_" + i;
			else characters[i].name = "Battle_Enemy_" + i;



			//Up to here is good.


			//gameData.FullFormationsCollection[].formation[]

			if (i < partyMembers)
			{
				charControl[i].Init(i, true);
			}
			else
			{
				charControl[i].Init(i, false);
			}
			//charControl[i].UpdateTileID();
			PlaceCharacterOnTheirTile(i);
			//tileScript.tiles[charControl[i].tileID].isOccupied = true;

		}
		//Debug.Log(enemyGroupID + "test4");

		battleUI.InitializeInfoBoxes();
		battleUI.attackOptionAmount = charControl[activeCharacter].attacksAmount;
		battleMenu.SetActive(true);
		battleUI.ChangeNotifText("Encountered an enemy!");

		turnCounter = 0;
		CalculateTurnOrderInPhase();
		StartCoroutine(WaitForBattleStart());

		PlayBattleMusic(isGroupSpecified);
	}
	IEnumerator WaitForBattleStart()
	{
		selecting = SelectingMenu.waiting;
		yield return new WaitForSeconds(2);
		StartTurn();
	}
//--------------------------End battle-------------------------------
	public void CalculateRewards()
	{
		totalGold = 0;
		totalExp = 0;
		allItems = new List<int>();

		for (int i = 0; i < gameData.FullFormationsCollection[enemyGroupID].formations.Length; i++)
		{
			totalGold += gameData.EnemyCollection[gameData.FullFormationsCollection[enemyGroupID].formations[i].classID].goldGain;
			totalExp += gameData.EnemyCollection[gameData.FullFormationsCollection[enemyGroupID].formations[i].classID].expGain;
			for (int e = 0; e < gameData.EnemyCollection[gameData.FullFormationsCollection[enemyGroupID].formations[i].classID].itemDrops.Length; e++)
			{
				int itemD = gameData.EnemyCollection[gameData.FullFormationsCollection[enemyGroupID].formations[i].classID].itemDrops[e];
				if(UnityEngine.Random.Range(0, 100) <= gameData.ItemsCollection[itemD].rarity)
				{
					allItems.Add(itemD);
				}
			}
		}
		
	}

	public void EndBattle()
    {
        gameState = GameState.Overworld;
        camSet = CameraSetting.OverworldCam;

		if (selectedTargetsTransform.Length != 0)
		{
			for (int i = 0; i < targetAmount; i++) //Eliminate the target cursors.
			{
				Destroy(selectedTargetsTransform[i].gameObject);
			}
			selectedTargetsTransform = new Transform[0];
		}

		for(int i = 0; i < partyMembers + enemyAmount; i++)
		{
			Destroy(charControl[i].gameObject);
			if (i < partyMembers) Destroy(battleUI.playerInfoBox[i].gameObject);
			else Destroy(battleUI.enemyInfoPopUp[i - partyMembers].gameObject);
		}

		cam_T.position = playerController.trans.position;
        battleMenu.SetActive(false);
		if(specifiedEncounter)
		{
			Debug.Log("les go");
			specifiedEncounter = false;
			eventManager.ContinueEvent();
		}

		PlayOverworldMusic();
    }


    //------------------------------Run-----------------------------
        public void RunFromBattle()
        {
			if (!specifiedEncounter)
			{
				if (UnityEngine.Random.Range(0, 3) >= 2)
				{
					battleUI.ChangeNotifText("Got away!");

					StartCoroutine(WaitForSuccesfulRun());
				}
				else if (debug)
				{
					battleUI.ChangeNotifText("Got away!");

					StartCoroutine(WaitForSuccesfulRun());
				}
				//else failed to run.
				else FailedToRun();
			}
			else
			{
				battleUI.ChangeNotifText("Can't run from this fight");
			}

        }
        public void FailedToRun()
        {
            battleUI.ChangeNotifText("Failed to run!");
			StartCoroutine(WaitForFailedRun()); 
        }
		
		IEnumerator WaitForSuccesfulRun()
		{
			selecting = SelectingMenu.waiting;
			yield return new WaitForSeconds(0.5f);
			
			transition.FadeTo(Color.black, 1);
			yield return new WaitForSeconds(0.5f);
			//Debug.Log("FadeFROM");
			EndBattle();
			transition.FadeFrom();
		}

		IEnumerator WaitForFailedRun()
		{
			selecting = SelectingMenu.waiting;
			yield return new WaitForSeconds(1.5f);
			EndTurn();
		}
    //----------------------------Victory----------------------------
        public void Victory()
        {
		//Debug.Log("Victory");
			levelUpScreenProgress = 0;
			PlayVictoryMusic();
			battleUI.VictorySequence();
			selecting = SelectingMenu.waiting;
        }
		public void EndVictory()
		{
			transition.FadeTo(Color.black);
			StartCoroutine(VictoryFade());
		}
		
	public void VictoryContinuation(int startNum)
	{
		
		battleUI.vicPanel.LevelUpCheck(startNum);
		
		
	}
		IEnumerator VictoryFade()
		{
			
			yield return new WaitForSeconds(2);
			battleUI.vicPanel.DeleteVicInfo();
			battleUI.vicPanel.PanelActiveReset();
			battleUI.victoryPanel.SetActive(false);
		//Debug.Log("new level " + gameData.Party[0].level + " exp " + gameData.Party[0].exp);
			EndBattle();
			transition.FadeFrom();
		}
	//-----------------------------Loss------------------------------
	public void GameOverCheck()
	{
		for(int i = 0; i < partyMembers; i++)
		{
			if (!charControl[i].isDead) break;

			if (i >= partyMembers - 1) GameOver();
		}
	}
	public void GameOver()
	{
		//Git gud
		transition.FadeToSceneChange(false, 2);
	}

//-------------------------------Pause---------------------------------    
	public void PauseToggle()
    {
        if (!pauseMenu.isAnimating)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pausePanel.SetActive(true);
                pauseMenu.PauseLoadAnimation();

            }
            else
            {
                pausePanel.SetActive(false);
            }
        }
    }
	//-----------------------------Audio-------------------------------
	public void PlayOverworldMusic()
	{
		soundPlayer.PlayMusic(SoundPlayer.MusicType.Overworld);
	}

	public void PlayBattleMusic(bool isSpec)
	{
		if (!isSpec) soundPlayer.PlayMusic(SoundPlayer.MusicType.Battle);
		else soundPlayer.NewBossMusic(gameData.FullFormationsCollection[enemyGroupID].music);
	}
	public void PlayVictoryMusic()
	{
		soundPlayer.PlayMusic(SoundPlayer.MusicType.Victory);
	}
//------------------------------Debug----------------------------------
	public void ToggleDebug()
	{
		debug = !debug;
		if(debug == true) debugMenu.SetActive(true);
		else debugMenu.SetActive(false);
	}

	

}
