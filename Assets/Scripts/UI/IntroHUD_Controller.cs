using UnityEngine;

public class IntroHUD_Controller : MonoBehaviour
{
    public GameObject battleUI;
    public GameObject optionsMenu;

    public void EnableBattleModeHUD(bool value)
    {
        battleUI.SetActive(value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (battleUI.activeSelf)
            {
                if (!optionsMenu.activeSelf) optionsMenu.SetActive(true);
                else optionsMenu.SetActive(false);
            }
        }
    }
}
