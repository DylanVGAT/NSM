using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip selectionMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip podiumMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(menuMusic);
                break;
            case "CharacterSelectionMenu":
                PlayMusic(selectionMusic); 
                break;
            case "Game":
                PlayMusic(gameMusic);
                break;
            case "Podium":
                PlayMusic(podiumMusic);
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        audioSource.volume = 0.1f;
        if (audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
