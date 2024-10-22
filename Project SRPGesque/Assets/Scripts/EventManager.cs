﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public GameManager gameManager;
	private OWPlayerController playerController;
	private DialogueBox dialogueBox;

	public GameObject[] eventObj;
	public EventScript[] events;

	public enum TypeOfEvent { Interaction, Range, StartMap};
	public enum Events { None, Dialogue, Chest, ReceiveItem, Fight, CheckTrigger, CheckOther, DestroyNPC, transitionScene};

	public int currentEvent;

	public int eventProgress;

    public void Init(GameManager gM, OWPlayerController pC)
    {
		gameManager = gM;
		playerController = pC;
		dialogueBox = gameManager.dialogueUI;
		
		GetAllEvents();

		for (int i = 0; i < events.Length; i++)
		{
			if(events[i].eventType == TypeOfEvent.StartMap)
			{
				MapStartEvent(events[i]);
				break;
			}
		}
	}

	

	public void GetAllEvents()
	{
		eventObj = GameObject.FindGameObjectsWithTag("Event");
		events = new EventScript[eventObj.Length];
		for (int i = 0; i < eventObj.Length; i++)
		{
			events[i] = eventObj[i].GetComponent<EventScript>();
			events[i].Init(this, gameManager, i);
		}
	}

	public void Interact()
	{
		//Generate a collision box in front of character to see if there is any Events script in its range.
		Collider[] receivers = playerController.overlap.GetBox();
		if (receivers == null) return;

		EventScript[] receiveEvents = new EventScript[receivers.Length];

		float shortestDistance = 0f;
		int closest = 0;

		for (int i = 0; i < receiveEvents.Length; i++)
		{
			receiveEvents[i] = receivers[i].GetComponent<EventScript>();
			if (receiveEvents[i].eventType == TypeOfEvent.Interaction)
			{

				if (i == 0)
				{
					shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
					closest = i;
				}
				else if (Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position) < shortestDistance)
				{
					shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
					closest = i;
				}
				else if (Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position) == shortestDistance)
				{
					if (Random.Range(0, 1) == 1)
					{
						shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
						closest = i;
					}
				}
			}
		}
		//Fom here on is general stuff.
		currentEvent = receiveEvents[closest].ID;
		playerController.isMoving = false;
		playerController.isRunning = false;

		eventProgress = 0;
		ContinueEvent();
	}

	public void EnterEventRange(EventScript receiveEvent)
	{
		//if (receiveEvent.hasBeenTriggered) return;

		currentEvent = receiveEvent.ID;
		playerController.isMoving = false;
		playerController.isRunning = false;

		eventProgress = 0;
		ContinueEvent();

		
		//Debug.Log("Enter event " + currentEvent);
	}

	public void MapStartEvent(EventScript receiveEvent)
	{
		if (receiveEvent.hasBeenTriggered) return;
		currentEvent = receiveEvent.ID;
		playerController.isMoving = false;
		playerController.isRunning = false;

		eventProgress = 0;
		ContinueEvent();
	}

	public void ContinueEvent()
	{
		gameManager.gameState = GameManager.GameState.Event;
		if (eventProgress < events[currentEvent].typeEvent.Length)
		{
			switch (events[currentEvent].typeEvent[eventProgress])
			{
				case Events.None:
					break;
				case Events.Dialogue:
					DialogueEvent(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.Chest:
					OpenChest(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.ReceiveItem:
					GainItem(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.Fight:
					StartFight(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.CheckTrigger:
					CheckEventIDTriggered();
					break;
				case Events.CheckOther:
					CheckOtherEvent(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.DestroyNPC:
					DestroyNPC(events[currentEvent].typeEventID[eventProgress]);
					break;
				case Events.transitionScene:
					TransitionToScene(events[currentEvent].typeEventID[eventProgress]);
					break;
				default:
					break;
			}
			
		}
		else
		{
			EndEvent(true);
		}	
	}

	public void EndEvent(bool trigger)
	{
		if(trigger) events[currentEvent].hasBeenTriggered = true;
		gameManager.gameData.EventCollection[events[currentEvent].eventTriggerID].hasBeenTriggered = events[currentEvent].hasBeenTriggered;
		//Debug.Log("[EVENT CHANGE] " + events[currentEvent].eventTriggerID + " trigger " + gameManager.gameData.EventCollection[events[currentEvent].eventTriggerID].hasBeenTriggered);

		gameManager.gameState = GameManager.GameState.Overworld;
	}

	public void CheckEventIDTriggered()
	{
		if (!gameManager.gameData.EventCollection[events[currentEvent].eventTriggerID].hasBeenTriggered)
		{
			eventProgress++;
			ContinueEvent();
		}
		else
		{
			EndEvent(false);
		}
	}

	public void CheckOtherEvent(int id)
	{
		
		if (gameManager.gameData.EventCollection[id].hasBeenTriggered)
		{
			eventProgress++;
			ContinueEvent();
		}
		else
		{
			EndEvent(false);
		}
	}

	public void DestroyNPC(int id)
	{
		Destroy(events[currentEvent].npc[id].gameObject);


		eventProgress++;
		ContinueEvent();
	}

	public void DialogueEvent(int id)
	{
		//Debug.Log("test1");
		dialogueBox.StartDialogue(id);
		eventProgress++;
	}
	public void GainItem(int id)
	{
		List<InventoryData> ItemL = new List<InventoryData>(gameManager.gameData.ItemInventory);

		for (int e = 0; e < ItemL.Count; e++)
		{
			if (id == ItemL[e].itemId) //You already have one of this item in your inventory
			{
				ItemL[e].amount++;
				break;	
			}
			else if (e == ItemL.Count - 1) //final of the loop and still no match with items in inevntory
			{
				string newItem = ItemL.Count.ToString() + '\t' + id.ToString() + '\t' + 1;
				ItemL.Add(new InventoryData(newItem));
					
			}
		}
		gameManager.gameData.ItemInventory = ItemL.ToArray();
		StartCoroutine(WaitForItemNotif(id));
	}

	public void OpenChest(int id)
	{
		events[currentEvent].anim.Play("Open");
		gameManager.soundPlayer.PlaySound(18, true);

		List<InventoryData> ItemL = new List<InventoryData>(gameManager.gameData.ItemInventory);

		for (int e = 0; e < ItemL.Count; e++)
		{
			if (id == ItemL[e].itemId) //You already have one of this item in your inventory
			{
				ItemL[e].amount++;
				break;
			}
			else if (e == ItemL.Count - 1) //final of the loop and still no match with items in inevntory
			{
				string newItem = ItemL.Count.ToString() + '\t' + id.ToString() + '\t' + 1;
				ItemL.Add(new InventoryData(newItem));

			}
		}
		gameManager.gameData.ItemInventory = ItemL.ToArray();
		StartCoroutine(WaitForChest(id, events[currentEvent].anim.GetCurrentAnimatorStateInfo(0).length));
		
	}
	public void StartFight(int id)
	{
		gameManager.SpecifiedBattleEncounterAnim(id);
		eventProgress++;
	}

	public void TransitionToScene(int id)
	{
		gameManager.transition.FadeToSceneChange(false, id);
	}

	IEnumerator WaitForChest(int id, float length)
	{
		yield return new WaitForSeconds(length - 0.1f);
		gameManager.notifBox.DisplayNotif("Received " + LanguageManager.langData.itemName[gameManager.gameData.ItemsCollection[id].name]);
		yield return new WaitForSeconds(1);
		eventProgress++;
		ContinueEvent();
	}

	IEnumerator WaitForItemNotif(int id)
	{
		gameManager.notifBox.DisplayNotif("Received " + LanguageManager.langData.itemName[gameManager.gameData.ItemsCollection[id].name]);
		yield return new WaitForSeconds(1);
		eventProgress++;
		ContinueEvent();
	}
}
