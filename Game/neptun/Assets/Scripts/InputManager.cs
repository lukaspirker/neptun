using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    private bool sprinting;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance == this)
        {
            Destroy(this.gameObject);   
        }
        else
        {
            instance = this;
        }

        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();

        onFoot.Sprint.performed += SprintPerformed;
        onFoot.Sprint.canceled += SprintCanceled;

    }

    public bool GetSprint()
    {
        return sprinting;
    }

    private void SprintPerformed(InputAction.CallbackContext context)
    {
        sprinting = true;
    }

    private void SprintCanceled(InputAction.CallbackContext context)
    {
        sprinting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
