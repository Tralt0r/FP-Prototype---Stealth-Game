using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for UI
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CS_PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float acceleration = 25f;
    public float airControl = 0.7f;
    public float groundFriction = 8f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRegenRate = 0.5f;
    public Slider staminaSlider;

    private float currentStamina;
    private Vector3 currentVelocity;

    [Header("Parkour")]
    public float wallCheckDistance = 1f;
    public float wallJumpForce = 8f;

    [Header("Wall Jump Settings")]
    public int maxWallJumps = 3;
    private int wallJumpCount = 0;

    [Header("Wall Layers")]
    public LayerMask wallLayer;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;

    public Transform cameraTransform;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    [Header("Camera Smoothing")]
    public float rotationSmoothTime = 0.02f;
    private float currentMouseX;
    private float currentMouseY;
    private float mouseXVelocity;
    private float mouseYVelocity;

    [Header("Controller Input")]
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool sprintHeld;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Sprint.performed += ctx => sprintHeld = true;
        controls.Player.Sprint.canceled += ctx => sprintHeld = false;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        currentStamina = maxStamina;
        if (staminaSlider != null)
            staminaSlider.maxValue = maxStamina;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
            HandleMovement();
            HandleJump();
            HandleStamina();
        }
    }

    void HandleMovement()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(
            transform.position,
            controller.radius,
            Vector3.down,
            out hit,
            controller.height / 2f + 0.1f,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        // Apply gravity
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        if (isGrounded && !wasGrounded)
            wallJumpCount = 0;
        wasGrounded = isGrounded;

        float x = moveInput.x;
        float z = moveInput.y;
        Vector3 inputDir = (cameraTransform.right * x + cameraTransform.forward * z);
        inputDir.y = 0f;
        if (inputDir.sqrMagnitude > 0f) inputDir.Normalize();

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;
        float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 horizontalVelocity = inputDir * targetSpeed;

        if (!isGrounded && horizontalVelocity.sqrMagnitude > 0.01f)
        {
            if (Physics.Raycast(
                transform.position + Vector3.up * 0.5f,
                horizontalVelocity.normalized,
                out hit,
                0.6f,
                ~0,
                QueryTriggerInteraction.Ignore
            ))
            {
                horizontalVelocity = Vector3.ProjectOnPlane(horizontalVelocity, hit.normal);
            }
        }

        currentVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
        controller.Move(currentVelocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            {
                if (isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                }
                else
                {
                    Vector3 rayOrigin = transform.position + Vector3.up * 1f;
                    RaycastHit hit;
                    if ((Physics.Raycast(rayOrigin, transform.right, out hit, wallCheckDistance, wallLayer) ||
                         Physics.Raycast(rayOrigin, -transform.right, out hit, wallCheckDistance, wallLayer)) &&
                         wallJumpCount < maxWallJumps)
                    {
                        Vector3 jumpDir = (hit.normal + Vector3.up).normalized;
                        currentVelocity = new Vector3(jumpDir.x, 0, jumpDir.z) * wallJumpForce;
                        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                        wallJumpCount++;
                    }
                }
            }
        }
    }

        void HandleMouseLook()
        {
            float targetMouseX = lookInput.x * mouseSensitivity;
            float targetMouseY = lookInput.y * mouseSensitivity;

            currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, rotationSmoothTime);
            currentMouseY = Mathf.SmoothDamp(currentMouseY, targetMouseY, ref mouseYVelocity, rotationSmoothTime);

            xRotation -= currentMouseY * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * currentMouseX * Time.deltaTime);
        }

        void HandleStamina()
        {
            bool isSprinting = sprintHeld && currentStamina > 0f; ;

            if (isSprinting)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;
                if (currentStamina < 0f) currentStamina = 0f;
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }

            if (staminaSlider != null)
                staminaSlider.value = currentStamina;
        }

        // Enable Controls
        void OnEnable()
        {
            controls.Enable();
        }

        // Disable Controls
        void OnDisable()
        {
            controls.Disable();
        }
}