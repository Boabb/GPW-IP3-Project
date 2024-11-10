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

    public void playerWalkRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
    }

    public void playerWalkLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
    }

    public void playerIdle()
    {
        playerAnimator.SetInteger("AnimationNumber", 0);
    }

}
