using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    public Button buyButton;
    public StatsController statsController;
    public TMP_Text moneyTMP;

    private void OnEnable()
    {
        UpdateMoneyCounter();
    }

    public void UpdateBuyAvailability(int cost)
    {
        // Check if there is enough money
        if (cost < statsController.availableMoney)
        {
            buyButton.interactable = true;
        }
        else buyButton.interactable = false;
    }

    public void ReduceMoney(int cost)
    {
        statsController.availableMoney -= cost;
        statsController.SaveStats();
        UpdateMoneyCounter();
    }

    public void UpdateMoneyCounter()
    {
        if (moneyTMP != null) moneyTMP.text = "Money: " + statsController.availableMoney.ToString();
    }
}
