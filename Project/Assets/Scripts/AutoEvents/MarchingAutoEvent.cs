using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingAutoEvent : AutoEvent
{
    public float lerpTime;
    CameraController camCon;

    private void Start()
    {
        camCon = GameManager.mainCamera.GetComponent<CameraController>();
    }

    public override void EventEnter(GameObject playerGO)
    {
        if (enterCount > 1)
        {
            return;
        }
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = -50;
        camCon.LerpToZoom(lerpTime, 1.5f);
        camCon.LerpToPosition(lerpTime, new Vector3(4.25f, 0.12f));
    }

    public override void EventStay(GameObject playerGO)
    {
        if (exitCount > 1)
        {
            return;
        }
        AudioManager.PlaySoundEffect(SoundEffectEnum.Marching);
    }

    public override void EventExit(GameObject playerGO)
    {
        if (exitCount > 1)
        {
            return;
        }
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = 0;
        camCon.LerpToZoom(lerpTime);
        camCon.LerpToPositionY(lerpTime);
        camCon.BeginFollowX();

	}
}


