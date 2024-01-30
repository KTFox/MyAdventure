using System;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

    public event EventHandler OnChangeStateToAttack;
    public event EventHandler OnAttack;

    [SerializeField] private float followTargetSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private float timeCooldownToStartAttack;

    private enum SkeletonState {
        Preparing,
        Attacking
    }

    private Rigidbody2D rb;
    private Animator animator;
    private HealthSystem healthSystem;
    private SkeletonState state;
    private float timeCountdownToAttack;
    private bool _targetInAttackRange;
    private bool _death;

    public bool CanAttack {
        get { return timeCountdownToAttack <= 0; }
    }

    public bool CanMove {
        get { return animator.GetBool(SkeletonAnimator.CAN_MOVE); }
    }

    public bool CanFlip {
        get { return animator.GetBool(SkeletonAnimator.CAN_FLIP); }
    }

    public bool TargetInAttackRange {
        get { return _targetInAttackRange; }
        private set {
            _targetInAttackRange = value;
        }
    }

    public bool Death {
        get { return _death; }
        set {
            _death = value;
        }
    }





    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        state = SkeletonState.Preparing;

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void FixedUpdate() {
        if (Death) {
            rb.velocity = Vector3.zero;
            return;
        }

        switch (state) {
            default:
            case SkeletonState.Preparing:
                timeCooldownToStartAttack -= Time.deltaTime;

                if (timeCooldownToStartAttack < 0) {
                    state = SkeletonState.Attacking;
                    OnChangeStateToAttack?.Invoke(this, EventArgs.Empty);
                }

                break;
            case SkeletonState.Attacking:
                timeCountdownToAttack -= Time.deltaTime;

                AttackTarget();

                break;
        }
    }

    private void OnDestroy() {
        healthSystem.OnDeath -= HealthSystem_OnDeath;

    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        Death = true;
    }

    private void AttackTarget() {
        Vector2 targetPos = FindObjectOfType<PlayerController>().transform.position;
        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
        float distToTarget = Vector2.Distance(transform.position, targetPos);

        // Attacking target logic
        if (distToTarget <= attackRange) {
            TargetInAttackRange = true;

            if (CanAttack) {
                OnAttack?.Invoke(this, EventArgs.Empty);
                timeCountdownToAttack = attackCooldown;
            }
        } else {
            TargetInAttackRange = false;

            HandleMovement(moveDir, followTargetSpeed);
        }

        // Flip sprite
        if (CanFlip) {
            if (transform.position.x > targetPos.x) {
                transform.localScale = new Vector2(-1, 1);
            } else if (transform.position.x < targetPos.x) {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private void HandleMovement(Vector2 moveDir, float moveSpeed) {
        if (CanMove) {
            rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
