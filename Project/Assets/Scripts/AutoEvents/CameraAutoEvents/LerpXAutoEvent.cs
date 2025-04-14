using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpXAutoEvent : AutoEvent
{
    [SerializeField] float lerpToPositionX;
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.LerpToPositionX(0.5f, lerpToPositionX);
    }
}
