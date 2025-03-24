using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    //initialised when game starts
    [Header("Designers Don't Change!")]
    //unity components
    public Rigidbody2D playerRigidbody;
    public Collider2D playerWalkingCollider;
    public Collider2D playerCrawlingCollider;
    public Collider2D playerInteractCollider;
    public Collider2D playerOverlapCheckCollider;
    //public Collider2D playerGroundedCollider;
    public Collider2D playerCatchClimbCollider;
    public Animator playerAnimatorComponent;
    public AnimationClip climbAnimationClip;
    public AnimationClip quickClimbAnimationClip;
    public SpriteRenderer playerSprite;

    // Static fallback variables using simple GetComponent expressions
    public static GameObject PlayerGO;
    static Rigidbody2D PlayerRigidbody => PlayerGO.GetComponent<Rigidbody2D>();
    static Collider2D PlayerWalkingCollider => GameObject.FindWithTag("PlayerWalkingCollider").GetComponent<Collider2D>();
    static Collider2D PlayerCrawlingCollider => GameObject.FindWithTag("PlayerCrawlingCollider").GetComponent<Collider2D>();
    static Collider2D PlayerInteractCollider => GameObject.FindWithTag("PlayerInteractCollider").GetComponent<Collider2D>();
    static Collider2D PlayerOverlapCheckCollider => GameObject.FindWithTag("PlayerOverlapCheckCollider").GetComponent<Collider2D>();
    // static Collider2D PlayerGroundedCollider => PlayerGO.GetComponent<Collider2D>();
    static Collider2D PlayerCatchClimbCollider => GameObject.FindWithTag("PlayerCatchClimbCollider").GetComponent<Collider2D>();
    static Animator PlayerAnimatorComponent => PlayerGO.GetComponentInChildren<Animator>();
    static SpriteRenderer PlayerSprite => PlayerAnimatorComponent.gameObject.GetComponent<SpriteRenderer>();

    //scripts
    public PlayerAnimator playerAnimator;
    public PlayerMovement playerMovement;

    //set in start
    [HideInInspector] public LayerMask playerLayer;

    [Header("For Designers")]
    public float playerRBMass = 1.0f; //this is the value used to set the mass of the player and should never be changed (only not const for testing!)

    //variables
    [HideInInspector] public float currentPlayerRBMass; //this is the mass of the player when it is moving an object (equal to the player mass plus the object mass)
    [HideInInspector] public bool pulling = false; //is the player currently pulling an object?
    [HideInInspector] public bool pushing = false; //is the player currently pushing an object?
    [HideInInspector] public bool shouldLimitMovement = false; //should the player's movement currently be limited? i.e clinging onto an object, entering elevator etc.
    [HideInInspector] public bool insideElevator = false; //is the player currently inside the elevator?
    [HideInInspector] public bool grounded = false; //is the player currently grounded?
    [HideInInspector] public bool crawling = false; //is the player currently crawling?
    [HideInInspector] public int animationNumber = 0; //what animation is currently trying to be active
    [HideInInspector] public float customPlayerVelocity; //used to change the player velocity to something custom for auto events (its an adder, to slow down this value should be negative)

    private void Awake()
    {
        GameManager.playerData = instance = this;

        PlayerGO = this.gameObject;

        // Only assign if the public variable is null
        if (playerRigidbody == null) playerRigidbody = PlayerRigidbody;
        if (playerWalkingCollider == null) playerWalkingCollider = PlayerWalkingCollider;
        if (playerCrawlingCollider == null) playerCrawlingCollider = PlayerCrawlingCollider;
        if (playerInteractCollider == null) playerInteractCollider = PlayerInteractCollider;
        if (playerOverlapCheckCollider == null) playerOverlapCheckCollider = PlayerOverlapCheckCollider;
        //if (playerGroundedCollider == null) playerGroundedCollider = PlayerGroundedCollider;
        if (playerCatchClimbCollider == null) playerCatchClimbCollider = PlayerCatchClimbCollider;
        if (playerAnimatorComponent == null) playerAnimatorComponent = PlayerAnimatorComponent;
        if (playerSprite == null) playerSprite = PlayerSprite;
    }
    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    public void FreezePlayer()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnfreezePlayer()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
