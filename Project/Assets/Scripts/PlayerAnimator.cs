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

        //Debug.Log("AnimationInteger: " +  animationNumber);
    }
 
    public void PlayerIdle()
    {

        playerAnimator.SetInteger("AnimationNumber", 0);
        
    }

    public void PlayerWalkRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
        spriteRenderer.flipY = false;
    }

    public void PlayerWalkLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;
    }

    public void PlayerCrawlRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = true;
    }

    public void PlayerCrawlLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = false;
    }

    public void PlayerPushLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = true;
    }

    public void PlayerPushRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = false; 
    }

    public void PlayerPullLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = true;
    }

    public void PlayerPullRight() 
    {

        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = false;
    }

    public void PlayerClingLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = true;
    }

    public void PlayerClingRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = false;
    }

    public void PlayerJumpLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 6); 
        spriteRenderer.flipX = false;
    }

    public void PlayerJumpRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 6);
        spriteRenderer.flipX = true;
    }

    public void PlayerFallLeft()
    {

        playerAnimator.SetInteger("AnimationNumber", 7);
        spriteRenderer.flipX = false;
    }

    public void PlayerFallRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 7);
        spriteRenderer.flipX = true;
    }

    public void PlayerLandLeft()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}


        playerAnimator.SetInteger("AnimationNumber", 8);
        spriteRenderer.flipX = false;
    }

    public void PlayerLandRight()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}

        playerAnimator.SetInteger("AnimationNumber", 8);
        spriteRenderer.flipX = true;

    }
    public void PlayerClimbLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 9);
        spriteRenderer.flipX = true;
    }

    public void PlayerClimbRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 9);
        spriteRenderer.flipX = false;
    }


}
