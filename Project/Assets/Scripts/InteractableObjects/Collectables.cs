using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : InteractableObject
{
    [SerializeField] GameObject CollectableVisual;
    [SerializeField] GameObject Animated;
    GameObject Player;
    AudioSource CollectableAudio;

    // Start is called before the first frame update
    void Start()
    {
        CollectableAudio = CollectableVisual.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CollectableVisual.activeSelf && !CollectableAudio.isPlaying) //automatically closes 
        {
            CollectableVisual.SetActive(false); 
            Player.GetComponent<GravityMovement>().isManoeuvring = false;
            Animated.SetActive(false);
        }
    }

    public override void Interaction(GameObject playerGO)
    {
        if (Animated.activeSelf)
        {
            Player = playerGO;
            Player.GetComponent<GravityMovement>().isManoeuvring = true;
            CollectableVisual.SetActive(true);
            CollectableAudio.Play();
        }
    }
}
