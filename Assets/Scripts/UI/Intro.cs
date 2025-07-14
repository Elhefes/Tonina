using UnityEngine;

public class Intro : MonoBehaviour
{
    public MusicPlayer musicPlayer;

    private void OnEnable()
    {
        musicPlayer.PlayIntroMusic();
    }

    public void EndIntro()
    {
        SceneChangingManager.Instance.LoadScene("IntroScene");
    }
}
