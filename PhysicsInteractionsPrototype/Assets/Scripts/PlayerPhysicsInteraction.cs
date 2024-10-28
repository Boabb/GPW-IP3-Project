using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPhysicsInteraction : MonoBehaviour
{
    Collider2D playerCollider;
    Rigidbody2D playerRb;

    Collider2D interactableCollider;
    Rigidbody2D interactableRigidbody;

    //Interactions interaction;
    LayerMask interactables;

    public float pushForce;
    public float pullForce;
    public float maxSpeed;

    float reachDistance = 100f; //temporary

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerRb = playerCollider.attachedRigidbody;
        interactables = LayerMask.GetMask("Interactable");
    }

    private void FixedUpdate()
    {
        try
        {
            float currentXDistance = transform.position.x - interactableCollider.transform.position.x;

            if (!SystemSettings.interact || currentXDistance > reachDistance)
            {
                interactableCollider = unsetInteractable();
            }
        }
        catch
        {
            interactableCollider = unsetInteractable();
        }

        if (interactableCollider == null)
        {
            interactableCollider = interact();
        }
        else
        {
            //if (SystemSettings.moveLeft || SystemSettings.moveRight)
            //{
            //    if (relativeMovementDirection(interactableCollider.gameObject))
            //    {
            //        pull(interactableRigidbody);
            //    }
            //    else
            //    {
            //        push(interactableRigidbody);
            //    }
            //}

            //interaction.moveWithPlayer(playerRb);

            //if (!interaction.moveWithPlayer(playerRb))
            //{
            //    interaction.interact = false;
            //    interactableCollider = null;
            //}
        }
    }

    Collider2D interact()
    {
        if (SystemSettings.interact) 
        {
            if (playerCollider.IsTouchingLayers(interactables))
            {
                return setInteractable();
            }
        }

        //try
        //{
        //    interaction.interact = false;
        //}
        //catch
        //{

        //}

        return unsetInteractable();
    }

    Collider2D setInteractable()
    {
        ContactFilter2D onlyInteractions = new ContactFilter2D();
        onlyInteractions.SetLayerMask(interactables);
        List<Collider2D> interactableColliders = new List<Collider2D>();
        playerCollider.OverlapCollider(onlyInteractions, interactableColliders);

        Collider2D toReturn = interactableColliders[0];

        Debug.Log("SetInteraction");

        toReturn.gameObject.transform.parent = transform;

        //interactableRigidbody = toReturn.attachedRigidbody;
        //toReturn.attachedRigidbody.constraints = RigidbodyConstraints2D.None;

        //interaction = interactableColliders[0].gameObject.GetComponent<Interactions>();
        //interaction.interact = true;
        //interaction.setOffset(playerRb);
        return toReturn;
    }

    Collider2D unsetInteractable()
    {
        try
        {
            interactableCollider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            interactableCollider.gameObject.transform.parent = null;
        }
        catch
        {
            //already null
        }
        //return interactableCollider;
        return null;
    }

    void push(Rigidbody2D toInteract)
    {
        Debug.Log("push: " + toInteract);
        if (SystemSettings.moveRight)
        {
            toInteract.AddForce(new Vector2(pushForce, 1));
        }
        else
        {
            toInteract.AddForce(new Vector2(-pushForce, 1));
        }
    }

    void pull(Rigidbody2D toInteract)
    {
        Debug.Log("pull");
        if (SystemSettings.moveRight)
        {
            toInteract.AddForce(new Vector2(pullForce, 1));
        }
        else
        {
            toInteract.AddForce(new Vector2(-pullForce, 1));
        }
    }

    bool relativeMovementDirection(GameObject comparator) //returns true if moving away from the comparator object
    {
        if (transform.position.x > comparator.transform.position.x && SystemSettings.moveRight || transform.position.x < comparator.transform.position.x && SystemSettings.moveLeft)
        {
            return true;
        }
        return false;
    }
}
