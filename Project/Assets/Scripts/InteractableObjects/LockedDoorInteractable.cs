using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorInteractable : DoorInteractable
{
    [SerializeField] bool lockedLeft = false;
    [SerializeField] bool locked = true;

    public override void Interaction(GameObject playerGO)
    {
        if (locked)
        {
            locked = CheckUnlock(playerGO);
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
        bool left = true;
        if (playerGO.transform.position.x > transform.position.x)
        {
            left = false;
        }

        if (lockedLeft && !left || !lockedLeft && left)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
