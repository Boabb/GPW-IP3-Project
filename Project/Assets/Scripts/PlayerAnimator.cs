using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    PlayerData playerData;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerAnimator = playerData.playerAnimatorComponent;
        spriteRenderer = playerData.playerSprite;
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

    public void PlayerIdle()
    {
        playerAnimator.SetInteger("AnimationNumber", 0);
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
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
    }

    public void PlayerCatchRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = false;
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
    }
}
