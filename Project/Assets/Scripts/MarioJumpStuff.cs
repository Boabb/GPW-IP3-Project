using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioJumpStuff : MonoBehaviour
{
    Collider2D groundCollider;
    Collider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        groundCollider = GetComponentInChildren<EdgeCollider2D>();
        playerCollider = GameManager.playerData.playerInteractCollider;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("in box");

        if (collision == playerCollider)
        {
            groundCollider.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            groundCollider.enabled = true;
        }
    }
}
