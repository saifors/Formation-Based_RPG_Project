using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public GameManager gameManager;
	public GameObject[] eventObj;
	public EventScript[] events;

	public enum TypeOfEvent { Interaction, Range};

    public void Init(GameManager gM)
    {
		gameManager = gM;

		GetAllEvents();

	}
	
	public void GetAllEvents()
	{
		eventObj = GameObject.FindGameObjectsWithTag("Event");
		events = new EventScript[eventObj.Length];
		for (int i = 0; i < eventObj.Length; i++)
		{
			events[i] = eventObj[i].GetComponent<EventScript>();
			events[i].Init(this);
		}
	}

	public void DialogueEvent()
	{

	}
}
