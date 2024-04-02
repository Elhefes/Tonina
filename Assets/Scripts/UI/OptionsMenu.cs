using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioController audioController;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    private void Awake()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeMusicVolume(musicVolumeSlider); });
        soundVolumeSlider.value = PlayerPrefs.GetFloat("soundSliderValue", 0.5f);
        soundVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeSoundVolume(soundVolumeSlider); });
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
