using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an auto event which applies all the logic that should do so once the player enters the vent in Level 3 (Wallenberg House) changes the movement type of the player to crawl
public class VentAutoEvent : AutoEvent
{
    //NOT CURRENTLY FUNCTIONAL             
    override public void EventEnter(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().crawling = true;
    }
}
