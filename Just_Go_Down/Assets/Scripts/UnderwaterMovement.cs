using UnityEngine;
using UnityEngine.InputSystem;

public class UnderwaterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float swimSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalLookLimit = 90f;
    [SerializeField] private bool invertY = false;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Breathing System")]
    [SerializeField] private BreathingSystem breathingSystem;

    private Rigidbody rb;
    private Swimming inputActions;
    private Vector3 currentVelocity;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

    public float SwimSpeed
    {
        get { return swimSpeed; }
        set { swimSpeed = value; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        inputActions = new Swimming();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Underwater.Enable();
    }

    private void OnDisable()
    {
        inputActions.Underwater.Disable();
    }

    private void Update()
    {
        HandleMouseLook();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleMouseLook()
    {
        // Read mouse delta directly
        Vector2 mouseDelta = inputActions.Underwater.Look.ReadValue<Vector2>();

        // Only apply rotation if mouse actually moved
        if (mouseDelta.sqrMagnitude < 0.01f) return;

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        if (invertY) mouseY = -mouseY;

        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        cameraTransform.localRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputActions.Underwater.Move.ReadValue<Vector2>();
        float verticalInput = inputActions.Underwater.VerticalMove.ReadValue<float>();

        Vector3 moveDirection = new Vector3(moveInput.x, verticalInput, moveInput.y);

        // Notify breathing system if player is moving
        if (breathingSystem != null)
        {
            bool isMoving = moveDirection.magnitude > 0.1f;
            breathingSystem.SetMoving(isMoving);
        }

        Vector3 targetVelocity = transform.TransformDirection(moveDirection) * swimSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }
}
