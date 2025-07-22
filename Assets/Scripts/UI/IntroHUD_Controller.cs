using UnityEngine;

public class IntroHUD_Controller : MonoBehaviour
{
    public GameObject battleUI;
    public GameObject[] presentationStartObjects;
    public GameObject pickUpSpearTextBox;
    public GameObject equipSpearTextBoxSwitchedVersion;
    public GameObject equipSpearTextBoxNotSwitchedVersion;
    public GameObject popUpTexts;
    public GameObject endTexts;
    public OptionsMenu optionsMenu;
    public bool presenting;

    private void Start()
    {
        optionsMenu.LoadAudioSettings();
    }

    public void EnableBattleModeHUD(bool value)
    {
        battleUI.SetActive(value);
        presentationStartObjects[0].SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneChangingManager.Instance.LoadScene("Tonina");
    }

    public void SetPresenting(bool value) { presenting = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !presenting)
        {
            if (battleUI.activeSelf)
            {
                if (!optionsMenu.gameObject.activeSelf) optionsMenu.gameObject.SetActive(true);
                else optionsMenu.gameObject.SetActive(false);
            }
        }
    }
}
