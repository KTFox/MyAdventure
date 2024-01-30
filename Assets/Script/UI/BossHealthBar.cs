using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Slider healthSlider;





    private void Awake() {
        healthSystem = FindObjectOfType<UndeadBossController>().GetComponent<HealthSystem>();
        healthSlider = GetComponent<Slider>();
    }

    private void Update() {
        healthSlider.value = healthSystem.NormallizedCurrentHealth;
    }
}
