using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    // Speed that the player movement uses to move the player
    private float moveSpeed;
    [Tooltip("Walking speed for the player")]
    [SerializeField] private float walkSpeed;
    [Tooltip("Sprinting speed for the player")]
    [SerializeField] private float sprintSpeed;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    [Tooltip("The amount of drag that the player will feel when grounded, slowing them down")]
    [SerializeField] private float groundDrag;
    [Tooltip("Determines the jump height of the player, along with impacts from physics (gravity, etc.)")]
    [SerializeField] private float jumpForce;
    [Tooltip("Cooldown before the player can jump again")]
    [SerializeField] private float jumpCooldown;
    [Tooltip("Multiplier for speed while flying through the air, also takes into account moveSpeed")]
    [SerializeField] private float airMultiplier;
    // Boolean for keeping track of when the player can jump based on the jump cooldown
    private bool readyToJump;

    [Header("Ground Check")]
    [Tooltip("The height of the player")]
    [SerializeField] private float playerHeight;
    [Tooltip("Layermask for checking for the ground, which is used for deciding when certain player actions can begin")]
    [SerializeField] private LayerMask groundLayer;
    [Tooltip("(Do not touch this) General boolean for checking if the player is on the ground")]
    [SerializeField] private bool grounded;

    // Global variables for storing player information
    // Global variable assigned by players input for calculating movement direction
    private float horizontalInput;
    // Global variable assigned by players input for calculating movement direction
    private float verticalInput;
    // Direction the player should move after the inputs have been multiplied in
    private Vector3 moveDirection;

    // References
    // Reference to the player rigidbody
    private Rigidbody rb;
    // Reference to the player's input actions
    private InputManager input;
    // Reference for the camera transform (used in calculating player direction)
    private Transform cameraTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        input = InputManager.instance;
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        // Raycast for checking if the player is on the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);

        UpdateInput();
        SpeedControl();
        StateHandler();

        // Drag handling
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Pulls player movement from the input manager and checks for jump input + conditions
    /// </summary>
    private void UpdateInput()
    {
        Vector2 movement = input.GetPlayerMovement();
        horizontalInput = movement.x;
        verticalInput = movement.y;

        // when to jump
        if (input.PlayerIsJumping() && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void StateHandler()
    {
        // State - Sprinting
        if (grounded && input.PlayerIsSprinting())
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // State - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // State - Air
        else
        {
            state = MovementState.air;
        }
    }

    /// <summary>
    /// Physically moves the player, uses camera forward to determine look direction, multiplied by the player's inputs
    /// Add force to rigidbody + normalize
    /// </summary>
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
        moveDirection.y = 0f;
        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    /// <summary>
    /// Caps the rigidbody velocity / overall player speed to avoid the player going faster than intended
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Makes the player jump, adds force to the rigidbody
    /// </summary>
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Resets the jump ability to "ready" when cooldown runs out
    /// </summary>
    private void ResetJump()
    {
        readyToJump = true;
    }
}
