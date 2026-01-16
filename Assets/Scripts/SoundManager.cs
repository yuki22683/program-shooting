using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;
    private List<AudioClip> audioClips = new List<AudioClip>();
    private readonly string musicFolder = "BGM";
    private bool isInitialized = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        Initialize();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized || audioSource == null || audioClips.Count == 0) return;

        if (!audioSource.isPlaying)
        {
            PlayRandomClip();
        }
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            Debug.LogWarning("SoundManager is already initialized.");
            return;
        }

        LoadMP3Files();

        if (audioClips.Count > 0)
        {
            PlayRandomClip();
            isInitialized  = true;
        }
        else
        {
            Debug.LogWarning("No audio clips found in Resource/BGM folder.");
        }
    }

    public void SetVolume(float volume)
    {
        if (!isInitialized || audioSource == null) return;
        audioSource.volume = Mathf.Clamp01(volume);
    }

    private void LoadMP3Files()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(musicFolder);
        if (clips.Length > 0)
        {
            audioClips.AddRange(clips);
            Debug.Log($"Loaded {clips.Length} audio clips from Resources/{musicFolder}");
        }
        else
        {
            Debug.LogError($"No audio clips found in Resources/{musicFolder}");
        }
    }

    private void PlayRandomClip()
    {
        if (audioClips.Count == 0) return;

        int randomIndex = Random.Range(0, audioClips.Count);
        audioSource.clip = audioClips[randomIndex];
        audioSource.volume = DataManager.gameSettings.soundSettings.isBgmMute ? 0f : 1f;
        audioSource.loop = true;
        audioSource.Play();
        Debug.Log($"Playing: {audioClips[randomIndex].name}");
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SoundManager");
                instance = go.AddComponent<SoundManager>();
            }
            return instance;
        }
    }

    public void SetSoundVolume(float volume)
    {
        if (!isInitialized || audioSource == null) return;

        AudioSource[]allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            if (source != audioSource)
            {
                //source.mute = mute;
                source.volume = Mathf.Clamp01(volume);
            }
        }
    }

    public void ApplyMuteToNewAudioSource(GameObject obj)
    {
        if (!isInitialized || audioSource == null) return;

        AudioSource source = obj.GetComponent<AudioSource>();
        if (source != null && source != audioSource)
        {
            if  (!DataManager.gameSettings.soundSettings.isSoundMute)
            {
                source.volume = Mathf.Clamp01(DataManager.gameSettings.soundSettings.soundVolume);
            }
            else
            {
                source.volume = 0.0f;
            }
            Debug.Log($"Applied volume to newly activated AudioSource on {obj.name}");
        }
    }
}
