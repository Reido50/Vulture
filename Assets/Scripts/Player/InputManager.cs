using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    private PlayerInput input;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return input.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return input.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped()
    {
        return input.Player.Jump.triggered;
    }
}