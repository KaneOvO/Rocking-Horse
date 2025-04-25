using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource mainTrackMusicSource;

    [Header("Tracks")]
    public AudioClip mainTrack;
    public AudioClip finalLapTrack;
    public AudioClip postRaceTrack;
    public AudioClip lap1Audio;
    public AudioClip lap2Audio;
    public AudioClip pickItemAudio;
    public AudioClip boostAudio;
    public AudioClip horseFallAudio;
    public AudioClip horseHitAudio;
    public AudioClip lassoAudio;
    public AudioClip carrotRocketAudio;

    private bool hasSwitched = false;
    private bool hasPlayedLap1 = false;
    private bool hasPlayedLap2 = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayMainTrack()
    {
        if (mainTrackMusicSource == null || mainTrack == null) return;

        mainTrackMusicSource.clip = mainTrack;
        mainTrackMusicSource.loop = true;
        mainTrackMusicSource.Play();

        Debug.Log("Main track started.");
    }

    public void SwitchToFinalLapMusic()
    {
        if (hasSwitched || finalLapTrack == null || mainTrackMusicSource == null)
            return;

        Debug.Log("Switching to final lap music!");

        mainTrackMusicSource.Stop();
        mainTrackMusicSource.clip = finalLapTrack;
        mainTrackMusicSource.Play();

        hasSwitched = true;
    }

    public void PlayPostRaceTrack()
    {
        if (mainTrackMusicSource == null || postRaceTrack == null) return;

        Debug.Log("Switching to post-race music!");

        mainTrackMusicSource.Stop();
        mainTrackMusicSource.clip = postRaceTrack;
        mainTrackMusicSource.Play();
    }

    public void PlayLap1Audio()
    {
        if (!hasPlayedLap1 && lap1Audio != null)
        {
            mainTrackMusicSource.PlayOneShot(lap1Audio);
            hasPlayedLap1 = true;
        }
    }

    public void PlayLap2Audio()
    {
        if (!hasPlayedLap2 && lap2Audio != null)
        {
            mainTrackMusicSource.PlayOneShot(lap2Audio);
            hasPlayedLap2 = true;
        }
    }

    public void ResetMusicTriggers()
    {
        hasSwitched = false;
        hasPlayedLap1 = false;
        hasPlayedLap2 = false;
    }
}
