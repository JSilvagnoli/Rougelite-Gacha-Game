using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public Transform playerCam;
    private float xRotation;

    [Header("Walking")]
    float horizontalInput;
    float verticalInput;
    [SerializeField] float walkingSpeed;
    [SerializeField] float turnSmoothTime;
    float turnSmoothVelocity;

    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float dashCooldownTime;
    [SerializeField] float dashCooldownTimeCounter;
    [SerializeField] bool isDashing;

    Rigidbody rb;

    public Vector2 look, walk;
    Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Finding the player camera by name
        GameObject playerCamGameObject = GameObject.Find("Main Character Camera");
        playerCam = playerCamGameObject.transform;
    }

    private void Update()
    {
        Look();

        // Resetting the dash cooldown time
        dashCooldownTimeCounter = dashCooldownTime;

        if (isDashing)
        {
            dashCooldownTimeCounter -= Time.deltaTime;

            ResetDash();
        }
    }

    private void FixedUpdate()
    {
        Walk();
    }

    // Player Inputs
    // Called when walk input is received
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        walk = context.ReadValue<Vector2>();
    }

    // Moves the character
    private void Walk()
    {
        horizontalInput = walk.x;
        verticalInput = walk.y;

        moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        Vector3 newMoveDirection = Vector3.zero;

        // Rotates the player along with the camera keeping forward constant
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;

            // Smoothly rotates the player
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            newMoveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

            Vector3 targetVelocity = newMoveDirection * walkingSpeed;

            // Sets the velocity directly with smooth interpolation
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * 5.0f);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Called when look input is received
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        look = context.ReadValue<Vector2>();
    }

    // Rotates the camera based on mouse input
    private void Look()
    {
        float mouseY = look.y * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    // Called when dash input is received
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        // Initiates dash if conditions are met
        if (context.started && !isDashing && dashCooldownTimeCounter > 0)
        {
            Dash();
        }
    }

    private void Dash()
    {
        isDashing = true;

        // Projects the forward direction of the player on the horizontal plane
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 forceToApply = forwardDirection * dashForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    // Resets the dash state
    private void ResetDash()
    {
        isDashing = false;
    }
}
