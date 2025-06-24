using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MusicManager : MonoBehaviour

{
    public static MusicManager Instance => instance;

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
    [SerializeField] private Button nextTrackButton;




    void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // Znič duplicitu!
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = defaultVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = defaultVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        ShufflePlaylist();
        PlayCurrentTrack();

        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (nextTrackButton != null)
            nextTrackButton.gameObject.SetActive(true);


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
    public void ResetMusicState()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        bossMusicPlaying = false;
        isMusicPaused = false;

        ShufflePlaylist();
        currentTrack = 0;
        PlayCurrentTrack();

        // 👇 nastav aj meno pesničky naspäť
        if (trackNameText != null)
            trackNameText.text = audioSource.clip.name;

        // a znovu aktivuj next track button ak si ho mal skrytý pri bossovi
        if (nextTrackButton != null)
            nextTrackButton.gameObject.SetActive(true);

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Znova nájdeme referencie, ktoré sa stratili po restartnutí scény
        if (trackNameText == null)
        {
            trackNameText = GameObject.Find("isPlaying")?.GetComponent<Text>();
            if (trackNameText != null)
                trackNameText.text = audioSource.clip != null ? audioSource.clip.name : "";
        }

        if (nextTrackButton == null)
        {
            GameObject btnObj = GameObject.Find("NextTrack");
            if (btnObj != null)
                nextTrackButton = btnObj.GetComponent<Button>();
        }
    }


}