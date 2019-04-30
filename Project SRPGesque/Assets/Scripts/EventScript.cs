using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
	[HideInInspector]
	public EventManager eventManager;

	public EventManager.TypeOfEvent eventType;

    public void Init(EventManager eM)
    {
		eM = eventManager;
    }


}
