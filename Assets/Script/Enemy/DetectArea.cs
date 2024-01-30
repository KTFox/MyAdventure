using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DetectArea : MonoBehaviour {

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private GameObject _target;
    private BoxCollider2D boxCollider;

    public GameObject Target {
        get { return _target; }
        private set {
            _target = value;
        }
    }

    public bool HasTarget {
        get { return Target != null; }
    }





    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        Init();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.PLAYER)) {
            Target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.PLAYER)) {
            Target = null;
        }
    }

    private void Init() {
        float xBoxSize = Vector2.Distance(pointA.position, pointB.position);
        float yBoxSize = 1.5f;

        boxCollider.size = new Vector2(xBoxSize, yBoxSize);
    }

}
