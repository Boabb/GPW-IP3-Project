using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorInteractable : DoorInteractable
{
    [SerializeField] bool lockedLeft = false;
    [SerializeField] bool locked = true;
    [SerializeField] Window window; // Reference to the window script

    public override void Interaction(GameObject playerGO)
    {
        if (locked)
        {
            locked = CheckUnlock(playerGO);
            
            if (window != null) // Ensure window reference exists
            {
                window.OpenWindow();
            }
            else
            {
                print("No window, BREAKING!");
            }
        }

        if (!locked)
        {
            base.Interaction(playerGO);

            
        }
        else
        {
            AudioManager.PlaySoundEffect(SoundEffectEnum.LockedDoor);
        }
    }

    bool CheckUnlock(GameObject playerGO)
    {
        bool left = playerGO.transform.position.x <= transform.position.x;
        return !(lockedLeft && !left || !lockedLeft && left);
        
        
    }
}