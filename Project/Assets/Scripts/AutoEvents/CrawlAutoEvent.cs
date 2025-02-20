using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an auto event which changes the movement type of the player to crawl
public class CrawlAutoEvent : AutoEvent
{
    override public void Event(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().crawling = true;
    }
}
