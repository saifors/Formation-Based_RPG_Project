using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public GameManager gameManager;
	private OWPlayerController playerController;
	private DialogueBox dialogueBox;

	public GameObject[] eventObj;
	public EventScript[] events;

	public enum TypeOfEvent { Interaction, Range};
	public enum InteractEvents { None, Dialogue, Chest, ReceiveItem};

	public int currentEvent;

	public int eventProgress;

    public void Init(GameManager gM, OWPlayerController pC)
    {
		gameManager = gM;
		playerController = pC;
		dialogueBox = gameManager.dialogueUI;
		
		GetAllEvents();

	}

	

	public void GetAllEvents()
	{
		eventObj = GameObject.FindGameObjectsWithTag("Event");
		events = new EventScript[eventObj.Length];
		for (int i = 0; i < eventObj.Length; i++)
		{
			events[i] = eventObj[i].GetComponent<EventScript>();
			events[i].Init(this, i);
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

			if(i == 0 )
			{
				shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
				closest = i;
			}
			else if(Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position) < shortestDistance)
			{
				shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
				closest = i;
			}
			else if (Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position) == shortestDistance)
			{
				if(Random.Range(0,1) == 1)
				{
					shortestDistance = Vector3.Distance(playerController.trans.position, receiveEvents[i].trans.position);
					closest = i;
				}
			}
		}

		currentEvent = receiveEvents[closest].ID;
		playerController.isMoving = false;
		playerController.isRunning = false;

		eventProgress = 0;
		ContinueEvent();
	}

	public void ContinueEvent()
	{
		if (eventProgress < events[currentEvent].interactEvent.Length)
		{
			switch (events[currentEvent].interactEvent[eventProgress])
			{
				case InteractEvents.None:
					break;
				case InteractEvents.Dialogue:
					DialogueEvent(events[currentEvent].interactID[eventProgress]);
					break;
				case InteractEvents.Chest:
					break;
				case InteractEvents.ReceiveItem:
					GainItem(events[currentEvent].interactID[eventProgress]);
					break;
				default:
					break;
			}
			eventProgress++;
		}
		else
		{
			EndEvent();
		}	
	}

	public void EndEvent()
	{
		gameManager.gameState = GameManager.GameState.Overworld;
	}

	public void DialogueEvent(int id)
	{
		//Debug.Log("test1");
		dialogueBox.StartDialogue(id);
	}
	public void GainItem(int id)
	{
		for (int e = 0; e < gameManager.gameData.ItemInventory.Count; e++)
		{
			if (id == gameManager.gameData.ItemInventory[e].itemId) //You already have one of this item in your inventory
			{
				gameManager.gameData.ItemInventory[e].amount++;
					
			}
			else if (e == gameManager.gameData.ItemInventory.Count - 1) //final of the loop and still no match with items in inevntory
			{
				string newItem = gameManager.gameData.ItemInventory.Count.ToString() + '\t' + id.ToString() + '\t' + 1;
				gameManager.gameData.ItemInventory.Add(new InventoryData(newItem));
					
			}
		}
	}

	
}
