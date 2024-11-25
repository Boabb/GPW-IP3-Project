using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    CollisionDetector collisionDetector = new CollisionDetector();
    MovementTypes movementTypes;

    //player info
    GameObject playerObject;
    Collider2D currentPlayerCollider;

    //ground check variables
    GameObject playerGroundObject; //rotated for movement up slopes
    float groundY;
    bool grounded;
    bool canJump;
    bool hasJumped = false;
    bool isFalling;

    //jumping variables
    bool isJumping = false;

    //gravity variables
    float terminalVelocity;
    float gravityForce;

    //movement variables
    GameManager.MovementType movementType;
    float currentHorizontalForce;
    float verticalSpeed;
    float horizontalSpeed;

    List<GameManager.UnwalkableCoordinates> unwalkableCoordinates;

    //manoeuvre variables
    public bool manoeuvring;
    bool interact;
    Manoeuvre currentManoeuvre;

    //layer masks
    LayerMask groundLayer;
    LayerMask playerLayer;
    LayerMask crawlLayer;
    LayerMask climbLayerRight;
    LayerMask climbLayerLeft;
    LayerMask interactbleLayerRight;
    LayerMask interactbleLayerLeft;
    LayerMask unwalkableLayerRight;
    LayerMask unwalkableLayerLeft;

    private void Start()
    {
        movementTypes = GetComponent<MovementTypes>();
        terminalVelocity = gameManager.GetTerminalVelocity();
        gravityForce = gameManager.GetGravityForce();

        movementTypes.gameManager = gameManager;
        movementType = GameManager.MovementType.walk;

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        crawlLayer = LayerMask.GetMask("CrawlSpace");
        climbLayerRight = LayerMask.GetMask("ClimbableRight");
        climbLayerLeft = LayerMask.GetMask("ClimbableLeft");
        interactbleLayerRight = LayerMask.GetMask("InteractableRight");
        interactbleLayerLeft = LayerMask.GetMask("InteractableLeft");
        unwalkableLayerRight = LayerMask.GetMask("WallRight");
        unwalkableLayerLeft = LayerMask.GetMask("WallLeft");

        playerGroundObject = GameObject.Find("PlayerGroundCollider");

        unwalkableCoordinates = new List<GameManager.UnwalkableCoordinates>();

        movementTypes.UpdateMovementType(ref currentPlayerCollider, ref currentHorizontalForce, true);
    }

    private void Update()
    {
        if (manoeuvring)
        {
            currentManoeuvre.UpdateManoeuvre();
        }
        else
        {
            CheckGround();
            CheckUnwalkables();
            GetAirType();
            DetectManoeuvre();
            ProcessInput();
            ApplyGravity();
            CheckManoeuvring();
        }
    }

    void CheckManoeuvring()
    {
        if (!manoeuvring)
        {
            currentManoeuvre = null;
        }
    }

    void GetAirType()
    {
        if (verticalSpeed > 0)
        {
            isJumping = true;
            isFalling = false;
        }
        else
        {
            isJumping = false;
        }

        if (verticalSpeed < 0)
        {
            isFalling = true;
            isJumping = false;
        }
        else
        {
            isFalling = false;
        }

        if (grounded) 
        {
            isFalling = false;
            isJumping = false;
            hasJumped = false;
        }
    }

    void ProcessInput()
    {
        if (SystemSettings.moveRight && !SystemSettings.moveLeft) //move right
        {
            movementTypes.UpdateMovementType(ref currentPlayerCollider, ref currentHorizontalForce, false);
            //moves you right by a speed

            for (int i = 0; i < unwalkableCoordinates.Count; i++)
            {
                if (GetPlayerRightEdge() > unwalkableCoordinates[i].rightX && GetPlayerLeftEdge() < unwalkableCoordinates[i].rightX && GetPlayerBottomEdge() < unwalkableCoordinates[i].rightY)
                {
                    transform.position = new Vector3(unwalkableCoordinates[i].rightX - (GetPlayerSize().x / 2), transform.position.y, transform.position.z);
                    currentHorizontalForce = 0;
                }
            }

            horizontalSpeed = currentHorizontalForce;
            Move();
            //changes animation
            //changes sound effect
        }
        else if (SystemSettings.moveLeft && !SystemSettings.moveRight) //move left
        {
            movementTypes.UpdateMovementType(ref currentPlayerCollider, ref currentHorizontalForce, true);
            //moves you left by a speed

            for (int i = 0; i < unwalkableCoordinates.Count; i++)
            {
                if (GetPlayerLeftEdge() < unwalkableCoordinates[i].leftX && GetPlayerRightEdge() > unwalkableCoordinates[i].leftX && GetPlayerBottomEdge() < unwalkableCoordinates[i].leftY)
                {
                    transform.position = new Vector3(unwalkableCoordinates[i].leftX + (GetPlayerSize().x / 2), transform.position.y, transform.position.z);
                    currentHorizontalForce = 0;
                }
            }
            horizontalSpeed = -currentHorizontalForce;
            Move();
            //changes animation
            //changes sound effect
        }

        if (SystemSettings.jump)
        {
            canJump = true; //temp
            //jumps
            if (canJump && grounded)
            {
                CheckGround();
                grounded = canJump;
                hasJumped = true;
                verticalSpeed = gameManager.GetBaseJumpForce();
                transform.position += playerGroundObject.transform.up * (verticalSpeed * Time.deltaTime);
                //changes animation
            }
        }

        if (SystemSettings.interact && interact)
        {
            //begins interaction with an interactable triggered by a hold
        }

        if (SystemSettings.tapInteract && interact)
        {
            //begins interaction with an interactable triggered by a tap
        }

        if (SystemSettings.tapLeft)
        {

            //begins climb manoeuvre
            if (currentManoeuvre.manoeuvreID == Manoeuvre.ManoeuvreID.catchClimb)
            {
                currentManoeuvre.UpdateManoeuvre();
            }
        }

        if(SystemSettings.tapRight)
        {
            //begins climb manoeuvre
            if (currentManoeuvre.manoeuvreID == Manoeuvre.ManoeuvreID.catchClimb)
            {
                currentManoeuvre.UpdateManoeuvre();
            }
        }

        //if() //no input
        //{
        //    //begins idle animation
        //}
    }

    void Move()
    {
        transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime),
            transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime),
            transform.position.z + playerGroundObject.transform.right.z);
    }

    void DetectManoeuvre()
    {
        //check crawl
        RaycastHit2D crawlHit = Physics2D.BoxCast(transform.position, GetPlayerSize(), 0, transform.up, 0, crawlLayer);

        //check interaction
        RaycastHit2D interactHit = Physics2D.BoxCast(transform.position, GetPlayerSize(), 0, transform.up, 0, interactbleLayerRight | interactbleLayerLeft);

        //check for catch
        RaycastHit2D catchHit = Physics2D.BoxCast(new Vector3(transform.position.x, transform.position.y - gameManager.GetCatchReach(), 0), GetPlayerSize(), 0, transform.up, 0, climbLayerRight | climbLayerLeft);
        Debug.Log("CatchHitCollider: " + catchHit.collider);
        if (crawlHit.collider != null && grounded)
        {
            if (movementType != GameManager.MovementType.crawl)
            {
                movementType = GameManager.MovementType.crawl;
            }
        }
        else if (catchHit.collider != null && isFalling)
        {
            if (catchHit.collider.gameObject.layer == 7)
            {
                //right
                currentManoeuvre = new CatchClimb(false, climbLayerRight, this);
            }
            else if (catchHit.collider.gameObject.layer == 8)
            {
                //left
                currentManoeuvre = new CatchClimb(true, climbLayerLeft, this);
            }
        }
        else if(interactHit.collider != null)
        {
            interact = true;
        }
        else
        {
            movementType = GameManager.MovementType.walk;
        }
    }

    void CheckGround()
    {
        Vector3 groundNormal;
        Vector3 playerSize = GetPlayerSize();
        float bottomEdgePosition = GetPlayerBottomEdge();
        int index = 0;

        RaycastHit2D[] groundChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerCollider.transform.position.x, bottomEdgePosition + (playerSize.y / 2)), 
            playerSize, 0, -currentPlayerCollider.transform.up, 1000, groundLayer);
        
        grounded = collisionDetector.DetectGround(groundChecks, new Vector3(currentPlayerCollider.transform.position.x, bottomEdgePosition), 
            currentPlayerCollider.gameObject, terminalVelocity, isJumping, ref index);

        if (index > -1) //if ground was detected
        {
            groundY = groundChecks[index].collider.ClosestPoint(currentPlayerCollider.transform.position).y;
            groundNormal = groundChecks[index].normal;
            playerGroundObject.transform.rotation = groundChecks[index].collider.transform.rotation;

            if (grounded) //and the player is grounded
            {
                transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0); //make sure they are on the ground
                verticalSpeed = 0; //stop them from falling any further
            }
            else //and the player is not grounded
            {
                verticalSpeed -= gravityForce * Time.deltaTime; //fall
            }
        }
        else //if there is no ground detected (this should never occur)
        {
            transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0); //put them level to the last ground that was detected
            verticalSpeed = 0; //stop from falling
        } 
    }

    void CheckUnwalkables()
    {
        Vector3 playerSize = GetPlayerSize();
        RaycastHit2D[] unwalkableRightChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerCollider.transform.position.x - 100, currentPlayerCollider.transform.position.y), playerSize, 0, currentPlayerCollider.gameObject.transform.right, 100 * 2, unwalkableLayerRight);
        RaycastHit2D[] unwalkableLeftChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerCollider.transform.position.x + 100, currentPlayerCollider.transform.position.y), playerSize, 0, -currentPlayerCollider.gameObject.transform.right, 100 * 2, unwalkableLayerLeft);
        
        unwalkableCoordinates.Clear();
        unwalkableCoordinates = collisionDetector.DetectUnwalkables(unwalkableRightChecks, unwalkableLeftChecks);
    }

   
    void ApplyGravity()
    {
        if (verticalSpeed < -terminalVelocity)
        {
            verticalSpeed = -terminalVelocity;
        }

        if (!grounded) //&& !hasCaught)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (verticalSpeed * Time.deltaTime), transform.position.z);
        }
    }
    Vector3 GetPlayerSize()
    {
        return currentPlayerCollider.bounds.size;
    }
    float GetPlayerBottomEdge()
    {
        return currentPlayerCollider.bounds.min.y;
    }
    float GetPlayerTopEdge()
    {
        return currentPlayerCollider.bounds.max.y;
    }
    float GetPlayerLeftEdge()
    {
        return currentPlayerCollider.bounds.min.x;
    }
    float GetPlayerRightEdge()
    {
        return currentPlayerCollider.bounds.max.x;
    }
}
