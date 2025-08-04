using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Water Settings")]
    [SerializeField] private float waterMoveSpeedMultiplier = 0.5f;
    [SerializeField] private float waterGravityMultiplier = 0.3f;
    [SerializeField] private float waterExitJumpForce = 10f;

    [Header("Swim Mode Settings")]
    [SerializeField] private bool _isInSwimMode = false;
    [SerializeField] private float _swimSpeed = 3f;
    [SerializeField] private float _swimGravityScale = 0.7f;

    private Rigidbody2D _rb;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isInWater;
    private float _originalGravityScale;
    private float _originalMoveSpeed;
    private BubbleShield _activeBubbleShield;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravityScale = 1f;
        _originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (_isInSwimMode)
        {
            HandleSwimMovement();
        }
        else
        {
            HandleNormalMovement();
        }
    }

    private void HandleNormalMovement()
    {
        float currentMoveSpeed = _isInWater ? moveSpeed * waterMoveSpeedMultiplier : moveSpeed;
        float moveX = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(moveX * currentMoveSpeed, _rb.linearVelocity.y);

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Only allow jumping when grounded and not in water
        if (Input.GetKeyDown(KeyCode.W) && _isGrounded && !_isInWater)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleSwimMovement()
    {
        // Four-directional swimming movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        Vector2 swimVelocity = new Vector2(moveX * _swimSpeed, moveY * _swimSpeed);
        _rb.linearVelocity = swimVelocity;
    }

    public void EnableSwimMode(float swimSpeed, float swimGravityScale)
    {
        _isInSwimMode = true;
        _swimSpeed = swimSpeed;
        _swimGravityScale = swimGravityScale;
        _rb.gravityScale = swimGravityScale;
        
        // Find active bubble shield
        _activeBubbleShield = FindObjectOfType<BubbleShield>();
    }

    public void DisableSwimMode()
    {
        _isInSwimMode = false;
        _rb.gravityScale = _originalGravityScale;
        _activeBubbleShield = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            _isInWater = true;
            if (!_isInSwimMode)
            {
                _rb.gravityScale = waterGravityMultiplier;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            _isInWater = false;
            
            // If in swim mode, destroy bubble shield when exiting water
            if (_isInSwimMode && _activeBubbleShield != null)
            {
                _activeBubbleShield.OnPlayerExitWater();
                // Add jump effect when exiting water to simulate jumping out
                Vector2 currentVelocity = _rb.linearVelocity;
                _rb.linearVelocity = new Vector2(currentVelocity.x, waterExitJumpForce);
            }
            
            _rb.gravityScale = 1f;
            
        }
    }
}
