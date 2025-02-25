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

    public void PlayerIdle()
    {
        playerAnimator.SetInteger("AnimationNumber", 0);
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        
    }

    public void PlayerWalkRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void PlayerWalkLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void PlayerCrawlRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0.255f, 0);
    }

    public void PlayerCrawlLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0.255f, 0);
    }

    public void PlayerPushLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
    }

    public void PlayerPushRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = false; 
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
    }

    public void PlayerPullLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
    }

    public void PlayerPullRight() 
    {
        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
    }

    public void PlayerCatchLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0f, -0.15f, 0);
    }

    public void PlayerCatchRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0f, -0.15f, 0);
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
}
