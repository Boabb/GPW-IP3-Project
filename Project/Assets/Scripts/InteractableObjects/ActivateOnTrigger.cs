using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTrigger : AutoEvent
{
    [SerializeField] GameObject[] toActivate;
    public override void EventEnter(GameObject playerGO)
    {
        foreach (GameObject obj in toActivate)
        {
            obj.SetActive(true);
        }
    }
}
