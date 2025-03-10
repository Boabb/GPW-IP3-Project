using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTags : MonoBehaviour
{
    bool debug = true;

    public bool interactable; //if the player taps the interact button while in contact with this collider, something happens
    public bool moveable; //if the player walks into this object while on the correct layer (foreground/background), the object is moved based on the direction the player is moving
    public bool quickClimbable; //if the player walks into this object while on the correct layer (foreground/background), the player climbs the object
    public bool clingClimbable; //if the player is in the cling radius of this object (regardless of layer), they will cling onto the edge of the object and the player will be able to climb it
    public bool foreground; //objects in the foreground are also tangible (the player can never just walk past them), if an object is quick climbable and moveable, it should be in the foreground and the moveable aspect will take priority
    public bool background; //objects in the background can be walked past if the player isn't actively 'interacting', this allows the player to move past moveable objects that are in the background
    public bool autoEvent; //when the player moves through the objects collider, an auto event occurs
    public bool elevatorCatch; //when the player catches onto the collider attached to the elevator, they will be physically stuck like when clinging but also triggering entering animation which gives the illusion of movement

    private void Start()
    {
        if (debug)
        {
            if (foreground && background)
            {
                Debug.LogError(gameObject + ": object cannot be foreground and background!");
            }

            if (quickClimbable && background && moveable)
            {
                Debug.LogError(gameObject + ": object cannot be quick climbable, background and moveable!");
            }
        }

        //put errors if object doesn't have the necessary scripts for its tags
    }

    private void Update()
    {

    }
}
