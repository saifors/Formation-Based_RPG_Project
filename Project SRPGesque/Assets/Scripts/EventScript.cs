using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
	[HideInInspector]
	public EventManager eventManager;

	public int ID;
	public int EventTriggerID;
	public Transform trans;
	public EventManager.TypeOfEvent eventType;

	public EventManager.InteractEvents[] interactEvent;
	public int[] interactID; //Can be dialogue or whatever is needed.

	public bool hasBeenTriggered;

	public void Init(EventManager eM, int index)
    {
		eM = eventManager;
		ID = index;
		trans = transform;
		hasBeenTriggered = false;
		//hasBeenTriggered = eventManager.gameManager.gameData.EventCollection[EventTriggerID];
    }


}
