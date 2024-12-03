using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed;
    public float gravity = -20f;
    public float jumpHeight = 3f;

    private bool lerpCrouch;
    private bool crouching;

    private float crouchTimer;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;    
    }

    // Start is called before the first frame update
    void Start()
    { 
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!crouching)
        {
            speed = 4;
            if (InputManager.Instance.GetSprint())
            {
                if (isGrounded)
                    HeadbobSystem.Instance.Frequency = 18f;
                    HeadbobSystem.Instance.Amount = 0.07f;
                    speed = 7;
            }
            else
            {
                HeadbobSystem.Instance.Frequency = 9f;
                HeadbobSystem.Instance.Amount = 0.04f;
                speed = 4;
            }
        }
        else
        {
            HeadbobSystem.Instance.Frequency = 6f;
            HeadbobSystem.Instance.Amount = 0.01f;
            speed = 2;
        }

        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight - 2.5f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
}
