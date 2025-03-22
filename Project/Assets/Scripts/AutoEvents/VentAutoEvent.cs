using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an auto event which changes the movement type of the player to crawl
public class VentAutoEvent : AutoEvent
{
    //NOT CURRENTLY FUNCTIONAL             
    override public void EventEnter(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().crawling = true;
    }
}
