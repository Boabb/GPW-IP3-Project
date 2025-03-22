using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingAutoEvent : AutoEvent
{
    bool activated = false;
    CameraController camCon;

    private void Start()
    {
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
    }

    public override void EventEnter(GameObject playerGO)
    {
        if(activated) return;
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = -50;
        camCon.LerpToZoom(0.5f, 1.5f);
        camCon.LerpToPositionY(0.5f, 0.12f);
    }

    public override void EventStay(GameObject playerGO)
    {
        if(activated) return;
        AudioManager.PlaySoundEffect(SoundEffectEnum.Marching);
    }

    public override void EventExit(GameObject playerGO)
    {
        if(activated) return;
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = 0;
        camCon.LerpToZoom(0.5f);
        camCon.LerpToPositionY(0.5f);
        activated = true;
    }
}


