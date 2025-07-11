using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData; //I have moved this to a static variable in GameManager for easier access and consistency (Singleton pattern)
    Collider2D uprightCollider;
    Collider2D crawlingCollider;
    Rigidbody2D playerRB2D;

    Collider2D currentPlayerCollider;
    MovementDirection movementDirection;
    MovementType movementType;

    [Header("For Designers")]
    [SerializeField] float walkingForce = 100f;
    [SerializeField] float crawlingForce = 40f;
    [SerializeField] float jumpForce = 1000f;

    [SerializeField] float gravityForce = 100f;
    [SerializeField] float dragForce = 100f;

    [SerializeField] float pushForce;
    [SerializeField] float pullForce;

    [SerializeField] float movementForce;

    [SerializeField] float slopeCheckDistance = 0.01f;
    [SerializeField] float maxSlopeAngle;

    PhysicsMaterial2D noFrictionMat => Resources.Load<PhysicsMaterial2D>("Physics Materials/NoFriction&Bounciness");
    PhysicsMaterial2D fullFrictionMat => Resources.Load<PhysicsMaterial2D>("Physics Materials/FullFriction&NoBounciness");

    Vector2 slopeNormalPerp;
    bool grounded;
    bool isOnSlope;
    bool canWalkOnSlope;
    float slopeSideAngle;
    float slopeDownAngle;
    float slopeDownAngleOld;
    private int contactsWithGround;

    private Coroutine groundCoroutine;

    enum MovementDirection
    {
        Left,
        Right
    }

    enum MovementType
    {
        None,
        Walking,
        Crawling,
        Pulling,
        Pushing,
        Jumping,
        Falling
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameManager.playerData;
        uprightCollider = playerData.playerWalkingCollider;
        crawlingCollider = playerData.playerCrawlingCollider;
        //groundedCollider = playerData.playerGroundedCollider;
        playerRB2D = playerData.playerRigidbody;

        currentPlayerCollider = uprightCollider;
        playerRB2D = GetComponent<Rigidbody2D>();
        movementType = MovementType.Walking;
        movementForce = walkingForce;

        if (pushForce == 0)
        {
            pushForce = walkingForce / 1.5f;
        }
        if (pullForce == 0)
        {
            pullForce = walkingForce / 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //temp
        //movementForce = walkingForce;
        //end temp
        GetMoveDirection();
        UpdateMovementType();
        UpdateMovementForce();
        SetGrounded();
        playerData.grounded = grounded;

        //Debug.Log("Ground: " + grounded);
    }

    private void GetMoveDirection()
    {
        if (SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveRight) && !SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveLeft))
        {
            movementDirection = MovementDirection.Right;
        }
        else if (!SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveRight) && SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveLeft))
        {
            movementDirection = MovementDirection.Left;
        }
    }

    private void FixedUpdate()
    {
        SetGrounded();
        Move();
        Jump();
        ApplyGravity();
        ApplyHorizontalDrag();
        ApplySlopeBehaviour();
    }
    /// <summary>
    /// Check for if the player should have a frictionless or full friction physics material or not based on slope angle.
    /// https://www.youtube.com/watch?app=desktop&v=QPiZSTEuZnw
    /// </summary>
    private void ApplySlopeBehaviour()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, playerData.playerWalkingCollider.bounds.extents.y, 0.0f);

        CheckSlopeVertical(checkPos);
        CheckSlopeHorizontal(checkPos);
        UpdateMaterialBasedOnMovement();
    }

    void CheckSlopeVertical(Vector2 checkPos)
    {
        RaycastHit2D slopeHitDown = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, LayerMask.GetMask("Ground"));

        if (slopeHitDown)
        {
            slopeNormalPerp = Vector2.Perpendicular(slopeHitDown.normal).normalized;
            slopeDownAngle = Vector2.Angle(slopeHitDown.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld) // Only update if angle changed
            {
                slopeDownAngleOld = slopeDownAngle;
                UpdateMaterialBasedOnSlope();
            }

            Debug.DrawRay(slopeHitDown.point, slopeNormalPerp, Color.red, 0.6f, false);
            Debug.DrawRay(slopeHitDown.point, slopeHitDown.normal, Color.yellow, 0.6f, false);
        }
    }

    void UpdateMaterialBasedOnSlope()
    {
        if (slopeDownAngle == 0f)
        {
            playerData.playerRigidbody.sharedMaterial = noFrictionMat;
        }
        else if (slopeDownAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
            playerData.playerRigidbody.sharedMaterial = noFrictionMat;
        }
        else
        {
            canWalkOnSlope = true;
            playerData.playerRigidbody.sharedMaterial = fullFrictionMat;
        }
    }

    private void RotatePlayerToMatchSlope()
    {
        if (currentPlayerCollider.transform.localEulerAngles.z != slopeDownAngle)
        {
            currentPlayerCollider.transform.localEulerAngles = new(currentPlayerCollider.transform.localEulerAngles.x, currentPlayerCollider.transform.localEulerAngles.y, slopeDownAngle);
        }
        //Right now this is silly because it affects all of the players children for some reason but could be worth looking at
        //if (playerData.playerRigidbody.rotation != slopeDownAngle)
        //{
        //    playerData.playerRigidbody.rotation = slopeDownAngle;
        //}
    }

    private void UpdateMaterialBasedOnMovement()
    {
        if (!SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveRight) && !SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveLeft))
        {
            playerData.playerRigidbody.sharedMaterial = fullFrictionMat;
        }
        else
        {
            playerData.playerRigidbody.sharedMaterial = noFrictionMat;
        }
    }

    void CheckSlopeHorizontal(Vector2 checkPos)
    {

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    void Move()
    {
        //Debug.Log("move left: " + SystemSettings.moveLeft + ". Move right:" +  SystemSettings.moveRight);

        //Debug.Log("Player mass: " + playerRB2D.mass);

        if (!playerData.shouldLimitMovement && !playerData.pulling)
        {
            var moveLeft = SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveLeft);
			var moveRight = SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.MoveRight);

			if (moveLeft || moveRight)
            {
                if (moveLeft && !moveRight)
                {
                    if (movementType == MovementType.Walking)
                    {
                        AudioManager.PlaySoundEffect(SoundEffectEnum.WoodenFootsteps);
                        playerData.playerAnimator.PlayerWalkLeft();
                    }
                    else if (movementType == MovementType.Crawling)
                    {
                        //add crawling anim and sound
                    }
                    else if (movementType == MovementType.Pushing)
                    {
                        //add push anim and sound
                    }

                    playerRB2D.velocity = new Vector3(-transform.right.x * movementForce * Time.fixedDeltaTime, playerRB2D.velocity.y, 0);
                }

                if (moveRight && !moveLeft)
                {
                    if (movementType == MovementType.Walking)
                    {
                        AudioManager.PlaySoundEffect(SoundEffectEnum.WoodenFootsteps);
                        playerData.playerAnimator.PlayerWalkRight();
                    }
                    else if (movementType == MovementType.Crawling)
                    {
                        //add crawling anim and sound
                    }

                    playerRB2D.velocity = new Vector3(transform.right.x * movementForce * Time.fixedDeltaTime, playerRB2D.velocity.y, 0);
                }
            }
            else if(movementType != MovementType.Jumping || grounded)
            {
                playerData.playerAnimator.PlayerIdle();
            }
        }
        else 
        {
            AudioManager.StopSoundEffect(SoundEffectEnum.WoodenFootsteps);
        }

    }

    void Jump()
    {
        if (SystemSettings.GetPlayerActionOn(SystemSettings.PlayerAction.Jump) && grounded && movementType != MovementType.Crawling)
        {
            switch (movementDirection)
            {
                case MovementDirection.Left:
                    playerData.playerAnimator.PlayerJumpLeft();
                    break;
                case MovementDirection.Right:
                    playerData.playerAnimator.PlayerJumpRight();
                    break;
            }

            if (Mathf.Abs(playerRB2D.velocity.y) <= 0.01f)
            {
                playerRB2D.velocity = new Vector3(playerRB2D.velocity.x, transform.up.y * jumpForce * Time.fixedDeltaTime, 0);
            }

            grounded = false;
            playerData.grounded = false;
        }
    }


    void UpdateMovementType()
    {
        if (playerData.crawling)
        {
            movementType = MovementType.Crawling;
            switch (movementDirection)
            {
                case MovementDirection.Left:
                    playerData.playerAnimator.PlayerCrawlLeft();
                    break;
                case MovementDirection.Right:
                    playerData.playerAnimator.PlayerCrawlRight();
                    break;

            }
        }
        else if (playerData.pushing)
        {
            movementType = MovementType.Pushing;
        }
        else if (playerData.pulling)
        {
            movementType = MovementType.Pulling;
        }
        else
        {
            movementType = MovementType.Walking;
        }
    }

    void UpdateMovementForce()
    {
        if (movementType == MovementType.Pulling)
        {
            uprightCollider.gameObject.SetActive(true);
            crawlingCollider.gameObject.SetActive(false);
            currentPlayerCollider = uprightCollider;
            movementForce = pullForce;
        }
        else if (movementType == MovementType.Pushing)
        {
            uprightCollider.gameObject.SetActive(true);
            crawlingCollider.gameObject.SetActive(false);
            currentPlayerCollider = uprightCollider;
            movementForce = pushForce;
        }
        else if (movementType == MovementType.Crawling)
        {
            uprightCollider.gameObject.SetActive(false);
            crawlingCollider.gameObject.SetActive(true);
            currentPlayerCollider = crawlingCollider;
            movementForce = crawlingForce;
        }
        else //default to walking
        {
            //temp?
            uprightCollider.gameObject.SetActive(true);
            crawlingCollider.gameObject.SetActive(false);
            currentPlayerCollider = uprightCollider;
            movementForce = walkingForce;
        }

        movementForce += playerData.customPlayerVelocity;
    }

    void ApplyGravity()
    {
        playerRB2D.velocity = new Vector3(playerRB2D.velocity.x, playerRB2D.velocity.y - (gravityForce * Time.fixedDeltaTime), 0);
    }

    void ApplyHorizontalDrag()
    {
        float dragValue = dragForce * Time.fixedDeltaTime;
        //Debug.Log("Player X velocity: " + playerRB2D.velocity.x);

        if (playerRB2D.velocity.x != 0) //the player is moving on the x axis
        {
            if (playerRB2D.velocity.x > 0) //positive movement
            {
                if (playerRB2D.velocity.x - dragValue < 0)
                {
                    //Debug.Log("positive should 0");
                    playerRB2D.velocity = new Vector3(0, playerRB2D.velocity.y, 0);
                }
                else
                {
                    //Debug.Log("positive");
                    playerRB2D.velocity = new Vector3(playerRB2D.velocity.x - (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
                }
            }
            else //negative movement
            {
                if (playerRB2D.velocity.x + dragValue > 0)
                {
                    //Debug.Log("negative should 0");
                    playerRB2D.velocity = new Vector3(0, playerRB2D.velocity.y, 0);
                }
                else
                {
                    //Debug.Log("negative");
                    playerRB2D.velocity = new Vector3(playerRB2D.velocity.x + (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
                }
            }
        }

        //playerRB2D.velocity = new Vector3(playerRB2D.velocity.x - (dragForce * Time.fixedDeltaTime), playerRB2D.velocity.y, 0);
    }

    void SetGrounded()
    {
        //OnCollisionStay2D();
    }

    public float GetMovementForce()
    {
        return movementForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("OnlyPlayer"))
        {
            contactsWithGround++;

            if (!grounded && groundCoroutine == null)
            {
                groundCoroutine = StartCoroutine(DelayedGround());
            }
        }

        if (playerData.animationNumber == 6)
        {
            switch (movementDirection)
            {
                case MovementDirection.Left:
                    playerData.playerAnimator.PlayerLandLeft();
                    break;
                case MovementDirection.Right:
                    playerData.playerAnimator.PlayerLandRight();
                    break;
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Mathf.Abs(playerRB2D.velocity.y) <= 0.01f && (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("OnlyPlayer")))
        {
            if (!grounded)
            {
                StartCoroutine(DelayedGround());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("OnlyPlayer"))
        {
            contactsWithGround--;
            if (contactsWithGround <= 0)
            {
                contactsWithGround = 0;
                grounded = false;
                playerData.grounded = false;
            }
        }
    }

    private IEnumerator DelayedGround()
    {
        yield return new WaitForSeconds(0.2f);

        if (contactsWithGround > 0 && !grounded)
        {
            grounded = true;
            playerData.grounded = true;
        }

        groundCoroutine = null;
    }
}
