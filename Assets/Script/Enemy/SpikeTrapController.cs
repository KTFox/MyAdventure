using System;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour {

    private const string IS_ACTIVE = "isActive";

    public static event EventHandler OnTrapActive;

    [SerializeField] private float damage;

    private Animator animator;
    private bool _isActive;

    public bool IsActive {
        get { return _isActive; }
        set {
            _isActive = value;
            animator.SetBool(IS_ACTIVE, value);
        }
    }





    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.PLAYER) && !IsActive) {
            collision.GetComponent<HealthSystem>().TakeDamge(damage);

            OnTrapActive?.Invoke(this, EventArgs.Empty);

            IsActive = true;
        }
    }

}
