using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour {

    [SerializeField] private float popupSpeed;
    [SerializeField] private float timeToFade;

    private Color startColor;
    private float timeElapsed;





    private void Start() {
        startColor = GetComponent<TextMeshPro>().color;
    }

    private void Update() {
        transform.position += new Vector3(0, popupSpeed * Time.deltaTime, 0);

        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToFade) {
            float newAlpha = startColor.a * (1 - timeElapsed / timeToFade);
            GetComponent<TextMeshPro>().color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        } else {
            Destroy(gameObject);
        }
    }

}
