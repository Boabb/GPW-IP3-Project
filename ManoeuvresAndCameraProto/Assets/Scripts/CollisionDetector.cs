using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector
{
    //used to check whether the player is grounded
    public bool DetectGround(RaycastHit2D[] groundChecks, Vector3 bottomEdgePosition, GameObject collisionGO, float terminalVelocity, bool isJumping, ref int index)
    {
        index = 0; //the index of the ground in the groundChecks array
        float checkGroundY; //the y coordinate of the ground that is currently being checked

        if (groundChecks.Length < 1)
        {
            Debug.LogError("No Ground Found");
            index = -1; //this is an error
            return true; //keeps the player from falling if there is no ground
        }
        else
        {
            checkGroundY = groundChecks[index].collider.ClosestPoint(collisionGO.transform.position).y; //sets the ground to the first object in the array

            for (int c = 0; c < groundChecks.Length; c++) //loops through the entire array of ground colliders
            {
                if (Vector2.Distance(bottomEdgePosition, new Vector2(collisionGO.transform.position.x, groundChecks[c].collider.ClosestPoint(collisionGO.transform.position).y)) < terminalVelocity * Time.deltaTime && !isJumping) //if the player is close enough to the ground and isnt currently jumping
                {
                    index = c; //the index changes to the ground that player is currently standing on
                    return true;
                }
            }

            if (Vector2.Distance(bottomEdgePosition, new Vector2(collisionGO.transform.position.x, checkGroundY)) > terminalVelocity * Time.deltaTime) //if the player is either jumping or not close enough to the ground for a collision
            {
                //index here is 0
                return false;
            }
        }

        return false; //should never occur
    }

    //used to detect coordinates that cannot be walked through
    public List<GameManager.UnwalkableCoordinates> DetectUnwalkables(RaycastHit2D[] unwalkableRightChecks, RaycastHit2D[] unwalkableLeftChecks)
    {
        List<GameManager.UnwalkableCoordinates> unwalkableCoords = new List<GameManager.UnwalkableCoordinates>();

        for (int i = 0; i < unwalkableRightChecks.Length; i++) //loops through all the detected objects
        {
            for (int j = 0; j < unwalkableLeftChecks.Length; j++) //loops through all the detected objects
            {
                if (unwalkableRightChecks[i].collider.transform.parent.gameObject == unwalkableLeftChecks[j].collider.transform.parent.gameObject) //if both objects are attached to the same parents
                {
                    GameManager.UnwalkableCoordinates unwalkableCoordinates = new GameManager.UnwalkableCoordinates();

                    unwalkableCoordinates.rightX = unwalkableRightChecks[i].collider.bounds.max.x;
                    unwalkableCoordinates.leftX = unwalkableLeftChecks[j].collider.bounds.min.x;
                    unwalkableCoordinates.rightY = unwalkableRightChecks[i].collider.bounds.max.y;
                    unwalkableCoordinates.leftY = unwalkableLeftChecks[j].collider.bounds.max.y;

                    unwalkableCoords.Add(unwalkableCoordinates); //add the coordinates to the list
                }
            }
        }

        return unwalkableCoords; //return the list
    }
}