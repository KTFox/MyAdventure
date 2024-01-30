using UnityEngine;

public class SkeletonAnimator : MonoBehaviour {

    private const string START_TO_ATTACK = "startToAttack";
    public const string CAN_MOVE = "canMove";
    public const string CAN_FLIP = "canFlip";
    private const string CAN_ATTACK = "canAttack";
    private const string ATTACK = "attack";
    private const string TARGET_IN_ATTACK_RANGE = "targetInAttackRange";
    private const string DEATH = "death";

    private Animator animator;
    private SkeletonController controller;
    private HealthSystem healthSystem;





    private void Awake() {
        animator = GetComponent<Animator>();
        controller = GetComponent<SkeletonController>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        controller.OnAttack += Controller_OnAttack;
        controller.OnChangeStateToAttack += Controller_OnChangeStateToAttack;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void Update() {
        animator.SetBool(TARGET_IN_ATTACK_RANGE, controller.TargetInAttackRange);
        animator.SetBool(CAN_ATTACK, controller.CanAttack);
    }

    private void OnDestroy() {
        controller.OnAttack -= Controller_OnAttack;
        controller.OnChangeStateToAttack -= Controller_OnChangeStateToAttack;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void Controller_OnAttack(object sender, System.EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

    private void Controller_OnChangeStateToAttack(object sender, System.EventArgs e) {
        animator.SetBool(START_TO_ATTACK, true);
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(DEATH, true);
    }

}
