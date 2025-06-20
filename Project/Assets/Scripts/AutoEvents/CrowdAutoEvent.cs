using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAutoEvent : AutoEvent
{
    bool audioPlayed = false;

    public override void EventEnter(GameObject playerGO)
    {
        if (!audioPlayed)
        {
            AudioManager.PlayVoiceOverWithSubtitles(VoiceOverEnum.Level1Track2);
            audioPlayed = true;
        }

        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = -50;
    }

    public override void EventExit(GameObject playerGO)
    {
        playerGO.GetComponentInParent<PlayerData>().customPlayerVelocity = 0;
    }
}
