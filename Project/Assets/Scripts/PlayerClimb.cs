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
            if (SystemSettings.tapRight && climbSide == ClimbSide.Left)
            {
                ClimbUpObjectLeft();
            }
            else if (SystemSettings.tapLeft && climbSide == ClimbSide.Right)
            {
                ClimbUpObjectRight();
            }
            else if ((SystemSettings.tapRight && climbSide == ClimbSide.Right) || (SystemSettings.tapLeft && climbSide == ClimbSide.Left))
            {
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
        }
    }

    void ClimbUpObjectLeft()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x  + offsetX, playerRB.gameObject.transform.position.y + offsetY);
        playerAnimator.PlayerIdle();
    }

    void ClimbUpObjectRight()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbSide = ClimbSide.None;
        climbType = ClimbType.None;

        playerRB.gameObject.transform.position = new Vector3(playerRB.gameObject.transform.position.x - offsetX, playerRB.gameObject.transform.position.y + offsetY);
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
    }

    void CatchClimb()
    {
        if (climbSide == ClimbSide.Left)
        {
            //animation left
            playerAnimator.PlayerCatchLeft();
        }
        else
        {
            //animation right
            playerAnimator.PlayerCatchRight();
        }
        climbType = ClimbType.Catch;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
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

    }
}
