using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public MusicEnvironment environment;
    public enum MusicEnvironment
    {
        Peaceful,
        Battle
    }

    public AudioSource audioSource;
    public AudioClip firstSong;
    public List<AudioClip> peacefulSongs;
    public List<AudioClip> battleSongs;
    private List<AudioClip> shuffledPlaylist;
    private int currentIndex;

    private void Start()
    {
        if (environment == MusicEnvironment.Peaceful) PlayPeacefulSongs(true);
        if (environment == MusicEnvironment.Battle) PlayBattleSong(0);
    }

    private void Update()
    {
        if (!audioSource.isPlaying && environment == MusicEnvironment.Peaceful)
        {
            PlayNextSong();
        }
    }

    public void PlayPeacefulSongs(bool firstPlay)
    {
        if (environment == MusicEnvironment.Battle)
        {
            StopBattleSong();
        }

        environment = MusicEnvironment.Peaceful;
        ShufflePlaylist(firstPlay);
        PlayNextSong();
    }

    public void PlayBattleSong(int songNumber)
    {
        if (environment == MusicEnvironment.Peaceful)
        {
            StopBattleSong();
        }

        environment = MusicEnvironment.Battle;
        audioSource.loop = true;
        audioSource.clip = battleSongs[songNumber];
        audioSource.Play();
    }

    private void PlayNextSong()
    {
        audioSource.clip = shuffledPlaylist[currentIndex];
        audioSource.Play();
        if ((currentIndex + 1) >= shuffledPlaylist.Count)
        {
            ShufflePlaylist(false);
            return;
        }
        currentIndex = (currentIndex + 1) % shuffledPlaylist.Count;
    }

    private void StopBattleSong()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }

    private void ShufflePlaylist(bool firstPlay)
    {
        shuffledPlaylist = new List<AudioClip>(peacefulSongs);
        // Create a random number generator
        System.Random rng = new System.Random();

        // Get the count of elements in the list
        int n = shuffledPlaylist.Count;

        // Iterate through the list
        while (n > 1)
        {
            n--;
            // Get a random index within the range of the list
            int k = rng.Next(n + 1);

            // Swap the elements at indices n and k
            AudioClip temp = shuffledPlaylist[k];
            shuffledPlaylist[k] = shuffledPlaylist[n];
            shuffledPlaylist[n] = temp;
        }

        // The newly shuffled list's first song won't be the previous list's last song
        if (shuffledPlaylist[0] == audioSource.clip) shuffledPlaylist.Reverse();

        // Play main theme song as the first song
        if (firstPlay)
        {
            if (shuffledPlaylist.Contains(firstSong))
            {
                shuffledPlaylist.Remove(firstSong);
            }
            shuffledPlaylist.Insert(0, firstSong);
        }

        currentIndex = 0;
    }
}
