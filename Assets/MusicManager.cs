using UnityEngine;
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
            audioSource = GetComponent<AudioSource>();

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
        if (!audioSource.isPlaying && !isMusicPaused)
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
}
