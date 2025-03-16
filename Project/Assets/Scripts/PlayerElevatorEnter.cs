using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevatorEnter : MonoBehaviour
{
    PlayerData playerData; //I have moved this to a static variable in GameManager for easier access and consistency (Singleton pattern)


    //Collider2D climbObjectCollider;
    //ObjectTags currentClimbObjectTags;
    //ElevatorEnterMode climbType;

    //public float offsetX = 1;
    //public float offsetY = 10;

    //enum ElevatorEnterMode
    //{
    //    None,
    //    Catch
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    playerData = GameManager.playerData;
    //    climbType = ElevatorEnterMode.None;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (climbType == ElevatorEnterMode.Catch)
    //    {
    //        playerData.clinging = true;
    //        if (!playerData.playerAnimator.enteringElevator)
    //        {
    //            UnFreezePlayer();
    //        }
    //        if (SystemSettings.tapInteract)
    //        {
    //            playerData.playerAnimator.PlayerElevatorEnter();
    //            playerData.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    //        }
    //        else
    //        {
    //            ElevatorCatch();
    //            playerData.playerRigidbody.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, playerData.playerRigidbody.gameObject.transform.position.y, playerData.playerRigidbody.gameObject.transform.position.z);
    //        }
    //    }
    //    else
    //    {
    //        playerData.clinging = false;

    //    }
    //}

    //void UnFreezePlayer()
    //{
    //    playerData.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    //    climbType = ElevatorEnterMode.None;
    //    playerData.playerAnimator.PlayerIdle();
    //}

    //void ElevatorCatch()
    //{
    //    Debug.Log("ElevatorCatch");

    //    climbType = ElevatorEnterMode.Catch;
    //}

    //private void OnTriggerStay2D(Collider2D collider)
    //{
    //    if(collider.gameObject.TryGetComponent<ObjectTags>(out var tags))
    //    {
    //        if (tags.elevatorCatch && collider.gameObject.GetComponentInParent<ElevatorAutoEvent>().elevatorOpen)
    //        {
    //            ElevatorCatch();
    //        }
    //    }    
    //    currentClimbObjectTags = tags;
    //    climbObjectCollider = collider;

    //}

    //private void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider == climbObjectCollider && climbType == ElevatorEnterMode.Quick)
    //    {
    //        climbType = ElevatorEnterMode.None;
    //    }
    //}
}
