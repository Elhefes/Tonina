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
            if (!mainMenuScreen.activeSelf)
            {
                if (OverlappingElementActive())
                {
                    DisableOverlappingElements();
                    return;
                }
                else if (player.textBox.gameObject.activeSelf)
                {
                    player.FreeTextSubject();
                    return;
                }
                optionsMenu.SetActive(true);
            }
        }
    }

    public void DisableOverlappingElements()
    {
        foreach (GameObject obj in overlappingElements)
        {
            obj.SetActive(false);
        }
    }

    bool OverlappingElementActive()
    {
        foreach (GameObject obj in overlappingElements)
        {
            if (obj.activeSelf) return true;
        }
        return false;
    }
}
