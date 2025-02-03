using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushPull : MonoBehaviour
{
    PlayerData playerData;
    Collider2D triggerCollider;
    PlayerAnimator playerAnimator;

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
        ChooseSuitableAnimation();
    }

    private void FixedUpdate()
    {
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
                    interactionType = InteractionType.Pull;
                    playerData.pulling = true;
                }
                else
                {
                    interactionType = InteractionType.Push;
                    playerData.pulling = false;
                }
            }
            else if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                if (interactionSide == InteractionSide.Right)
                {
                    interactionType = InteractionType.Pull;
                    playerData.pulling = true;
                }
                else
                {
                    interactionType = InteractionType.Push;
                    playerData.pulling = false;
                }
            }
            else
            {
                interactionType = InteractionType.None;
                playerData.pulling = false;
            }
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
        else
        {
            playerAnimator.PlayerIdle();
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
        pushPullMoveableObject = collider.gameObject.GetComponent<MoveableObject>(); //gets the object tags component of the pushPullObject
        
        if (pushPullMoveableObject != null)
        {
            currentPushPullObject = collider; //sets the pushPullObject to the collider
            playerData.currentPlayerRBMass = (playerData.playerRBMass + pushPullMoveableObject.objectRBMass)/2; //sets the player mass
            pushPullMoveableObject.currentObjectRBMass = (pushPullMoveableObject.objectRBMass + playerData.playerRBMass)/2; //sets the pushPullObject mass
        }
        else
        {
            DetachPushPullObject();
        }
    }

    void DetachPushPullObject()
    {
        if (currentPushPullObject != null && pushPullMoveableObject != null)
        {
            pushPullMoveableObject.currentObjectRBMass = pushPullMoveableObject.objectRBMass;
        }

        playerData.currentPlayerRBMass = playerData.playerRBMass;
        currentPushPullObject = null;
        pushPullMoveableObject = null;
        interactionType = InteractionType.None;
        interactionSide = InteractionSide.None;
    }

    //NOTE: THIS MUST OCCUR AFTER OTHER PLAYER COLLISION CHECKS!!
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Tiggered!");
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
