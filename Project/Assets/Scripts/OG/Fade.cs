using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] Follower follower;
    [SerializeField] Renderer Fade1;
    [SerializeField] Renderer Fade2;

    [SerializeField] MusicController musicController; //this is a bad way of doing this, it shouldnt be in the fade it should be in an autoevent and fade itself should be
                                                       //an autoevent

    Collider2D fadeCollider;

    Color hidden = new Color(1,1,1,1);
    Color revealed = new Color(1,1,1,0);

    float timer = 0;
    float adder = 1f;

    AudioSource audioSource;

    public bool collision = false;
    bool switchOverTime = false;
    bool switchBetween = true;

    bool playAudio = true;

    bool shake = true;
    float shakeAdder = 0;

    private void OnValidate()
    {
        Fade1.sharedMaterial.color = revealed;
    }
    private void Awake()
    {
        fadeCollider = GetComponentInChildren<Collider2D>();
        audioSource = GetComponentInChildren<AudioSource>();

        Fade1.material.color = hidden;
        Fade2.material.color = revealed;
    }

    private void Update()
    {
        if (collision)
        {
            collision = false;
            switchBetween = !switchBetween;
            switchOverTime = true;
        }

        if (switchOverTime)
        {
            SwitchOverTime();
        }


    }

    void QuickSwitch()
    {
        if (switchBetween)
        {
            Fade1.material.color = hidden;
            Fade2.material.color = revealed;
        }
        else
        {
            Fade2.material.color = hidden;
            Fade1.material.color = revealed;
        }
    }

    void SwitchOverTime()
    {
        timer += adder * Time.deltaTime;

        if (audioSource.isPlaying == false && playAudio)
        {
            playAudio = false;
            audioSource.Play();
            musicController.SwitchToVent();
        }

        if (switchBetween)
        {
            Fade1.material.color = new Color(1,1,1, timer);
            Fade2.material.color = new Color(1, 1, 1, 1 - timer);
        }
        else
        {
            Fade1.material.color = new Color(1, 1, 1, 1 - timer);
            Fade2.material.color = new Color(1, 1, 1, timer);
        }

        shakeAdder += 1.15f * Time.deltaTime;

        if (shakeAdder > 8 && shake)
        {
            shake = false;
            follower.StartShake();
            musicController.SwitchToOutOfVent();
        }

        if (timer >= 1 && !shake)
        {
            timer = 0;
            switchOverTime = false;
        }
    }
}
