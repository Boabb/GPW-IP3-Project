using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestimonyInteractable : InteractableObject
{
    [SerializeField] int testimonyNumber;
    public override void Interaction(GameObject playerGO)
    {
        AudioManager.PlayVoiceOverWithSubtitles((VoiceOverEnum.Level1Track1 + testimonyNumber), 1f);
    }
}
