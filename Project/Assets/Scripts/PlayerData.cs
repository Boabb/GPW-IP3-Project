using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //initialised when game starts
    [Header("Designers Don't Change!")]
    //unity components
    public GameObject playerGO;
    public Rigidbody2D playerRigidbody;
    public Collider2D playerWalkingCollider;
    public Collider2D playerCrawlingCollider;
    public Collider2D playerInteractCollider;
    public Collider2D playerOverlapCheckCollider;
    public Collider2D playerGroundedCollider;
    public Collider2D playerCatchClimbCollider;
    public Animator playerAnimatorComponent;
    public SpriteRenderer playerSprite;

    //scripts
    public PlayerAnimator playerAnimator;
    public PlayerMovement playerMovement;

    //set in start
    [HideInInspector] public LayerMask playerLayer;

    [Header("For Designers")]
    public float playerRBMass = 1.0f; //this is the value used to set the mass of the player and should never be changed (only not const for testing!)

    //variables
    [HideInInspector] public float currentPlayerRBMass; //this is the mass of the player when it is moving an object (equal to the player mass plus the object mass)
    [HideInInspector] public bool pulling = false; //is the player currently pushing an object?
    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");

        currentPlayerRBMass = playerRBMass;
        playerRigidbody.mass = currentPlayerRBMass;
    }

    private void Update()
    {
        playerRigidbody.mass = currentPlayerRBMass;
    }
}
