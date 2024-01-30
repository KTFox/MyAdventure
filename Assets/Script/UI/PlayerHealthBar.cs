using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    private HealthSystem healthSystem;
    private Slider healthSlider;





    private void Awake() {
        healthSystem = FindObjectOfType<PlayerController>().GetComponent<HealthSystem>();
        healthSlider = GetComponent<Slider>();
    }

    private void Update() {
        healthSlider.value = healthSystem.NormallizedCurrentHealth;
    }

}
