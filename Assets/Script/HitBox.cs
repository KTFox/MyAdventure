using UnityEngine;

public class HitBox : MonoBehaviour {

    private enum HitboxType {
        PlayerHitbox,
        EnemyHitbox
    }

    [SerializeField] private HitboxType type;
    [SerializeField] private bool _flyHitbox;
    [SerializeField] private float damage;
    [SerializeField] private float timeToDestroy;

    private float timeElapsed;

    public bool FlyHitbox {
        get { return _flyHitbox; }
    }





    private void Update() {
        timeElapsed += Time.deltaTime;

        if (FlyHitbox) {
            if (timeElapsed >= timeToDestroy) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (type) {
            default:
            case HitboxType.PlayerHitbox:
                if (collision.gameObject.CompareTag(TagString.ENEMY)) {
                    collision.GetComponent<HealthSystem>().TakeDamge(damage);
                }
                break;
            case HitboxType.EnemyHitbox:
                if (collision.gameObject.CompareTag(TagString.PLAYER)) {
                    collision.GetComponent<HealthSystem>().TakeDamge(damage);
                }
                break;
        }

        if (FlyHitbox) {
            Destroy(gameObject);
        }
    }

}
