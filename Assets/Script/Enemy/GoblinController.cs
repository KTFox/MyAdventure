using System;
using UnityEngine;

public class GoblinController : MonoBehaviour {

    public event EventHandler OnAttack;

    [Header("Goblin parameters")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float followTargetSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;

    [Header("Set up")]
    [SerializeField] private DetectArea detectArea;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float distToSwitchPoint;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    private HealthSystem healthSystem;
    private float timeCountdownToAttack;
    private bool _targetInAttackRange;
    private bool _death;

    public bool CanMove {
        get { return animator.GetBool(GoblinAnimator.CAN_MOVE); }
    }

    public bool CanFlip {
        get { return animator.GetBool(GoblinAnimator.CAN_FLIP); }
    }

    public bool TargetInAttackRange {
        get { return _targetInAttackRange; }
        private set {
            _targetInAttackRange = value;
        }
    }

    public bool CanAttack {
        get { return timeCountdownToAttack <= 0; }
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
        currentPoint = pointA;

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void OnDestroy() {
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void Update() {
        if (Death) {
            rb.velocity = Vector2.zero;
            return;
        }

        timeCountdownToAttack -= Time.deltaTime;

        if (!detectArea.HasTarget) {
            Patrol();
        } else {
            AttackTarget();
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        Death = true;
    }

    private void Patrol() {
        float distToDestination = Vector2.Distance(transform.position, currentPoint.position);
        Vector2 moveDir = (currentPoint.position - transform.position).normalized;

        // Set destination point
        if (distToDestination < distToSwitchPoint && currentPoint == pointA) {
            currentPoint = pointB;
        } else if (distToDestination < distToSwitchPoint && currentPoint == pointB) {
            currentPoint = pointA;
        }

        // Move to destination point
        HandleMovement(moveDir, patrolSpeed);

        // Flip sprite
        if (CanFlip) {
            if (currentPoint == pointB) {
                transform.localScale = new Vector2(-1, 1);
            } else if (currentPoint == pointA) {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private void AttackTarget() {
        Vector2 targetPos = detectArea.Target.transform.position;
        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
        float distToTarget = Vector2.Distance(transform.position, targetPos);

        // Attacking logic
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
        Gizmos.DrawWireSphere(pointA.position, distToSwitchPoint);
        Gizmos.DrawWireSphere(pointB.position, distToSwitchPoint);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }

}
