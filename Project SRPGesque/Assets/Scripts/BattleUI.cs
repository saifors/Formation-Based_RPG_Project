﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleUI : MonoBehaviour 
{
	private Vector2 axis;

    [Header("Action Scroll")]
    public float scrollCooldown;
    private float scrollCooldownCounter;
    public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;
	public TextMeshProUGUI[] actions;
	
	
	public GameManager gameManager;
	public BattleTransition battleTransition;

	[Header("Notifications")]
    public GameObject notifPanel;
	private RectTransform notifTrans;
	private Vector2 notifStartMeasurements;
    private Color notifTextColor;
	private Color notifBgColor;
	private float textFadeCounter;
    private float notifAlpha;
    public bool notifShown;
   
    [Header("Menus")]
    public GameObject battleMenu;
    public GameObject actionMenu;
    public GameObject attackMenu;
	public GameObject itemMenu;
	public ItemBoxBattle itemBox;
    public GameObject partyInfo;
    public GameObject victoryPanel;
	public VictoryPanel vicPanel;
	public GameObject turnPanel;
	public TurnAnims turnPanelAnim;

	[Header("Attack Menu")]
    //External Sources definition
    public GameObject attackNames;
    public TextMeshProUGUI[] attackName;
    private RectTransform[] attackNamePos;
    public TextMeshProUGUI SelectedAttackDescription;
    public TextMeshProUGUI SelectedAttackStats;
	public Sprite[] portraitSprites;

    //Attack Option Selection
    public int attackOptionAmount;
    public Vector2 atkSelVector;
    public int attackOptionSelected;
    //Attack Option Selection Limits
    public int atkHorizontalLimit;//depending on atkSelVector.y and attacksAmount of charControl is either 1 or 2.
    public int atkVerticalLimit; // depenidng on atkSelVector.x and attacksAmount of charControl is between 1 and three
    public int currentAtkHorizontalLimit; 
    public int currentAtkVerticalLimit;

	[Header("Item Menu")]
	public Vector2 itemSelVector;
	public int itemVertLimit;

	[Header("Images behind the selections")]
	public CanvasGroup selectionImage;
	public Transform selectionImage_trans;
    public RectTransform attackSelection;
	public TextMeshProUGUI battleNotificationText;
    public Image battleNotificationBg;
    public SoundPlayer soundPlayer;
    [Header("HP, MP and SP Bars")]
    
    public GameObject enemyInfoPrefab;
    public EnemyInfoPopUp[] enemyInfoPopUp;
    public Transform partyMemberInfoBoxes;
    public float maxPlayerBarWidth = 235.59f;
    public float maxPlayerBarPosX = 0;
    public GameObject playerInfoPrefab;
    public PlayerInfoBox[] playerInfoBox;
	public bool finishedHPBarAnim;

    // Use this for initialization
    public void Init () 
	{
		//To test menu stuff.
		
		scrollCooldown = 0.3f ;

		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		selectionImage_trans = selectionImage.GetComponent<RectTransform>();
		notifTrans = notifPanel.GetComponent<RectTransform>();
		notifStartMeasurements = new Vector2(0,138);
        notifTextColor = battleNotificationText.color;
        notifBgColor = battleNotificationBg.color;

		battleTransition = GetComponentInChildren<BattleTransition>();
		battleTransition.Init(gameManager);

		turnPanelAnim = GetComponentInChildren<TurnAnims>();
		turnPanel = turnPanelAnim.gameObject;
		turnPanelAnim.Init(gameManager);
		turnPanel.SetActive(false);
        
        soundPlayer = gameManager.soundPlayer;

		itemBox = itemMenu.GetComponent<ItemBoxBattle>();
		itemBox.Init(gameManager);
		vicPanel = victoryPanel.GetComponent<VictoryPanel>();
		vicPanel.Init();

        attackName = attackNames.GetComponentsInChildren<TextMeshProUGUI>();
        attackNamePos = new RectTransform[attackName.Length];
        for(int i = 0; i < attackNamePos.Length; i++) 
        {
            attackNamePos[i] = attackName[i].GetComponent<RectTransform>();
        }
        attackOptionSelected = Mathf.FloorToInt(atkSelVector.y + (atkSelVector.x * 2));
        attackSelection.localPosition = attackNamePos[attackOptionSelected].localPosition;

		LoadActions();
		SetAttackScroll();
    
        attackMenu.SetActive(false);
        battleMenu.SetActive(false);
        actionMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
        //if (gameManager.isPaused) return;

        if (scrollCooldownCounter <= scrollCooldown + 0.5f) scrollCooldownCounter += Time.deltaTime;
        //Selecting action Command
        if (gameManager.selecting == GameManager.SelectingMenu.selectingAction)
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
        
        else if(gameManager.selecting == GameManager.SelectingMenu.selectingAttack)
        {
            
            
            if(scrollCooldownCounter >= scrollCooldown)
            {
                if(axis != Vector2.zero)
                {
                    CalculateAttackSelectionLimits();
                }

                if(axis.y > 0) //This one needs limits depending on attacksAmoun
                {
                    atkSelVector.y--; //Up
                    if(atkSelVector.y < 0) atkSelVector.y++;               
                }
                else if(axis.y < 0)
                {
                    atkSelVector.y++; //Down
                    if(atkSelVector.y >= currentAtkVerticalLimit) atkSelVector.y--;
                }
                if(axis.x > 0) //This one needs limits depending on attacksAmoun
                {
                    atkSelVector.x++; // Right
                    if(atkSelVector.x >= currentAtkHorizontalLimit) atkSelVector.x--;
                }
                else if(axis.x < 0 )
                {
                    atkSelVector.x--; //Left
                    if(atkSelVector.x < 0) atkSelVector.x++;
                }
                if(axis != Vector2.zero)
                {
                    if (attackOptionAmount % 2 != 0) //odd
                    {
                        if (atkSelVector.x >= atkHorizontalLimit-1 && atkSelVector.y >= atkVerticalLimit-1)//make it so diagonal won't work to bug shit.
                        {
                            atkSelVector.x--;
                        }
                    }
					gameManager.soundPlayer.PlaySound(3, true);
					attackOptionSelected = Mathf.FloorToInt(atkSelVector.x + (atkSelVector.y * 2));
					attackSelection.localPosition = attackNamePos[attackOptionSelected].localPosition;
                    scrollCooldownCounter = 0;
                    UpdateAttackInfo(attackOptionSelected); // Make it get the attackID of the characters selected attack
                    
                }
            }
        }
		else if (gameManager.selecting == GameManager.SelectingMenu.selectingItem)
		{
			if (scrollCooldownCounter >= scrollCooldown)
			{
				if(axis.x > 0)
				{
					itemSelVector.x++;
				}
				else if (axis.x < 0)
				{
					itemSelVector.x--;
				}
				if (axis.y > 0)
				{
					itemSelVector.y--;
				}
				else if (axis.y < 0)
				{
					itemSelVector.y++;
				}
				if (axis != Vector2.zero)
				{
					if (itemBox.itemTexts.Length != 0)
					{
						if (itemBox.itemTexts.Length < 2 && itemSelVector.x > 0)
						{
							itemSelVector.x--;
						}
						if (itemSelVector.x < 0) itemSelVector.x++;
						else if (itemSelVector.x > 1) itemSelVector.x--;

						if (itemSelVector.y < 0) itemSelVector.y++;

						if (itemSelVector.x == 0)
						{
							itemVertLimit = itemBox.vertLimits[0];
						}
						else
						{
							itemVertLimit = itemBox.vertLimits[1];
						}

						if (itemSelVector.y > itemVertLimit - 1)
						{
							itemSelVector.y--;
						}

						scrollCooldownCounter = 0;
						gameManager.soundPlayer.PlaySound(3, true);
						itemBox.selected = Mathf.FloorToInt(itemSelVector.x + (itemSelVector.y * 2)); ;
						itemBox.PlaceSelection();
					}
				}
			}

		}
		else if(gameManager.selecting == GameManager.SelectingMenu.selectingTarget)
        {
            
            
        }
        else if(gameManager.selecting == GameManager.SelectingMenu.victoryScreen)
        {

        }

        //Notification fades after a while
		if(notifShown)
		{
			textFadeCounter += Time.deltaTime;
			if(textFadeCounter >= 1.5f)
			{
                notifAlpha -=  Time.deltaTime * 2;
                notifTextColor.a = notifAlpha; 
                notifBgColor.a = notifAlpha;
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

	private void LoadActions()
	{
		actions[0].text = LanguageManager.langData.actions["Action_Atk"];
		actions[1].text = LanguageManager.langData.actions["Action_Def"];
		actions[2].text = LanguageManager.langData.actions["Action_Mov"];
		actions[3].text = LanguageManager.langData.actions["Action_Itm"];
		actions[4].text = LanguageManager.langData.actions["Action_Run"];
	}

    public void InitializeInfoBoxes()
    {
		partyInfo.SetActive(true);
		playerInfoBox = new PlayerInfoBox[gameManager.partyMembers];
        for (int i = 0; i < gameManager.partyMembers; i++)
        {
            GameObject obj = Instantiate(playerInfoPrefab);
            obj.transform.SetParent(partyMemberInfoBoxes);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(418*i, 0, 0); //placehodler coordinates
            obj.name = "PlayerInfo_" + i;

            playerInfoBox[i] = obj.GetComponent<PlayerInfoBox>();
            playerInfoBox[i].levelNum.text = "Lv." + gameManager.charControl[i].level.ToString();
			playerInfoBox[i].portrait.sprite = portraitSprites[gameManager.gameData.Party[i].portraitId];

        }

		enemyInfoPopUp = new EnemyInfoPopUp[gameManager.enemyAmount];
        for (int i = gameManager.partyMembers; i < gameManager.enemyAmount + gameManager.partyMembers; i++)
        {
            GameObject obj = Instantiate(enemyInfoPrefab);
			//Probably in need of optimiation.
			Canvas objCanvas = obj.GetComponent<Canvas>();
			objCanvas.worldCamera = gameManager.cam;
			Transform objTrans;
			objTrans = obj.GetComponent<Transform>();

            objTrans.position = gameManager.charControl[i].transform.position;
			objTrans.position = new Vector3(objTrans.position.x, objTrans.position.y + 1.5f, objTrans.position.z);
            objTrans.localScale = new Vector3(0.0055f, 0.006f, 0.006f);
            objTrans.eulerAngles = new Vector3(0,45,0);

			obj.name = "EnemyInfo_" + i;
            enemyInfoPopUp[i - gameManager.partyMembers] = obj.GetComponent<EnemyInfoPopUp>();
            enemyInfoPopUp[i - gameManager.partyMembers].levelText.text = gameManager.charControl[i].level.ToString();

            //Debug.Log("Testing this shit");

        }
        
        for(int i = 0; i < gameManager.partyMembers + gameManager.enemyAmount; i++)
		{
			LoadLifeBars(i);
		}
    }

    public void UpdateAttackInfo(int attack)
    {
        //Todo: Change Displayed Description to selected attacks description
        //Check these a bit more in depth.
        string descKey = gameManager.gameData.AttackList[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack]].descKey;
        SelectedAttackDescription.text = LanguageManager.langData.attackDesc[descKey];
		//Change displayed Power and MP to that of the attack 
		SelectedAttackStats.text = "Power: " + gameManager.gameData.AttackList[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack]].strength + System.Environment.NewLine + "MP: " + gameManager.gameData.AttackList[gameManager.charControl[gameManager.activeCharacter].attacksLearned[attack]].mpCost;

        //Hard: Change Displayed range/AoE of attack 
    }
    public void CalculateAttackSelectionLimits() // Complete, now even takes attacksAmount in account. 
    {
        if(attackOptionAmount % 2 == 0) //is multiple of 2 (0, 2, 4, 6)
        {
            //Consistent atkHorizontalLimit, always the same, 
            currentAtkHorizontalLimit = 2;
            //and atkVerticalLimit doesn't change regardless of the atkSelVector.x
            currentAtkVerticalLimit = attackOptionAmount / 2; // because thats the absolute max Horizontal limit as ther are only 2 collumns.
        }
        else // for attack amount of 1, 3 and 5.
        {
            //Horizontal limit is dependant on relation between attack amount and atkSelVector.y
            atkHorizontalLimit = 2; 
            
            
            if(attackOptionAmount == 1) atkVerticalLimit = 1;
            else if(attackOptionAmount == 3) atkVerticalLimit = 2;
            else if(attackOptionAmount == 5) atkVerticalLimit = 3;   

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

	public void OpenItemMenu()
	{
		itemMenu.SetActive(true);
		actionMenu.SetActive(false);
		partyInfo.SetActive(false);
		itemSelVector = Vector2.zero;
		itemBox.CalculateItemBox();
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
		gameManager.soundPlayer.PlaySound(3, true);
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
		gameManager.soundPlayer.PlaySound(3, true);
	}

    public void ReturnToAttackSelect()
    {
        soundPlayer.PlaySound(1, true);
        InitiateAttackSelection(); 
    }

    public void InitiateAttackSelection()
    {
        gameManager.selecting = GameManager.SelectingMenu.selectingAttack;
		attackOptionAmount = gameManager.charControl[gameManager.activeCharacter].attacksAmount;

        atkSelVector = Vector2.zero;
        attackOptionSelected = Mathf.FloorToInt(atkSelVector.y + (atkSelVector.x * 2));
        attackSelection.position = attackNamePos[attackOptionSelected].position;
        UpdateAttackInfo(attackOptionSelected); // placeholder, attack selected will take the attack from attack array in charControl to get everything right

        for (int i = 0; i < gameManager.charControl[gameManager.activeCharacter].maxAttacks; i++)
        {
            attackName[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < attackOptionAmount; i++)
        {
            attackName[i].gameObject.SetActive(true);
            string nameKey = gameManager.gameData.AttackList[gameManager.charControl[gameManager.activeCharacter].attacksLearned[i]].nameKey;
            attackName[i].text = LanguageManager.langData.attackName[nameKey];
        }
		/*
		for (int i = 0; i < gameManager.gameData.AttackList.Count; i++)
		{
			Debug.Log(i + "iteration " + gameManager.gameData.AttackList[i].id);
			Debug.Log(gameManager.gameData.AttackList[i].nameKey);
		}*/
		
        //gameManager.selectedTarget[] = 0;
        //gameManager.selectedTargetsTransform[].position = gameManager.tileScript.tileTransform[selectedTarget].position;
        //gameManager.selectedTargetsTransform[].gameObject.SetActive(false);
        
        partyInfo.SetActive(false);
        attackMenu.SetActive(true);
    }

	public void PlayerTurnUIChange()
	{
		actionMenu.SetActive(true);
		partyInfo.SetActive(true);
		attackMenu.SetActive(false);
		turnPanel.SetActive(true);
		turnPanelAnim.StartTurnAnim(gameManager.charControl[gameManager.activeCharacter].name, CharacterStats.Alliance.Player);
	}
	public void EnemyTurnUIChange()
	{
		actionMenu.SetActive(false);
		partyInfo.SetActive(true);
		attackMenu.SetActive(false);
		turnPanel.SetActive(true);
		turnPanelAnim.StartTurnAnim(gameManager.charControl[gameManager.activeCharacter].name, CharacterStats.Alliance.Enemy);
	}
	public void VictorySequence()
	{
		victoryPanel.SetActive(true);
		actionMenu.SetActive(false);
		partyInfo.SetActive(false);
		vicPanel.DisplayVictory();
	}

	public void LoadLifeBars(int charID)
	{
		//Percentage of life = (life*100)/MaxHP
		float lifePercent = (gameManager.charControl[charID].currentHp * 100) / gameManager.charControl[charID].hp;
		float mpPercent = (gameManager.charControl[charID].currentMp * 100) / gameManager.charControl[charID].mp;

		float maxWidth;

		Vector2 hpSize;
		Vector2 mpSize;

		if (gameManager.charControl[charID].alliance == CharacterStats.Alliance.Player)
		{
			hpSize.y = playerInfoBox[charID].barTransform[0].sizeDelta.y;
			mpSize.y = playerInfoBox[charID].barTransform[1].sizeDelta.y;

			maxWidth = 235;

			playerInfoBox[charID].barText[0].text = gameManager.charControl[charID].currentHp + "/" + gameManager.charControl[charID].hp;
			playerInfoBox[charID].barText[1].text = gameManager.charControl[charID].currentMp + "/" + gameManager.charControl[charID].mp;
		}
		else
		{
			hpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].sizeDelta.y;
			mpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].sizeDelta.y;

			maxWidth = 175;
		}

		hpSize.x = maxWidth * lifePercent / 100;

		mpSize.x = maxWidth * mpPercent / 100;

		//HP Bar 
		if (charID < gameManager.partyMembers)
		{
			playerInfoBox[charID].barTransform[0].sizeDelta = hpSize;
			playerInfoBox[charID].barTransform[1].sizeDelta = mpSize;
		}
		else
		{

			enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].sizeDelta = hpSize;
			//MP Bar
			enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].sizeDelta = mpSize;
		}
	}

	public void UpdateLifeBars(int charID)
    {
		UpdateHPBar(charID);
		UpdateMPBar(charID);
	}

	public void UpdateHPBar(int charID)
	{
		finishedHPBarAnim = false;

		float lifePercent = (gameManager.charControl[charID].currentHp * 100) / gameManager.charControl[charID].hp;
		
		float maxWidth;

		Vector2 hpSize;
		
		if (gameManager.charControl[charID].alliance == CharacterStats.Alliance.Player)
		{
			hpSize.y = playerInfoBox[charID].barTransform[0].sizeDelta.y;
			
			maxWidth = 235;

			playerInfoBox[charID].barText[0].text = gameManager.charControl[charID].currentHp + "/" + gameManager.charControl[charID].hp;
			}
		else
		{
			hpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].sizeDelta.y;
			maxWidth = 175;
		}

		hpSize.x = maxWidth * lifePercent / 100;

		

		//HP Bar 
		if (charID < gameManager.partyMembers)
		{
			float duration;
			//faster depending on how much goes down
			float oldLifeP = playerInfoBox[charID].barTransform[0].sizeDelta.x * 100 / maxWidth;
			if(oldLifeP - lifePercent > 0) duration = ( oldLifeP - lifePercent )/*Percent difference*/ / 100f * 1.5f; 
			else duration = (lifePercent - oldLifeP) / 100f * 1.5f;
			if (duration < 0.2f) duration = 0.2f;
			playerInfoBox[charID].barTransform[0].DOSizeDelta(hpSize, duration, true).OnComplete(gameManager.charControl[charID].DeathCheck);
		}
		else
		{
			float duration;
			float oldLifeP = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].sizeDelta.x * 100 / maxWidth;
			if (oldLifeP - lifePercent > 0) duration = (oldLifeP - lifePercent)/*Percent difference*/ / 100f * 1.5f;
			else duration = (lifePercent - oldLifeP) / 100f * 1.5f;
			if (duration < 0.2f) duration = 0.2f;
			enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].DOSizeDelta(hpSize, duration, true).OnComplete(gameManager.charControl[charID].DeathCheck);
		}
	}

	public void UpdateMPBar(int charID)
	{
		float mpPercent = (gameManager.charControl[charID].currentMp * 100) / gameManager.charControl[charID].mp;

		float maxWidth;
		
		Vector2 mpSize;

		if (gameManager.charControl[charID].alliance == CharacterStats.Alliance.Player)
		{
			mpSize.y = playerInfoBox[charID].barTransform[1].sizeDelta.y;

			maxWidth = 235;

			playerInfoBox[charID].barText[1].text = gameManager.charControl[charID].currentMp + "/" + gameManager.charControl[charID].mp;
		}
		else
		{
			mpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].sizeDelta.y;

			maxWidth = 175;
		}

		mpSize.x = maxWidth * mpPercent / 100;
		

		//HP Bar 
		if (charID < gameManager.partyMembers)
		{
			float duration;
			float oldMpP = playerInfoBox[charID].barTransform[1].sizeDelta.x * 100 / maxWidth;
			if (oldMpP - mpPercent > 0) duration = (oldMpP - mpPercent)/*Percent difference*/ / 100f * 1.5f;
			else duration = (mpPercent - oldMpP) / 100f * 1.5f;
			if (duration < 0.2f) duration = 0.2f;
			playerInfoBox[charID].barTransform[1].DOSizeDelta(mpSize, duration, true);
		}
		else
		{
			float duration;
			float oldMpP = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].sizeDelta.x * 100 / maxWidth;
			if (oldMpP - mpPercent > 0) duration = (oldMpP - mpPercent)/*Percent difference*/ / 100f * 1.5f;
			else duration = (mpPercent - oldMpP) / 100f * 1.5f;
			if (duration < 0.2f) duration = 0.2f;
			enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].DOSizeDelta(mpSize, duration, true);
		}
	}

    public void ChangeNotifText(string notifText) //This belongs here
	{
		notifShown = true;
        notifPanel.SetActive(true);
		notifTrans.DOSizeDelta(notifStartMeasurements, 0.25f, true).From().SetEase(Ease.OutSine);
        battleNotificationText.text = notifText;
        notifTextColor.a = 1;
        notifBgColor.a = 0.5f;
        battleNotificationText.color = notifTextColor;
        battleNotificationBg.color = notifBgColor;
        textFadeCounter = 0;
        notifAlpha = 1;
        
	}

	public void TurnActiveVisuals(int activeId)
	{
		for (int i = 0; i < playerInfoBox.Length; i++)
		{
			if (i != activeId)
			{
				playerInfoBox[i].canvas.DOFade(0.5f, 0.2f);
				playerInfoBox[i].trans.DOAnchorPosY(0, 0.25f, true);
			}
			else
			{
				playerInfoBox[i].canvas.DOFade(1, 0.2f);
				playerInfoBox[i].trans.DOAnchorPosY(20, 0.25f, true);
			}
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
