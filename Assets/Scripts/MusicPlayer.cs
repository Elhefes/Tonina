using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private bool playingPeacefulSongs;
    private bool playingBattleSongs;

    public bool startWithWaitingMusic;

    public AudioLooper audioLooper;
    public AudioSource audioSource;
    public AudioClip firstSong;
    public AudioClip introMusic;
    public AudioClip waitingMusic;
    public AudioClip camazoCaveMusic;
    public List<AudioClip> peacefulSongs;
    public List<AudioClip> battleSongs;
    private List<AudioClip> shuffledPlaylist;
    private int currentIndex;

    private void Start()
    {
        if (startWithWaitingMusic) PlayWaitingMusic();
        else PlayPeacefulSongs(true);
    }

    private void ResetBools()
    {
        playingPeacefulSongs = false;
        playingBattleSongs = false;
    }

    private void Update()
    {
        if (!audioSource.isPlaying && playingPeacefulSongs)
        {
            PlayNextSong();
        }
    }

    public void PlayPeacefulSongs(bool firstPlay)
    {
        if (playingPeacefulSongs) return;
        if (playingBattleSongs)
        {
            audioSource.Stop();
        }

        ResetBools();

        audioSource.loop = false;
        playingPeacefulSongs = true;
        ShufflePlaylist(firstPlay);
        PlayNextSong();
    }

    public void PlayBattleSong(int songNumber)
    {
        ResetBools();
        audioSource.Stop();
        playingBattleSongs = true;
        audioSource.loop = true;
        audioSource.clip = battleSongs[songNumber];
        audioSource.Play();
    }

    private void PlayNextSong()
    {
        if (shuffledPlaylist != null)
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
    }

    private void ShufflePlaylist(bool firstPlay)
    {
        if (peacefulSongs.Count < 1) return;
        shuffledPlaylist = new List<AudioClip>(peacefulSongs);

        if (shuffledPlaylist != null)
        {
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

    public void PlayIntroMusic()
    {
        ResetBools();
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = audioLooper.trimSilence(introMusic);
        audioSource.Play();
    }

    public void PlayWaitingMusic()
    {
        ResetBools();
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = audioLooper.trimSilence(waitingMusic);
        audioSource.Play();
    }

    public void ToggleCamazoCaveMusic(bool value)
    {
        if (value)
        {
            ResetBools();
            audioSource.Stop();
            audioSource.loop = true;
            audioSource.clip = audioLooper.trimSilence(camazoCaveMusic);
            audioSource.Play();
        }
        else if (!playingPeacefulSongs && !playingBattleSongs) PlayPeacefulSongs(false);
    }
}
