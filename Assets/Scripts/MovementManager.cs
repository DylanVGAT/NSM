using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deacceleration = 100f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private int maxJump = 2;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float jumpCooldown = 0.05f;
    private float jumpCooldownCounter = 0f;
    [SerializeField] private AudioClip jumpSFX;
    private AudioSource audioSource;



    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.05f;

    // Runtime states
    public Vector2 MovementInput { get; set; }
    public bool JumpBuffered { private get; set; }

    private float currentSpeed;
    private Vector2 oldMovementInput;

    private bool isGrounded;
    private bool hasLeftGround;
    private readonly bool hasJumpedThisFrame;

    private int jumpCount;

    private float coyoteCounter;
    private float jumpBufferCounter;
    private float timeSinceLastJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();


    }

    private void FixedUpdate()
    {
        HandleGroundCheck();
        HandleCoyoteTime();
        HandleJump();
        HandleMovement();

        timeSinceLastJump += Time.fixedDeltaTime;
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            hasLeftGround = false;
            jumpCount = 0;
            coyoteCounter = coyoteTime;
        }
        else if (!hasLeftGround)
        {
            hasLeftGround = true;
            coyoteCounter = coyoteTime;
        }
    }

    private void HandleCoyoteTime()
    {
        if (!isGrounded)
            coyoteCounter -= Time.fixedDeltaTime;
    }

    private void HandleJump()
    {
        if (JumpBuffered)
        {
            jumpBufferCounter = jumpBufferTime;
            JumpBuffered = false;
        }

        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.fixedDeltaTime;

        bool canJump = jumpBufferCounter > 0f && jumpCount < maxJump;

        if (canJump && jumpCooldownCounter <= 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            if (jumpSFX != null)
                audioSource.volume = 0.1f;
                audioSource.PlayOneShot(jumpSFX);

            jumpCount++;
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
            timeSinceLastJump = 0f;
            jumpCooldownCounter = jumpCooldown;
        }

        if (jumpCooldownCounter > 0f)
            jumpCooldownCounter -= Time.fixedDeltaTime;
    }


    private void HandleMovement()
    {
        if (MovementInput.magnitude > 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.fixedDeltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb.velocity = new Vector2(oldMovementInput.x * currentSpeed, rb.velocity.y);
    }

    public void AssignGroundCheck(Transform groundCheckTransform)
    {
        groundCheck = groundCheckTransform;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
