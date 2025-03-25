using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    PlayerData playerData; //I have moved this to a static variable in GameManager for easier access and consistency (Singleton pattern)
    Rigidbody2D playerRB;
    PlayerAnimation playerAnimator;
    Coroutine climbingTask;
    Collider2D climbObjectCollider;
    ObjectTags currentClimbObjectTags;
    ClimbSide climbSide;
    ClimbType climbType;

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
        climbingTask = null;
        climbType = ClimbType.None;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (climbType == ClimbType.Catch)
        {
            playerData.shouldLimitMovement = true;

            if (SystemSettings.tapRight && climbSide == ClimbSide.Left && climbingTask == null)
            {
                climbingTask = StartCoroutine(ClimbUpObjectLeft());
            }
            else if (SystemSettings.tapLeft && climbSide == ClimbSide.Right && climbingTask == null)
            {
                climbingTask = StartCoroutine(ClimbUpObjectRight());
            }
            else if ((SystemSettings.tapRight && climbSide == ClimbSide.Right) || (SystemSettings.tapLeft && climbSide == ClimbSide.Left))
            {
                LetGoOfObject();
            }
            else if(climbingTask != null)
            {

            }
            else
            {
                CatchClimb();
            }
        }
        else
        {
            playerData.shouldLimitMovement = false;

            if (climbType == ClimbType.Quick)
            {
                if (SystemSettings.tapRight && climbSide == ClimbSide.Left)
                {
                    QuickClimbLeft();
                }
                else if (SystemSettings.tapLeft && climbSide == ClimbSide.Right)
                {
                    QuickClimbRight();
                }
            }
        }
    }

    IEnumerator ClimbUpObjectLeft()
    {
        playerAnimator.PlayerClimbUpLeft();
        yield return new WaitForSeconds(playerData.climbAnimationClip.length);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x  + offsetX, playerRB.gameObject.transform.position.y + offsetY);
        playerAnimator.PlayerIdle();

        climbingTask = null;
    }

    IEnumerator ClimbUpObjectRight()
    {
        playerAnimator.PlayerClimbUpRight();
        yield return new WaitForSeconds(playerData.climbAnimationClip.length);

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x - offsetX, playerRB.gameObject.transform.position.y + offsetY);
        playerAnimator.PlayerIdle();

        climbingTask = null;
    }

    void QuickClimbLeft()
    {
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        //add animation and delay for animation

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x + offsetX, playerRB.gameObject.transform.position.y + climbObjectCollider.bounds.size.y);
        playerAnimator.PlayerIdle();
    }

    void QuickClimbRight()
    {
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
        playerData.shouldLimitMovement = true;
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

        if (tags.clingClimbable && playerRB.velocity.y < 0)
        {
            if (currentClimbObjectTags.background == true)
            {
                if (!climbObjectCollider.gameObject.GetComponentInParent<BackgroundObject>().CheckOverlap())
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
