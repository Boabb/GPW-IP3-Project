using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    PlayerData playerData;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;

    int animationNumber;

    //animation bools: this should probably be redone as some kind of bitwise enum but I'm tired and can't brain that right now

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerAnimator = playerData.playerAnimatorComponent;
        spriteRenderer = playerData.playerSprite;
    }

    private void Update()
    {
        animationNumber = playerAnimator.GetInteger("AnimationNumber");
        playerData.animationNumber = animationNumber;

        Debug.Log("AnimationInteger: " +  animationNumber);
    }

    private void FlipBasedOnDirection()
    {
        switch (playerData.movementDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                spriteRenderer.flipX = false;
                break;
            case PlayerMovement.MovementDirection.Right:
                spriteRenderer.flipX = true; 
                break;
        }
    }
 
    public void PlayerIdle()
    {

        playerAnimator.SetInteger("AnimationNumber", 0);
        
    }

    public void PlayerWalkRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 1);
        FlipBasedOnDirection();
    }

    public void PlayerWalkLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 1);
        FlipBasedOnDirection();
    }

    public void PlayerCrawlRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 2);
        FlipBasedOnDirection();
    }

    public void PlayerCrawlLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 2);
        FlipBasedOnDirection();
    }

    public void PlayerPushLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 3);
        FlipBasedOnDirection();
    }

    public void PlayerPushRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 3);
        FlipBasedOnDirection(); 
    }

    public void PlayerPullLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 4);
        FlipBasedOnDirection();
    }

    public void PlayerPullRight() 
    {

        playerAnimator.SetInteger("AnimationNumber", 4);
        FlipBasedOnDirection();
    }

    public void PlayerClingLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        FlipBasedOnDirection();
    }

    public void PlayerClingRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 5);
        FlipBasedOnDirection();
    }

    public void PlayerJumpLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 6); 
        FlipBasedOnDirection();
    }

    public void PlayerJumpRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 6);
        FlipBasedOnDirection();
    }

    public void PlayerFallLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 7);
        FlipBasedOnDirection();
    }

    public void PlayerFallRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 7);
        FlipBasedOnDirection();
    }

    public void PlayerLandLeft()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}


        playerAnimator.SetInteger("AnimationNumber", 8);
        FlipBasedOnDirection();
    }

    public void PlayerLandRight()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}

        playerAnimator.SetInteger("AnimationNumber", 8);
        FlipBasedOnDirection();

    }


}
