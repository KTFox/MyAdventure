using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(TagString.PLAYER)) {
            SceneManager.LoadScene(SceneNameString.BOSS_LEVEL);
        }
    }

}
