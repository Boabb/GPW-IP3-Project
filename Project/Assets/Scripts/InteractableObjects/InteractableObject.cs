using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected GameObject player;
    protected float offset;
    protected int interactionCount = 0;
	protected bool interactOnce = true;

    public abstract void Interaction(GameObject playerGO);

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
            player = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
            player = null;
		}
	}

    protected virtual void Update()
    {
        if(player != null &&
			SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.Interact) &&
			(interactOnce && interactionCount <= 0 || interactOnce == false))
        {
			++interactionCount;
			Interaction(player);
		}
	}
}
