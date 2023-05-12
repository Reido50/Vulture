using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private InputManager input;
    private Transform cameraTransform;

    [Header("Player movement values")]
    [Tooltip("The default move speed of the player")]
    [SerializeField] private float walkSpeed = 4.0f;
    [Tooltip("The sprint move speed of the player")]
    [SerializeField] private float sprintSpeed = 8.0f;
    [Tooltip("The default jump height for the player")]
    [SerializeField] private float jumpHeight = 1.0f;
    [Tooltip("The default gravity value")]
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Settings values")]
    [Tooltip("Settings toggle for sprint being held / being a toggle -- false = hold sprint, true = toggle sprint")]
    public bool toggleSprint = false;
    [Tooltip("Settings toggle for crouch being held / being a toggle -- false = hold crouch, true = toggle crouch")]
    public bool toggleCrouch = false;

    private void Start()
    {
        input = InputManager.instance;
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float playerSpeed = walkSpeed;
        if(SprintCheck())
        {
            playerSpeed = sprintSpeed;
        }

        Vector2 movement = input.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        move.Normalize();
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (input.PlayerJumped() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool SprintCheck()
    {
        bool canSprint = true;
        if(input.PlayerIsSprinting() == 0)
        {
            canSprint = false;
        }

        Debug.Log(input.PlayerIsSprinting());
        return canSprint;
    }
}