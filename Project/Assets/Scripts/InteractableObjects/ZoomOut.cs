using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : InteractableObject
{
    CameraController camCon;

    private void Start()
    {
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
    }

    public override void Interaction(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = -40;
        camCon.LerpToZoom(2f, 8.5f);
        camCon.LerpToPositionY(2f, 6.15f);
    }
}
