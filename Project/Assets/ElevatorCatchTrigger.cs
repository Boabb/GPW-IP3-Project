using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCatchTrigger : MonoBehaviour
{
    PlayerData playerData;
    private bool armchairInZone;
    private bool playerInZone;

    Collider2D climbObjectCollider;
    ElevatorEnterMode climbType;

    enum ElevatorEnterMode
    {
        None,
        Catch
    }

    void Start()
    {
        playerData = GameManager.playerData;

        climbObjectCollider = GetComponent<Collider2D>();

        climbType = ElevatorEnterMode.None;
    }

    void FixedUpdate()
    {
        if(armchairInZone && playerInZone)
        {
            playerData.playerAnimator.PlayerElevatorEnter();
            playerData.clinging = true;
            playerData.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            if (!playerData.playerAnimator.enteringElevator)
            {
                UnFreezePlayer();
            }

            else
            {
                ElevatorCatch();
                playerData.playerRigidbody.gameObject.transform.position = new Vector3(climbObjectCollider.bounds.center.x, playerData.playerRigidbody.gameObject.transform.position.y, playerData.playerRigidbody.gameObject.transform.position.z);
            }
        }
        else
        {
            playerData.clinging = false;
        }
    }

    void UnFreezePlayer()
    {
        playerData.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        climbType = ElevatorEnterMode.None;
        playerData.playerAnimator.PlayerIdle();
    }

    void ElevatorCatch()
    {
        Debug.Log("ElevatorCatch");

        climbType = ElevatorEnterMode.Catch;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Armchair"))
        {
            armchairInZone = true;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true;
        }
    }
}
