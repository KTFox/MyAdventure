using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GoblinAnimator : MonoBehaviour {

    // Animation parameters
    public const string CAN_MOVE = "canMove";
    public const string CAN_FLIP = "canFlip";
    private const string HAS_TARGET = "hasTarget";
    private const string TARGET_IN_ATTACK_RANGE = "targetInAttackRange";
    private const string ATTACK = "attack";
    private const string DEATH = "death";

    [SerializeField] private DetectArea detectArea;

    private GoblinController goblinController;
    private Animator animator;
    private HealthSystem healthSystem;





    private void Awake() {
        animator = GetComponent<Animator>();
        goblinController = GetComponent<GoblinController>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Update() {
        animator.SetBool(HAS_TARGET, detectArea.HasTarget);
        animator.SetBool(TARGET_IN_ATTACK_RANGE, goblinController.TargetInAttackRange);
    }

    private void Start() {
        goblinController.OnAttack += GoblinController_OnAttack;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void OnDestroy() {
        goblinController.OnAttack -= GoblinController_OnAttack;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void GoblinController_OnAttack(object sender, System.EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(DEATH, true);
    }

}
