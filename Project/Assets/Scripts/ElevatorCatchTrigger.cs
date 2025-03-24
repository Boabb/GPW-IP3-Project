using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCatchTrigger : MonoBehaviour
{
    //public static bool DisableColliders;

    //PlayerData playerData;
    //GameObject armchair;
    //Rigidbody2D armchairRB;
    //EdgeCollider2D elevatorGround;
    //private bool armchairInZone;
    //private bool playerInZone;

    //Collider2D climbObjectCollider;
    //ElevatorEnterMode climbType;

    //enum ElevatorEnterMode
    //{
    //    None,
    //    Catch
    //}

    //void Start()
    //{
    //    playerData = GameManager.playerData;

    //    climbObjectCollider = GetComponent<Collider2D>();

    //    climbType = ElevatorEnterMode.None;
    //}

    //void FixedUpdate()
    //{
    //    if(armchairInZone && playerInZone)
    //    {
    //        if(armchair != null)
    //        {
    //            armchairRB.constraints = RigidbodyConstraints2D.FreezeAll;
    //            if(climbType == ElevatorEnterMode.Catch)
    //            {
                    
    //            }
    //            else
    //            {
    //                ElevatorCatch();
    //                var collider = armchair.GetComponent<BoxCollider2D>();
    //                armchairRB.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, climbObjectCollider.bounds.center.y + collider.bounds.size.y/2, playerData.playerRigidbody.gameObject.transform.position.z);
    //            }
    //        }   
    //        else
    //        {
    //            armchair = GameObject.FindGameObjectWithTag("Armchair").transform.parent.gameObject;
    //            armchairRB = armchair.GetComponent<Rigidbody2D>();
    //        }

    //        if (!playerData.insideElevator)
    //        {
    //            playerData.playerAnimator.PlayerElevatorEnter();
    //            playerData.insideElevator = true;
    //            playerData.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    //        }

    //        else
    //        {
    //            ElevatorCatch();
    //            playerData.playerRigidbody.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, climbObjectCollider.bounds.center.y + playerData.playerWalkingCollider.bounds.size.y/2, playerData.playerRigidbody.gameObject.transform.position.z);

    //        }
    //    }
    //    else
    //    {
    //        playerData.insideElevator = false;
    //    }
    //    if(ElevatorAutoEvent.ToBeEnabled != null)
    //    {
    //        if ((bool)ElevatorAutoEvent.ToBeEnabled)
    //        {
    //            playerInZone = false;
    //            UnFreezePlayer();
    //        }
    //    }
    //    if(DisableColliders)
    //    {
    //        if ((bool)ElevatorAutoEvent.ToBeEnabled)
    //        {
    //            playerInZone = false;
    //            UnFreezePlayer();
    //        }
    //        var temp = transform.parent.GetComponentsInChildren<Collider2D>();
    //        foreach (var collider in temp)
    //        {
    //            collider.enabled = false;
    //        }
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

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Armchair"))
    //    {
    //        armchairInZone = true;
    //    }
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        playerInZone = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Armchair"))
    //    {
    //        armchairInZone = false;
    //    }
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        playerInZone = false;
    //    }
    //}
}
