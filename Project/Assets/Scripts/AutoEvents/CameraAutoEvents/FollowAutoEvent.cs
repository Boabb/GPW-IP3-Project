using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAutoEvent : AutoEvent
{
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.BeginFollow();
    }
}
