using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerData playerData;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;

    [SerializeField] SpriteRenderer[] familyRenderers;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerAnimator = playerData.playerAnimatorComponent;
        spriteRenderer = playerData.playerSprite;
    }

    private void Update()
    {
        //Debug.Log(playerAnimator.GetInteger("AnimationNumber"));
    }

    public void PlayerWalkRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = true;
        spriteRenderer.flipY = false;

        for (int i = 0; i < familyRenderers.Length; i++)
        {
            familyRenderers[i].flipX = true;
            familyRenderers[i].flipY = false;
        }

        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void PlayerWalkLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 1);
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;

        for (int i = 0; i < familyRenderers.Length; i++)
        {
            familyRenderers[i].flipX = false;
            familyRenderers[i].flipY = false;
        }

        spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void PlayerIdle()
    {
        if (!playerData.shouldLimitMovement && !(SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveLeft) || SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveRight)))
        {
            playerAnimator.SetInteger("AnimationNumber", 0);
            spriteRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
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

    public void PlayerClingLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = true;
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(0.12f, 0, 0);
    }

    public void PlayerClingRight()
    {
        playerAnimator.SetInteger("AnimationNumber", 5);
        spriteRenderer.flipX = false;
        //spriteRenderer.gameObject.transform.localPosition = new Vector3(-0.12f, 0, 0);
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
    public void PlayerClimbUpLeft()
    {
        playerAnimator.SetInteger("AnimationNumber", 9);
        spriteRenderer.flipX = true;
    }

    public void PlayerClimbUpRight()
    {

        playerAnimator.SetInteger("AnimationNumber", 9);
        spriteRenderer.flipX = false;
    }

    public void PlayerElevatorEnter()
    {
        if (!playerData.insideElevator)
        {
            playerData.insideElevator = true;
            playerAnimator.SetTrigger("EnterElevator");

            var temp = playerAnimator.GetCurrentAnimatorStateInfo(0).ToString();
            StartCoroutine(ElevatorAnimation(temp));
        }
    }

    private IEnumerator ElevatorAnimation(string animatorState)
    {
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorState))
        {
            yield return null;
        }
        playerData.insideElevator = true;
        playerData.shouldLimitMovement = false;
    }
}
