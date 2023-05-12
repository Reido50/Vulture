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

    [Tooltip("The default move speed of the player")]
    [SerializeField] private float playerSpeed = 2.0f;
    [Tooltip("The default jump height for the player")]
    [SerializeField] private float jumpHeight = 1.0f;
    [Tooltip("The default gravity value")]
    [SerializeField] private float gravityValue = -9.81f;

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
}