using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMovement : MonoBehaviour
{
    enum MovementType
    {
        Walking,
        Crawling
    }

    MovementType movementType;

    [Header("Gravity Settings")]
    /// <summary> The horizontal force applied when walking. </summary> 
    [SerializeField] float BaseWalkForce = 10f;
    /// <summary> The horizontal force applied when crawling. </summary> 
    [SerializeField] float BaseCrawlForce = 5f;
    ///<summary> The vertical force applied when jumping. </summary>
    [SerializeField] float BaseJumpForce = 15f;
    ///<summary> The force of gravity pulling the agent downwards </summary>
    [SerializeField] float GravityForce = 0.2f; 
    /// <summary> //the force which slows the agent while it is in the air </summary>
    [SerializeField] float AirResistance = 0.05f;
    ///<summary> The maximum speed the agent can fall at. </summary>
    [SerializeField] float TerminalVelocity = 15f;
    ///<summary> The margin in which the agent can land. </summary>
    [SerializeField] float GroundedMargin = 0.03f; //this should be roughly equal to TerminalVelocity * time.DeltaTime
    ///<summary> The margin in which the agent can reach a target. </summary>
    [SerializeField] float TargetMargin = 0.1f;

    //player info
    Vector3 rightEdgePosition;
    Vector3 leftEdgePosition;
    Vector3 topEdgePosition;
    Vector3 bottomEdgePosition;

    Vector3 playerSize;
    Collider2D playerCollisionCollider;

    Collider2D playerGroundCollider;
    GameObject playerGroundObject;

    //grounded varaibles
    float castDistance;
    float groundY;
    Vector3 groundNormal;

    //Movement variables
    float currentHorizontalForce;
    float horizontalSpeed;
    float verticalSpeed;

    //Manoeuvre variables
    Transform originalPosition;
    Transform targetPosition;
    float transformProgress;

    Quaternion originalRotation;
    Quaternion targetRotation;
    float rotationProgress;

    //layer masks
    LayerMask groundLayer;
    LayerMask playerLayer;
    LayerMask crawlLayer;
    LayerMask climbLayer;

    //booleans
    bool isManoeuvring;
    bool grounded;
    bool isJumping;
    bool canJump;

    private void Start()
    {
        //set initial values
        grounded = true;
        isManoeuvring = false;
        movementType = MovementType.Walking;

        castDistance = 100;

        groundLayer = LayerMask.GetMask("Ground"); 
        playerLayer = LayerMask.GetMask("Player");
        crawlLayer = LayerMask.GetMask("CrawlSpace");
        climbLayer = LayerMask.GetMask("Climbable");

        playerCollisionCollider = GetComponent<Collider2D>();
        playerSize = playerCollisionCollider.bounds.size;

        playerGroundObject = GameObject.Find("PlayerGroundCollider");
        playerGroundCollider = playerGroundObject.GetComponent<Collider2D>();

        DetectGround();
    }

    // Update is called once per frame
    void Update()
    {
        if (verticalSpeed > 0)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (grounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    
        getMovementType();
        userInput();
        DetectGround();
        Movement();
    }

    void getMovementType()
    {
        if (movementType != MovementType.Crawling && Physics2D.IsTouchingLayers(playerCollisionCollider, crawlLayer))
        {
            movementType = MovementType.Crawling;
        }
        else if (movementType != MovementType.Walking)
        {
            movementType = MovementType.Walking;
        }

        switch (movementType)
        {
            case MovementType.Walking:
                currentHorizontalForce = BaseWalkForce;
                break;
            case MovementType.Crawling:
                currentHorizontalForce = 0;
                break;
        }
    }

    void userInput()
    {
        if (!isManoeuvring)
        {
            if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                horizontalSpeed = currentHorizontalForce;
                transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), transform.position.z + playerGroundObject.transform.right.z);
            }
            else if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                horizontalSpeed = -currentHorizontalForce;
                transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), transform.position.z + playerGroundObject.transform.right.z);
            }

            if (SystemSettings.jump && canJump)
            {
                grounded = canJump;
                verticalSpeed = BaseJumpForce;
                transform.position = new Vector3(transform.position.x, transform.position.y + (verticalSpeed * Time.deltaTime), transform.position.z);

            }

            if (SystemSettings.interact)
            {
                Interact();
            }
        }
    }

    void Interact()
    {

    }

    void Movement()
    {
        if (!isManoeuvring)
        {
            ApplyGravity();
        }
        else
        {
            ApplyManoeuvre();
        }
    }

    void ApplyGravity()
    {
        if (verticalSpeed < -TerminalVelocity)
        {
            verticalSpeed = -TerminalVelocity;
        }

        if (!grounded)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (verticalSpeed * Time.deltaTime), transform.position.z);
        }
    }

    void ApplyManoeuvre()
    {

    }

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    void DetectGround()
    {
        GetEdgePositions(gameObject);

        RaycastHit2D[] groundChecks = Physics2D.BoxCastAll(new Vector3(bottomEdgePosition.x, bottomEdgePosition.y + (playerSize.y / 2)), playerSize, 0, -gameObject.transform.up, castDistance, groundLayer);
        float checkGroundY;
        int indexCheck = 0;

        try
        {
            checkGroundY = groundChecks[0].collider.ClosestPoint(transform.position).y;

            for (int c = 0; c < groundChecks.Length; c++)
            {
                if (Vector2.Distance(bottomEdgePosition, new Vector2(transform.position.x, groundChecks[c].collider.ClosestPoint(transform.position).y)) < GroundedMargin && !isJumping)
                {
                    checkGroundY = groundChecks[c].collider.ClosestPoint(transform.position).y;
                    groundY = checkGroundY;
                    indexCheck = c;

                    transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0);
                    grounded = true;
                    verticalSpeed = 0;
                    horizontalSpeed = 0;
                    break;
                }
            }
        }
        catch
        {
            Debug.LogError("No Ground Found");
            checkGroundY = groundY;
        }

        if (Vector2.Distance(bottomEdgePosition, new Vector2(transform.position.x, checkGroundY)) > GroundedMargin)
        {
            grounded = false;
            groundY = checkGroundY;
            groundNormal = groundChecks[indexCheck].normal;
            verticalSpeed -= GravityForce;
        }

        playerGroundObject.transform.rotation = groundChecks[indexCheck].collider.transform.rotation;
    }

    void GetEdgePositions(GameObject objectToGet)
    {
        rightEdgePosition = new Vector3(objectToGet.transform.position.x - (playerSize.x / 2), objectToGet.transform.position.y, objectToGet.transform.position.z);
        leftEdgePosition = new Vector3(objectToGet.transform.position.x + (playerSize.x / 2), objectToGet.transform.position.y, objectToGet.transform.position.z);
        topEdgePosition = new Vector3(objectToGet.transform.position.x, objectToGet.transform.position.y + (playerSize.y / 2), objectToGet.transform.position.z);
        bottomEdgePosition = new Vector3(objectToGet.transform.position.x, objectToGet.transform.position.y - (playerSize.y / 2), objectToGet.transform.position.z);    
    }

    void ClimbUpObstacle()
    {
        isManoeuvring = true;
    }

    void Manoeuvre()
    {
        //this will contain animation stuff, and remove control from the player during the period using the isManuveuring bool
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bottomEdgePosition - transform.up * castDistance, playerSize);
    }
}
