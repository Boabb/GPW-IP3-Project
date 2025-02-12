using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityMovement : MonoBehaviour
{
    public struct UnwalkableCoordinates
    {
        public float rightX, leftX, rightY, leftY;
    }

    enum MovementType
    {
        Walking,
        Crawling,
        CatchRight,
        CatchLeft,
        MovableRight,
        MovableLeft
    }

    enum InteractionType
    {
        None,
        Push,
        Pull
    }

    InteractionType interactionType;
    MovementType movementType;
    PlayerAnimator playerAnimator;

    public float speedMultiplier = 1;

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
    //[SerializeField] float GroundedMargin = 0.03f; //this should be roughly equal to TerminalVelocity * Time.deltaTime (all uses should be replaced with TerminalVelocity * Time.deltaTime)
    ///<summary> The margin in which the agent can reach a target. </summary>
    [SerializeField] float TargetMargin = 0.1f;

    //player info
    Vector3 rightEdgePosition;
    Vector3 leftEdgePosition;
    Vector3 topEdgePosition;
    Vector3 bottomEdgePosition;

    Vector3 playerSize;
    Collider2D playerUprightCollider;
    Collider2D playerCrawlingCollider;

    GameObject playerGroundObject;
    Collider2D playerGroundCollider;

    Collider2D climbCollider;

    GameObject interactingObject;
    GameObject currentPlayerColliderGO;

    [SerializeField] Fade fader;

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
    List<UnwalkableCoordinates> unwalkableCoords;

    //layer masks
    LayerMask groundLayer;
    LayerMask playerLayer;
    LayerMask crawlLayer;
    LayerMask catchLayerRight;
    LayerMask catchLayerLeft;
    LayerMask movableLayerRight;
    LayerMask movableLayerLeft;
    LayerMask unwalkableLayerRight;
    LayerMask unwalkableLayerLeft;
    LayerMask interactableLayer;
    LayerMask autoEventLayer;

    //booleans
    public bool isManoeuvring;
    bool grounded;
    bool isJumping;
    bool hasJumped;
    bool canJump;
    bool hasCaught;

    private void Start()
    {
        //set initial values
        grounded = true;
        isManoeuvring = false;
        movementType = MovementType.Walking;
        interactionType = InteractionType.None;

        castDistance = 1000;

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        crawlLayer = LayerMask.GetMask("CrawlSpace");
        catchLayerRight = LayerMask.GetMask("ClimbableRight");
        catchLayerLeft = LayerMask.GetMask("ClimbableLeft");
        movableLayerRight = LayerMask.GetMask("MovableRight");
        movableLayerLeft = LayerMask.GetMask("MovableLeft");
        unwalkableLayerRight = LayerMask.GetMask("WallRight");
        unwalkableLayerLeft = LayerMask.GetMask("WallLeft");
        interactableLayer = LayerMask.GetMask("Interactable");
        autoEventLayer = LayerMask.GetMask("AutoEvent");

        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        playerUprightCollider = GameObject.Find("UprightCollider").GetComponent<Collider2D>();
        playerCrawlingCollider = GameObject.Find("CrawlingCollider").GetComponent<Collider2D>();

        currentPlayerColliderGO = playerUprightCollider.gameObject;
        playerSize = playerUprightCollider.bounds.size;

        playerGroundObject = GameObject.Find("PlayerGroundCollider");
        playerGroundCollider = playerGroundObject.GetComponent<Collider2D>();

        unwalkableCoords = new List<UnwalkableCoordinates>();

        DetectGround();
        DetectUnwalkables();
        DetectAutoEvents();
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
            AudioManager.PlaySoundEffect(SoundEffect.LandOnWood);
        }

        if (grounded && movementType == MovementType.Walking)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        GetPlayerEdgePositions();
        MovementTypeLogic();
        UserInput();

        if (movementType != MovementType.CatchLeft && movementType != MovementType.CatchRight)
        {
            DetectGround();
        }

        DetectUnwalkables();
        DetectAutoEvents();
        Movement();
    }

    void MovementTypeLogic()
    {
        GetPlayerEdgePositions();
        RaycastHit2D catchHit = Physics2D.BoxCast(new Vector3(transform.position.x, transform.position.y - playerSize.y / 4), new Vector3(playerSize.x, playerSize.y / 2), 0, transform.up, 0, catchLayerRight | catchLayerLeft);
        RaycastHit2D movableHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, movableLayerRight | movableLayerLeft);
        RaycastHit2D crawlHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, crawlLayer);

        //Grounded Logic
        if (grounded)
        {
            //Crawling
            if (crawlHit.collider != null)
            {
                if (movementType != MovementType.Crawling)
                {
                    fader.collision = true;
                    //transform.position = new Vector3(transform.transform.position.x, transform.position.y - 0.25f);
                }
                hasCaught = false;
                movementType = MovementType.Crawling;
            }
            //Movables
            else if (movableHit.collider != null && SystemSettings.interact)
            {
                interactingObject = movableHit.collider.GetComponentInParent<MovableInteractable>().gameObject;
                transform.position = movableHit.collider.transform.position;
                if (movableHit.collider.gameObject.layer == 9)
                {
                    movementType = MovementType.MovableRight;
                }
                else if (movableHit.collider.gameObject.layer == 10)
                {
                    movementType = MovementType.MovableLeft;
                }
            }
            else
            {
                movementType = MovementType.Walking;
            }
        }
        //Mid-Air Logic
        else
        {
            if (crawlHit.collider != null)
            {
                return;
            }
            else if (catchHit.collider != null && !isJumping && hasJumped)
            {
                if (catchHit.collider.gameObject.layer == 7)
                {
                    ActivateJumpCatch(MovementType.CatchRight, catchLayerRight, catchHit.collider);
                }
                else if (catchHit.collider.gameObject.layer == 8)
                {
                    ActivateJumpCatch(MovementType.CatchLeft, catchLayerLeft, catchHit.collider);
                }
            }
            else if (movementType != MovementType.CatchRight && movementType != MovementType.CatchLeft)
            {
                if (movementType == MovementType.Crawling)
                {
                    fader.collision = true;
                    //transform.position = new Vector3(transform.transform.position.x, transform.position.y + 0.5f);
                }
                hasCaught = false;
                movementType = MovementType.Walking;
            }
        }

        switch (movementType)
        {
            case MovementType.Walking:
                currentHorizontalForce = BaseWalkForce;
                currentPlayerColliderGO = playerUprightCollider.gameObject;
                playerSize = playerUprightCollider.bounds.size;
                break;
            case MovementType.Crawling:
                currentHorizontalForce = BaseCrawlForce;
                currentPlayerColliderGO = playerCrawlingCollider.gameObject;
                playerSize = playerCrawlingCollider.bounds.size;
                break;
            case MovementType.CatchRight:
                break;
            case MovementType.CatchLeft:
                break;
        }

        void ActivateJumpCatch(MovementType movement, LayerMask layer, Collider2D newClimbCollider)
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
            climbCollider = newClimbCollider;
            transform.position = climbCollider.transform.position;
            climbCollider.gameObject.SetActive(false);
            hasCaught = true;
        }
    }

    void UserInput()
    {
        GetPlayerEdgePositions();

        if (!isManoeuvring)
        {
            if ((SystemSettings.tapRight && movementType == MovementType.CatchRight) || (SystemSettings.tapLeft && movementType == MovementType.CatchLeft))
            {
                ClimbUpObstacle();
            }

            if (SystemSettings.moveRight && !SystemSettings.moveLeft) //movng to the right
            {
                bool specialRightPos = false;
                if (movementType == MovementType.MovableRight)
                {
                    specialRightPos = true;
                    Vector3 tempRightEdge;
                    GetEdgePositions(interactingObject.GetComponent<Collider2D>());
                    tempRightEdge = rightEdgePosition;
                    GetPlayerEdgePositions();
                    rightEdgePosition = tempRightEdge;
                }

                for (int i = 0; i < unwalkableCoords.Count; i++) //check if walking into something
                {
                    if (rightEdgePosition.x >= unwalkableCoords[i].rightX && leftEdgePosition.x < unwalkableCoords[i].rightX && bottomEdgePosition.y < unwalkableCoords[i].rightY) //if walking into something
                    {
                        float teleportFloat = playerSize.x / 2;
                        if (specialRightPos)
                        {
                            teleportFloat += interactingObject.GetComponent<Collider2D>().bounds.size.x;
                        }
                        transform.position = new Vector3(unwalkableCoords[i].rightX - teleportFloat, transform.position.y, transform.position.z);
                        horizontalSpeed = 0;
                        return;
                    }
                }

                if (movementType == MovementType.Walking) //walk right
                {
                    AudioManager.PlaySoundEffect(SoundEffect.WoodenFootsteps);
                    playerAnimator.PlayerWalkRight();
                }
                else if (movementType == MovementType.Crawling) //crawl right
                {
                    AudioManager.PlaySoundEffect(SoundEffect.Vent);
                    playerAnimator.PlayerCrawlRight();
                }

                if (movementType == MovementType.CatchRight) //should never occur 
                {
                    return;
                }
                else if (movementType == MovementType.CatchLeft) //fall from obstacle
                {
                    hasCaught = false;
                    movementType = MovementType.Walking;
                }

                if (movementType == MovementType.MovableRight) //push object
                {
                    AudioManager.PlaySoundEffect(SoundEffect.WoodenScrape);
                    playerAnimator.PlayerPushRight();
                    interactionType = InteractionType.Push;
                    currentHorizontalForce = BasePushForce;
                    interactingObject.GetComponent<MovableInteractable>().Interaction(gameObject);
                }
                else if (movementType == MovementType.MovableLeft) //pull object
                {
                    AudioManager.PlaySoundEffect(SoundEffect.Vent);
                    playerAnimator.PlayerPullLeft();
                    interactionType = InteractionType.Pull;
                    currentHorizontalForce = BasePullForce;
                    interactingObject.GetComponent<MovableInteractable>().Interaction(gameObject);
                }

                horizontalSpeed = currentHorizontalForce * speedMultiplier;
                transform.position = new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), transform.position.z + playerGroundObject.transform.right.z);
            }
            else if (SystemSettings.moveLeft && !SystemSettings.moveRight) //move to the left
            {
                bool specialLeftPos = false;
                if (movementType == MovementType.MovableLeft)
                {
                    specialLeftPos = true;
                    Vector3 tempLeftEdge;
                    GetEdgePositions(interactingObject.GetComponent<Collider2D>());
                    tempLeftEdge = leftEdgePosition;
                    GetPlayerEdgePositions();
                    leftEdgePosition = tempLeftEdge;
                }

                for (int i = 0; i < unwalkableCoords.Count; i++) //loop walking into objects
                {
                    if (leftEdgePosition.x <= unwalkableCoords[i].leftX && rightEdgePosition.x > unwalkableCoords[i].leftX && bottomEdgePosition.y < unwalkableCoords[i].leftY) //if walking into something
                    {
                        float teleportFloat = playerSize.x / 2;
                        if (specialLeftPos)
                        {
                            teleportFloat += interactingObject.GetComponent<Collider2D>().bounds.size.x;
                        }
                        transform.position = new Vector3(unwalkableCoords[i].leftX + teleportFloat, transform.position.y, transform.position.z);
                        horizontalSpeed = 0;
                        return;
                    }
                }

                if (movementType == MovementType.Walking) //walk to the left
                {
                    AudioManager.PlaySoundEffect(SoundEffect.WoodenFootsteps);
                    playerAnimator.PlayerWalkLeft();
                }
                else if (movementType == MovementType.Crawling) //crawl to the left
                {
                    AudioManager.PlaySoundEffect(SoundEffect.Vent);
                    playerAnimator.PlayerCrawlLeft();
                }

                if (movementType == MovementType.CatchLeft) //should never occur 
                {
                    return;
                }
                else if (movementType == MovementType.CatchRight) //fall off obstacle
                {
                    hasCaught = false;
                    movementType = MovementType.Walking;
                }

                if (movementType == MovementType.MovableRight) //pull object
                {
                    AudioManager.PlaySoundEffect(SoundEffect.WoodenFootsteps);
                    playerAnimator.PlayerPullRight();
                    interactionType = InteractionType.Pull;
                    currentHorizontalForce = BasePullForce;
                    interactingObject.GetComponent<MovableInteractable>().Interaction(gameObject);
                }
                else if (movementType == MovementType.MovableLeft) //push object
                {
                    AudioManager.PlaySoundEffect(SoundEffect.WoodenFootsteps);
                    playerAnimator.PlayerPushLeft();
                    interactionType = InteractionType.Push;
                    currentHorizontalForce = BasePushForce;
                    interactingObject.GetComponent<MovableInteractable>().Interaction(gameObject);
                }

                GetPlayerEdgePositions();
                horizontalSpeed = -currentHorizontalForce * speedMultiplier;
                transform.position = 
                    new Vector3(transform.position.x + (playerGroundObject.transform.right.x * horizontalSpeed * Time.deltaTime), 
                    transform.position.y + (horizontalSpeed * playerGroundObject.transform.right.y * Time.deltaTime), 
                    transform.position.z + playerGroundObject.transform.right.z);
            }
            else if (((!SystemSettings.moveLeft && !SystemSettings.moveRight) || (SystemSettings.moveLeft && SystemSettings.moveRight)) && movementType != MovementType.Crawling && movementType != MovementType.MovableRight && movementType != MovementType.MovableLeft)
            {
                playerAnimator.PlayerIdle();
            }

            if (SystemSettings.jump && canJump)
            {
                DetectGround();
                //grounded = canJump;
                hasJumped = true;
                verticalSpeed = BaseJumpForce;
                transform.position += playerGroundCollider.transform.up * (verticalSpeed * Time.deltaTime);
            }

            if (SystemSettings.tapInteract)
            {
                RaycastHit2D interactingHit = Physics2D.BoxCast(transform.position, playerSize, 0, transform.up, 0, interactableLayer);

                if (interactingHit.collider != null)
                {
                    interactingObject = interactingHit.collider.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject; //this line of code is stupid... if something doesn't to do this door, it is quite possible this.
                                                                                                                                                     //it just goes back four times in the hierachy to find the object with the door interactable.
                    interactingObject.GetComponent<InteractableObject>().Interaction(gameObject);
                }
            }
        }
    }

    void Movement()
    {
        if (!isManoeuvring)
        {
            ApplyGravity();
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

    //https://www.youtube.com/watch?v=P_6W-36QfLA
    //used to check whether the player is grounded
    public void DetectGround()
    {
        GetPlayerEdgePositions();
        RaycastHit2D[] groundChecks = Physics2D.BoxCastAll(new Vector3(bottomEdgePosition.x, bottomEdgePosition.y + (playerSize.y / 2)), playerSize, 0, -currentPlayerColliderGO.transform.up, castDistance, groundLayer);

        int index = 0; //the index of the ground in the groundChecks array
        float checkGroundY; //the y coordinate of the ground that is currently being checked
        grounded = false;

        if (groundChecks.Length < 1)
        {
            Debug.LogError("No Ground Detected");
            index = -1; //this is an error
            grounded = true; //keeps the player from falling if there is no ground

        }
        else
        {
            checkGroundY = groundChecks[index].collider.ClosestPoint(currentPlayerColliderGO.transform.position).y; //sets the ground to the first object in the array

            for (int c = 0; c < groundChecks.Length; c++) //loops through the entire array of ground colliders
            {
                if (Vector2.Distance(bottomEdgePosition, new Vector2(currentPlayerColliderGO.transform.position.x, groundChecks[c].collider.ClosestPoint(currentPlayerColliderGO.transform.position).y)) < TerminalVelocity * Time.deltaTime && !isJumping) 
                    //if the player is close enough to the ground and isnt currently jumping
                {
                    index = c; //the index changes to the ground that player is currently standing on
                    grounded = true;
                    break;
                }
            }
        }

        if (index > -1) //if ground was detected
        {
            groundY = groundChecks[index].collider.ClosestPoint(currentPlayerColliderGO.transform.position).y;
            groundNormal = groundChecks[index].normal;
            playerGroundObject.transform.rotation = groundChecks[index].collider.transform.rotation;

            if (grounded) //and the player is grounded
            {
                transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0); //make sure they are on the ground
                verticalSpeed = 0; //stop them from falling any further
            }
            else //and the player is not grounded
            {
                verticalSpeed -= GravityForce * Time.deltaTime; //fall
            }
        }
        else //if there is no ground detected (this should never occur)
        {
            transform.position = new Vector3(transform.position.x, groundY + (playerSize.y / 2), 0); //put them level to the last ground that was detected
            verticalSpeed = 0; //stop from falling
        }

        if (grounded)
        {
            try //try to activate the climbCollider
            {
                climbCollider.gameObject.SetActive(true);
            }
            catch
            {

            }
        }
    }

    public void DetectUnwalkables()
    {
        GetPlayerEdgePositions();
        RaycastHit2D[] unwalkableRightChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerColliderGO.transform.position.x - castDistance, currentPlayerColliderGO.transform.position.y), playerSize, 0, currentPlayerColliderGO.gameObject.transform.right, castDistance * 2, unwalkableLayerRight);
        RaycastHit2D[] unwalkableLeftChecks = Physics2D.BoxCastAll(new Vector3(currentPlayerColliderGO.transform.position.x + castDistance, currentPlayerColliderGO.transform.position.y), playerSize, 0, -currentPlayerColliderGO.gameObject.transform.right, castDistance * 2, unwalkableLayerLeft);

        unwalkableCoords.Clear();

        for (int i = 0; i < unwalkableRightChecks.Length; i++) //loops through all the detected objects
        {
            for (int j = 0; j < unwalkableLeftChecks.Length; j++) //loops through all the detected objects
            {
                if (unwalkableRightChecks[i].collider.transform.parent.gameObject == unwalkableLeftChecks[j].collider.transform.parent.gameObject) //if both objects are attached to the same parents
                {
                    UnwalkableCoordinates unwalkableCoordinates = new UnwalkableCoordinates();

                    unwalkableCoordinates.rightX = unwalkableRightChecks[i].collider.bounds.max.x;
                    unwalkableCoordinates.leftX = unwalkableLeftChecks[j].collider.bounds.min.x;
                    unwalkableCoordinates.rightY = unwalkableRightChecks[i].collider.bounds.max.y;
                    unwalkableCoordinates.leftY = unwalkableLeftChecks[j].collider.bounds.max.y;

                    unwalkableCoords.Add(unwalkableCoordinates); //add the coordinates to the list
                }
            }
        }
    }

    public void DetectAutoEvents()
    {
        GetPlayerEdgePositions();
        RaycastHit2D[] autoEvents = Physics2D.BoxCastAll(transform.position, playerSize, 0, transform.right, 0, autoEventLayer);

        if (autoEvents.Length > 0)
        {
            autoEvents[0].collider.gameObject.GetComponent<AutoEvent>().Event(gameObject);
        }
    }

    void GetEdgePositions(Collider2D objectToGet)
    {
        leftEdgePosition = new Vector3(objectToGet.bounds.min.x, objectToGet.transform.position.y, objectToGet.transform.position.z);
        rightEdgePosition = new Vector3(objectToGet.bounds.max.x, objectToGet.transform.position.y, objectToGet.transform.position.z);
        topEdgePosition = new Vector3(objectToGet.transform.position.x, objectToGet.bounds.max.y, objectToGet.transform.position.z);
        bottomEdgePosition = new Vector3(objectToGet.transform.position.x, objectToGet.bounds.min.y, objectToGet.transform.position.z);
    }

    void GetPlayerEdgePositions()
    {
        leftEdgePosition = new Vector3(currentPlayerColliderGO.transform.position.x - (playerSize.x / 2), currentPlayerColliderGO.transform.position.y, currentPlayerColliderGO.transform.position.z);
        rightEdgePosition = new Vector3(currentPlayerColliderGO.transform.position.x + (playerSize.x / 2), currentPlayerColliderGO.transform.position.y, currentPlayerColliderGO.transform.position.z);
        topEdgePosition = new Vector3(currentPlayerColliderGO.transform.position.x, currentPlayerColliderGO.transform.position.y + (playerSize.y / 2), currentPlayerColliderGO.transform.position.z);
        bottomEdgePosition = new Vector3(currentPlayerColliderGO.transform.position.x, currentPlayerColliderGO.transform.position.y - (playerSize.y / 2), currentPlayerColliderGO.transform.position.z);
    }

    void ClimbUpObstacle()
    {
        isManoeuvring = true;

        if (climbCollider != null)
        {
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
        }
        else
        {
            Debug.LogWarning("Warning: climbCollider == null, occurs when jumping back onto a interactable object");
            DetectGround();
        }

        movementType = MovementType.Walking;
        hasCaught = false;
        isManoeuvring = false;
    }
}
