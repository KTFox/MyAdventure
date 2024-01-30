using UnityEngine;

public class GameDataManager : MonoBehaviour {

    public static GameDataManager Instance { get; private set; }   

    public PlayerHealthSO playerHealthSO;





    private void Awake() {
        Instance = this;
    }

    public void ResetData() {
        playerHealthSO.currentHealth = playerHealthSO.defaultHealth;
    }

}
