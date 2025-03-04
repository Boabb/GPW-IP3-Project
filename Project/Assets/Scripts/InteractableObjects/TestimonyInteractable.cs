using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestimonyInteractable : InteractableObject
{
    [SerializeField] int testimonyNumber;
    public override void Interaction(GameObject playerGO)
    {
        AudioManager.PlayVoiceOverAudio((VoiceOver)testimonyNumber);
    }
}
