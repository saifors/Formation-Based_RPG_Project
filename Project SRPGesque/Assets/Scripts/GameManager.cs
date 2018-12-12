using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public bool debug;
	private float timeCounter;
	private Transform cam_T;
	private OWPlayerController playerController;
    public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam}; public CameraSetting camSet;
    public enum GameState { Overworld, Battle, GameMenu};
    public GameState gameState;
	public bool randomEcountersOn;
	public float encounterMinimumPercent = 100;
	private BattleUI battleUI;
	public GameObject battleMenu;
	public GameObject debugMenu;
	public GameObject battleCharacterPrefab;
	public GameObject battleEnemyPrefab;

    public int partyMembers;
	private CharacterStats charStats;
    public GameObject[] characters;
	public int activeCharacter; 
    [HideInInspector] public CharControl_Battle[] charControl;

    public int initialEnemyAmount;
    public int enemyAmount;
    public GameObject[] enemies;
    public EnemyControl_Battle[] enemyControl;

	[HideInInspector] public AttackInfoManager attackInfo;

    


    [HideInInspector] public TileScript tileScript;
    private Transform battlefield;
	
	// Use this for initialization
	void Start () 
	{
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
        gameState = GameState.Overworld;
		battleUI = GameObject.FindGameObjectWithTag("UI").GetComponent<BattleUI>();
		
		charStats = GetComponent<CharacterStats>();
		attackInfo = GetComponent<AttackInfoManager>();

		tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
        tileScript.Init();
        battlefield = GameObject.FindGameObjectWithTag("Battlefield").GetComponent<Transform>();

        randomEcountersOn = true;//Depending on the area.
		partyMembers = 1; //PLACHEOLDER
        characters = new GameObject[partyMembers];
        charControl = new CharControl_Battle[partyMembers];

        enemyAmount = 1; //PLACHEOLDER
        enemies = new GameObject[enemyAmount];
        enemyControl = new EnemyControl_Battle[enemyAmount];

        for (int i = 0; i < partyMembers; i++)
        {
            characters[i] = Instantiate(battleCharacterPrefab);
            characters[i].name = "Battle_Char_" + i;
            charControl[i] = characters[i].GetComponent<CharControl_Battle>();
            charControl[i].rowSize = tileScript.yTiles;

            charStats.CreateCharacterStats(0, 10, 12, 5, 3, 2, 4); //PLACHEOLDER;
            charStats.SetTileOccupied(0, new Vector2(3,4), tileScript.yTiles);

            charControl[i].Init(i);
            charControl[i].UpdateTileID();
            PlaceCharacterOnTheirTile(i);
            tileScript.tiles[charControl[i].tileID].isOccupied = true;
        }

        for(int i = 0; i < enemyAmount; i++)
        {
            enemies[i] = Instantiate(battleEnemyPrefab);
            enemies[i].name = "Battle_Enemy_" + i;
            enemyControl[i] = enemies[i].GetComponent<EnemyControl_Battle>();
            enemyControl[i].rowSize = tileScript.yTiles;

            //charStats.CreateCharacterStats(0, 10, 12, 5, 3, 2, 4); //PLACHEOLDER;
            //charStats.SetTileOccupied(0, new Vector2(3, 4), tileScript.yTiles);


            // THIS NEEDS SOME WORK


            enemyControl[i].Init(i);
            enemyControl[i].UpdateTileID();
            PlaceCharacterOnTheirTile(i);
            tileScript.tiles[enemyControl[i].tileID].isOccupied = true;
        }

        battleUI.tileCollumnSize = tileScript.xTiles ; //these two ar numbers not counting zero as a part of em
        battleUI.tileRowSize = tileScript.yTiles ;
        battleUI.tileAmount = tileScript.tileAmount -1;
        battleUI.startLimit = new Vector2(battleUI.tileCollumnSize/2, 0);
        battleUI.endLimit = new Vector2(battleUI.tileCollumnSize, battleUI.tileRowSize);
        battleUI.startTargetLimit = new Vector2(0, 0);
        battleUI.endTargetLimit = new Vector2(battleUI.tileCollumnSize/2, battleUI.tileRowSize);


        MoveFormation(0, charControl[0].tile); //PLACEHOLDER
		
	}
	
	
	// Update is called once per frame
	void Update () 
	{
        


		if(gameState == GameState.Overworld)
		{
			if(camSet == CameraSetting.OverworldCam)
			{
				if (playerController.isMoving)
				{
					cam_T.position = playerController.trans.position;
				}
			}
			if(randomEcountersOn)
			{
				if(playerController.isMoving)
				{
					timeCounter += Time.deltaTime;
					if(timeCounter >= 1)
					{
						encounterMinimumPercent -= 2.5f;
						
						if(Random.Range(0 , 100) >= encounterMinimumPercent)
						{
							InitializeEncounter();
							encounterMinimumPercent = 100;
						}
						timeCounter = 0;
					}
					
					
				}
			}

			//Las mierdas de debug.
			if(debug)
			{

			}
		}
		if(gameState == GameState.Battle)
		{
			

			if(debug)
			{

			}
		}
        
	
	}
	public void InitializeEncounter()
	{
		playerController.isMoving = false;
		gameState = GameState.Battle;
		camSet = CameraSetting.BattleCam;
		cam_T.position = battlefield.position;
		battleUI.attackAmount = charControl[activeCharacter].attacksAmount;

		

		battleMenu.SetActive(true);
		battleUI.ChangeNotifText("Encountered an enemy!");
	}
	public void EndBattle()
	{
		gameState = GameState.Overworld;
		camSet = CameraSetting.OverworldCam;
		cam_T.position = playerController.trans.position;
		battleMenu.SetActive(false);
	}
    
    public void MoveFormation(int charID, Vector2 tiles)
    {
        charStats.SetTileOccupied(charID, tiles, tileScript.yTiles);
        charControl[charID].UpdateTileID();
        PlaceCharacterOnTheirTile(charID);
    }
    public void PlaceCharacterOnTheirTile(int charID)
    {
        characters[charID].transform.position = tileScript.tileTransform[PlayerPrefs.GetInt(charID + "_TileID") ].position;
    }
	public void RunFromBattle()
	{
		if(Random.Range(0,3) >= 2)
		{
			EndBattle();
		}
		else if(debug)
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

	public void ToggleDebug()
	{
		debug = !debug;
		if(debug == true) debugMenu.SetActive(true);
		else debugMenu.SetActive(false);
	}

	public void ToggleEncounterRate()
	{
		randomEcountersOn = !randomEcountersOn;
	}

}
