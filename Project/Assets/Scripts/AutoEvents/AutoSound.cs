using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSound : AutoEvent
{
    [SerializeField] Fade fader;
    [SerializeField] Camera mainCamera;
    AudioSource sound;
    bool played = false;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public override void Event(GameObject playerGO)
    {
        if (!played)
        {
            played = true;
            sound.Play();
            //playerGO.GetComponent<GravityMovement>().speedMultiplier = 0.1f;
            //mainCamera.orthographicSize = 2f;
            fader.collision = true;
        }
    }
}
