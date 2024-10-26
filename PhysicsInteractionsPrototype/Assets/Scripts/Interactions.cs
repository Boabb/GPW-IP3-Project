using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    //Rigidbody2D rb;
    public bool interact;
    Vector2 offset;

    public enum InteractionType
    {
        moveWithPlayer,
    }

    public InteractionType interactionType; 
    
    private void Start()
    {
        interact = false;
        //rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!interact)
        {
            //rb.constraints = RigidbodyConstraints2D.None;
            //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX;

            //rb.velocity = Vector3.zero;
        }
    }

    public void setOffset(Rigidbody2D player)
    {
        offset = new Vector2(transform.position.x, 0) - new Vector2(player.position.x, 0);
    }

    public void moveWithPlayer(Rigidbody2D player)
    {
        Debug.Log("Bloop");
        transform.position = player.position + offset;

        //rb.position = player.position;

        //rb.constraints = RigidbodyConstraints2D.None;
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //rb.velocity = player.velocity;
    }
}
