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

    private bool hasSwitched = false;

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
}
