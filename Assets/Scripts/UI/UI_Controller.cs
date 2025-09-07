using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject[] overlappingElements;
    public GameObject optionsMenu;
    public GameObject mainMenuScreen;
    public Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
            if (player.blackFader.activeSelf) return;
            if (mainMenuScreen != null && mainMenuScreen.activeSelf) return;

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
            optionsMenu.SetActive(true);
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
