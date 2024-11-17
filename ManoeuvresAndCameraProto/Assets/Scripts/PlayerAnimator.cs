using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayerWalkRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.eulerAngles = Vector3.zero;
    }

    public void PlayerWalkLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.eulerAngles = Vector3.zero;
    }

    public void PlayerIdle()
    {
        playerAnimator.SetInteger("AnimationNumber", 0);
    }

    public void PlayerCrawlRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipY = true;
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
    }

    public void PlayerCrawlLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipY = false;
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
    }
}
