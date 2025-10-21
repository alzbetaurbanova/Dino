#pragma warning disable 0618
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicManager>(true);
            }
            return _instance;
        }
    }

    [SerializeField] private AudioClip[] musicPlaylist;
    private AudioSource audioSource;

    private List<AudioClip> shuffledPlaylist = new List<AudioClip>();
    private int currentTrack = 0;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 0.2f;
    public bool isMusicPaused = false;

    [Header("Boss Music Settings")]
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float fadeDuration = 1.5f;

    private Coroutine fadeCoroutine;
    private bool bossMusicPlaying = false;
    private bool hasFocus = true;
    [SerializeField] private Text trackNameText;
    [SerializeField] private Button nextTrackButton;

    void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        audioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        audioSource.volume = savedVolume;

        if (volumeSlider != null)
        {
            SetupSlider(volumeSlider, savedVolume);
        }

        ShufflePlaylist();
        PlayCurrentTrack();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetupSlider(Slider slider, float savedVolume)
    {
        slider.value = savedVolume;
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        if (!hasFocus || isMusicPaused || bossMusicPlaying) return;

        if (!audioSource.isPlaying)
        {
            NextTrack();
        }
    }

    private void ShufflePlaylist()
    {
        if (musicPlaylist == null || musicPlaylist.Length == 0) return;

        shuffledPlaylist = new List<AudioClip>(musicPlaylist);

        for (int i = 0; i < shuffledPlaylist.Count; i++)
        {
            int rnd = Random.Range(i, shuffledPlaylist.Count);
            var temp = shuffledPlaylist[i];
            shuffledPlaylist[i] = shuffledPlaylist[rnd];
            shuffledPlaylist[rnd] = temp;
        }
    }

    private void PlayCurrentTrack()
    {
        if (shuffledPlaylist.Count == 0) return;

        audioSource.clip = shuffledPlaylist[currentTrack];
        audioSource.Play();

        if (trackNameText != null)
            trackNameText.text = audioSource.clip.name;
    }


    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }

    private void NextTrack()
    {
        currentTrack++;

        if (currentTrack >= shuffledPlaylist.Count)
        {
            AudioClip lastClip = shuffledPlaylist.Count > 0 ? shuffledPlaylist[shuffledPlaylist.Count - 1] : null;
            ShufflePlaylist();

            if (lastClip != null && shuffledPlaylist.Count > 1 && shuffledPlaylist[0] == lastClip)
            {
                var temp = shuffledPlaylist[0];
                shuffledPlaylist[0] = shuffledPlaylist[1];
                shuffledPlaylist[1] = temp;
            }

            currentTrack = 0;
        }

        PlayCurrentTrack();
    }

    public void SkipToNextTrack()
    {
        if (bossMusicPlaying || isMusicPaused) return;

        currentTrack++;

        if (currentTrack >= shuffledPlaylist.Count)
            currentTrack = 0;

        PlayCurrentTrack();
    }


    public void PlayBossMusic()
    {
        if (nextTrackButton != null)
            nextTrackButton.gameObject.SetActive(false);


        if (bossMusicPlaying) return;

        bossMusicPlaying = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndPlayBossTrack());
    }

    private IEnumerator FadeOutAndPlayBossTrack()
    {
        if (audioSource == null || bossMusic == null)
        {
            bossMusicPlaying = false;
            yield break;
        }

        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.clip = bossMusic;
        audioSource.Play();

        if (trackNameText != null)
            trackNameText.text = bossMusic.name;


        float targetVolume = volumeSlider != null ? volumeSlider.value : PlayerPrefs.GetFloat("MusicVolume", defaultVolume);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void ResumePlaylist()
    {
        if (nextTrackButton != null)
            nextTrackButton.gameObject.SetActive(true);


        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndResumePlaylist());

    }

    private IEnumerator FadeOutAndResumePlaylist()
    {
        if (audioSource == null)
        {
            bossMusicPlaying = false;
            yield break;
        }

        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        ShufflePlaylist();
        currentTrack = 0;
        PlayCurrentTrack();

        float targetVolume = volumeSlider != null ? volumeSlider.value : PlayerPrefs.GetFloat("MusicVolume", defaultVolume);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        bossMusicPlaying = false;
    }

    public void ResetMusicState()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        bossMusicPlaying = false;
        isMusicPaused = false;

        ShufflePlaylist();
        currentTrack = 0;
        PlayCurrentTrack();

        if (trackNameText != null && audioSource.clip != null)
            trackNameText.text = audioSource.clip.name;

        if (nextTrackButton != null)
            nextTrackButton.gameObject.SetActive(true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (volumeSlider == null)
        {
            Slider foundSlider = FindObjectOfType<Slider>(true);
            if (foundSlider != null)
            {
                volumeSlider = foundSlider;
                float savedVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
                SetupSlider(volumeSlider, savedVolume);
            }
        }

        if (trackNameText == null)
        {
            trackNameText = GameObject.Find("isPlaying")?.GetComponent<Text>();
            if (trackNameText != null && audioSource.clip != null)
                trackNameText.text = audioSource.clip.name;
        }

        if (nextTrackButton == null)
        {
            nextTrackButton = GameObject.Find("NextTrack")?.GetComponent<Button>();
        }
    }
}