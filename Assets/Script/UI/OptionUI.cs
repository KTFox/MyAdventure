using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour {

    public static OptionUI Instance { get; private set; }

    [Header("Key Binding Button")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button meleeAttackButton;
    [SerializeField] private Button rangedAttackButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button dashButton;

    [Header("Key Binding Text")]
    [SerializeField] private TextMeshProUGUI moveUpButonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI meleeAttackButtonText;
    [SerializeField] private TextMeshProUGUI rangedAttackButtonText;
    [SerializeField] private TextMeshProUGUI jumpButtonText;
    [SerializeField] private TextMeshProUGUI dashButtonText;

    [Header("Audio Option")]
    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;

    [Header("Audio Text")]
    [SerializeField] private TextMeshProUGUI soundEffectButtonText;
    [SerializeField] private TextMeshProUGUI musicButtonText;

    [Header("Set up")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform pressToRebindUI;





    private void Awake() {
        Instance = this;

        // Key Binding Buttons
        moveUpButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Move_Right);
        });
        meleeAttackButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Melee_Attack);
        });
        rangedAttackButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Ranged_Attack);
        });
        jumpButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Jump);
        });
        dashButton.onClick.AddListener(() => {
            RebindBinding(GameInputManager.Binding.Dash);
        });

        // Audio Option
        soundEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        // Set Up Buttons
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Start() {
        Hide();
        HidePressToRebindKey();

        UpdateVisual();

        GameInputManager.Instance.OnPause += GameInputManager_OnPause;
    }

    private void OnDestroy() {
        GameInputManager.Instance.OnPause -= GameInputManager_OnPause;
    }

    private void GameInputManager_OnPause(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectButtonText.text = $"Sound Effect: {Mathf.Round(SoundManager.Instance.Volume * 10)}";
        musicButtonText.text = $"Music: {Mathf.Round(MusicManager.Instance.Volume * 10)}";

        moveUpButonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Up);
        moveDownButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Down);
        moveLeftButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Left);
        moveRightButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Right);
        meleeAttackButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Melee_Attack);
        rangedAttackButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Ranged_Attack);
        jumpButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Jump);
        dashButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Dash);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void ShowPressToRebindUI() {
        pressToRebindUI.gameObject.SetActive(true);
    }

    public void HidePressToRebindKey() {
        pressToRebindUI.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInputManager.Binding binding) {
        ShowPressToRebindUI();
        GameInputManager.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

}
