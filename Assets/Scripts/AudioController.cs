using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource musicController;
    public AudioSource soundController;
    public Button musicToggleButton;
    public Sprite musicIcon;
    public Sprite muteMusicIcon;
    private bool musicOn = true;

    private void Awake()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicController.volume = savedVolume;

        int musicMuted = PlayerPrefs.GetInt("musicMuted", 0);
        if (musicMuted == 1)
        {
            SetMusic(false);
        }
        else
        {
            SetMusic(true);
        }
    }

    public void SetMusic(bool value)
    {
        musicOn = value;
        if (musicOn)
        {
            musicToggleButton.image.sprite = musicIcon;
            PlayerPrefs.SetInt("musicMuted", 0);
            float volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
            musicController.volume = volume;
        }
        else
        {
            PlayerPrefs.SetInt("musicMuted", 1);
            musicToggleButton.image.sprite = muteMusicIcon;
            musicController.volume = 0;
        }
    }

    public void ToggleMusic()
    {
        SetMusic(!musicOn);
    }

    public void ChangeMusicVolume(Slider musicVolumeSlider)
    {
        SetMusic(true);
        PlayerPrefs.SetInt("musicMuted", 0);
        float volume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicController.volume = volume;
    }
}
