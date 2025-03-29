using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1StartAutoEvent : AutoEvent
{
    public override void EventEnter(GameObject playerGO)
    {
        AudioManager.PlayVoiceOverWithSubtitles(VoiceOverEnum.Level1Track1);

    }

    public override void EventExit(GameObject playerGO)
    {
        
    }
}
