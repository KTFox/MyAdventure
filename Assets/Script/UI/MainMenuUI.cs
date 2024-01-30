using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;





    private void Awake() {
        Time.timeScale = 1.0f;

        playButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneNameString.LEVEL_1);
            GameDataManager.Instance.ResetData();
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

}
