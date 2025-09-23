using UnityEngine;

public class Intro : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    private bool fadingMusicDown;
    private float musicVolumeAtStart;
    private float musicFadeIncrement;

    private void OnEnable()
    {
        musicPlayer.PlayIntroMusic();
        musicVolumeAtStart = musicPlayer.audioSource.volume;
    }

    public void StartLoadingIntroScene()
    {
        SceneChangingManager.Instance.LoadScene("IntroScene");
    }

    public void StartFadingMusicDown()
    {
        musicFadeIncrement = musicVolumeAtStart / 250f; // 2.5 Seconds in FixedUpdate
        fadingMusicDown = true;
    }

    private void FixedUpdate()
    {
        if (fadingMusicDown)
        {
            musicPlayer.audioSource.volume -= musicFadeIncrement;
        }
    }
}
