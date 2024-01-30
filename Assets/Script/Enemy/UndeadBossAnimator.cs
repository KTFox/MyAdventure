using UnityEngine;

public class UndeadBossAnimator : MonoBehaviour {

    public const string CAN_MOVE = "canMove";
    public const string CAN_FLIP = "canFlip";
    private const string ATTACK = "attack";
    private const string SUMMON = "summon";
    private const string HAS_TRANSFORMED = "hasTransformed";
    private const string DEATH = "death";

    private Animator animator;
    private UndeadBossController bossController;
    private HealthSystem healthSystem;





    private void Awake() {
        animator = GetComponent<Animator>();
        bossController = GetComponent<UndeadBossController>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        bossController.OnAttack += BossController_OnAttack;
        UndeadBossController.OnTransform += BossController_OnTransform;
        bossController.OnSummonEnemy += BossController_OnSummonEnemy;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void OnDestroy() {
        bossController.OnAttack -= BossController_OnAttack;
        UndeadBossController.OnTransform -= BossController_OnTransform;
        bossController.OnSummonEnemy -= BossController_OnSummonEnemy;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(DEATH, true);
    }

    private void BossController_OnSummonEnemy(object sender, System.EventArgs e) {
        animator.SetTrigger(SUMMON);
    }

    private void BossController_OnTransform(object sender, System.EventArgs e) {
        animator.SetBool(HAS_TRANSFORMED, true);
    }

    private void BossController_OnAttack(object sender, System.EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

}
