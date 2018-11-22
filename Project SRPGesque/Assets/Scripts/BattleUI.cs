using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour {

	public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;

	public float axis;
	public bool selectingAction;

	public float scrollCooldownCounter;
	public float scrollCooldown;
	private GameManager gameManager;

	[Header("Images behind the selections")]
	public CanvasGroup selectionImage;
	public Transform selectionImage_trans;
	public Text battleNotificationText;
	private Color fontColor_Opague;
	private Color fontColor_Transparent;
	private float textFadeCounter;


	// Use this for initialization
	void Start () 
	{
		//To test menu stuff.
		selectingAction = true;
		scrollCooldown = 0.3f ;

		gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		selectionImage_trans = selectionImage.GetComponent<RectTransform>();

		fontColor_Opague = new Color(255,255,255,255);
		fontColor_Transparent = new Color(255,255,255,0);

		SetAttackScroll();

	}
	
	// Update is called once per frame
	void Update () 
	{
		scrollCooldownCounter += Time.deltaTime; 
		if(selectingAction)
		{
			//Put in a timeCounter so it doesn't read every frame
			if(axis < 0 && scrollCooldownCounter >= scrollCooldown)
			{
				SelectNextCommand();
				scrollCooldownCounter = 0;
				
			}
			else if(axis > 0 && scrollCooldownCounter >= scrollCooldown)
			{
				SelectPreviousCommand();
				scrollCooldownCounter = 0;
			}
		}
		if(battleNotificationText.color != fontColor_Transparent)
		{
			textFadeCounter += Time.deltaTime;
			if(textFadeCounter >= 4)
			{
				
				battleNotificationText.color = Color.Lerp(fontColor_Opague, fontColor_Transparent, Mathf.PingPong(Time.time, 1)); // Attention this is where you left off.
			}
		}
	}

    public void SetAxis(float inputAxis)
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

	}

	public void ChangeNotifText(string notifText)
	{
		battleNotificationText.text = notifText;
		battleNotificationText.color = fontColor_Opague;
	}


	#region Sets

	void SetAttackScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , 63.8f );
		
		
	}
	void SetDefendScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , 31.8f );
		
	}
	void SetMoveScroll()
	{
		selectionImage_trans.localPosition = new Vector2( 0 , 0 );
		
		
	}
	void SetItemScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , -32.2f );
		
	}
	void SetRunScroll()
	{
		
		selectionImage_trans.localPosition = new Vector2( 0 , -64.19f );
		
	}

	#endregion
}
