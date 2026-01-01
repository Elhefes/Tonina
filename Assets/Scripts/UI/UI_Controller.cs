using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject[] overlappingElements;
    public GameObject[] battleUIPopUps;
    public GameObject optionsMenu;
    public GameObject mainMenuScreen;
    public Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
            if (player.blackFader.activeSelf || player.mouseLook.inCutScene || 
                (player.battlefieldMenu != null && player.battlefieldMenu.waveController.battleWinningScreen.activeInHierarchy) ||
                (player.losingScreen != null && player.losingScreen.gameObject.activeInHierarchy)) return;
            if (mainMenuScreen != null && mainMenuScreen.activeSelf) return;

            if (optionsMenu != null && optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                return;
            }

            if (OverlappingElementActive())
            {
                DisableOverlappingElements();
                return;
            }
            else if (player.textBox != null && player.textBox.gameObject.activeSelf)
            {
                player.FreeTextSubject();
                return;
            }
            if (optionsMenu != null) optionsMenu.SetActive(true);
        }
    }

    public void DisableOverlappingElements()
    {
        foreach (GameObject obj in overlappingElements)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    bool OverlappingElementActive()
    {
        foreach (GameObject obj in overlappingElements)
        {
            if (obj != null && obj.activeSelf) return true;
        }
        return false;
    }
}
