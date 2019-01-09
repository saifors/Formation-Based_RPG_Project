 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
//Debug
    public bool debug;
    public GameObject debugMenu;
//Basics
    private float timeCounter;
    public Vector2 axis;
    private Transform cam_T;
    private OWPlayerController playerController;
    public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam }; public CameraSetting camSet;
    public enum GameState { Overworld, Battle, GameMenu };
    public GameState gameState;

//Encounters
    public bool randomEcountersOn;
    public float encounterMinimumPercent = 100;

//Battle Interface (Canvas)    
    public GameObject battleMenu;
    public BattleUI battleUI;
    public enum SelectingMenu { selectingAction, selectingAttack, selectingTarget, selectingMove, victoryScreen};
    public SelectingMenu selecting;

//Prefabs For Player and Enemy characters    
    public GameObject battleCharacterPrefab;
    public GameObject battleEnemyPrefab;

//Database
    private CharacterStats charStats;
    [HideInInspector] public AttackInfoManager attackInfo;

//Playable Characters: Amount, Object and Controller Arrays, Active/Currently Selected, 
    public int partyMembers;    
    public GameObject[] characters;
    [HideInInspector] public CharControl_Battle[] charControl;
    public int activeCharacter;
    
//Enemy Characters: Current Amount and start of battle Amount, Object and Controller Arrays, Check Amount alive.
    public int initialEnemyAmount;
    public int enemyAmount;
    public GameObject[] enemies;
    public EnemyControl_Battle[] enemyControl;
    public int enemyDefeated;

//Battle
    //Battle - Move
        public GameObject cursor; //Prefab
        private float tileSelectCooldownCounter;
        
    //Battle - Attack
        public GameObject targetCursor; //Prefab
        public int currentAttack;
        public Vector2 targetMargin;
    //Tiles
        [HideInInspector] public TileScript tileScript;
        public int tileAmount;
        public Vector2 tileVectorSize;
        public Vector2 tileSelection;
        public int selectedTile; // looks at tileSelection and gets the Id from those
        public Vector2[] selectionLimit = new Vector2[4]; //Start Limit and End Limit for Move and for Target.
        
        private Transform selectedTileIndicator;
        

    //Target
        public int targetAmount; //How many Tiles are in the range of the attack
    public Vector2 targetOrigin;
        public int[] selectedTargets; // All tileIDs affected by current attack;
        public Vector2[] selectedTargetVector; //X and Y of all tiles in range
        public Transform[] selectedTargetsTransform;
        
    
// Pause Stuff
    public bool isPaused;
    public GameObject pausePanel;
//Location of the where battles take place
    private Transform battlefield;

//Audio
    public SoundPlayer soundPlayer;

//Start
    // Use this for initialization
    void Start()
    {
        gameState = GameState.Overworld;
        randomEcountersOn = true;//Depending on the area. Maybe a scene database indicating whether true or false?.
        
        //Initialization of Objects
            cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();

            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
            battleUI = GameObject.FindGameObjectWithTag("UI").GetComponent<BattleUI>();
            
            soundPlayer = gameObject.GetComponent<SoundPlayer>();
            tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
            tileScript.Init();
            battlefield = GameObject.FindGameObjectWithTag("Battlefield").GetComponent<Transform>();
        
        //Databases.
            charStats = GetComponent<CharacterStats>();
            attackInfo = GetComponent<AttackInfoManager>();
            //General Info

        //Placeholder Character Creation
            charStats.CreateCharacterStats("Player", 0, 1, 10, 12, 5, 3, 2, 4); //PLACHEOLDER;
            charStats.SetTileOccupied("Player", 0, new Vector2(3, 4), tileScript.yTiles);

            charStats.CreateCharacterStats("Enemy", 0, 4, 70, 12, 5, 3, 2, 4); //PLACHEOLDER;
            charStats.SetTileOccupied("Enemy", 0, new Vector2(2, 2), tileScript.yTiles);
        
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

        //MoveFormation(0, charControl[0].tile); //PLACEHOLDER
        battleUI.Init();
    }

