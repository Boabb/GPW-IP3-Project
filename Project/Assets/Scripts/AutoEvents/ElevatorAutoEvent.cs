using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorAutoEvent : AutoEvent
{
    public static bool? ToBeEnabled;
    [SerializeField] private GameObject gameObjectsToBeEnabled;
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
        if(ToBeEnabled != null)
        {
            gameObjectsToBeEnabled.SetActive((bool)ToBeEnabled);
        }
        if (openCloseAnimator != null)
        {

            if (openCloseAnimator.GetCurrentAnimatorStateInfo(0).IsName(elevatorEnterableStateName))
            {
                elevatorOpen = true;
            }
        }
    }
    public override void EventExit(GameObject playerGO)
    {
        gameObjectsToBeEnabled.SetActive(false);
    }
}
