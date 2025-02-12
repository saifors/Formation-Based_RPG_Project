﻿using System.Collections;
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

	
	public Animator anim;

	public NPCEvent[] npc;

	public void Init(EventManager eM, GameManager gM, int index)
    {
		eventManager = eM;
		gameManager = gM;
		ID = index;
		trans = transform;
		if (typeEvent.Length != typeEventID.Length) Debug.LogError("Event " + ID + "'s array lengths or Events and event IDs don't align");
		//Debug.Log("[EVENT LOAD] " + gameManager.gameData.EventCollection[eventTriggerID].id + " trigger " + gameManager.gameData.EventCollection[eventTriggerID].hasBeenTriggered);
		hasBeenTriggered = gameManager.gameData.EventCollection[eventTriggerID].hasBeenTriggered;

		for (int i = 0; i < typeEvent.Length; i++)
		{
			if(typeEvent[i] == EventManager.Events.Chest)
			{
				anim = GetComponentInChildren<Animator>();
				if (hasBeenTriggered) anim.Play("IdleOpen");
				break;
			}
		}

		npc = GetComponentsInChildren<NPCEvent>();
		for (int i = 0; i < npc.Length; i++)
		{
			npc[i].Init();
		}
    }


}
