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
    bool hasJumped;
    bool isFalling;

    //jumping variables
    bool isJumping;

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
    bool manoeuvring;
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

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        crawlLayer = LayerMask.GetMask("CrawlSpace");
        climbLayerRight = LayerMask.GetMask("ClimbableRight");
        climbLayerLeft = LayerMask.GetMask("ClimbableLeft");
        interactbleLayerRight = LayerMask.GetMask("InteractableRight");
        interactbleLayerLeft = LayerMask.GetMask("InteractableLeft");
        unwalkableLayerRight = LayerMask.GetMask("WallRight");
        unwalkableLayerLeft = LayerMask.GetMask("WallLeft");
    }

    private void Update()
    {
        if (manoeuvring)
        {
            currentManoeuvre.UpdateManoeuvre();
        }
        else
        {
            ProcessInput();
        }
    }

    void ProcessInput()
    {
        if (SystemSettings.moveRight && !SystemSettings.moveLeft) //move right
        {
            HoldRight();
        }
        else if (SystemSettings.moveLeft && !SystemSettings.moveRight) //move left
        {
            HoldLeft();
        }

        if (SystemSettings.jump) //jump
        {
            TapJump();
        }
    }

    void Move()
    {
        transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime),
            transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime),
            transform.position.z + playerGroundObject.transform.right.z);
    }

    void CheckGround()
    {
        Vector3 groundNormal;
        Vector3 playerSize = GetPlayerSize();
        float bottomEdgePosition = GetPlayerBottomEdge();
        int index = 0;

        RaycastHit2D[] groundChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerCollider.transform.position.x, bottomEdgePosition + (playerSize.y / 2)), 
            playerSize, 0, -currentPlayerCollider.transform.up, 0, groundLayer);
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

    public void HoldLeft()
    {
        //moves you left by a speed
        if (movementType == GameManager.MovementType.pushPullRight)
        {
            currentHorizontalForce = gameManager.GetBasePushForce();
        }
        else if (movementType == GameManager.MovementType.pushPullLeft)
        {
            currentHorizontalForce = gameManager.GetBasePullForce();
        }

        horizontalSpeed = -currentHorizontalForce;
        Move();
        //changes animation
        //changes sound effect
    }

    public void HoldRight()
    {
        //moves you right by a speed
        if (movementType == GameManager.MovementType.pushPullLeft)
        {
            currentHorizontalForce = gameManager.GetBasePushForce();
        }
        else if (movementType == GameManager.MovementType.pushPullRight)
        {
            currentHorizontalForce = gameManager.GetBasePullForce();
        }

        horizontalSpeed = currentHorizontalForce;
        Move();
        //changes animation
        //changes sound effect
    }

    public void TapLeft()
    {
        //begins climb manoeuvre 
    }

    public void TapRight()
    {
        //begins climb manoeuvre
    }

    public void TapJump()
    {
        //jumps
        if(canJump && grounded)
        {
            hasJumped = true;
            verticalSpeed = gameManager.GetBaseJumpForce();
            transform.position += playerGroundObject.transform.up * (verticalSpeed * Time.deltaTime);

            //changes animation
        }
    }

    public void HoldInteract()
    {
        //begins interaction with an interactable triggered by a hold
    }

    public void TapInteract()
    {
        //begins interaction with an interactable triggered by a tap
    }

    public void NoInput()
    {
        //begins idle animation
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
