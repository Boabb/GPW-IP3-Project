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

    public ClimbSide climbSide;
    public ClimbType climbType;

    public bool abortedClimb = false;
    Coroutine climbingTask = null;
    public float animTimeRemaining = 0;
    public bool CanMove { get { if (abortedClimb || (climbingTask == null && animTimeRemaining <= 0)) { return true; } else { return false; } } } 

    public float offsetX = 1;
    public float offsetY = 10;

    public enum ClimbSide
    {
        None,
        Left,
        Right
    };

    public enum ClimbType
    {
        None,
        Quick,
        Cling
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
        if (climbType == ClimbType.Cling)
        {
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
                if (climbingTask != null)
                {
                    abortedClimb = true;
                    climbingTask = null;
                }
                LetGoOfObject();
            }
        }

        else
        {
            if (climbingTask == null)
            {
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
        if(!CanMove)
        {
            animTimeRemaining -= Time.deltaTime;
        }
    }

    public IEnumerator ClimbUpObjectLeft()
    {
        playerData.clinging = true;
        animTimeRemaining += playerData.climbAnimationClip.length;
        yield return new WaitUntil(() => CanMove);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;
        playerData.clinging = false;

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x  + offsetX, playerRB.gameObject.transform.position.y + offsetY);

        playerAnimator.PlayerIdle();
    }

    public IEnumerator ClimbUpObjectRight()
    {
        playerData.clinging = true;
        animTimeRemaining += playerData.climbAnimationClip.length;
        yield return new WaitUntil(() => CanMove);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;
        playerData.clinging = false;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x - offsetX, playerRB.gameObject.transform.position.y + offsetY);
        playerAnimator.PlayerIdle();
    }

    public IEnumerator QuickClimbLeft()
    {
        playerData.clinging = true;
        animTimeRemaining += playerData.climbAnimationClip.length;
        yield return new WaitUntil(() => CanMove);

        climbSide = ClimbSide.None;
        climbType = ClimbType.None;
        playerData.clinging = false;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x + offsetX, playerRB.gameObject.transform.position.y + climbObjectCollider.bounds.size.y);
        playerAnimator.PlayerIdle();
    }

    public IEnumerator QuickClimbRight()
    {
        playerData.clinging = true;
        animTimeRemaining += playerData.climbAnimationClip.length;
        yield return new WaitUntil(() => CanMove);


        climbSide = ClimbSide.None;
        climbType = ClimbType.None;
        playerData.clinging = false;

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
        playerData.clinging = true;
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
        climbType = ClimbType.Cling;
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

        if (tags.clingClimbable && playerRB.velocity.y < 0)
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
