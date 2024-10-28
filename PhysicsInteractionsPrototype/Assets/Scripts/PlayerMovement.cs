using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 playerSize;
    Collider2D playerCollider;
    Rigidbody2D playerRb;

    int groundLayer;
    int playerLayer;
    int interactableLayer;

    public int maxSpeed;
    public int jumpPower;
    public int climbSpeed;
    public int uprightSpeed;
    public int crawlSpeed;
    public float pushForce;
    public float pullForce;

    public float groundCheckRadius;

    public bool interacting;

    float speed;
    [SerializeField] Collider2D jumpSpace; //from inspector

    Vector2 groundNormal;
    float groundPosition;
    bool grounded;

    bool canJump;
    bool hasCaught;

    float caughtHeight;
    float playerTempCaughtHeight;    

    Collider2D interactableCollider;
    Rigidbody2D interactableRigidbody;
    float reachDistance = 100f; //temporary

    enum MovementMode
    {
        upright,
        crawl
    }

    MovementMode movementMode = MovementMode.upright;

    void Start()
    {
        speed = uprightSpeed;
        canJump = true;

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
        interactableLayer = LayerMask.GetMask("Interactable");
        playerSize = GetComponent<Collider2D>().bounds.size;
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        jumpSpace.excludeLayers = playerLayer; 
    }

    void Update()
    {
    
    }

    private void FixedUpdate()
    {
        checkInteract();

        checkGround();
        checkMovementType();

        move();
        jump();
        jumpAndCatch();
        climb();
    }

    void checkInteract()
    {
        try
        {
            float currentXDistance = transform.position.x - interactableCollider.transform.position.x;

            if (!SystemSettings.interact || currentXDistance > reachDistance)
            {
                interactableCollider = unsetInteractable();
            }
        }
        catch
        {
            interactableCollider = unsetInteractable();
        }

        if (interactableCollider == null)
        {
            interactableCollider = interact();
        }
    }

    Collider2D interact()
    {
        if (SystemSettings.interact)
        {
            if (playerCollider.IsTouchingLayers(interactableLayer))
            {
                return setInteractable();
            }
        }

        return unsetInteractable();
    }

    Collider2D setInteractable()
    {
        ContactFilter2D onlyInteractions = new ContactFilter2D();
        onlyInteractions.SetLayerMask(interactableLayer);
        List<Collider2D> interactableColliders = new List<Collider2D>();
        playerCollider.OverlapCollider(onlyInteractions, interactableColliders);

        Collider2D toReturn = interactableColliders[0];
        toReturn.gameObject.transform.parent = transform;
        return toReturn;
    }

    Collider2D unsetInteractable()
    {
        try
        {
            interactableCollider.gameObject.transform.parent = null;
        }
        catch
        {
            //already null
        }
        return null;
    }

    void checkMovementType()
    {
        //https://discussions.unity.com/t/raycast-layermask-parameter/802897/7
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + (playerSize.y / 2)), transform.right, 0.1f + playerSize.x, ~playerLayer);
        RaycastHit2D notClear = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y - (playerSize.y / 4)), transform.right, 0.1f + playerSize.x, ~playerLayer);

        RaycastHit2D roof = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + playerSize.x), new Vector2 (playerSize.y, playerSize.x), 0, Vector2.zero, 0, ~playerLayer);

        if (!roof && movementMode == MovementMode.crawl)
        {
            movementMode = MovementMode.upright;
            speed = uprightSpeed;
            //canJump = true;
            playerRb.SetRotation(0);
        }        
        
        if (grounded && !interacting && hit && !notClear && movementMode == MovementMode.upright)
        {
            movementMode = MovementMode.crawl;
            speed = crawlSpeed;
            //canJump = false;
            playerRb.velocity = Vector2.zero;
            
            playerRb.SetRotation(90);
        }
    }

    void checkGround()
    {
        float playerModifer;
        Vector2 rayDirection;
        if (movementMode == MovementMode.crawl)
        {
            playerModifer = playerSize.x / 2;
            rayDirection = -transform.right;
        }
        else
        {
            playerModifer = playerSize.y / 2;
            rayDirection = -transform.up;
        }

        int combinedLayer = groundLayer | interactableLayer;

        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, rayDirection, Mathf.Infinity, combinedLayer);
        if (hit)
        {
            groundPosition = transform.position.y - hit.distance + playerModifer;
            groundNormal = hit.normal;
        }

        if (transform.position.y > groundPosition - groundCheckRadius && transform.position.y < groundPosition + groundCheckRadius)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void move()
    {
        if (interactableCollider != null)
        {
            if (relativeMovementDirection(interactableCollider.gameObject))
            {
                speed = pullForce;
            }
            else
            {
                speed = pushForce;
            }
        }
        else if (movementMode == MovementMode.upright)
        {
            speed = uprightSpeed;
        }

        if (hasCaught) //climb caught object
        {
            playerRb.AddForce(new Vector2(0, climbSpeed));

            if (transform.position.y - (playerSize.y / 2) > caughtHeight)
            {
                hasCaught = false;
            }
        }
        else if (SystemSettings.moveRight && !SystemSettings.moveLeft) //walk right
        {
            float currentSpeed = groundNormal.x + speed;
            checkMaxSpeed(currentSpeed);

            playerRb.AddForce(new Vector2(currentSpeed, groundNormal.y));
        }
        else if (SystemSettings.moveLeft && !SystemSettings.moveRight) //walk left
        {
            float currentSpeed = groundNormal.x + -speed;
            checkMaxSpeed(currentSpeed);

            playerRb.AddForce(new Vector2(currentSpeed, groundNormal.y));
        }

        void checkMaxSpeed(float currentSpeed)
        {
            if (currentSpeed < -maxSpeed || currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;

            }
        }
    }

    void jump()
    {
        //if (jumpSpace.IsTouchingLayers(Physics2D.AllLayers))
        //{
        //    canJump = false;
        //}

        if (hasCaught || interacting || movementMode == MovementMode.crawl)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        if (canJump && grounded && SystemSettings.jump)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
            playerRb.AddForce(new Vector2(0, jumpPower));
        }
    }

    void jumpAndCatch()
    {
        RaycastHit2D clear = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + ((playerSize.y / 2) + (playerSize.y / 4))), transform.right, 0.1f + playerSize.x, ~playerLayer);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (playerSize.x / 2) - 0.05f, transform.position.y + (playerSize.y / 4)), transform.right, 0.1f + playerSize.x, ~playerLayer);

        if (!interacting && !hasCaught && !grounded && hit && !clear)
        {
            hasCaught = true;
            caughtHeight = hit.collider.bounds.max.y;
            playerTempCaughtHeight = hit.collider.bounds.max.y - (playerSize.y / 2);

            transform.position = new Vector2(transform.position.x, playerTempCaughtHeight);
        }
    }

    void climb()
    {

    }

    float angularLerp(float lerp1, float lerp2, ref float currentProgress, float animSpeed)
    {
        currentProgress += animSpeed;
        return Mathf.LerpAngle(lerp1, lerp2, currentProgress);
    }

    float positionLerp(float lerp1, float lerp2, ref float currentProgress, float animSpeed)
    {
        currentProgress += animSpeed;
        return Mathf.Lerp(lerp1, lerp2, currentProgress);
    }

    bool relativeMovementDirection(GameObject comparator) //returns true if moving away from the comparator object
    {
        if (transform.position.x > comparator.transform.position.x && SystemSettings.moveRight || transform.position.x < comparator.transform.position.x && SystemSettings.moveLeft)
        {
            return true;
        }
        return false;
    }
}
