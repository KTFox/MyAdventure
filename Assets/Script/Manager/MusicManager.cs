using UnityEngine;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float _volume = 0.3f;

    public float Volume {
        get { return _volume; }
        private set { _volume = value; }
    }





    private void Awake() {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        audioSource.volume = Volume;
    }

    public void ChangeVolume() {
        Volume += 0.1f;

        if (Volume > 1f) {
            Volume = 0f;
        }

        audioSource.volume = Volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, Volume);
        PlayerPrefs.Save();
    }

}
