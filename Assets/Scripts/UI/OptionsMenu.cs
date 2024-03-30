using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button closeOptionsButton;
    public Button optionsMenuBackround;

    public void CloseOptions()
    {
        gameObject.SetActive(false);
    }
}
