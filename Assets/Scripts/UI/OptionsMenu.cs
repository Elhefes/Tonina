using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioController audioController;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    public GameObject normalMenu;
    public GameObject confirmReturnHomeMenu;

    public GameObject returnHomeButton;
    public GameObject mainMenuButton;

    public bool returnFromBuilder;

    public GameObject battleReturnConfirmText;
    public GameObject builderReturnConfirmText;

    public GameObject returnFromBattleButton;
    public GameObject returnFromBuilderButton;

    public Player player;

    private void Awake()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.2f);
        musicVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeMusicVolume(musicVolumeSlider); });
        soundVolumeSlider.value = PlayerPrefs.GetFloat("soundSliderValue", 0.5f);
        soundVolumeSlider.onValueChanged.AddListener(delegate { audioController.ChangeSoundVolume(soundVolumeSlider); });
    }

    public void SetBuilderMode() { returnFromBuilder = true; }
    public void SetBattleMode() { returnFromBuilder = false; }

    private void OnEnable()
    {
        audioController.LoadAudioSettings();

        if (normalMenu != null) normalMenu.SetActive(true);
        if (confirmReturnHomeMenu != null) confirmReturnHomeMenu.SetActive(false);

        if (player != null)
        {
            if (player.insideKingHouse)
            {
                returnHomeButton.SetActive(false);
                mainMenuButton.SetActive(true);
                return;
            }
        }

        if (mainMenuButton != null) mainMenuButton.SetActive(false);
        if (returnHomeButton != null) returnHomeButton.SetActive(true);

        if (battleReturnConfirmText != null && builderReturnConfirmText != null)
        {
            if (returnFromBuilder)
            {
                builderReturnConfirmText.SetActive(true);
                returnFromBuilderButton.SetActive(true);
                battleReturnConfirmText.SetActive(false);
                returnFromBattleButton.SetActive(false);
            }
            else
            {
                builderReturnConfirmText.SetActive(false);
                returnFromBuilderButton.SetActive(false);
                battleReturnConfirmText.SetActive(true);
                returnFromBattleButton.SetActive(true);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
