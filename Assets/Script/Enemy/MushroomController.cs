using System;
using UnityEngine;

public class MushroomController : MonoBehaviour {

    public event EventHandler OnAttack;

    [Header("Set up")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float distToSwitchPoint;
    [SerializeField] private DetectArea detectArea;
    [SerializeField] private GameObject mushroomBulletPref;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform launchPoint;

    [Header("Mushroom parameters")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float attackCooldown;

    private Rigidbody2D rb;
    private Animator animator;
    private HealthSystem healthSystem;
    private Transform currentPoint;
    private float timeCountdownToAttack;
    private bool _death;

    public bool CanMove {
        get { return animator.GetBool(MushroomAnimator.CAN_MOVE); }
    }

    public bool CanFlip {
        get { return animator.GetBool(MushroomAnimator.CAN_FLIP); }
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
        Vector2 moveDir = (currentPoint.position - transform.position).normalized;
        float disToDesPoint = Vector2.Distance(currentPoint.position, transform.position);

        // Set current point
        if (disToDesPoint <= distToSwitchPoint && currentPoint == pointA) {
            currentPoint = pointB;
        } else if (disToDesPoint <= distToSwitchPoint && currentPoint == pointB) {
            currentPoint = pointA;
        }

        // Patrol
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
        Transform targetPos = detectArea.Target.transform;

        // Attack target
        if (CanAttack) {
            OnAttack?.Invoke(this, EventArgs.Empty);
            timeCountdownToAttack = attackCooldown;
        }

        // Flip sprite
        if (CanFlip) {
            if (transform.position.x > targetPos.position.x) {
                transform.localScale = new Vector2(-1, 1);
            } else if (transform.position.x < targetPos.position.x) {
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
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(pointA.position, distToSwitchPoint);
        Gizmos.DrawWireSphere(pointB.position, distToSwitchPoint);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }





    /*
     Animation event
     */
    public void LaunchTitle() {
        float flyDir = transform.localScale.x;

        GameObject arrow = Instantiate(mushroomBulletPref, launchPoint.position, Quaternion.identity);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

        arrowRb.velocity = new Vector2(flyDir * bulletSpeed, arrowRb.velocity.y);
    }

}
