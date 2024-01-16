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
    [SerializeField] float walkingSpeed, currentSpeed;
    [SerializeField] float accelerationTime = 0.2f;
    [SerializeField] float decelerationTime = 0.2f;
    float accelerationVelocitySmoothing;
    float decelerationVelocitySmoothing;
    float noInputTimer = 0.0f;
    float noInputDurationThreshold = 0.5f;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();

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

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            newMoveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

            // Acceleration
            if (currentSpeed < walkingSpeed)
            {
                float targetSpeed = moveDirection.magnitude * walkingSpeed;
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref accelerationVelocitySmoothing, accelerationTime);

                rb.AddForce(newMoveDirection * currentSpeed * 5.0f * Time.deltaTime, ForceMode.VelocityChange);

                if (currentSpeed > walkingSpeed)
                {
                    LimitVelocity();
                }
            }

            // Reset no input timer when moving
            noInputTimer = 0.0f;

            // Circular Movement
            if (walk.magnitude >= 0.1f)
            {
                float radius = 0.5f;

                float timeCounter = Time.time * currentSpeed;

                Vector3 circularForce = new Vector3(Mathf.Sin(timeCounter) * radius, 0.0f, Mathf.Cos(timeCounter) * radius);

                rb.AddForce(circularForce, ForceMode.Acceleration);
            }
        }
        // Deceleration
        else
        {
            // Increment no input timer when not moving
            noInputTimer += Time.fixedDeltaTime;

            // Apply deceleration only if the no input duration threshold is reached
            if (noInputTimer >= noInputDurationThreshold)
            {
                float decelerationTargetSpeed = 0.0f;

                currentSpeed = Mathf.SmoothDamp(currentSpeed, decelerationTargetSpeed, ref decelerationVelocitySmoothing, decelerationTime);

                rb.AddForce(-newMoveDirection * currentSpeed * 20.0f * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    // Limits the characters velocity
    private void LimitVelocity()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0.0f;
        currentSpeed = velocity.magnitude;

        // Smoothly decrease the speed when currentSpeed is greater than walkingSpeed
        float targetSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime);

        Vector3 limitedVelocity = velocity.normalized * targetSpeed;
        rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        look = context.ReadValue<Vector2>();
    }

    private void Look()
    {
        float mouseY = look.y * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!enabled)
        {
            return;
        }

        if (context.started && !isDashing && dashCooldownTimeCounter > 0)
        {
            Dash();
        }
    }

    private void Dash()
    {
        isDashing = true;

        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 forceToApply = forwardDirection * dashForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        isDashing = false;
    }
}
