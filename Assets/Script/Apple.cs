using System;
using UnityEngine;

public class Apple : MonoBehaviour {

    public static event EventHandler OnHasPickedUpSFX;

    [SerializeField] private float healAmount;





    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.PLAYER)) {
            collision.GetComponent<HealthSystem>().GetHeal(healAmount);

            OnHasPickedUpSFX?.Invoke(this, EventArgs.Empty);

            gameObject.SetActive(false);
        }
    }

}
