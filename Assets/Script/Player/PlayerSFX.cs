using UnityEngine;

public class PlayerSFX : MonoBehaviour {

    private PlayerController playerController;
    private PlayerPositionCheck playerPositionCheck;
    private float footstepTimer;
    private float footstepTimerMax = 0.2f;
    private float climbLadderTimer;
    private float climbLadderTimerMax = 0.2f;






    private void Awake() {
        playerController = GetComponent<PlayerController>();
        playerPositionCheck = GetComponent<PlayerPositionCheck>();
    }

    private void Update() {
        HandlePlayerFootstepSound();
        HandlePlayClimbLadderSound();
    }

    private void HandlePlayerFootstepSound() {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer < 0f) {
            footstepTimer = footstepTimerMax;

            if (playerController.IsRunning && playerPositionCheck.IsGrounded) {
                SoundManager.Instance.PlayPlayerFootstepSound(transform.position);
            }
        }
    }

    private void HandlePlayClimbLadderSound() {
        climbLadderTimer -= Time.deltaTime;

        if (climbLadderTimer < 0f) {
            climbLadderTimer = climbLadderTimerMax;

            if (playerController.IsClimbing && playerPositionCheck.IsLadder) {
                SoundManager.Instance.PlayPlayerClimbLadderSound(transform.position);
            }
        }
    }

}
