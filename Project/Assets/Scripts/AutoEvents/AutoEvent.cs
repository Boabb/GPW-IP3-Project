using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoEvent : MonoBehaviour
{    
    public abstract void Event(GameObject playerGO);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Event(collision.gameObject);
    }
}
