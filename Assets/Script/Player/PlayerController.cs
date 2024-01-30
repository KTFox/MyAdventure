using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; private set; }

    public static event EventHandler OnPlayerDeath;
    public static event EventHandler OnPlayMeleeAttackSFX;
    public static event EventHandler OnPlayRangedAttackSFX;
    public static event EventHandler OnPlayJumpSFX;
    public static event EventHandler OnPlayDashSFX;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private float dashImpulse;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private Transform launchArrowPoint;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;

    private Rigidbody2D rb;
    private Animator animator;
    private TrailRenderer trailRenderer;
    private PlayerPositionCheck postionCheck;
    private HealthSystem healthSystem;

    private float defaultGravity;
    private bool isFacingRight = true;
    private bool canDash = true;
    private bool isDashing;
    private float dashingTime = 0.2f;
    private bool _isRunning;
    private bool _isClimbing;
    private float _yVelocity;
    private bool _isDeath;

    public bool IsRunning {
        get { return _isRunning; }
        private set { _isRunning = value; }
    }

    public bool IsClimbing {
        get { return _isClimbing; }
        private set { _isClimbing = value; }
    }

    public bool CanMove {
        get { return animator.GetBool(PlayerAnimator.CAN_MOVE); }
    }

    public bool CanFlip {
        get { return animator.GetBool(PlayerAnimator.CAN_FLIP); }
    }

    public float YVelocity {
        get { return _yVelocity; }
        private set { _yVelocity = value; }
    }

    public bool IsDeath {
        get { return _isDeath; }
        set {
            _isDeath = value;
        }
    }





    private void Awake() {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
        postionCheck = GetComponent<PlayerPositionCheck>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        defaultGravity = rb.gravityScale;

        healthSystem.CurrentHealth = GameDataManager.Instance.playerHealthSO.currentHealth;

        healthSystem.OnDeath += HealthSystem_OnDeath;
        GameInputManager.Instance.OnJump += GameInputManager_OnJump;
        GameInputManager.Instance.OnDash += GameInputManager_OnDash;
    }

    private void FixedUpdate() {
        if (IsDeath) {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isDashing) {
            return;
        }

        HandleMovement();
        HandleClimbladder();
        FlipSprite();

        YVelocity = rb.velocity.y;
    }

    private void OnDestroy() {
        GameDataManager.Instance.playerHealthSO.currentHealth = healthSystem.CurrentHealth;

        healthSystem.OnDeath -= HealthSystem_OnDeath;
        GameInputManager.Instance.OnJump -= GameInputManager_OnJump;
        GameInputManager.Instance.OnDash -= GameInputManager_OnDash;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        IsDeath = true;

        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    private void GameInputManager_OnJump(object sender, System.EventArgs e) {
        if (IsDeath) {
            return;
        }

        if (!CanMove) {
            return;
        }

        if (postionCheck.IsGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);

            OnPlayJumpSFX?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInputManager_OnDash(object sender, System.EventArgs e) {
        if (IsDeath) {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!IsRunning || postionCheck.IsLadder) {
            return;
        }

        if (canDash) {
            StartCoroutine(Dash());
        }
    }

    private void HandleMovement() {
        float moveDir = GameInputManager.Instance.GetMoveInput();

        if (CanMove) {
            rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        } else {
            rb.velocity = Vector2.zero;
        }

        IsRunning = rb.velocity.x != 0f;
    }

    private void HandleClimbladder() {
        float moveDir = GameInputManager.Instance.GetClimbInput();

        if (postionCheck.IsLadder) {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, moveDir * climbSpeed);
        } else {
            rb.gravityScale = defaultGravity;
        }

        IsClimbing = rb.velocity.y != 0f;
    }

    private void FlipSprite() {
        if (CanFlip) {
            if (isFacingRight && rb.velocity.x < 0) {
                rb.transform.localScale = new Vector2(-1, 1);
                isFacingRight = false;
            } else if (!isFacingRight && rb.velocity.x > 0) {
                rb.transform.localScale = new Vector2(1, 1);
                isFacingRight = true;
            }
        }
    }

    IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        rb.gravityScale = 0f;

        float dashDir = GameInputManager.Instance.GetMoveInput();
        rb.velocity = new Vector2(dashDir * dashImpulse, 0);

        trailRenderer.emitting = true;

        OnPlayDashSFX?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(dashingTime);

        trailRenderer.emitting = false;
        isDashing = false;
        rb.gravityScale = defaultGravity;

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }





    public void LaunchArrow() {
        float flyDir = transform.localScale.x;

        GameObject arrow = Instantiate(arrowPrefab, launchArrowPoint.position, Quaternion.identity);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

        arrowRb.velocity = new Vector2(flyDir * arrowSpeed, arrowRb.velocity.y);
    }

    public void PlayMeleeAttackSFX() {
        OnPlayMeleeAttackSFX?.Invoke(this, EventArgs.Empty);
    }

    public void PlayRangedAttackSFX() {
        OnPlayRangedAttackSFX?.Invoke(this, EventArgs.Empty);
    }

}
