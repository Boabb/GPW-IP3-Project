using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevatorCatch : MonoBehaviour
{
    PlayerData playerData;
    Rigidbody2D playerRB;
    PlayerAnimator playerAnimator;

    Collider2D climbObjectCollider;
    ObjectTags currentClimbObjectTags;
    ElevatorEnterMode climbType;

    public float offsetX = 1;
    public float offsetY = 10;

    enum ElevatorEnterMode
    {
        None,
        Catch
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponentInParent<PlayerData>();
        playerRB = playerData.playerRigidbody;
        playerAnimator = playerData.playerAnimator;
        climbType = ElevatorEnterMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (climbType == ElevatorEnterMode.Catch)
        {
            playerData.clinging = true;
            if (!playerAnimator.enteringElevator)
            {
                UnFreezePlayer();
            }
            else
            {
                ElevatorCatch();
                playerRB.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, playerRB.gameObject.transform.position.y, playerRB.gameObject.transform.position.z);
            }
        }
        else
        {
            playerData.clinging = false;

        }
    }

    void UnFreezePlayer()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbType = ElevatorEnterMode.None;
        playerAnimator.PlayerIdle();
    }

    void ElevatorCatch()
    {
        Debug.Log("ElevatorCatch");
        playerData.clinging = true;
        playerAnimator.PlayerElevatorEnter();

        climbType = ElevatorEnterMode.Catch;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent<ObjectTags>(out var tags))
        {
            if (tags.elevatorCatch)
            {
                ElevatorCatch();
            }
        }    
        currentClimbObjectTags = tags;
        climbObjectCollider = collider;

    }

    //private void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider == climbObjectCollider && climbType == ElevatorEnterMode.Quick)
    //    {
    //        climbType = ElevatorEnterMode.None;
    //    }
    //}
}
