using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoEvent : MonoBehaviour
{    
    public virtual void EventEnter(GameObject playerGO)
    {

    }
    public virtual void EventExit(GameObject playerGO)
    {

    }
    public virtual void EventStay(GameObject playerGO)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventEnter(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventExit(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        EventStay(collision.gameObject);
    }
}
