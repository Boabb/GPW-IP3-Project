using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource currentSound;
    AudioSource previousSound;
    [SerializeField] AudioSource firstRoom;
    [SerializeField] AudioSource vent;
    [SerializeField] AudioSource outOfVent;

    bool inProgressSwitch;
    // Start is called before the first frame update
    void Start()
    {
        currentSound = firstRoom;
        currentSound.loop = true;
        currentSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousSound != null)
        {
            if (!previousSound.isPlaying && !currentSound.isPlaying)
            {
                currentSound.Play();
            }
        }
    }

    public void SwitchToVent()
    {
        previousSound = currentSound;
        previousSound.loop = false;
        currentSound = vent;
        currentSound.loop = true;
    }

    public void SwitchToOutOfVent()
    {
        previousSound = currentSound;
        previousSound.loop = false;
        currentSound = outOfVent;
        currentSound.loop = true;
    }
}
