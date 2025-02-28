using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAutoEvent : AutoEvent
{
    override public void EventEnter(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().crawling = false;
    }
}
