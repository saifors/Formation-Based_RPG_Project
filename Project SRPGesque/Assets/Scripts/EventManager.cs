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
	public enum InteractEvents { None, Dialogue, Chest};

	public int currentEvent;

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

		switch (receiveEvents[closest].interactEvent)
		{
			case InteractEvents.None:
				break;
			case InteractEvents.Dialogue:
				DialogueEvent();
				break;
			case InteractEvents.Chest:
				break;
			default:
				break;
		}
	}

	public void DialogueEvent()
	{
		dialogueBox.StartDialogue(events[currentEvent].interactID);
	}

	
}
