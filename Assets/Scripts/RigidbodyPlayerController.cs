using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{
    [Header("Camera Controls")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField, Range(0, 10)] private float mouseXSpeed = 1f;
    [SerializeField, Range(0, 10)] private float mouseYSpeed = 1f;
    [SerializeField, Range(0, 90)] private float cameraClampAngle = 85f;
    [SerializeField, Range(0, 1)] private float cameraSmoothTime = 0.1f;
    [Space]
    [SerializeField, Range(0, 5)] private float headBobAmplitude = 1f;
    [SerializeField, Range(0, 50)] private float headBobFrequency = 1f;
    [Space]
    [Header("Movement Controls")]
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float sqrMaxVelocity = 20f;
    [SerializeField] private float jumpStrength = 5f;
    [SerializeField, Range(0, 1)] private float airDrag = 0.1f;
    [SerializeField, Range(0, 10)] private float stairAssist = 1f;

    private float moveX, moveZ = 0;
    private float mouseX, mouseY = 0;

    // Needed for rotation smoothing
    private float currentRotationVelocityX;
    private float currentRotationVelocityY;

    private Rigidbody playerBody;
    private CapsuleCollider playerCollider;
    private Vector3 cameraTargetEulers;
    private Vector3 cameraTargetStartPos;

    public bool IsMoving => moveX != 0 || moveZ != 0;
    public bool IsSprinting => Input.GetButton("Sprint") && IsGrounded && IsMoving;
    public bool IsJumping => Input.GetButton("Jump") && IsGrounded;
    public bool IsGrounded { get; private set; }
    

    private void Awake()
    {
        if (TryGetComponent(out Rigidbody body))
        {
            playerBody = body;
        }
        else
            Debug.LogError("Missing player rigidbody component!");

        if (TryGetComponent(out CapsuleCollider collider))
        {
            playerCollider = collider;
        }
        else
            Debug.LogError("Missing player capsule collider component!");

        cameraTargetEulers = cameraTarget.eulerAngles;
        cameraTargetStartPos = cameraTarget.localPosition;

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
        if (GameManager.Instance.IsGamePaused) return;

        // Movement
        moveX = Input.GetAxis("Horizontal") * moveSpeed;
        moveZ = Input.GetAxis("Vertical") * moveSpeed;

        if (IsSprinting)
        {
            DoHeadBob();
        }
        else if (cameraTarget.localPosition != cameraTargetStartPos)
        {
            ResetCamera();
        }

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

    private void DoHeadBob()
    {
        Vector3 newPos = Vector3.zero;
        newPos.y += Mathf.Sin(Time.time * headBobFrequency) * headBobAmplitude * Time.deltaTime;
        cameraTarget.localPosition += newPos;
    }
    
    private void ResetCamera()
    {
        cameraTarget.localPosition = Vector3.MoveTowards(cameraTarget.localPosition, cameraTargetStartPos, 0.01f);
    }

    private void FixedUpdate()
    {
        IsGrounded = CheckIfGrounded();
        HandlePlayerMove();
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
    }

    private bool CheckIfGrounded()
    {
        bool isColliding = Physics.OverlapBox(
            transform.TransformPoint(playerCollider.center),
            playerCollider.bounds.extents,
            transform.rotation,
            LayerMask.GetMask("Ground", "Stairs")).Length > 0;

        if (Physics.Raycast(playerBody.position, Vector3.down, out RaycastHit hit, playerCollider.bounds.extents.y + 0.1f))
        {
            return isColliding && 
                hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit.transform.gameObject.layer == LayerMask.NameToLayer("Stairs");
        }

        return false;
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

        // Check for stairs
        if (Physics.Raycast(playerBody.position, Vector3.down, out RaycastHit hit, playerCollider.bounds.extents.y + 0.1f))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Stairs" && !IsJumping)
            {
                moveForce.y += stairAssist;
            }
        }

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
            cameraSmoothTime
        );
        float angleY = Mathf.SmoothDampAngle(
            cameraTarget.eulerAngles.y,
            cameraTargetEulers.y,
            ref currentRotationVelocityY,
            cameraSmoothTime
        );

        cameraTarget.localRotation = Quaternion.Euler(angleX, angleY, 0);
    }
}
