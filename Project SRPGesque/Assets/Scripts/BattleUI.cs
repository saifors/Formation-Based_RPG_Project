using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour {

	public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;

	public Vector2 axis;
    public enum SelectingMenu { selectingAction, selectingAttack, selectingMove};
    public SelectingMenu selecting;

	public float scrollCooldownCounter;
	public float scrollCooldown;
	private GameManager gameManager;

	
    private Color notifTextColor;
	private Color notifBgColor;
	private float textFadeCounter;
    public float notifAlpha;
    public bool notifShown;
    public GameObject notifPanel;
    public GameObject actionMenu; //May be better to get a Game Object array that takes in all the children of batle menu.

    public int tileCollumnSize;
    public int tileRowSize;
    public Vector2 startLimit;
    public Vector2 endLimit;
    public int tileAmount;
    public int selectedTile;
    public Vector2 tileSelection;
    public int calculationTile;
    public float tileSelectCooldownCounter;
    public GameObject cursor;
    private Transform selectedTileIndicator;

    [Header("Images behind the selections")]
	public CanvasGroup selectionImage;
	public Transform selectionImage_trans;
	public Text battleNotificationText;
    public Image battleNotificationBg;

	// Use this for initialization
	void Start () 
	{
		//To test menu stuff.
		selecting = SelectingMenu.selectingAction;
		scrollCooldown = 0.3f ;

		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		selectionImage_trans = selectionImage.GetComponent<RectTransform>();
        notifTextColor = battleNotificationText.color;
        notifBgColor = battleNotificationBg.color;

        GameObject obj;
        obj = Instantiate(cursor);
        selectedTileIndicator = obj.GetComponent<Transform>();
        selectedTileIndicator.gameObject.SetActive(false);

        SetAttackScroll();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(scrollCooldownCounter <= scrollCooldown + 0.5f) scrollCooldownCounter += Time.deltaTime;
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
		if(command == CommandSelection.Attack)
		{
			//Go to attack menu
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

	public void ReturnToCommandSelection()
	{
        selecting = SelectingMenu.selectingAction;
        selectedTileIndicator.gameObject.SetActive(false);
        actionMenu.SetActive(true);
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

                if (tileSelection.x < startLimit.x) tileSelection.x++;

            }
            else
            if (axis.y < 0) //DownRight
            {
                //tile + 1
                //I don't even know anymore
                tileSelection.y++;

                if (tileSelection.y >= endLimit.y) tileSelection.y--;
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
                }

            }
        }
        else if (axis.x < 0) //Left
        {
            if (axis.y > 0) //UpLeft
            {
                //tile - 1
                tileSelection.y--;

                if (tileSelection.y < startLimit.y) tileSelection.y++;

            }
            else if (axis.y < 0) //DownLeft
            {
                // tile + ytiles 
                tileSelection.x++;
                if (tileSelection.x >= endLimit.x) tileSelection.x--;

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
