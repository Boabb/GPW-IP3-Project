using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XFollowAutoEvent : AutoEvent
{
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.BeginFollowX();
        GameManager.cameraController.StopFollowY();
    }
}
