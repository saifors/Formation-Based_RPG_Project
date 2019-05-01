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

	public EventManager.InteractEvents[] interactEvent;
	public int[] interactID; //Can be dialogue or whatever is needed.

	public bool hasBeenTriggered;

	public void Init(EventManager eM, GameManager gM, int index)
    {
		eM = eventManager;
		gameManager = gM;
		ID = index;
		trans = transform;
		//hasBeenTriggered = false;
		hasBeenTriggered = gameManager.gameData.EventCollection[0].hasBeenTriggered;
    }


}
