using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAutoEvent : AutoEvent
{
    public override void EventEnter(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = -50;
        AudioManager.PlayVoiceOverAudio(VoiceOver.Level1Track2);
    }

    public override void EventExit(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = 0;
    }
}
