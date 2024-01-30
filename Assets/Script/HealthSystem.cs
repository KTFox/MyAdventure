using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public class OnTakeDamageArgs : EventArgs {
        public float damage;
    }

    public class OnGetHealArgs : EventArgs {
        public float healAmount;
    }

    public event EventHandler<OnTakeDamageArgs> OnTakeDamage;
    public event EventHandler<OnGetHealArgs> OnGetHeal;
    public event EventHandler OnDeath;

    [SerializeField] private bool isPlayerHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    private bool _invincible;

    public float MaxHealth {
        get { return _maxHealth; }
    }

    public float CurrentHealth {
        get { return _currentHealth; }
        set {
            _currentHealth = value;

            if (_currentHealth <= 0f) {
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public float NormallizedCurrentHealth {
        get { return CurrentHealth / MaxHealth; }
    }

    public bool IsVincible {
        get { return _invincible; }
        set {
            _invincible = value;
        }
    }





    private void Start() {
        if (!isPlayerHealth) {
            CurrentHealth = MaxHealth;
        }
    }

    public void TakeDamge(float damage) {
        float factDamage;

        if (IsVincible) {
            factDamage = 0f;
        } else {
            if (damage <= CurrentHealth) {
                factDamage = damage;
            } else {
                factDamage = CurrentHealth;
            }
        }

        CurrentHealth -= factDamage;

        OnTakeDamage?.Invoke(this, new OnTakeDamageArgs() { damage = factDamage });
    }

    public void GetHeal(float healAmount) {
        float factHeal;
        float maxAvailableHeal = MaxHealth - CurrentHealth;

        if (healAmount <= maxAvailableHeal) {
            factHeal = healAmount;
        } else {
            factHeal = maxAvailableHeal;
        }

        CurrentHealth += factHeal;

        OnGetHeal?.Invoke(this, new OnGetHealArgs() { healAmount = factHeal });
    }

}
