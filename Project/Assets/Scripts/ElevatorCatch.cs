using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCatch : MonoBehaviour
{
    PlayerData playerData; //I have moved this to a static variable in GameManager for easier access and consistency (Singleton pattern)
    public Rigidbody2D rb;
    public PlayerAnimator animator;

    Collider2D climbObjectCollider;
    ObjectTags currentClimbObjectTags;
    ElevatorEnterMode climbType;

    public float offsetX = 1;
    public float offsetY = 10;

    enum ElevatorEnterMode
    {
        None,
        Catch
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameManager.playerData;
        if (gameObject.layer == LayerMask.NameToLayer("Player") && animator == null)
        {
            animator = playerData.playerAnimator;
        }
        else if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (gameObject.layer == LayerMask.NameToLayer("Player") && rb == null)
        {
            rb = playerData.playerRigidbody;
        }

        climbType = ElevatorEnterMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (climbType == ElevatorEnterMode.Catch)
        {
            playerData.clinging = true;
            if (!animator.enteringElevator)
            {
                UnFreezePlayer();
            }
            if (SystemSettings.tapInteract)
            {
                animator.PlayerElevatorEnter();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                ElevatorCatch();
                rb.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, rb.gameObject.transform.position.y, rb.gameObject.transform.position.z);
            }
        }
        else
        {
            playerData.clinging = false;

        }
    }

    void UnFreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbType = ElevatorEnterMode.None;
        animator.PlayerIdle();
    }

    void ElevatorCatch()
    {
        Debug.Log("ElevatorCatch");

        climbType = ElevatorEnterMode.Catch;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent<ObjectTags>(out var tags))
        {
            if (tags.elevatorCatch && collider.gameObject.GetComponentInParent<ElevatorAutoEvent>().elevatorOpen)
            {
                ElevatorCatch();
            }
        }    
        currentClimbObjectTags = tags;
        climbObjectCollider = collider;

    }

    //private void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider == climbObjectCollider && climbType == ElevatorEnterMode.Quick)
    //    {
    //        climbType = ElevatorEnterMode.None;
    //    }
    //}
}
