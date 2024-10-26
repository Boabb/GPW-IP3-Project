using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsInteraction : MonoBehaviour
{
    Collider2D playerCollider;
    Collider2D interactionCollider;
    Interactions interaction;
    LayerMask interactables;

    Rigidbody2D playerRb;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerRb = playerCollider.attachedRigidbody;
        interactables = LayerMask.GetMask("Interactable");
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!SystemSettings.interact)
        {
            interactionCollider = null;
        }

        if (interactionCollider == null)
        {
            interactionCollider = interact();
        }
        else
        {
            interaction.moveWithPlayer(playerRb);
        }
    }

    Collider2D interact()
    {
        if (playerCollider.IsTouchingLayers(interactables))
        {
            Debug.Log("Can interact");
            if (SystemSettings.interact)
            {
                ContactFilter2D onlyInteractions = new ContactFilter2D();
                onlyInteractions.SetLayerMask(interactables);
                List<Collider2D> interactableColliders = new List<Collider2D>();
                playerCollider.OverlapCollider(onlyInteractions, interactableColliders);

                interaction = interactableColliders[0].gameObject.GetComponent<Interactions>();
                interaction.interact = true;
                interaction.setOffset(playerRb);
                return interactableColliders[0];
            }
        }

        try
        {
            interaction.interact = false;
        }
        catch
        {

        }

        return null;
    }
}
