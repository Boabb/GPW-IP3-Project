using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundObject : MonoBehaviour
{
    Collider2D objectCollider;

    [Header("Designers Don't Change!")]
    [SerializeField] PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        objectCollider = GetComponent<Collider2D>();
        //objectCollider.excludeLayers = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (!SystemSettings.interact && playerData.grounded) //when the player isn't holding the interact button and isnt in the air
        {
            objectCollider.excludeLayers = playerData.playerLayer; //the collider excludes the player layer
            //the player can walk past the object 
        }
        else //when the player is holding the interact button
        {
            bool overlap = CheckOverlap();

            if (overlap)
            {
                objectCollider.excludeLayers = playerData.playerLayer; //the collider excludes the player layer
                                                                       //the player can walk past the object
            }
            else
            {
                objectCollider.excludeLayers = 0; //the collider excludes nothing
                                                  //the player cannot walk past the object
            }
        }
    }

    public bool CheckOverlap()
    {
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        objectCollider.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);

        bool returnBool = false;

        for (int i = 0; i < overlappingColliders.Count; i++)
        {
            if (overlappingColliders[i] == playerData.playerOverlapCheckCollider) //if the collider overlaps with the player collider
            {
                returnBool = true;
                break;
            }
            else //if it doesnt overlap with the player collider
            {
                returnBool = false;
            }
        }

        return returnBool;
    }
}
