using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchClimb : Manoeuvre
{
    public bool left;
    LayerMask layerMask;

    Collider2D climbCollider;
    GameObject player;
    PlayerController playerCon;

    public CatchClimb(bool isLeft, LayerMask layer, PlayerController playerController)
    {
        manoeuvreID = ManoeuvreID.catchClimb;
        left = isLeft;
        layerMask = layer;
        player = playerController.gameObject;
        playerCon = playerController;

        BeginManoeuvre(playerController);
    }

    public override void BeginManoeuvre(PlayerController playerController)
    {
        //catch

        if (climbCollider != null)
        {
            climbCollider.gameObject.SetActive(true);
        }

        climbCollider = Physics2D.BoxCast(player.transform.position, player.GetComponent<Collider2D>().bounds.size, 0, player.transform.up, 0, layerMask).collider;
        player.transform.position = climbCollider.transform.position;
        climbCollider.gameObject.SetActive(false);
    }

    public override void UpdateManoeuvre()
    {
        //manoeuvre activated
        if (left && SystemSettings.tapLeft)
        {
            //enter manoeuvre in here
            playerCon.manoeuvring = true;
            climbCollider.gameObject.SetActive(true);
            Collider2D[] attachedColliders = climbCollider.GetComponentsInChildren<Collider2D>();
            Collider2D targetCollider = new Collider2D();

            for (int i = 0; i < attachedColliders.Length; i++)
            {
                if (attachedColliders[i] != climbCollider)
                {
                    targetCollider = attachedColliders[i];
                }
            }

            player.transform.position = targetCollider.transform.position;
            EndManoeuvre();
        }
        else if (!left && SystemSettings.tapRight) 
        {
            //enter manoeuvre in here
            playerCon.manoeuvring = true;
            climbCollider.gameObject.SetActive(true);
            Collider2D[] attachedColliders = climbCollider.GetComponentsInChildren<Collider2D>();
            Collider2D targetCollider = new Collider2D();

            for (int i = 0; i < attachedColliders.Length; i++)
            {
                if (attachedColliders[i] != climbCollider)
                {
                    targetCollider = attachedColliders[i];
                }
            }

            player.transform.position = targetCollider.transform.position;
            EndManoeuvre();
        }
    }

    public override void EndManoeuvre() 
    {
        //finish manoeuvre
        playerCon.manoeuvring = false;
    }
}
