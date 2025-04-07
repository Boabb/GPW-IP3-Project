using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpYAutoEvent : AutoEvent
{
    [SerializeField] float lerpToPositionY;
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.LerpToPositionY(0.5f, lerpToPositionY);
    }
}
