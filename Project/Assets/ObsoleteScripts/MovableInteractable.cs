using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableInteractable : InteractableObject
{
    public override void Interaction(GameObject playerGO)
    {
        if(player != null)
        {
			offset = player.transform.position.x - transform.position.x;
		}    
    }
}
