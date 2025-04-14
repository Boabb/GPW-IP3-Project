using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTrigger : AutoEvent
{
    [SerializeField] GameObject[] toDeactivate;
    public override void EventEnter(GameObject playerGO)
    {
        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
