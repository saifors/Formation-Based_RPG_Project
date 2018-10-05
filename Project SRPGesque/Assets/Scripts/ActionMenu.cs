using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour {

	public enum CommandSelection {Attack, Defend, Move, Item, Run};
	public CommandSelection command;

	public float axis;
	public bool selectingAction;

	public float scrollCooldownCounter;
	public float scrollCooldown;

	[Header("Images behind the selections")]
	public CanvasGroup AttackImage;
	public CanvasGroup DefenseImage;
	public CanvasGroup MoveImage;
	public CanvasGroup ItemImage;
	public CanvasGroup RunImage;

	// Use this for initialization
	void Start () 
	{
		//To test menu stuff.
		selectingAction = true;
		scrollCooldown = 0.3f ;
		SetAttackScroll();

	}
	
	// Update is called once per frame
	void Update () 
	{
		axis = Input.GetAxis("Vertical");
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
		}
		else 
		{
			Debug.Log("Tried to confirm an Action command selection but nothing was selected");
		}
	}

	public void ReturnToCommandSelection()
	{

	}
	public void SetCommandAlphaToZero()
	{
		AttackImage.alpha = 0;
		DefenseImage.alpha = 0;
		MoveImage.alpha = 0;
		ItemImage.alpha = 0;
		RunImage.alpha = 0;
	}

	#region Sets

	void SetAttackScroll()
	{
		
		SetCommandAlphaToZero();
		AttackImage.alpha = 1;
		
	}
	void SetDefendScroll()
	{
		
		SetCommandAlphaToZero();
		DefenseImage.alpha = 1;
	}
	void SetMoveScroll()
	{
		
		SetCommandAlphaToZero();
		MoveImage.alpha = 1;
	}
	void SetItemScroll()
	{
		
		SetCommandAlphaToZero();
		ItemImage.alpha = 1;
	}
	void SetRunScroll()
	{
		
		SetCommandAlphaToZero();
		RunImage.alpha = 1;
	}

	#endregion
}
