using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAutoEvent : AutoEvent
{
    [SerializeField] float zoom;
    public override void EventEnter(GameObject playerGO)
    {
        GameManager.cameraController.LerpToZoom(0.5f, zoom);
    }
}
