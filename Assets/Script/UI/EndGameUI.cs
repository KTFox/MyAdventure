using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI endgameText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;





    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneNameString.MAIN_MENU);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        Hide();

        PlayerController.OnPlayerDeath += PlayerController_OnPlayerDeath;
        UndeadBossController.OnUndeadBossDeath += UndeadBossController_OnUndeadBossDeath;
    }

    private void OnDestroy() {
        PlayerController.OnPlayerDeath -= PlayerController_OnPlayerDeath;
        UndeadBossController.OnUndeadBossDeath -= UndeadBossController_OnUndeadBossDeath;

    }

    private void PlayerController_OnPlayerDeath(object sender, System.EventArgs e) {
        Invoke(nameof(ShowLosingGameUI), 1f);
    }

    private void UndeadBossController_OnUndeadBossDeath(object sender, System.EventArgs e) {
        Invoke(nameof(ShowWinGameUI), 1f);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowLosingGameUI() {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        endgameText.text = "YOU LOSE";
    }

    private void ShowWinGameUI() {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        endgameText.text = "YOU WIN";
    }

}
