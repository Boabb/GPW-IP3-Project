using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAutoEvent : AutoEvent
{
    public Animator openCloseAnimator;
    public void Awake()
    {
        openCloseAnimator = GetComponent<Animator>();
    }
    public override void EventEnter(GameObject playerGO)
    {
        openCloseAnimator.SetTrigger("Open/Close");
    }
}
