using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityMovement : MonoBehaviour
{
    enum MovementType
    {
        Walking,
        Crawling,
        CatchRight,
        CatchLeft,
        InteractRight,
        InteractLeft
    }

    enum InteractionType
    {
        None,
        Push,
        Pull
    }

    InteractionType interactionType;
    MovementType movementType;

    [Header("Gravity Settings")]
    /// <summary> The horizontal force applied when walking. </summary> 
    [SerializeField] float BaseWalkForce = 10f;
    /// <summary> The horizontal force applied when crawling. </summary> 
    [SerializeField] float BaseCrawlForce = 5f;
    ///<summary> The horizontal force applied when pushing an object. </summary>
    [SerializeField] float BasePushForce = 6f;
    ///<summary> The horizontal force applied when pulling an object. </summary>
    [SerializeField] float BasePullForce = 4f;
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

    GameObject playerGroundObject;
    Collider2D playerGroundCollider;

    Collider2D climbCollider;

    GameObject interactingObject;

    //map edges
    float upperLimit;
    float lowerLimit;
    float rightLimit;
    float leftLimit;

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

    //unwalkable variables
    float unwalkableRight;
    float unwalkableLeft;

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

    //booleans
    bool isManoeuvring;
    bool grounded;
    bool isJumping;
    bool hasJumped;
    bool canJump;
    bool hasCaught;
    bool isInteracting;

    private void Start()
    {
        //set initial values
        grounded = true;
        isManoeuvring = false;
        movementType = MovementType.Walking;
        interactionType = InteractionType.None;

        castDistance = 100;

        groundLayer = LayerMask.GetMask("Ground"); 
        playerLayer = LayerMask.GetMask("Player");
        crawlLayer = LayerMask.GetMask("CrawlSpace");
        climbLayerRight = LayerMask.GetMask("ClimbableRight");
        climbLayerLeft = LayerMask.GetMask("ClimbableLeft");
        interactbleLayerRight = LayerMask.GetMask("InteractableRight");
        interactbleLayerLeft = LayerMask.GetMask("InteractableLeft");
        unwalkableLayerRight = LayerMask.GetMask("WallRight");
        unwalkableLayerLeft = LayerMask.GetMask("WallLeft");

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

        if (hasJumped && grounded)
        {
            hasJumped = false;
        }

        if (grounded || movementType == MovementType.Crawling)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        GetEdgePositions(gameObject);
        getMovementType();
        userInput();
        DetectGround();
        Movement();
    }

    void maintainAreaLimits() //stops the player from leaving the level area
    {
        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        }

        if (transform.position.x < leftLimit) 
        {
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z); 
        }

        if (transform.position.y > upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit, transform.position.z);
        }

        if (transform.position.y < lowerLimit)
        {
            transform.position = new Vector3(transform.position.x, lowerLimit, transform.position.z);
        }
    }

    void getMovementType()
    {
        RaycastHit2D catchHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, climbLayerRight | climbLayerLeft);
        RaycastHit2D interactHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, interactbleLayerRight | interactbleLayerLeft);
        RaycastHit2D crawlHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, crawlLayer);

        if (crawlHit.collider != null && grounded) //no animation or rotation
        {
            hasCaught = false;
            movementType = MovementType.Crawling;
        }
        else if (interactHit.collider != null && grounded && SystemSettings.interact)
        {
            interactingObject = interactHit.collider.GetComponentInParent<InteractableObject>().gameObject;
            transform.position = interactHit.collider.transform.position;
            if (interactHit.collider.gameObject.layer == 9)
            {
                movementType = MovementType.InteractRight;
                isInteracting = true;
            }
            else if (interactHit.collider.gameObject.layer == 10)
            {
                movementType = MovementType.InteractLeft;
                isInteracting = true;
            }
        }
        else if (catchHit.collider != null && !isJumping && hasJumped && !grounded)
        {
            if (catchHit.collider.gameObject.layer == 7)
            {
                activateJumpCatch(MovementType.CatchRight, climbLayerRight);
            }
            else if (catchHit.collider.gameObject.layer == 8)
            {
                activateJumpCatch(MovementType.CatchLeft, climbLayerLeft);
            }
        }
        else if (movementType != MovementType.CatchRight && movementType != MovementType.CatchLeft)
        {
            hasCaught = false;
            movementType = MovementType.Walking;
        }

        switch (movementType)
        {
            case MovementType.Walking:
                currentHorizontalForce = BaseWalkForce;
                break;
            case MovementType.Crawling:
                currentHorizontalForce = BaseCrawlForce;
                break;
            case MovementType.CatchRight:
                break;
            case MovementType.CatchLeft:
                break;
        }

        void activateJumpCatch(MovementType movement, LayerMask layer)
        {
            movementType = movement;
            hasJumped = false;
            try
            {
                climbCollider.gameObject.SetActive(true);
            }
            catch
            {

            }
            climbCollider = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, layer).collider;
            transform.position = climbCollider.transform.position;
            climbCollider.gameObject.SetActive(false);
            hasCaught = true;
        }
    }

    void userInput()
    {
        if (movementType == MovementType.InteractRight || movementType == MovementType.InteractLeft)
        {

        }

        if (!isManoeuvring)
        {
            if ((SystemSettings.tapRight && movementType == MovementType.CatchRight) || (SystemSettings.tapLeft && movementType == MovementType.CatchLeft))
            {
                ClimbUpObstacle();
            }

            if (SystemSettings.moveRight && !SystemSettings.moveLeft)
            {
                if (movementType == MovementType.CatchRight)
                {
                    return;
                }
                else if (movementType == MovementType.CatchLeft)
                {
                    hasCaught = false;
                    movementType = MovementType.Walking;
                }

                if (movementType == MovementType.InteractRight)
                {
                    interactionType = InteractionType.Push;
                    currentHorizontalForce = BasePushForce;
                    interactingObject.GetComponent<InteractableObject>().Interact(gameObject); //this is messy, and expensive
                }
                else if (movementType == MovementType.InteractLeft)
                {
                    interactionType = InteractionType.Pull;
                    currentHorizontalForce = BasePullForce;
                    interactingObject.GetComponent<InteractableObject>().Interact(gameObject);
                }
                horizontalSpeed = currentHorizontalForce;
                transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), transform.position.z + playerGroundObject.transform.right.z);
            }
            else if (SystemSettings.moveLeft && !SystemSettings.moveRight)
            {
                if (movementType == MovementType.CatchLeft)
                {
                    return;
                }
                else if (movementType == MovementType.CatchRight)
                {
                    hasCaught = false;
                    movementType = MovementType.Walking;
                }

                if (movementType == MovementType.InteractRight)
                {
                    interactionType = InteractionType.Pull;
                    currentHorizontalForce = BasePullForce;
                    interactingObject.GetComponent<InteractableObject>().Interact(gameObject);
                }
                else if (movementType == MovementType.InteractLeft)
                {
                    interactionType = InteractionType.Push;
                    currentHorizontalForce = BasePushForce;
                    interactingObject.GetComponent<InteractableObject>().Interact(gameObject);
                }
                horizontalSpeed = -currentHorizontalForce;
                transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), transform.position.z + playerGroundObject.transform.right.z);
            }

            if (grounded)
            {
                try
                {
                    climbCollider.gameObject.SetActive(true);
                }
                catch 
                {

                }
            }

            if (SystemSettings.jump && canJump)
            {
                Debug.Log("Is JUmpinh");
                grounded = canJump;
                hasJumped = true;
                verticalSpeed = BaseJumpForce;
                transform.position = new Vector3(transform.position.x, transform.position.y + (verticalSpeed * Time.deltaTime), transform.position.z);
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

        if (!grounded && !hasCaught)
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

        RaycastHit2D unwalkableRightCheck = Physics2D.BoxCast(transform.position, playerSize, 0, transform.right, castDistance, unwalkableLayerRight);
        RaycastHit2D unwalkableLeftCheck = Physics2D.BoxCast(transform.position, playerSize, 0, -transform.right, castDistance, unwalkableLayerLeft);

        float checkGroundY;
        int indexCheck = 0;


        //if (transform.position.x + (playerSize.x / 2) > unwalkableRight)
        //{            
        //    transform.position = new Vector3(unwalkableRight - (playerSize.x / 2), transform.position.y, transform.position.z);
        //}

        try
        {
            unwalkableRight = unwalkableRightCheck.collider.transform.position.x;
        }
        catch
        {

        }

        if (transform.position.x - (playerSize.x / 2) < unwalkableLeft)
        {
            transform.position = new Vector3(unwalkableLeft + (playerSize.x / 2), transform.position.y, transform.position.z);
        }
        try
        {
            unwalkableLeft = unwalkableLeftCheck.collider.transform.position.x;
        }
        catch
        {

        }

        try
        {
            checkGroundY = groundChecks[0].collider.ClosestPoint(transform.position).y;

            for (int c = 0; c < groundChecks.Length; c++)
            {
                if (Vector2.Distance(bottomEdgePosition, new Vector2(transform.position.x, groundChecks[c].collider.ClosestPoint(transform.position).y)) < TerminalVelocity * Time.deltaTime && !isJumping)
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

            if (Vector2.Distance(bottomEdgePosition, new Vector2(transform.position.x, checkGroundY)) > TerminalVelocity * Time.deltaTime)
            {
                grounded = false;
                groundY = checkGroundY;
                groundNormal = groundChecks[indexCheck].normal;
                verticalSpeed -= GravityForce * Time.deltaTime;
            }

            playerGroundObject.transform.rotation = groundChecks[indexCheck].collider.transform.rotation;
        }
        catch
        {
            Debug.LogError("No Ground Found");
            checkGroundY = groundY;
            transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0);
            grounded = true;
            verticalSpeed = 0;
            horizontalSpeed = 0;
        }
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

        //temp
        climbCollider.gameObject.SetActive(true);
        Collider2D[] attachedColliders = climbCollider.GetComponentsInChildren<Collider2D>();
        Collider2D targetCollider = new Collider2D();

        for (int i = 0; i < attachedColliders.Length; i++)
        {
            if (attachedColliders[i] != climbCollider)
            {
                targetCollider = attachedColliders[i];
            }
        }

        transform.position = targetCollider.transform.position;
        movementType = MovementType.Walking;
        isManoeuvring = false;
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
