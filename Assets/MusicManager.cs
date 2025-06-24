using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicPlaylist;
    private AudioSource audioSource;

    private List<AudioClip> shuffledPlaylist = new List<AudioClip>();
    private int currentTrack = 0;

    private static MusicManager instance;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 0.2f;
    public bool isMusicPaused = false;

    // 🔥 BOSS MUSIC SYSTEM
    [Header("Boss Music Settings")]
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float fadeDuration = 1.5f;
    
    private Coroutine fadeCoroutine;
    private bool bossMusicPlaying = false;
    private bool hasFocus = true;
    [SerializeField] private Text trackNameText;
    void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = defaultVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = defaultVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ShufflePlaylist();
            PlayCurrentTrack();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        // Ak nemáme fokus alebo pauza, nič nerob
        if (!hasFocus || isMusicPaused || bossMusicPlaying) return;

        if (!audioSource.isPlaying)
        {
            NextTrack();
        }
    }

    private void ShufflePlaylist()
    {
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
            audioSource.volume = volume;
    }

    private void NextTrack()
    {
        currentTrack++;

        if (currentTrack >= shuffledPlaylist.Count)
        {
            // Aby sa nezopakovala tá istá pesnička po sebe:
            AudioClip lastClip = shuffledPlaylist[shuffledPlaylist.Count - 1];
            ShufflePlaylist();

            if (shuffledPlaylist[0] == lastClip && shuffledPlaylist.Count > 1)
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


    // ⏬ BOSS MUSIC WITH FADE OUT
    public void PlayBossMusic()
    {
        if (bossMusicPlaying) return;

        bossMusicPlaying = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndPlayBossTrack());
    }

    private IEnumerator FadeOutAndPlayBossTrack()
    {
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

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volumeSlider.value, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volumeSlider.value;
    }

    // ⏫ VOLITEĽNE: Na návrat k playlistu
    public void ResumePlaylist()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndResumePlaylist());
    }

    private IEnumerator FadeOutAndResumePlaylist()
    {
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

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volumeSlider.value, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volumeSlider.value;
        bossMusicPlaying = false;
    }
}
