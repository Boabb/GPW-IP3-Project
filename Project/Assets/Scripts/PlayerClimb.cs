using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    PlayerData playerData;
    Rigidbody2D playerRB;
    PlayerAnimator playerAnimator;

    Collider2D climbObjectCollider;
    ObjectTags currentClimbObjectTags;

    ClimbSide climbSide;
    ClimbType climbType;

    Coroutine climbingTask = null;

    public float offsetX = 1;
    public float offsetY = 10;

    enum ClimbSide
    {
        None,
        Left,
        Right
    };

    enum ClimbType
    {
        None,
        Quick,
        Catch
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponentInParent<PlayerData>();
        playerRB = playerData.playerRigidbody;
        playerAnimator = playerData.playerAnimator;
        climbType = ClimbType.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (climbType == ClimbType.Catch)
        {
            playerData.catching = true;

            if (climbingTask == null)
            {
                if (SystemSettings.tapRight && climbSide == ClimbSide.Left)
                {
                    playerAnimator.PlayerClimbLeft();
                    climbingTask = StartCoroutine(ClimbUpObjectLeft());
                    // Might be more efficient for this to get called in FSM for JumpUp/Climb
                }
                else if (SystemSettings.tapLeft && climbSide == ClimbSide.Right)
                {
                    playerAnimator.PlayerClimbRight();
                    climbingTask = StartCoroutine(ClimbUpObjectRight());
                    // Should maybe get called in FSM for JumpUp/Climb
                }
            }
            if ((SystemSettings.tapRight && climbSide == ClimbSide.Right) || (SystemSettings.tapLeft && climbSide == ClimbSide.Left))
            {
                if(climbingTask != null)
                { 
                    StopCoroutine(climbingTask);
                    climbingTask = null; 
                }
                LetGoOfObject();
            }
            else
            {
                CatchClimb();
            }
        }
        else
        {
            playerData.catching = false;

            if (climbType == ClimbType.Quick)
            {
                if (SystemSettings.tapRight && climbSide == ClimbSide.Left) 
                {
                    climbingTask = StartCoroutine(QuickClimbLeft());                
                }
                else if (SystemSettings.tapLeft && climbSide == ClimbSide.Right)
                {
                    climbingTask = StartCoroutine(QuickClimbRight());
                }
            }
        }
    }

    public IEnumerator ClimbUpObjectLeft()
    {
        yield return new WaitForSeconds(playerData.climbAnimationClip.length);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x  + offsetX, playerRB.gameObject.transform.position.y + offsetY);

        playerAnimator.PlayerIdle();
    }

    public IEnumerator ClimbUpObjectRight()
    {
        yield return new WaitForSeconds(playerData.climbAnimationClip.length);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x - offsetX, playerRB.gameObject.transform.position.y + offsetY);
        playerAnimator.PlayerIdle();
    }

    public IEnumerator QuickClimbLeft()
    {
        yield return new WaitForSeconds(playerData.quickClimbAnimationClip.length);

        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x + offsetX, playerRB.gameObject.transform.position.y + climbObjectCollider.bounds.size.y);
        playerAnimator.PlayerIdle();
    }

    public IEnumerator QuickClimbRight()
    {
        yield return new WaitForSeconds(playerData.quickClimbAnimationClip.length);

        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x - offsetX, playerRB.gameObject.transform.position.y + climbObjectCollider.bounds.size.y);
        playerAnimator.PlayerIdle();
    }

    void LetGoOfObject()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        playerAnimator.PlayerIdle();
    }

    void QuickClimb()
    {
        climbType = ClimbType.Quick;
        Debug.Log("QuickClimb");
    }

    void CatchClimb()
    {
        Debug.Log("CatchClimb");
        playerData.catching = true;
        if (climbSide == ClimbSide.Left)
        {
            //animation left
            playerAnimator.PlayerClingLeft();
        }
        else
        {
            //animation right
            playerAnimator.PlayerClingRight();
        }
        climbType = ClimbType.Catch;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Climb");
        ObjectTags tags = collider.gameObject.GetComponentInParent<ObjectTags>();
        currentClimbObjectTags = tags;
        climbObjectCollider = collider;

        if (collider.bounds.center.x > playerRB.gameObject.transform.position.x)
        {
            climbSide = ClimbSide.Left;
        }
        else
        {
            climbSide = ClimbSide.Right;
        }

        if (tags.quickClimbable)
        {
            QuickClimb();
        }

        if (tags.catchClimbable && playerRB.velocity.y < 0)
        {
            if (currentClimbObjectTags.background == true)
            {
                if(!climbObjectCollider.gameObject.GetComponentInParent<BackgroundObject>().CheckOverlap())
                {
                    CatchClimb();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == climbObjectCollider && climbType == ClimbType.Quick)
        {
            climbType = ClimbType.None;
            climbSide = ClimbSide.None;
        }
    }
}
