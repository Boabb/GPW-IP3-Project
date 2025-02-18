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
    public bool idle;
    public bool walkLeft;
    public bool walkRight;
    public bool crawlLeft;
    public bool crawlRight;
    public bool pushLeft;
    public bool pushRight;
    public bool pullLeft;
    public bool pullRight;
    public bool clingLeft;
    public bool clingRight;
    public bool jumpLeft;
    public bool jumpRight;
    public bool fallLeft;
    public bool fallRight;
    public bool landLeft;
    public bool landRight;
    public bool climbLeft;
    public bool climbRight;

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

        ChooseAnimation();
        ResetBools();
    }

    void ChangeState(int currentAnim)
    {
        //if (idle ||
        //    walkLeft || walkRight || 
        //    crawlLeft || crawlRight || 
        //    pushLeft || pushRight || 
        //    pullLeft || pullRight || 
        //    clingLeft || clingRight || 
        //    jumpLeft || jumpRight || 
        //    fallLeft || fallRight || 
        //    landLeft || landRight || 
        //    climbLeft || climbRight)
        //{

        //}
        //else
        //{

        //}

        if (animationNumber == currentAnim)
        {
            playerAnimator.SetBool("ChangeState", false);
        }
        else
        {
            playerAnimator.SetBool("ChangeState", true);
        }
    }

    void ChooseAnimation()
    {
        if (clingLeft)
        {
            PlayerClingLeft();
        }
        else if (clingRight)
        {
            PlayerClingRight();
        }
        else if (climbLeft)
        {
            //implement climb
        }
        else if (climbRight)
        {
            //implement climb
        }
        else if (pushLeft)
        {
            PlayerPushLeft();
        }
        else if (pushRight)
        {
            PlayerPushRight();
        }
        else if (pullLeft)
        {
            PlayerPullLeft();
        }
        else if (pullRight)
        {
            PlayerPullRight();
        }
        else if (walkLeft)
        {
            PlayerWalkLeft();
        }
        else if (walkRight)
        {
            PlayerWalkRight();
        }
        else if (crawlLeft)
        {
            PlayerCrawlLeft();
        }
        else if (crawlRight)
        {
            PlayerCrawlRight();
        }
        else if (landLeft)
        {
            PlayerLandLeft();
        }
        else if (landRight)
        {
            PlayerLandRight();
        }
        else if (jumpLeft)
        {
            PlayerJumpLeft();
        }
        else if (jumpRight)
        {
            PlayerJumpRight();
        }
        else if (fallLeft)
        {
            PlayerFallLeft();
        }
        else if (fallRight)
        {
            PlayerFallRight();
        }
        else //idle
        {
            PlayerIdle();
        }
    }

    private void ResetBools()
    {
        idle = false;
        walkLeft = false;
        walkRight = false;
        crawlLeft = false;
        crawlRight = false;
        pushLeft = false;
        pushRight = false;
        pullLeft = false;
        pullRight = false;
        clingLeft = false;
        clingRight = false;
        jumpLeft = false;
        jumpRight = false;
        fallLeft = false;
        fallRight = false;
        landLeft = false;
        landRight = false;
        climbLeft = false;
        climbRight = false;
    }

    void PlayerIdle()
    {
        playerAnimator.SetInteger("AnimationNumber", 0);
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        ChangeState(0);
    }

    void PlayerWalkRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        ChangeState(1);
    }

    void PlayerWalkLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        ChangeState(1);
    }

    void PlayerCrawlRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0.255f, 0);
        ChangeState(2);
    }

    void PlayerCrawlLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 2);
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0.255f, 0);
        ChangeState(2);
    }

    void PlayerPushLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
        ChangeState(3);
    }

    void PlayerPushRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 3);
        spriteRenderer.flipX = false; 
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
        ChangeState(3);
    }

    void PlayerPullLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = true;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
        ChangeState(4);
    }

    void PlayerPullRight() 
    {
        playerAnimator.SetInteger("AnimationNumber", 4);
        spriteRenderer.flipX = false;
        spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
        ChangeState(4);
    }

    void PlayerClingLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = true;
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
        ChangeState(5);
    }

    void PlayerClingRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = false;
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
        ChangeState(5);
    }

    void PlayerJumpLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 6); 
        spriteRenderer.flipX = false;
        ChangeState(6);
    }

    void PlayerJumpRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 6);
        spriteRenderer.flipX = true;
        ChangeState(6);
    }

    void PlayerFallLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 7);
        spriteRenderer.flipX = false;
        ChangeState(7);
    }

    void PlayerFallRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 7);
        spriteRenderer.flipX = true;
        ChangeState(7);
    }

    void PlayerLandLeft()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}

        playerAnimator.SetInteger("AnimationNumber", 8);
        spriteRenderer.flipX = false;
        ChangeState(8);
    }

    void PlayerLandRight()
    {
        //if (playerAnimator.GetInteger("AnimationNumber") == 7 || playerAnimator.GetInteger("AnimationNumber") == 6)
        //{
        //}

        playerAnimator.SetInteger("AnimationNumber", 8);
        spriteRenderer.flipX = true;
        ChangeState(8);
    }
}
