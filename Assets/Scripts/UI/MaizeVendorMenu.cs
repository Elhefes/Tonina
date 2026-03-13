using UnityEngine;

public class MaizeVendorMenu : MonoBehaviour
{
    public MoneyCounter moneyCounter;
    public int maizePlaceCost;
    public int[] maizeEarCosts;

    public GameObject[] textObjects;
    public GameObject maizePlaceBuyMenu;
    public MaizeHandler maizeHandler;

    private void OnEnable()
    {
        foreach (GameObject obj in textObjects) obj.SetActive(false);
        moneyCounter.UpdateMoneyCounter();
        if (!GameState.Instance.progressionData.maizePlaceUnlocked)
        {
            maizePlaceBuyMenu.SetActive(true);
            moneyCounter.UpdateBuyAvailability(maizePlaceCost);
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
        maizePlaceBuyMenu.SetActive(false);

        moneyCounter.UpdateBuyAvailability(maizeEarCosts[m]);
    }

    public void BuyThis()
    {
        if (maizePlaceBuyMenu.activeSelf)
        {
            GameState.Instance.progressionData.maizePlaceUnlocked = true;
            moneyCounter.ReduceMoney(maizePlaceCost);
            GameState.Instance.SaveWorld();
            maizePlaceBuyMenu.SetActive(false);
            textObjects[0].SetActive(true);
        }
        else BuyMoreStartingMaize();
        moneyCounter.UpdateMoneyCounter();
    }

    private void BuyMoreStartingMaize()
    {
        if (GameState.Instance.progressionData.maizeProductionLevel >= textObjects.Length) return;

        GameState.Instance.progressionData.maizeProductionLevel++;
        int newMaizeProductionLevel = GameState.Instance.progressionData.maizeProductionLevel;

        foreach (GameObject obj in textObjects) obj.SetActive(false);

        SetMaizeProductionLevel(newMaizeProductionLevel);
        moneyCounter.ReduceMoney(maizeEarCosts[newMaizeProductionLevel - 1]);

        if (newMaizeProductionLevel < textObjects.Length)
        {
            textObjects[newMaizeProductionLevel].SetActive(true);
            moneyCounter.UpdateBuyAvailability(maizeEarCosts[newMaizeProductionLevel]);
        }

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
