using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioJumpStuff : MonoBehaviour
{
    Collider2D groundCollider;

    // Start is called before the first frame update
    void Start()
    {
        groundCollider = GetComponentInChildren<EdgeCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("in box");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            groundCollider.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            groundCollider.enabled = true;
        }
    }
}