// Update
    void Update()
    {

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
                        if (timeCounter >= 1)
                        {
                            encounterMinimumPercent -= 2.5f;

                            if (Random.Range(0, 100) >= encounterMinimumPercent)
                            {
                                InitializeEncounter();
                                encounterMinimumPercent = 100;
                            }
                            timeCounter = 0;
                        }


                    }
                }
            }

            //Las mierdas de debug.
            if (debug)
            {

            }
        }
        if (gameState == GameState.Battle)
        {
            if(selecting ==  SelectingMenu.selectingMove)
            {
                if(tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;
                //Controls for moving around the selected tiles.
                if(tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
                {
                    
                    FormationMovement();
                }
            }
            if(selecting == SelectingMenu.selectingTarget)
            {
                if(tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;
                
                if(tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
                {
                    AttackTargetMovement();
                    
                }
            }

            if (debug)
            {

            }
        }


    }
    
//Input
        public void ConfirmSelectedCommand()
        {
            soundPlayer.PlaySound(0,1, true);
            if(battleUI.command == BattleUI.CommandSelection.Attack)
            {
                battleUI.InitiateAttackSelection();

            }
            else if(battleUI.command == BattleUI.CommandSelection.Defend)
            {
                //Go to Select defend
            }
            else if(battleUI.command == BattleUI.CommandSelection.Move)
            {
                //Go to move menu
                selecting = SelectingMenu.selectingMove;
                
                tileSelection = charControl[0].tile; //0 is a placeholder for now Will be replaced with an Int indicating the active character when that's a thing            
                selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileVectorSize.y));
                selectedTileIndicator.gameObject.SetActive(true);
                selectedTileIndicator.position = tileScript.tileTransform[selectedTile].position;
                battleUI.actionMenu.SetActive(false);
                //gameManager.MoveFormation(0,5);
            }
            else if(battleUI.command == BattleUI.CommandSelection.Item)
            {
                //Go to Item Menu
            }
            else if(battleUI.command == BattleUI.CommandSelection.Run)
            {
                //Execute Running away
                RunFromBattle();
            }
            else 
            {
                Debug.Log("Tried to confirm an Action command selection but nothing was selected");
            }
        }
        public void ReturnToCommandSelection()
	    {
            soundPlayer.PlaySound(1,1, true);
            selecting = SelectingMenu.selectingAction;
            selectedTileIndicator.gameObject.SetActive(false);
            battleUI.attackMenu.SetActive(false);
            battleUI.partyInfo.SetActive(true);
            battleUI.actionMenu.SetActive(true);
	    }
        public void ConfirmAttackSelection()
        {
            //This is all going to change once it's an array of course
            soundPlayer.PlaySound(0,1, true);
            GameObject[] objTarget;
            
            currentAttack = charControl[activeCharacter].attacksLearned[battleUI.attackOptionSelected];
            //Debug.Log("Attack " + currentAttack);
            
            targetAmount = 0;
            targetMargin = attackInfo.attackRangeSize[currentAttack];

            for(int i = 0; i < targetMargin.x * targetMargin.y; i++)
            {
                if(attackInfo.attackRangeActive[currentAttack][i] == 1) 
                {
                    targetAmount++;
                }
            }

            objTarget = new GameObject[targetAmount];
            selectedTargetsTransform = new Transform[targetAmount];

            for (int i = 0; i < targetAmount; i++)
            {
                objTarget[i] = Instantiate(targetCursor);
                objTarget[i].name = "TargetCursor_" + i;
                selectedTargetsTransform[i] = objTarget[i].GetComponent<Transform>();
            }
            targetOrigin.x = 2;
            targetOrigin.y = 2;
       
            TargetPlacement();


            //selectedTargetVector[0] = Vector2.zero;
            //selectedTarget[i] = selectedTargetVector[i].x + selectedTargetVector[i].y;
            //selectedTargetsTransform[i].gameObject.SetActive(true);
            //selectedTargetsTransform[i].position = gameManager.tileScript.tileTransform[selectedTarget[i]].position;
            battleUI.attackMenu.SetActive(false);
            selecting = SelectingMenu.selectingTarget;
        }
        public void ReturnToAttackSelect()
        {
            soundPlayer.PlaySound(1,1, true);

            for(int i = 0; i < targetAmount; i++) //Eliminate the target cursors.
            {
                Destroy(selectedTargetsTransform[i].gameObject);
            }
            selectedTargetsTransform = new Transform[0];

            battleUI.InitiateAttackSelection(); 
        }

        public void SetAxis(Vector2 inputAxis)
        {
            axis = inputAxis;
        }

    public void ConfirmAttackTarget()
        {
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
//-------------------------Tile Selection----------------------
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
                    soundPlayer.PlaySound(2,1, true);
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
                    soundPlayer.PlaySound(2,1, true);
                }
            }
            else //Right
            {
                //tile - ytiles + 1
                tileSelection.x--;
                tileSelection.y++;
                if (tileSelection.x < selectionLimit[0].x && tileSelection.y >= selectionLimit[1].y) soundPlayer.PlaySound(2,1, true);
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
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                tileSelection.x++;
                if (tileSelection.x >= selectionLimit[1].x) 
                {
                    tileSelection.x--;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else //Left
            {
                //tile + ytiles - 1
                tileSelection.x++;
                tileSelection.y--;
                if (tileSelection.x >= selectionLimit[1].x && tileSelection.y < selectionLimit[0].y) soundPlayer.PlaySound(2,1, true);
                if(tileSelection.x >= selectionLimit[1].x) tileSelection.x--;
                if (tileSelection.y < selectionLimit[0].y) tileSelection.y++;

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            tileSelection.x--;
            tileSelection.y--;
            if (tileSelection.x < selectionLimit[0].x && tileSelection.y < selectionLimit[0].y) soundPlayer.PlaySound(2,1, true);
            if(tileSelection.x < selectionLimit[0].x) tileSelection.x++;
            if(tileSelection.y < selectionLimit[0].y) tileSelection.y++;

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            tileSelection.x++;
            tileSelection.y++;
            if (tileSelection.y >= selectionLimit[1].y && tileSelection.x >= selectionLimit[1].x) soundPlayer.PlaySound(2, 1, true);

            if (tileSelection.y >= selectionLimit[1].y) tileSelection.y--;    
            if(tileSelection.x >= selectionLimit[1].x) tileSelection.x--;
            

        }

        if (axis.x != 0 || axis.y != 0)
        {
            selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileVectorSize.y));
            selectedTileIndicator.position = tileScript.tileTransform[selectedTile].position;

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
                    soundPlayer.PlaySound(2, 1, true);
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
                    soundPlayer.PlaySound(2, 1, true);
                }
            }
            else //Right
            {
                //tile - ytiles + 1
                targetOrigin.x--;
                targetOrigin.y++;
                if (targetOrigin.x < selectionLimit[2].x && targetOrigin.y + targetMargin.x > selectionLimit[3].y) soundPlayer.PlaySound(2, 1, true);
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
                    soundPlayer.PlaySound(2, 1, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                targetOrigin.x++;
                if (targetOrigin.x + targetMargin.x - 1 > selectionLimit[3].x)
                {
                    targetOrigin.x--;
                    soundPlayer.PlaySound(2, 1, true);
                }

            }
            else //Left
            {
                //tile + ytiles - 1
                targetOrigin.x++;
                targetOrigin.y--;
                if (targetOrigin.x + targetMargin.y > selectionLimit[3].x && targetOrigin.y < selectionLimit[2].y) soundPlayer.PlaySound(2, 1, true);
                if (targetOrigin.x + targetMargin.y > selectionLimit[3].x) targetOrigin.x--;
                if (targetOrigin.y < selectionLimit[2].y) targetOrigin.y++;

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            targetOrigin.x--;
            targetOrigin.y--;
            if (targetOrigin.x < selectionLimit[2].x && targetOrigin.y < selectionLimit[2].y) soundPlayer.PlaySound(2, 1, true);
            if (targetOrigin.x < selectionLimit[2].x) targetOrigin.x++;
            if (targetOrigin.y < selectionLimit[2].y) targetOrigin.y++;

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            targetOrigin.x++;
            targetOrigin.y++;
            if (targetOrigin.y + targetMargin.y >= selectionLimit[3].y && targetOrigin.x + targetMargin.x - 1 > selectionLimit[3].x) soundPlayer.PlaySound(2, 1, true);

            if (targetOrigin.y + targetMargin.y >= selectionLimit[3].y) targetOrigin.y--;
            if (targetOrigin.x + targetMargin.y > selectionLimit[3].x) targetOrigin.x--;


        }

        if (axis.x != 0 || axis.y != 0)
        {
            TargetPlacement();

            tileSelectCooldownCounter = 0;
            //gameManager.MoveFormation(0, selectedTile);
        }

    }

    public void TargetPlacement()
    {
        //Calculate which should be selected Targets using 
        int targetsCounter = 0;
        selectedTargets = new int[targetAmount];
        selectedTargetVector = new Vector2[targetAmount];        

        //Declaring values for row and column
        int whatColumn; //X                
        int whatRow; //Y 
        whatRow = 0;
        whatColumn = 0;

        for (int i = 0; i < attackInfo.attackRangeActive[currentAttack].Length; i++) //For all of the attacks range size 
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
            
            if(attackInfo.attackRangeActive[currentAttack][i] == 1) //Is the current tile being inspected a part of the range?
            {

                whatColumn = i - whatRow * Mathf.FloorToInt(targetMargin.x);
                
                //To Do: Make it look at the given rangeSize.x  to determine on which row it is
                selectedTargetVector[targetsCounter].x = whatColumn + targetOrigin.y;
                selectedTargetVector[targetsCounter].y = whatRow + targetOrigin.x;
                

                selectedTargets[targetsCounter] = Mathf.FloorToInt(selectedTargetVector[targetsCounter].x + (selectedTargetVector[targetsCounter].y * tileVectorSize.y)); //Store this as a tileID for Selected targets
                
                targetsCounter++;
            }
            
        }

        for(int i = 0; i < targetAmount; i++)
        {
            selectedTargetsTransform[i].position = tileScript.tileTransform[selectedTargets[i]].position + new Vector3(0,0.005f,0);
        }
    }

    public void MoveFormation(int charID, Vector2 tiles)
    {
        charStats.SetTileOccupied("Player", charID, tiles, tileScript.yTiles);
        charControl[charID].UpdateTileID("Player");
        PlaceCharacterOnTheirTile("Player", charID);
    }
    public void PlaceCharacterOnTheirTile(string alliance, int charID)
    {
        if (alliance == "Player") characters[charID].transform.position = tileScript.tileTransform[PlayerPrefs.GetInt(alliance + charID + "_TileID")].position;
        else if (alliance == "Enemy") enemies[charID].transform.position = tileScript.tileTransform[PlayerPrefs.GetInt(alliance + charID + "_TileID")].position;
    }

