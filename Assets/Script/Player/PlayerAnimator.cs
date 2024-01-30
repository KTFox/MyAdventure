using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    // Animation parameters
    private const string IS_RUNNING = "isRunning";
    private const string IS_CLIMBING = "isClimbing";
    private const string IS_GROUNDED = "isGrounded";
    private const string IS_LADDER = "isLadder";
    public const string CAN_MOVE = "canMove";
    public const string CAN_FLIP = "canFlip";
    private const string y_VELOCITY = "yVelocity";
    private const string MELEE_ATTACK = "meleeAttack";
    private const string RANGED_ATTACK = "rangedAttack";
    private const string TAKE_HIT = "takeHit";
    private const string DEATH = "death";

    private Animator animator;
    private PlayerPositionCheck positionCheck;
    private HealthSystem healthSystem;





    private void Awake() {
        animator = GetComponent<Animator>();
        positionCheck = GetComponent<PlayerPositionCheck>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        GameInputManager.Instance.OnMeleeAttack += GameInputManager_OnMeleeAttack;
        GameInputManager.Instance.OnRangedAttack += GameInputManager_OnRangedAttack;
        healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void Update() {
        animator.SetBool(IS_GROUNDED, positionCheck.IsGrounded);
        animator.SetBool(IS_LADDER, positionCheck.IsLadder);
        animator.SetBool(IS_RUNNING, PlayerController.Instance.IsRunning);
        animator.SetBool(IS_CLIMBING, PlayerController.Instance.IsClimbing);
        animator.SetFloat(y_VELOCITY, PlayerController.Instance.YVelocity);
    }

    private void OnDestroy() {
        GameInputManager.Instance.OnMeleeAttack -= GameInputManager_OnMeleeAttack;
        GameInputManager.Instance.OnRangedAttack -= GameInputManager_OnRangedAttack;
        healthSystem.OnTakeDamage -= HealthSystem_OnTakeDamage;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(DEATH, true);
    }

    private void HealthSystem_OnTakeDamage(object sender, HealthSystem.OnTakeDamageArgs e) {
        animator.SetTrigger(TAKE_HIT);
    }

    private void GameInputManager_OnRangedAttack(object sender, System.EventArgs e) {
        if (!positionCheck.IsGrounded) {
            return;
        }

        animator.SetTrigger(RANGED_ATTACK);
    }

    private void GameInputManager_OnMeleeAttack(object sender, System.EventArgs e) {
        if (!positionCheck.IsGrounded) {
            return;
        }

        animator.SetTrigger(MELEE_ATTACK);
    }

}
