using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour {

    [SerializeField] private float fadeTime;

    private SpriteRenderer spriteRenderer;
    private GameObject objToRemove;
    private Color startColor;
    private float fadeElapsed;





    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        objToRemove = animator.gameObject;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;

        fadeElapsed = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (fadeElapsed < fadeTime) {
            fadeElapsed += Time.deltaTime;

            float newAlpha = (1 - fadeElapsed / fadeTime);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        } else {
            objToRemove.SetActive(false);
        }
    }

}
