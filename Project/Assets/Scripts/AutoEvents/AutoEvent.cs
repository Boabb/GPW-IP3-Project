using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoEvent : MonoBehaviour
{
    protected int enterCount = 0;
	protected int stayCount = 0;
	protected int exitCount = 0;

	public virtual void EventEnter(GameObject playerGO)
    {

    }
	public virtual void EventStay(GameObject playerGO)
	{

	}
	public virtual void EventExit(GameObject playerGO)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ++enterCount;
		EventEnter(collision.gameObject);
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
		++stayCount;
		EventStay(collision.gameObject);
	}
	private void OnTriggerExit2D(Collider2D collision)
    {
        ++exitCount;
		EventExit(collision.gameObject);
    }
}
