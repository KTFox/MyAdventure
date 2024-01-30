using TMPro;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PopupTextLauncher : MonoBehaviour {

    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private TextMeshPro healText;
    [SerializeField] private Vector3 offset;

    private HealthSystem healthSystem;





    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() {
        healthSystem.OnTakeDamage += HealthSystem_OnTakeDamage;
        healthSystem.OnGetHeal += HealthSystem_OnGetHeal;
    }

    private void OnDestroy() {
        healthSystem.OnTakeDamage -= HealthSystem_OnTakeDamage;
        healthSystem.OnGetHeal -= HealthSystem_OnGetHeal;
    }

    private void HealthSystem_OnTakeDamage(object sender, HealthSystem.OnTakeDamageArgs e) {
        TextMeshPro popupText = Instantiate(damageText, transform.position + offset, Quaternion.identity);
        popupText.text = e.damage.ToString();
    }

    private void HealthSystem_OnGetHeal(object sender, HealthSystem.OnGetHealArgs e) {
        TextMeshPro popupText = Instantiate(healText, transform.position + offset, Quaternion.identity);
        popupText.text = e.healAmount.ToString();
    }

}
