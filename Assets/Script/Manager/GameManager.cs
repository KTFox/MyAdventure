using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused;





    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameInputManager.Instance.OnPause += GameInputManager_OnPause;
    }

    private void OnDestroy() {
        GameInputManager.Instance.OnPause -= GameInputManager_OnPause;
    }

    private void GameInputManager_OnPause(object sender, EventArgs e) {
        TogglePauseGame();
    }

    public void TogglePauseGame() {
        if (!isGamePaused) {
            Time.timeScale = 0f;

            isGamePaused = true;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;

            isGamePaused = false;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

}
