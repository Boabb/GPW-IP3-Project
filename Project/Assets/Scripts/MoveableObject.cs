using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveableObject : MonoBehaviour
{
    //public float objectRBMass = 0.0f; //this should be 0 unless the object is moveable, then it should be equal to the mass of the object's rigidbody
    Rigidbody2D objectRB;
    Collider2D groundCollider;

    //[Header("Designers Don't Change!")]
    PlayerData playerData; //I have made this a static variable as part of GameManager for easier access and consistency
    //[HideInInspector] public float currentObjectRBMass; //this is the mass of the object when it is being moved by the player (equal to the player mass plus the object mass)

    private void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();
        groundCollider = GetComponentInChildren<EdgeCollider2D>();

        playerData = GameManager.playerData;
        //currentObjectRBMass = objectRBMass;
        //objectRB.mass = currentObjectRBMass;
    }

    private void Update()
    {
        //objectRB.mass = currentObjectRBMass;

        //stop objects from being moveable when Suzanne is jumping/falling
        if (!playerData.grounded)
        {
            objectRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            objectRB.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        //if (playerData.playerWalkingCollider.bounds.min.y <= groundCollider.bounds.max.y)
        //{
        //    Physics2D.IgnoreCollision(groundCollider, playerData.playerWalkingCollider, true);
        //}
        //else
        //{
        //    Physics2D.IgnoreCollision(groundCollider, playerData.playerWalkingCollider, false);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) //should also specify here whether the player is moving in the direction in which pushing would also 
        {

        }
    }


}
