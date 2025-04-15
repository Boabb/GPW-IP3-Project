using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushPull : MonoBehaviour
{
    PlayerData playerData;
    Collider2D triggerCollider;
    PlayerAnimation playerAnimator;

    Collider2D currentPushPullObject;
    MoveableObject pushPullMoveableObject;
    InteractionType interactionType;
    InteractionSide interactionSide;

    float movementForce = 0f;

    enum InteractionSide
    {
        None,
        Left, 
        Right 
    };

    enum InteractionType
    {
        None,
        Push,
        Pull
    }

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        triggerCollider = playerData.playerInteractCollider;
        playerAnimator = playerData.playerAnimator;

        currentPushPullObject = null;
        interactionType = InteractionType.None;
        interactionSide = InteractionSide.None;
    }

    private void Update()
    {
        CheckIfPushPullObjectIsEnabled();
        FindInteractionType();  
    }

    private void FixedUpdate()
    {
        ChooseSuitableAnimation();
        MoveObject();
    }

    void CheckIfPushPullObjectIsEnabled()
    {
        if (currentPushPullObject != null && !currentPushPullObject.enabled)
        {
            DetachPushPullObject();
        }
    }

    void FindInteractionType()
    {
        if (currentPushPullObject != null)
        {
            Vector3 colliderPos = currentPushPullObject.bounds.center;
            Vector3 triggerPos = triggerCollider.bounds.center;

            //find the interaction side
            if (colliderPos.x > triggerPos.x)
            {
                interactionSide = InteractionSide.Left;
            }
            else
            {
                interactionSide = InteractionSide.Right;
            }

            if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                if (interactionSide == InteractionSide.Left)
                {
                    if(currentPushPullObject.GetComponent<ObjectTags>().foreground && !SystemSettings.interact)
                    {
                        interactionType = InteractionType.None;
                        playerData.pulling = false;
                        playerData.pushing = false;
                    }
                    else
                    {
                        interactionType = InteractionType.Pull;
                        playerData.pulling = true;
                        playerData.pushing = false;
                    }
                }
                else
                {
                    interactionType = InteractionType.Push;
                    playerData.pushing = true;
                    playerData.pulling = false;
                }
            }
            else if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                if (interactionSide == InteractionSide.Right)
                {
                    if (currentPushPullObject.GetComponent<ObjectTags>().foreground && !SystemSettings.interact)
                    {
                        interactionType = InteractionType.None;
                        playerData.pulling = false;
                        playerData.pushing = false;
                    }
                    else
                    {
                        interactionType = InteractionType.Pull;
                        playerData.pulling = true;
                        playerData.pushing = false;
                    }
                }
                else
                {
                    interactionType = InteractionType.Push;
                    playerData.pushing = true;
                    playerData.pulling = false;
                }
            }
            else
            {
                interactionType = InteractionType.None;
                playerData.pulling = false;
                playerData.pushing = false;
            }
        }
        else
        {
            interactionType = InteractionType.None;
            playerData.pulling = false;
            playerData.pushing = false;
        }
    }

    void ChooseSuitableAnimation()
    {
        if (interactionSide == InteractionSide.Left)
        {
            if (interactionType == InteractionType.Pull)
            {
                playerAnimator.PlayerPullLeft();
            }
            else if (interactionType == InteractionType.Push)
            {
                playerAnimator.PlayerPushLeft();
            }
            else
            {
                playerAnimator.PlayerIdle();
            }
        }
        else if (interactionSide == InteractionSide.Right)
        {
            if (interactionType == InteractionType.Pull)
            {
                playerAnimator.PlayerPullRight();
            }
            else if (interactionType == InteractionType.Push)
            {
                playerAnimator.PlayerPushRight();
            }
            else
            {
                playerAnimator.PlayerIdle();
            }
        }
    }

    void MoveObject()
    {
        //Debug.Log("object mass" + currentPushPullObject.attachedRigidbody.mass);
        if (interactionType == InteractionType.Pull && interactionSide != InteractionSide.None)
        {
            movementForce = playerData.playerMovement.GetMovementForce();

            if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                currentPushPullObject.attachedRigidbody.AddForce(-transform.right * movementForce);
            }

            if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                currentPushPullObject.attachedRigidbody.AddForce(transform.right * movementForce);
            }
        }
    }
    void AttachPushPullObject(Collider2D collider)
    {
        pushPullMoveableObject = collider.gameObject.GetComponent<MoveableObject>(); //gets the moveableobject component of the pushPullObject
        var objectTags = collider.GetComponentInParent<ObjectTags>();

        if (pushPullMoveableObject != null && playerData.grounded)
        {
            if (objectTags != null)
            {
                if ((!SystemSettings.interact && objectTags.foreground) || SystemSettings.interact)
                {
                    currentPushPullObject = collider; //sets the pushPullObject to the collider
                }
            }
        }
        else
        {
            DetachPushPullObject();
        }
    }

    void DetachPushPullObject()
    {
        playerData.currentPlayerRBMass = playerData.playerRBMass;
        currentPushPullObject = null;
        pushPullMoveableObject = null;
        interactionType = InteractionType.None;
        interactionSide = InteractionSide.None;
    }

    //NOTE: THIS MUST OCCUR AFTER OTHER PLAYER COLLISION CHECKS!!
    //why...?
    //I don't remember, this is why we write better comments
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Tiggered!");
        if (currentPushPullObject == null)
        {
            AttachPushPullObject(collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (currentPushPullObject == null)
        {
            AttachPushPullObject(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == currentPushPullObject)
        {
            DetachPushPullObject();
        }
    }
}
