using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour {

    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float _volume = 1f;
    public float Volume {
        get { return _volume; }
        private set {
            _volume = value;
        }
    }





    private void Awake() {
        Instance = this;

        Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, 1f);
    }

    private void Start() {
        PlayerController.OnPlayMeleeAttackSFX += PlayerController_OnMeleeAttack;
        PlayerController.OnPlayRangedAttackSFX += PlayerController_OnPlayRangedAttackSFX;
        PlayerController.OnPlayJumpSFX += PlayerController_OnPlayPlayerJumpSFX;
        PlayerController.OnPlayDashSFX += PlayerController_OnPlayDashSFX;
        PlayerPositionCheck.OnPlayPlayerLandingSFX += PlayerPositionCheck_OnPlayPlayerLandingSFX;

        UndeadBossController.OnPlayAttackSFX += UndeadBossController_OnPlayAttackSFX;
        UndeadBossController.OnTransform += UndeadBossController_OnTransform;

        Apple.OnHasPickedUpSFX += Apple_OnHasPickedUpSFX;
        SpikeTrapController.OnTrapActive += SpikeTrapController_OnTrapActive;

        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void OnDestroy() {
        PlayerController.OnPlayMeleeAttackSFX -= PlayerController_OnMeleeAttack;
        PlayerController.OnPlayRangedAttackSFX -= PlayerController_OnPlayRangedAttackSFX;
        PlayerController.OnPlayJumpSFX -= PlayerController_OnPlayPlayerJumpSFX;
        PlayerController.OnPlayDashSFX -= PlayerController_OnPlayDashSFX;
        PlayerPositionCheck.OnPlayPlayerLandingSFX -= PlayerPositionCheck_OnPlayPlayerLandingSFX;

        UndeadBossController.OnPlayAttackSFX -= UndeadBossController_OnPlayAttackSFX;
        UndeadBossController.OnTransform -= UndeadBossController_OnTransform;

        Apple.OnHasPickedUpSFX -= Apple_OnHasPickedUpSFX;
        SpikeTrapController.OnTrapActive -= SpikeTrapController_OnTrapActive;

        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }

    private void PlayerController_OnMeleeAttack(object sender, System.EventArgs e) {
        PlayerController playerController = sender as PlayerController;
        PlaySound(audioClipRefsSO.playerMeleeAttack, playerController.transform.position);
    }

    private void PlayerController_OnPlayRangedAttackSFX(object sender, System.EventArgs e) {
        PlayerController playerController = sender as PlayerController;
        PlaySound(audioClipRefsSO.playerRangedAttack, playerController.transform.position);
    }

    private void PlayerController_OnPlayPlayerJumpSFX(object sender, System.EventArgs e) {
        PlayerController playerController = sender as PlayerController;
        PlaySound(audioClipRefsSO.playerJump, playerController.transform.position);
    }

    private void PlayerController_OnPlayDashSFX(object sender, System.EventArgs e) {
        PlayerController playerController = sender as PlayerController;
        PlaySound(audioClipRefsSO.playerDash, playerController.transform.position);
    }

    private void PlayerPositionCheck_OnPlayPlayerLandingSFX(object sender, System.EventArgs e) {
        PlayerPositionCheck playerPositionCheck = sender as PlayerPositionCheck;
        PlaySound(audioClipRefsSO.playerLanding, playerPositionCheck.transform.position);
    }

    private void UndeadBossController_OnPlayAttackSFX(object sender, System.EventArgs e) {
        UndeadBossController undeadBossController = sender as UndeadBossController;
        PlaySound(audioClipRefsSO.undeadBossAttack, undeadBossController.transform.position);
    }

    private void UndeadBossController_OnPlaySkillSFX(object sender, System.EventArgs e) {
        UndeadBossController undeadBossController = sender as UndeadBossController;
        PlaySound(audioClipRefsSO.undeadBosSkill, undeadBossController.transform.position);
    }

    private void UndeadBossController_OnTransform(object sender, System.EventArgs e) {
        UndeadBossController undeadBossController = sender as UndeadBossController;
        PlaySound(audioClipRefsSO.undeadBossTransform, undeadBossController.transform.position);
    }

    private void Apple_OnHasPickedUpSFX(object sender, System.EventArgs e) {
        Apple apple = sender as Apple;
        PlaySound(audioClipRefsSO.healPickUp, apple.transform.position);
    }

    private void SpikeTrapController_OnTrapActive(object sender, System.EventArgs e) {
        SpikeTrapController spikeTrap = sender as SpikeTrapController;
        PlaySound(audioClipRefsSO.spikeTrap, spikeTrap.transform.position);
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.unpauseGame, Camera.main.transform.position);
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.pauseGame, Camera.main.transform.position);
    }

    public void PlayPlayerFootstepSound(Vector2 position) {
        PlaySound(audioClipRefsSO.playerFootstep, position);
    }

    public void PlayPlayerClimbLadderSound(Vector2 position) {
        PlaySound(audioClipRefsSO.playerClimbLadder, position);
    }

    private void PlaySound(AudioClip audioClip, Vector2 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * Volume);
    }

    public void ChangeVolume() {
        Volume += 0.1f;

        if (Volume > 1f) {
            Volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, Volume);
        PlayerPrefs.Save();
    }

}
