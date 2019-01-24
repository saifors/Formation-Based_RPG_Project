using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour 
{
	private Vector2 axis;

    [Header("Action Scroll")]
    public float scrollCooldown;
    private float scrollCooldownCounter;
    public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;
	
	
	public GameManager gameManager;

	[Header("Notifications")]
    public GameObject notifPanel;
    private Color notifTextColor;
	private Color notifBgColor;
	private float textFadeCounter;
    private float notifAlpha;
    public bool notifShown;
   
    [Header("Menus")]
    public GameObject battleMenu;
    public GameObject actionMenu;
    public GameObject attackMenu;
    public GameObject partyInfo;
    public GameObject victoryPanel;
    

    [Header("Attack Menu")]
    //External Sources definition
    public GameObject attackNames;
    public Text[] attackName;
    private Transform[] attackNamePos;
    public Text SelectedAttackDescription;
    public Text SelectedAttackStats;

    //Attack Option Selection
    public int attackOptionAmount;
    public Vector2 atkSelVector;
    public int attackOptionSelected;
    //Attack Option Selection Limits
    public int atkHorizontalLimit;//depending on atkSelVector.y and attacksAmount of charControl is either 1 or 2.
    public int atkVerticalLimit; // depenidng on atkSelVector.x and attacksAmount of charControl is between 1 and three
    public int currentAtkHorizontalLimit; 
    public int currentAtkVerticalLimit; 

    [Header("Images behind the selections")]
	public CanvasGroup selectionImage;
	public Transform selectionImage_trans;
    public Transform attackSelection;
	public Text battleNotificationText;
    public Image battleNotificationBg;
    public SoundPlayer soundPlayer;
    [Header("HP, MP and SP Bars")]
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
		
		scrollCooldown = 0.3f ;

		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		selectionImage_trans = selectionImage.GetComponent<RectTransform>();
        notifTextColor = battleNotificationText.color;
        notifBgColor = battleNotificationBg.color;
        
        soundPlayer = gameManager.soundPlayer;
        
        maxEnemyBarWidth = 170;
        maxEnemyBarPosX = 85;        


        attackName = attackNames.GetComponentsInChildren<Text>();
        attackNamePos = new Transform[attackName.Length];
        for(int i = 0; i < attackNamePos.Length; i++) 
        {
            attackNamePos[i] = attackName[i].GetComponent<Transform>();
        }
        attackOptionSelected = Mathf.FloorToInt(atkSelVector.y + (atkSelVector.x * 2));
        attackSelection.position = attackNamePos[attackOptionSelected].position;
        
    
        attackMenu.SetActive(false);
        battleMenu.SetActive(false);
        

        SetAttackScroll();

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
                    attackOptionSelected = Mathf.FloorToInt(atkSelVector.x + (atkSelVector.y * 2));
                    attackSelection.position = attackNamePos[attackOptionSelected].position;
                    scrollCooldownCounter = 0;
                    UpdateAttackInfo(attackOptionSelected); // Make it get the attackID of the characters selected attack
                    
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

    public void InitializeInfoBoxes()
    {
        playerInfoBox = new PlayerInfoBox[gameManager.partyMembers];
        for (int i = 0; i < gameManager.partyMembers; i++)
        {
            GameObject obj = Instantiate(playerInfoPrefab);
            obj.transform.SetParent(partyMemberInfoBoxes);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(-559, -21.8f, 0); //placehodler coordinates
            obj.name = "PlayerInfo_" + i;

            playerInfoBox[i] = obj.GetComponent<PlayerInfoBox>();
            playerInfoBox[i].levelNum.text = "Lv." + gameManager.charControl[i].level.ToString();
        }

		enemyInfoPopUp = new EnemyInfoPopUp[gameManager.enemyAmount];
        for (int i = gameManager.partyMembers; i < gameManager.enemyAmount + gameManager.partyMembers; i++)
        {
            GameObject obj = Instantiate(enemyInfoPrefab);

            
            
            obj.transform.position = new Vector3(238, 168, 0); //placehodler coordinates
            obj.name = "EnemyInfo_" + i;
            enemyInfoPopUp[i - gameManager.partyMembers] = obj.GetComponent<EnemyInfoPopUp>();
            enemyInfoPopUp[i - gameManager.partyMembers].levelText.text = gameManager.charControl[i].level.ToString();

            //Debug.Log("Testing this shit");

        }
        
        
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

    public void ReturnToAttackSelect()
    {
        soundPlayer.PlaySound(1,1, true);
        InitiateAttackSelection(); 
    }

    public void InitiateAttackSelection()
    {
        gameManager.selecting = GameManager.SelectingMenu.selectingAttack;

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
            //Fix this?
            attackName[i].text = gameManager.attackInfo.attackNames[gameManager.charControl[gameManager.activeCharacter].attacksLearned[i]];
        }

        //gameManager.selectedTarget[] = 0;
        //gameManager.selectedTargetsTransform[].position = gameManager.tileScript.tileTransform[selectedTarget].position;
        //gameManager.selectedTargetsTransform[].gameObject.SetActive(false);
        
        partyInfo.SetActive(false);
        attackMenu.SetActive(true);
    }

    public void UpdateLifeBars(int charID)
    {
        //Percentage of life = (life*100)/MaxHP
        float lifePercent = (gameManager.charControl[charID].currentHp*100)/ gameManager.charControl[charID].hp;
        float mpPercent = (gameManager.charControl[charID].currentMp*100)/ gameManager.charControl[charID].mp;

		float maxWidth;

		Vector2 hpSize;
		Vector2 mpSize;

		if(gameManager.charControl[charID].alliance == CharacterStats.Alliance.Player)
		{
			hpSize.y = playerInfoBox[charID].barTransform[0].sizeDelta.x;
			mpSize.y = playerInfoBox[charID].barTransform[1].sizeDelta.x;

			maxWidth = 235;
		}
		else
		{
			hpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[0].sizeDelta.y;
			mpSize.y = enemyInfoPopUp[charID - gameManager.partyMembers].barTransform[1].sizeDelta.y;

			maxWidth = 170;
		}
		hpSize.y = 15.42f;
		mpSize.y = 11.5f;

		hpSize.x = maxWidth/lifePercent * 100;

		mpSize.x = maxWidth / mpPercent * 100;

		//HP Bar 
		if(charID < gameManager.partyMembers)
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

		//EJEMPLO
		/*
		RectTransform rect;
		Vector2 size = rect.sizeDelta;
		maxSize = size.x;//width
		minSize = 0;

		rect.sizeDelta = size;*/
        
    }

    public void ChangeNotifText(string notifText) //This belongs here
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
