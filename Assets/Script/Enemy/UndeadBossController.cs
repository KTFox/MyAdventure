using System;
using System.Collections.Generic;
using UnityEngine;

public class UndeadBossController : MonoBehaviour {

    public static event EventHandler OnUndeadBossDeath;
    public static event EventHandler OnPlayAttackSFX;
    public static event EventHandler OnTransform;
    public event EventHandler OnAttack;
    public event EventHandler OnSummonEnemy;

    [SerializeField] private float followTargetSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float summonCooldown;
    [SerializeField] private Transform form2Transform;

    [Header("Summon skill set up")]
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private Transform spawnPoint3;

    private enum BossState {
        Form1,
        Form2
    }

    private Rigidbody2D rb;
    private Animator animator;
    private HealthSystem healthSystem;
    private float timeCountdownToAttack;
    private float timeCountdownToSummon;
    private bool _hasSummoned;
    private BossState state;

    public bool CanSummon {
        get { return timeCountdownToSummon <= 0; }
    }

    public bool CanMove {
        get { return animator.GetBool(UndeadBossAnimator.CAN_MOVE); }
    }

    public bool CanFlip {
        get { return animator.GetBool(UndeadBossAnimator.CAN_FLIP); }
    }

    public bool CanAttack {
        get { return timeCountdownToAttack <= 0; }
    }

    public bool HasSummoned {
        get { return _hasSummoned; }
        set {
            _hasSummoned = value;
        }
    }





    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        state = BossState.Form1;

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void FixedUpdate() {
        switch (state) {
            default:
            case BossState.Form1:
                timeCountdownToAttack -= Time.deltaTime;

                HandleForm1Logic();

                if (healthSystem.CurrentHealth <= (healthSystem.MaxHealth / 2)) {
                    state = BossState.Form2;

                    timeCountdownToSummon = summonCooldown;

                    OnTransform?.Invoke(this, EventArgs.Empty);
                }

                break;
            case BossState.Form2:
                HandleForm2Logic();

                break;
        }
    }

    private void OnDestroy() {
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        OnUndeadBossDeath?.Invoke(this, EventArgs.Empty);
    }

    private void HandleForm1Logic() {
        Vector2 targetPos = FindObjectOfType<PlayerController>().transform.position;
        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
        float distToTarget = Vector2.Distance(transform.position, targetPos);

        // Attacking target logic
        if (distToTarget <= attackRange) {
            if (CanAttack) {
                OnAttack?.Invoke(this, EventArgs.Empty);
                timeCountdownToAttack = attackCooldown;
            }
        } else {
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

    private void HandleForm2Logic() {
        // Can summon one time
        if (!HasSummoned) {
            timeCountdownToSummon -= Time.deltaTime;

            if (CanSummon) {
                HasSummoned = true;

                timeCountdownToSummon = summonCooldown;

                OnSummonEnemy?.Invoke(this, EventArgs.Empty);
            }
        } else {
            SkeletonController skeleton = GameObject.FindObjectOfType<SkeletonController>();
            if (skeleton != null) {
                Debug.Log("Invincible");
                healthSystem.IsVincible = true;
            } else {
                Debug.Log("Vincible");
                healthSystem.IsVincible = false;
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





    /*
     Animation events
     */
    public void SwitchToForm2Trans() {
        transform.position = form2Transform.position;
        transform.localScale = form2Transform.localScale;
    }

    public void SummonEnemy() {
        Instantiate(skeletonPrefab, spawnPoint1.position, Quaternion.identity, transform.parent);
        Instantiate(skeletonPrefab, spawnPoint2.position, Quaternion.identity, transform.parent);
        Instantiate(skeletonPrefab, spawnPoint3.position, Quaternion.identity, transform.parent);
    }

    public void PlayAttackSFX() {
        OnPlayAttackSFX?.Invoke(this, EventArgs.Empty);
    }

}
