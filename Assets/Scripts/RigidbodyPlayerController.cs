using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float sqrMaxVelocity = 20f;
    [SerializeField] private float jumpStrength = 5f;
    [SerializeField] private float airDrag = 0.1f;

    [SerializeField] private float mouseXSpeed = 1f;
    [SerializeField] private float mouseYSpeed = 1f;
    [SerializeField] private float cameraClampAngle = 85f;
    [SerializeField] private float cameraSmoothTime = 0.1f;

    private float moveX, moveZ = 0;
    private float mouseX, mouseY = 0;

    // Needed for rotation smoothing
    private float currentRotationVelocityX;
    private float currentRotationVelocityY;

    private Rigidbody playerBody;
    private Vector3 cameraTargetEulers;

    public bool IsMoving => moveX != 0 || moveZ != 0;
    public bool IsGrounded => playerBody.velocity.y == 0;
    public bool IsSprinting => Input.GetButton("Sprint") && IsGrounded;
    public bool IsJumping => Input.GetButton("Jump") && IsGrounded;
    

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody body))
        {
            playerBody = body;
        }
        else
            Debug.LogError("Missing player rigidbody component!");

        cameraTargetEulers = cameraTarget.eulerAngles;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Movement
        moveX = Input.GetAxis("Horizontal") * moveSpeed;
        moveZ = Input.GetAxis("Vertical") * moveSpeed;

        // Camera
        mouseX = Input.GetAxis("Mouse X") * mouseXSpeed;
        mouseY = Input.GetAxis("Mouse Y") * mouseYSpeed;
        
        cameraTargetEulers.x -= mouseY;
        cameraTargetEulers.x = Mathf.Clamp(cameraTargetEulers.x, -cameraClampAngle, cameraClampAngle);
        cameraTargetEulers.y = (cameraTargetEulers.y + mouseX) % 360;

        // Mouse focus
        if (Input.GetButtonUp("Cancel") && !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetButtonUp("Fire1") && Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    private void FixedUpdate()
    {
        HandlePlayerMove();
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
    }

    private void HandlePlayerMove()
    {
        if (!IsGrounded)
        {
            playerBody.velocity = new Vector3(
                (1 - airDrag) * playerBody.velocity.x, 
                playerBody.velocity.y,
                (1 - airDrag) * playerBody.velocity.z
            );
        }

        Vector3 moveForce = moveX * Time.deltaTime * cameraTarget.right + 
            moveZ * Time.deltaTime * Vector3.Cross(cameraTarget.right, playerBody.transform.up).normalized;

        Vector3 velocityXZ = new(playerBody.velocity.x, 0, playerBody.velocity.z);
        if (velocityXZ.sqrMagnitude < (IsSprinting ? sqrMaxVelocity + sprintSpeed : sqrMaxVelocity))
        {
            playerBody.AddForce(moveForce, ForceMode.VelocityChange);
        }

        Vector3 jumpForce = Vector3.up * jumpStrength;
        if (IsJumping)
        {
            playerBody.AddForce(jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleCameraRotation()
    {
        float angleX = Mathf.SmoothDampAngle(
            cameraTarget.eulerAngles.x,
            cameraTargetEulers.x,
            ref currentRotationVelocityX,
            cameraSmoothTime,
            float.MaxValue,
            Time.fixedDeltaTime
        );
        float angleY = Mathf.SmoothDampAngle(
            cameraTarget.eulerAngles.y,
            cameraTargetEulers.y,
            ref currentRotationVelocityY,
            cameraSmoothTime,
            float.MaxValue,
            Time.fixedDeltaTime
        );

        cameraTarget.rotation = Quaternion.Euler(angleX, angleY, 0);
    }
}
