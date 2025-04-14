using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPositionAutoEvent : AutoEvent
{
    [SerializeField] Vector3 lerpPosition;
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.LerpToPosition(0.5f, lerpPosition);
    }
}
