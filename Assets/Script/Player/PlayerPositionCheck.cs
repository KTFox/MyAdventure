using System;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {

    public static event EventHandler OnPlayPlayerLandingSFX;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isLadder;

    public bool IsGrounded {
        get { return _isGrounded; }
        private set { _isGrounded = value; }
    }

    public bool IsLadder {
        get { return _isLadder; }
        private set { _isLadder = value; }
    }





    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(TagString.GROUND)) {
            IsGrounded = true;

            OnPlayPlayerLandingSFX?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(TagString.GROUND)) {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.LADDER)) {
            IsLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(TagString.LADDER)) {
            IsLadder = false;
        }
    }

}
