﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour 
{
	private Vector2 axis;
    public enum SelectingMenu { selectingAction, selectingAttack, selectingTarget, selectingMove, victoryScreen};
    public SelectingMenu selecting;
    
    public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;

	private float scrollCooldownCounter;
	public float scrollCooldown;
	private GameManager gameManager;

	
    private Color notifTextColor;
	private Color notifBgColor;
	private float textFadeCounter;
    private float notifAlpha;
    public bool notifShown;
    public GameObject notifPanel;
    [Header("Menus")]
    public GameObject battleMenu;
    public GameObject actionMenu;
    public GameObject attackMenu;
    public GameObject partyInfo;
    public GameObject victoryPanel;
    
    [Header("Everything for tiles with movement")]
    
    public int tileAmount;
    public int selectedTile;
    public Vector2 tileSelection;
    private float tileSelectCooldownCounter;
    public GameObject cursor;
    private Transform selectedTileIndicator;
    [HideInInspector] public int tileCollumnSize;
    [HideInInspector] public int tileRowSize;
    [HideInInspector] public Vector2 startLimit;
    [HideInInspector] public Vector2 endLimit;

    [Header("Attack Menu")]
    public GameObject attackNames;
    public Text[] attackName;
    private Transform[] attackNamePos;
    public Vector2 atkSelVector;
    public int atkHorizontalLimit;//depending on atkSelVector.y and attacksAmount of charControl is either 1 or 2.
    public int atkVerticalLimit; // depenidng on atkSelVector.x and attacksAmount of charControl is between 1 and three
    public int currentAtkHorizontalLimit; 
    public int currentAtkVerticalLimit; 
    public int attackAmount;
    public int attackSelected;
    [HideInInspector] public Vector2 startTargetLimit;
    [HideInInspector] public Vector2 endTargetLimit;
    public Text SelectedAttackDescription;
    public Text SelectedAttackStats;
    public Vector2 selectedTargetVector; //will be an array late
    //insert some vector2 to indicate the size of the target to adapt the limits once we got range working
    public GameObject targetCursor;
    public int selectedTarget; //Will be an array later
    public Transform selectedTargetsTransform; //will be an array later

    [Header("Images behind the selections")]
	public CanvasGroup selectionImage;
	public Transform selectionImage_trans;
    public Transform attackSelection;
	public Text battleNotificationText;
    public Image battleNotificationBg;
    public SoundPlayer soundPlayer;
    [Header("HP, MP and SP Bars")]
    public Transform enemyBarParent;
    public float maxEnemyBarWidth = 170;
    public float maxEnemyBarPosX = 85;
    public GameObject enemyInfoPrefab;
    public EnemyInfoPopUp[] enemyInfoPopUp;
    public Transform partyMemberInfoBoxes;
    public float maxPlayerBarWidth = 235.59f;
    public float maxPlayerBarPosX = 0;
    public GameObject playerInfoPrefab;
    public PlayerInfoBox[] playerInfoBox;

    // Use this for initialization
    public void Init () 
	{
		//To test menu stuff.
		selecting = SelectingMenu.selectingAction;
		scrollCooldown = 0.3f ;

		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		selectionImage_trans = selectionImage.GetComponent<RectTransform>();
        notifTextColor = battleNotificationText.color;
        notifBgColor = battleNotificationBg.color;
        
        soundPlayer = gameManager.soundPlayer;
        
        GameObject objCursor;
        objCursor = Instantiate(cursor);
        selectedTileIndicator = objCursor.GetComponent<Transform>();
        selectedTileIndicator.gameObject.SetActive(false);

        GameObject target;
        target = Instantiate(targetCursor);
        selectedTargetsTransform = target.GetComponent<Transform>();
        selectedTargetsTransform.gameObject.SetActive(false);

        

        maxEnemyBarWidth = 170;
        maxEnemyBarPosX = 85;
        //This should be in it's own void that is called upon an initializeEnounter
        enemyInfoPopUp = new EnemyInfoPopUp[gameManager.enemyAmount];
        for (int i = 0; i < gameManager.enemyAmount; i++)
        {
            GameObject obj = Instantiate(enemyInfoPrefab);
            
            obj.transform.SetParent(enemyBarParent);
            obj.transform.localScale = new Vector3(1,1,1);
            obj.transform.localPosition = new Vector3(-118, 318, 0); //placehodler coordinates
            obj.name = "EnemyInfo_" + i;
            
            enemyInfoPopUp[i] = obj.GetComponent<EnemyInfoPopUp>();
            enemyInfoPopUp[i].levelText.text = gameManager.enemyControl[i].level.ToString();
            
            //Debug.Log("Testing this shit");

        }

        //PlaceHolder, will be in a for and it's own void later on
        playerInfoBox = new PlayerInfoBox[gameManager.enemyAmount];
        for(int i = 0; i < gameManager.partyMembers; i++)
        {
            GameObject obj = Instantiate(playerInfoPrefab);
            obj.transform.SetParent(partyMemberInfoBoxes);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(-559, -21.8f, 0); //placehodler coordinates
            obj.name = "PlayerInfo_" + i;

            playerInfoBox[i] = obj.GetComponent<PlayerInfoBox>();
            playerInfoBox[i].levelNum.text = "Lv." + gameManager.charControl[i].level.ToString();
        }
        


        attackName = attackNames.GetComponentsInChildren<Text>();
        attackNamePos = new Transform[attackName.Length];
        for(int i = 0; i < attackNamePos.Length; i++) 
        {
            attackNamePos[i] = attackName[i].GetComponent<Transform>();
        }
        attackSelected = Mathf.FloorToInt(atkSelVector.y + (atkSelVector.x * 2));
        attackSelection.position = attackNamePos[attackSelected].position;
        
    
        attackMenu.SetActive(false);
        battleMenu.SetActive(false);
        

        SetAttackScroll();

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (gameManager.isPaused) return;

        if (scrollCooldownCounter <= scrollCooldown + 0.5f) scrollCooldownCounter += Time.deltaTime;
        //Selecting action Command
        if (selecting == SelectingMenu.selectingAction)
		{
			//Put in a timeCounter so it doesn't read every frame
			if(axis.y < 0 && scrollCooldownCounter >= scrollCooldown)
			{
				SelectNextCommand();
				scrollCooldownCounter = 0;
				
			}
			else if(axis.y > 0 && scrollCooldownCounter >= scrollCooldown)
			{
				SelectPreviousCommand();
				scrollCooldownCounter = 0;
			}
		}
        else if (selecting == SelectingMenu.selectingMove)
        {
            if(tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;
            //Controls for moving around the selected tiles.
            if(tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
            {
                FormationMovement();
            }
        }
        else if(selecting == SelectingMenu.selectingAttack)
        {
            
            
            if(scrollCooldownCounter >= scrollCooldown)
            {
                if(axis != Vector2.zero)
                {
                    CalculateAttackSelectionLimits();
                }

                if(axis.y > 0) //This one needs limits depending on attacksAmoun
                {
                    atkSelVector.y--; 
                    if(atkSelVector.y < 0) atkSelVector.y++;               
                }
                else if(axis.y < 0)
                {
                    atkSelVector.y++; 
                    if(atkSelVector.y >= currentAtkVerticalLimit) atkSelVector.y--;
                }
                if(axis.x > 0) //This one needs limits depending on attacksAmoun
                {
                    atkSelVector.x++;
                    if(atkSelVector.x >= currentAtkHorizontalLimit) atkSelVector.x--;
                }
                else if(axis.x < 0 )
                {
                    atkSelVector.x--;
                    if(atkSelVector.x < 0) atkSelVector.x++;
                }
                if(axis != Vector2.zero)
                {
                    attackSelected = Mathf.FloorToInt(atkSelVector.x + (atkSelVector.y * 2));
                    attackSelection.position = attackNamePos[attackSelected].position;
                    scrollCooldownCounter = 0;
                    UpdateAttackInfo(attackSelected); // Make it get the attackID of the characters selected attack
                    
                }
            }
        }
        else if(selecting == SelectingMenu.selectingTarget)
        {
            if (tileSelectCooldownCounter < 1) tileSelectCooldownCounter += Time.deltaTime;
            //Controls for moving around the selected tiles.
            if (tileSelectCooldownCounter >= 0.25f) // This section below is utter prefection don't touch
            {
                AttackTargetMovement();
            }
            
        }
        else if(selecting == SelectingMenu.victoryScreen)
        {

        }

        //Notification fades after a while
		if(notifShown)
		{
			textFadeCounter += Time.deltaTime;
			if(textFadeCounter >= 3)
			{
                notifAlpha -=  Time.deltaTime;
                notifTextColor.a = notifAlpha; 
                notifBgColor.a = notifAlpha/2;
                battleNotificationText.color = notifTextColor;
                battleNotificationBg.color = notifBgColor;
                if (notifAlpha <= 0)
                {
                    notifAlpha = 0;
                    notifShown = false;
                    notifPanel.SetActive(false);
                }
            }
		}
	}

    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    public void UpdateAttackInfo(int attack)
    {
        //Todo: Change Displayed Description to selected attacks description
        //Check these a bit more in depth.
        SelectedAttackDescription.text = gameManager.attackInfo.attackDescriptions[ gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack] ];

        //Change displayed Power and MP to that of the attack 
        SelectedAttackStats.text = "Power: " + gameManager.attackInfo.attackStrengths[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack]] + System.Environment.NewLine + "MP: " + gameManager.attackInfo.attackMpCosts[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack]];

        //Hard: Change Displayed range/AoE of attack 
    }
    public void CalculateAttackSelectionLimits() // Complete, now even takes attacksAmount in account. 
    {
        if(attackAmount % 2 == 0) //is multiple of 2 (0, 2, 4, 6)
        {
            //Consistent atkHorizontalLimit, always the same, 
            currentAtkHorizontalLimit = 2;
            //and atkVerticalLimit doesn't change regardless of the atkSelVector.x
            currentAtkVerticalLimit = attackAmount / 2; // because thats the absolute max Horizontal limit as ther are only 2 collumns.
        }
        else // for attack amount of 1, 3 and 5.
        {
            //Horizontal limit is dependant on relation between attack amount and atkSelVector.y
            atkHorizontalLimit = 2; 
            
            //This shit is wrong
            if(attackAmount == 1) atkVerticalLimit = 1;
            else if(attackAmount == 3) atkVerticalLimit = 2;
            else if(attackAmount == 5) atkVerticalLimit = 3;   

            if(atkSelVector.x == 0) currentAtkVerticalLimit = atkVerticalLimit;
            else currentAtkVerticalLimit = atkVerticalLimit - 1;   

            if(atkSelVector.y < atkVerticalLimit-1)
            {
                currentAtkHorizontalLimit = atkHorizontalLimit;
            }
            else 
            {
                currentAtkHorizontalLimit = atkHorizontalLimit - 1;
            }
        }
    }

	public void SelectNextCommand()
	{
		if(command <= CommandSelection.Item) command++;
		else command = 0;
		switch (command)
		{
			case CommandSelection.Attack:
				SetAttackScroll();
				break;
			case CommandSelection.Defend:
				SetDefendScroll();
				break;
			case CommandSelection.Move:
				SetMoveScroll();
				break;
			case CommandSelection.Item:
				SetItemScroll();
				break;
			case CommandSelection.Run:
				SetRunScroll();
				break;
			default:
				break;
		}
		
	}

	public void SelectPreviousCommand()
	{
		if(command > 0) command--;
		else command = CommandSelection.Run;
		switch (command)
		{
			case CommandSelection.Attack:
				SetAttackScroll();
				break;
			case CommandSelection.Defend:
				SetDefendScroll();
				break;
			case CommandSelection.Move:
				SetMoveScroll();
				break;
			case CommandSelection.Item:
				SetItemScroll();
				break;
			case CommandSelection.Run:
				SetRunScroll();
				break;
			default:
				break;
		}
		
	}

	public void ConfirmSelectedCommand()
	{
		soundPlayer.PlaySound(0,1, true);
        if(command == CommandSelection.Attack)
		{
            InitiateAttackSelection();

        }
		else if(command == CommandSelection.Defend)
		{
			//Go to Select defend
		}
		else if(command == CommandSelection.Move)
		{
            //Go to move menu
            selecting = SelectingMenu.selectingMove;
            
            tileSelection = gameManager.charControl[0].tile; //0 is a placeholder for now Will be replaced with an Int indicating the active character when that's a thing            
            selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileRowSize));
            selectedTileIndicator.gameObject.SetActive(true);
            selectedTileIndicator.position = gameManager.tileScript.tileTransform[selectedTile].position;
            actionMenu.SetActive(false);
            //gameManager.MoveFormation(0,5);
		}
		else if(command == CommandSelection.Item)
		{
			//Go to Item Menu
		}
		else if(command == CommandSelection.Run)
		{
			//Execute Running away
			gameManager.RunFromBattle();
		}
		else 
		{
			Debug.Log("Tried to confirm an Action command selection but nothing was selected");
		}
	}

    public void ConfirmAttackSelection()
    {
        //This is all going to change once it's an array of course
        soundPlayer.PlaySound(0,1, true);
        selectedTargetVector = Vector2.zero;
        selectedTarget = 0;
        selectedTargetsTransform.gameObject.SetActive(true);
        selectedTargetsTransform.position = gameManager.tileScript.tileTransform[selectedTarget].position;
        attackMenu.SetActive(false);
        selecting = SelectingMenu.selectingTarget;
    }

    public void ConfirmAttackTarget()
    {
        if(gameManager.tileScript.tiles[selectedTarget].isOccupied)
        {
            soundPlayer.PlaySound(0,1, true);
            for (int i = 0; i < gameManager.enemyAmount; i++) //4 is a placeholder for enemy amount
            {
                if(gameManager.enemyControl[i].tileID == selectedTarget)
                {
                    gameManager.enemyControl[i].Damage(gameManager.attackInfo.attackStrengths[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attackSelected]]);
                    
                }

            }
            //gameManager.charControl[gameManager.activeCharacter].UseMp(gameManager.attackInfo.attackMpCosts[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attackSelected]]);
        }
        else soundPlayer.PlaySound(2,1, true);
        
    }

	public void ReturnToCommandSelection()
	{
        soundPlayer.PlaySound(1,1, true);
        selecting = SelectingMenu.selectingAction;
        selectedTileIndicator.gameObject.SetActive(false);
        attackMenu.SetActive(false);
        partyInfo.SetActive(true);
        actionMenu.SetActive(true);
	}

    public void ReturnToAttackSelect()
    {
        soundPlayer.PlaySound(1,1, true);
        InitiateAttackSelection(); 
    }

    public void InitiateAttackSelection()
    {
        selecting = SelectingMenu.selectingAttack;

        atkSelVector = Vector2.zero;
        attackSelected = Mathf.FloorToInt(atkSelVector.y + (atkSelVector.x * 2));
        attackSelection.position = attackNamePos[attackSelected].position;
        UpdateAttackInfo(attackSelected); // placeholder, attack selected will take the attack from attack array in charControl to get everything right

        for (int i = 0; i < gameManager.charControl[gameManager.activeCharacter].maxAttacks; i++)
        {
            attackName[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < attackAmount; i++)
        {
            attackName[i].gameObject.SetActive(true);
            //Fix this?
            attackName[i].text = gameManager.attackInfo.attackNames[gameManager.charControl[gameManager.activeCharacter].attacksLearned[i]];
        }

        selectedTarget = 0;
        selectedTargetsTransform.position = gameManager.tileScript.tileTransform[selectedTarget].position;
        selectedTargetsTransform.gameObject.SetActive(false);
        
        partyInfo.SetActive(false);
        attackMenu.SetActive(true);
    }

    public void UpdateEnemyBars(int enemyID)
    {
        //Percentage of life = (life*100)/MaxHP
        float lifePercent = (gameManager.enemyControl[enemyID].currentHp*100)/ gameManager.enemyControl[enemyID].hp;
        float mpPercent = (gameManager.enemyControl[enemyID].currentMp*100)/ gameManager.enemyControl[enemyID].mp;

        float hpWidth = (lifePercent * maxEnemyBarWidth)/100;
        float hpPosX = ((maxEnemyBarPosX * lifePercent) / 100) - maxEnemyBarPosX;

        float mpWidth = (mpPercent * maxEnemyBarWidth)/100;
        float mpPosX = ((maxEnemyBarPosX * mpPercent) / 100) - maxEnemyBarPosX;

        //HP Bar 
        enemyInfoPopUp[enemyID].barTransform[0].sizeDelta = new Vector2(hpWidth, 15.42f);
        enemyInfoPopUp[enemyID].barTransform[0].localPosition = new Vector2(hpPosX, 0);
        //MP Bar
        enemyInfoPopUp[enemyID].barTransform[1].sizeDelta = new Vector2(mpWidth, 11.5f);
        
    }
    public void UpdatePlayerBars(int playerID)
    {
        float lifePercent = (gameManager.charControl[playerID].currentHp * 100) / gameManager.charControl[playerID].hp;
        float mpPercent = (gameManager.charControl[playerID].currentMp * 100) / gameManager.charControl[playerID].mp;

        float hpWidth = (lifePercent * maxPlayerBarWidth) / 100;
        float hpPosX = ((maxPlayerBarPosX * lifePercent) / 100) - maxPlayerBarPosX;

        float mpWidth = (mpPercent * maxPlayerBarWidth) / 100;
        float mpPosX = ((maxPlayerBarPosX * mpPercent) / 100) - maxPlayerBarPosX;

        //HP Bar 
        playerInfoBox[playerID].barTransform[0].sizeDelta = new Vector2(hpWidth, 42.86f);
        playerInfoBox[playerID].barTransform[0].localPosition = new Vector2(hpPosX, 0);
        //MP Bar
        playerInfoBox[playerID].barTransform[1].sizeDelta = new Vector2(mpWidth, 42.86f);
        playerInfoBox[playerID].barTransform[1].localPosition = new Vector2(mpPosX, 0);
    }

    public void ChangeNotifText(string notifText)
	{
		notifShown = true;
        notifPanel.SetActive(true);
        battleNotificationText.text = notifText;
        notifTextColor.a = 1;
        notifBgColor.a = 0.5f;
        battleNotificationText.color = notifTextColor;
        battleNotificationBg.color = notifBgColor;
        textFadeCounter = 0;
        notifAlpha = 1;
        
	}
    public void FormationMovement()
    {
        if (axis.x > 0) //Right
        {
            if (axis.y > 0) //Upright
            {
                //tile - ytiles
                tileSelection.x--;

                if (tileSelection.x < startLimit.x) 
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

                if (tileSelection.y >= endLimit.y) 
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
                if (tileSelection.x < startLimit.x || tileSelection.y >= endLimit.y)
                {
                    tileSelection.x++;
                    tileSelection.y--;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
        }
        else if (axis.x < 0) //Left
        {
            if (axis.y > 0) //UpLeft
            {
                //tile - 1
                tileSelection.y--;

                if (tileSelection.y < startLimit.y) 
                {
                    tileSelection.y++;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                tileSelection.x++;
                if (tileSelection.x >= endLimit.x) 
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
                if (tileSelection.x >= endLimit.x || tileSelection.y < startLimit.y)
                {
                    tileSelection.x--;
                    tileSelection.y++;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            tileSelection.x--;
            tileSelection.y--;
            if (tileSelection.x < startLimit.x || tileSelection.y < startLimit.y)
            {
                tileSelection.x++;
                tileSelection.y++;
                soundPlayer.PlaySound(2,1, true);
            }

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            tileSelection.x++;
            tileSelection.y++;
            if (tileSelection.x >= endLimit.x || tileSelection.y >= endLimit.y)
            {
                tileSelection.x--;
                tileSelection.y--;
                soundPlayer.PlaySound(2,1, true);
            }

        }

        if (axis.x != 0 || axis.y != 0)
        {
            selectedTile = Mathf.FloorToInt(tileSelection.y + (tileSelection.x * tileRowSize));
            selectedTileIndicator.position = gameManager.tileScript.tileTransform[selectedTile].position;

            tileSelectCooldownCounter = 0;
            //gameManager.MoveFormation(0, selectedTile);
        }
    }

    public void AttackTargetMovement()
    {
        if (axis.x > 0) //Right
        {
            if (axis.y > 0) //Upright
            {
                //tile - ytiles
                selectedTargetVector.x--;

                if (selectedTargetVector.x < startTargetLimit.x) 
                {
                    selectedTargetVector.x++;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else
            if (axis.y < 0) //DownRight
            {
                //tile + 1
                //I don't even know anymore
                selectedTargetVector.y++;

                if (selectedTargetVector.y >= endTargetLimit.y) 
                {
                    selectedTargetVector.y--;
                    soundPlayer.PlaySound(2,1, true);
                }
            }
            else //Right
            {
                //tile - ytiles + 1
                selectedTargetVector.x--;
                selectedTargetVector.y++;
                if (selectedTargetVector.x < startTargetLimit.x || selectedTargetVector.y >= endTargetLimit.y)
                {
                    selectedTargetVector.x++;
                    selectedTargetVector.y--;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
        }
        else if (axis.x < 0) //Left
        {
            if (axis.y > 0) //UpLeft
            {
                //tile - 1
                selectedTargetVector.y--;

                if (selectedTargetVector.y < startTargetLimit.y) 
                {
                    selectedTargetVector.y++;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                selectedTargetVector.x++;
                if (selectedTargetVector.x >= endTargetLimit.x) 
                {
                    selectedTargetVector.x--;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
            else //Left
            {
                //tile + ytiles - 1
                selectedTargetVector.x++;
                selectedTargetVector.y--;
                if (selectedTargetVector.x >= endTargetLimit.x || selectedTargetVector.y < startTargetLimit.y)
                {
                    selectedTargetVector.x--;
                    selectedTargetVector.y++;
                    soundPlayer.PlaySound(2,1, true);
                }

            }
        }
        else if (axis.y > 0) //Up
        {
            //tile - ytiles - 1
            selectedTargetVector.x--;
            selectedTargetVector.y--;
            if (selectedTargetVector.x < startTargetLimit.x || selectedTargetVector.y < startTargetLimit.y)
            {
                selectedTargetVector.x++;
                selectedTargetVector.y++;
                soundPlayer.PlaySound(2,1, true);
            }

        }
        else if (axis.y < 0) //Down
        {
            //tile + ytiles + 1
            selectedTargetVector.x++;
            selectedTargetVector.y++;
            if (selectedTargetVector.x >= endTargetLimit.x || selectedTargetVector.y >= endTargetLimit.y)
            {
                selectedTargetVector.x--;
                selectedTargetVector.y--;
                soundPlayer.PlaySound(2,1, true);
            }

        }

        if (axis.x != 0 || axis.y != 0)
        {
            selectedTarget = Mathf.FloorToInt(selectedTargetVector.y + (selectedTargetVector.x * tileRowSize));
            selectedTargetsTransform.position = gameManager.tileScript.tileTransform[selectedTarget].position;

            tileSelectCooldownCounter = 0;
            
        }
    }

	#region Sets

	void SetAttackScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , 138.3f );
		
		
	}
	void SetDefendScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , 65.3f );
		
	}
	void SetMoveScroll()
	{
		selectionImage_trans.localPosition = new Vector2( 0 , -7.79f );
		
		
	}
	void SetItemScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , -80.8f );
		
	}
	void SetRunScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , -153.9f );
		
	}

	#endregion
}
