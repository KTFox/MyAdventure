using UnityEngine;

public class SpriteScroller : MonoBehaviour {

    [SerializeField] Vector2 offset;

    private Material material;






    private void Awake() {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update() {
        material.mainTextureOffset += offset * Time.deltaTime;
    }

}
