using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSound : AutoEvent
{
    [SerializeField] Fade fader;
    [SerializeField] Camera mainCamera;
    bool played = false;

    public override void EventEnter(GameObject playerGO)
    {
        if (!played)
        {
            played = true;
            AudioManager.PlayVoiceOverAudio(VoiceOver.Wallenberg2);
            //playerGO.GetComponent<GravityMovement>().speedMultiplier = 0.1f;
            //mainCamera.orthographicSize = 2f;
            fader.collision = true;
        }
    }
}