//-------------------------Start Battle-------------------------
    public void ToggleEncounterRate()
	{
		randomEcountersOn = !randomEcountersOn;
	}
    public void InitializeEncounter()
    {
        playerController.isMoving = false;
        gameState = GameState.Battle;
        camSet = CameraSetting.BattleCam;
        cam_T.position = battlefield.position;
        
        enemyDefeated = 0;
        
        //Player Initialization
        partyMembers = PlayerPrefs.GetInt("PartyMembers", 1);
        characters = new GameObject[partyMembers];
        charControl = new CharControl_Battle[partyMembers];
        for (int i = 0; i < partyMembers; i++)
        {
            characters[i] = Instantiate(battleCharacterPrefab);
            characters[i].name = "Battle_Char_" + i;
            charControl[i] = characters[i].GetComponent<CharControl_Battle>();
            charControl[i].rowSize = tileScript.yTiles;



            charControl[i].Init(i, true);
            charControl[i].UpdateTileID("Player");
            PlaceCharacterOnTheirTile("Player", i);
            tileScript.tiles[charControl[i].tileID].isOccupied = true;
        }

        //Enemy Initialization
        enemyAmount = 1; //PLACHEOLDER
        enemies = new GameObject[enemyAmount];
        enemyControl = new EnemyControl_Battle[enemyAmount];
        for(int i = 0; i < enemyAmount; i++)
        {
            enemies[i] = Instantiate(battleEnemyPrefab);
            enemies[i].name = "Battle_Enemy_" + i;
            enemyControl[i] = enemies[i].GetComponent<EnemyControl_Battle>();
            enemyControl[i].rowSize = tileScript.yTiles;
            // THIS NEEDS SOME WORK
            enemyControl[i].Init(i, false);
            enemyControl[i].UpdateTileID("Enemy");
            PlaceCharacterOnTheirTile("Enemy", i);
            tileScript.tiles[enemyControl[i].tileID].isOccupied = true;
        }

        battleUI.InitializeInfoBoxes();
        battleUI.attackOptionAmount = charControl[activeCharacter].attacksAmount;
        battleMenu.SetActive(true);
        battleUI.ChangeNotifText("Encountered an enemy!");
    }
//--------------------------End battle-------------------------------
    public void EndBattle()
    {
        gameState = GameState.Overworld;
        camSet = CameraSetting.OverworldCam;
        cam_T.position = playerController.trans.position;
        battleMenu.SetActive(false);
    }
    //------------------------------Run-----------------------------
        public void RunFromBattle()
        {
            if (Random.Range(0, 3) >= 2)
            {
                EndBattle();
            }
            else if (debug)
            {
                EndBattle();
            }
            //else failed to run.
            else FailedToRun();
        }
        public void FailedToRun()
        {
            battleUI.ChangeNotifText("Failed to run!");
        }
    //----------------------------Victory----------------------------
        public void Victory()
        {
            battleUI.victoryPanel.SetActive(true);
            selecting = SelectingMenu.victoryScreen;
        }
    
//-------------------------------Pause---------------------------------    
    public void PauseToggle()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }
//------------------------------Debug----------------------------------
	public void ToggleDebug()
	{
		debug = !debug;
		if(debug == true) debugMenu.SetActive(true);
		else debugMenu.SetActive(false);
	}

	

}
