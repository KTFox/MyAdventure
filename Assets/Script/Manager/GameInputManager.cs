using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour {

    private const string PLAYER_PREFS_BINDING = "InputBindings";

    public static GameInputManager Instance { get; private set; }

    public event EventHandler OnJump;
    public event EventHandler OnDash;
    public event EventHandler OnMeleeAttack;
    public event EventHandler OnRangedAttack;
    public event EventHandler OnPause;

    public enum Binding {
        Move_Up, Move_Down, Move_Left, Move_Right, Melee_Attack, Ranged_Attack, Jump, Dash
    }

    private PlayerInputAction playerInputAction;





    private void Awake() {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING)) {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));
        }

        playerInputAction.Player.Enable();

        playerInputAction.Player.Jump.performed += Jump_performed;
        playerInputAction.Player.Dash.performed += Dash_performed;
        playerInputAction.Player.MeleeAttack.performed += MeleeAttack_performed;
        playerInputAction.Player.RangedAttack.performed += RangedAttack_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;   
    }

    private void OnDestroy() {
        playerInputAction.Player.Jump.performed -= Jump_performed;
        playerInputAction.Player.Dash.performed -= Dash_performed;
        playerInputAction.Player.MeleeAttack.performed -= MeleeAttack_performed;
        playerInputAction.Player.RangedAttack.performed -= RangedAttack_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void RangedAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRangedAttack?.Invoke(this, EventArgs.Empty);
    }

    private void MeleeAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMeleeAttack?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public float GetMoveInput() {
        float inputValue = playerInputAction.Player.Move.ReadValue<float>();

        return inputValue;
    }

    public float GetClimbInput() {
        float inputValue = playerInputAction.Player.Climb.ReadValue<float>();

        return inputValue;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Climb.bindings[2].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Climb.bindings[1].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Melee_Attack:
                return playerInputAction.Player.MeleeAttack.bindings[0].ToDisplayString();
            case Binding.Ranged_Attack:
                return playerInputAction.Player.RangedAttack.bindings[0].ToDisplayString();
            case Binding.Jump:
                return playerInputAction.Player.Jump.bindings[0].ToDisplayString();
            case Binding.Dash:
                return playerInputAction.Player.Dash.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Climb;
                bindingIndex = 2;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Climb;
                bindingIndex = 1;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Melee_Attack:
                inputAction = playerInputAction.Player.MeleeAttack;
                bindingIndex = 0;
                break;
            case Binding.Ranged_Attack:
                inputAction = playerInputAction.Player.RangedAttack;
                bindingIndex = 0;
                break;
            case Binding.Jump:
                inputAction = playerInputAction.Player.Jump;
                bindingIndex = 0;
                break;
            case Binding.Dash:
                inputAction = playerInputAction.Player.Dash;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDING, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
    }

}
