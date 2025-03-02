using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an auto event which changes the movement type of the player to crawl
public class VentAutoEvent : AutoEvent
{
    [SerializeField] Fade fade;
    public void Awake()
    {
        fade = GameObject.FindGameObjectWithTag(GameManager.ventFadeTag).GetComponent<Fade>();
    }               
    override public void EventEnter(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().crawling = true;
        fade.collision = true;
    }
}
