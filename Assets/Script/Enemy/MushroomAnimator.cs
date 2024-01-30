using UnityEngine;

public class MushroomAnimator : MonoBehaviour {

    public const string CAN_MOVE = "canMove";
    public const string CAN_FLIP = "canFlip";
    private const string HAS_TARGET = "hasTarget";
    private const string ATTACK = "attack";
    private const string DEATH = "death";

    [SerializeField] private DetectArea detectArea;

    private Animator animator;
    private MushroomController mushroomController;
    private HealthSystem healthSystem;





    private void Awake() {
        animator = GetComponent<Animator>();
        mushroomController = GetComponent<MushroomController>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        mushroomController.OnAttack += MushroomController_OnAttack;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void OnDestroy() {
        mushroomController.OnAttack -= MushroomController_OnAttack;
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    private void Update() {
        animator.SetBool(HAS_TARGET, detectArea.HasTarget);
    }

    private void MushroomController_OnAttack(object sender, System.EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(DEATH, true);
    }

}
