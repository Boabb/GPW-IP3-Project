using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAutoEvent : AutoEvent
{
    public Animator openCloseAnimator;
    public bool elevatorOpen;
    public string elevatorEnterableStateName = "ElevatorOpen";
    public void Awake()
    {
        openCloseAnimator = GetComponent<Animator>();
    }
    public override void EventEnter(GameObject playerGO)
    {

        openCloseAnimator.SetTrigger("Open/Close");
    }
    public override void EventStay(GameObject playerGO)
    {
        if (openCloseAnimator != null)
        {

            if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName(elevatorEnterableStateName))
            {
                elevatorOpen = true;
            }
        }
    }
}
