using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAutoEvent : AutoEvent
{
   [SerializeField] GameObject controlTag;

    public override void EventEnter(GameObject playerGO)
    {
        controlTag.SetActive(true);
    }

    public override void EventExit(GameObject playerGO)
    {
        controlTag.SetActive(false);    
    }
}
