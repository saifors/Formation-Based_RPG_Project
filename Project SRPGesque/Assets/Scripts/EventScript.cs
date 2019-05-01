using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
	[HideInInspector]
	public EventManager eventManager;
	public GameManager gameManager;

	public int ID;
	public int eventTriggerID;
	public Transform trans;
	public EventManager.TypeOfEvent eventType;

	public EventManager.Events[] typeEvent;
	public int[] typeEventID; //Can be dialogue or whatever is needed.

	public bool hasBeenTriggered;

	public void Init(EventManager eM, GameManager gM, int index)
    {
		eM = eventManager;
		gameManager = gM;
		ID = index;
		trans = transform;
		if (typeEvent.Length != typeEventID.Length) Debug.LogError("Event " + ID + "'s array lengths or Events and event IDs don't align");
		hasBeenTriggered = gameManager.gameData.EventCollection[eventTriggerID].hasBeenTriggered;
    }


}
