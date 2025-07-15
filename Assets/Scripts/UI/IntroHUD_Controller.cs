using UnityEngine;

public class IntroHUD_Controller : MonoBehaviour
{
    public GameObject battleUI;
    public OptionsMenu optionsMenu;

    private void Start()
    {
        optionsMenu.LoadAudioSettings();
    }

    public void EnableBattleModeHUD(bool value)
    {
        battleUI.SetActive(value);
    }

    public void ReturnToMainMenu()
    {
        SceneChangingManager.Instance.LoadScene("Tonina");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (battleUI.activeSelf)
            {
                if (!optionsMenu.gameObject.activeSelf) optionsMenu.gameObject.SetActive(true);
                else optionsMenu.gameObject.SetActive(false);
            }
        }
    }
}
