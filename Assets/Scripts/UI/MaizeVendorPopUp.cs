using UnityEngine;

public class MaizeVendorPopUp : MonoBehaviour
{
    public GameObject[] textObjects;
    public GameObject maizePlaceBuyMenu;
    public GameObject buyMoreMaizeButton;
    public MaizeHandler maizeHandler;

    private void OnEnable()
    {
        foreach (GameObject obj in textObjects) obj.SetActive(false);
        if (!GameState.Instance.progressionData.maizePlaceUnlocked)
        {
            EnableBuyMaizePlaceMenu(true);
            return;
        }
        int m = GameState.Instance.progressionData.maizeProductionLevel;
        if (m > 0)
        {
            if (m < textObjects.Length) textObjects[m].SetActive(true);
        }
        else
            {
                textObjects[0].SetActive(true);
            }
        EnableBuyMaizePlaceMenu(false);
    }

    private void EnableBuyMaizePlaceMenu(bool value)
    {
        maizePlaceBuyMenu.SetActive(value);
        buyMoreMaizeButton.SetActive(!value);
    }

    public void BuyMaizePlace()
    {
        GameState.Instance.progressionData.maizePlaceUnlocked = true;
        GameState.Instance.SaveWorld();
        EnableBuyMaizePlaceMenu(false);
        textObjects[0].SetActive(true);
    }

    public void BuyMoreStartingMaize()
    {
        if (GameState.Instance.progressionData.maizeProductionLevel >= textObjects.Length) return;

        GameState.Instance.progressionData.maizeProductionLevel++;
        int newMaizeProductionLevel = GameState.Instance.progressionData.maizeProductionLevel;

        foreach (GameObject obj in textObjects) obj.SetActive(false);
        if (newMaizeProductionLevel < textObjects.Length) textObjects[newMaizeProductionLevel].SetActive(true);

        SetMaizeProductionLevel(newMaizeProductionLevel);
        GameState.Instance.SaveWorld();
    }

    private void SetMaizeProductionLevel(int newMaizeProductionLevel)
    {
        // TODO: Make Maize Place incrementally cheaper also
        if (newMaizeProductionLevel == 3)
        {
            // MP Price = 120
        }
        else if (newMaizeProductionLevel == 4)
        {
            // MP Price = 100
        }
    }
}
