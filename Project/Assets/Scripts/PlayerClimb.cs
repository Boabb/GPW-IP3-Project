using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    PlayerData playerData;

    Collider2D climbObjectCollider;
    ObjectTags currentClimbObjectTags;
    ClimbSide climbSide;
    ClimbType climbType;

    enum ClimbSide
    {
        None,
        Left,
        Right
    };

    enum ClimbType
    {
        None,
        Quick,
        Catch
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        climbType = ClimbType.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void QuickClimb()
    {

    }

    void CatchClimb()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ObjectTags tags = collider.gameObject.GetComponent<ObjectTags>();

        if(tags.quickClimbable)
        {
            currentClimbObjectTags = tags;
            climbObjectCollider = collider;
            climbType = ClimbType.Quick;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        ObjectTags tags = collider.gameObject.GetComponent<ObjectTags>();

        if (tags.catchClimbable)
        {
            currentClimbObjectTags = tags;
            climbObjectCollider = collider;
            climbType = ClimbType.Catch;
        }
    }
}
