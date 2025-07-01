using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAutoEvent : AutoEvent
{
   [SerializeField] GameObject controlTag;

    public override void EventEnter(GameObject playerGO)
    {
		if (playerGO.layer == LayerMask.NameToLayer("Player"))
		{
			controlTag.SetActive(true);
		}
    }

    public override void EventExit(GameObject playerGO)
    {
		if (playerGO.layer == LayerMask.NameToLayer("Player"))
		{
			controlTag.SetActive(false);
		}
    }
}
