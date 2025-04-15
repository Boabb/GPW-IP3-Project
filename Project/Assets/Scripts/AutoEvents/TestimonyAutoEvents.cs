using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestimonyAutoEvents : AutoEvent
{
    bool activated = false;
    [SerializeField] VoiceOverEnum voiceClip;

    public override void EventEnter(GameObject playerGO)
    {
        AudioManager.PlayVoiceOverWithSubtitles(voiceClip);
    }
}
