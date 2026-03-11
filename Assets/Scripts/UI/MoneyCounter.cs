using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    public StatsController statsController;
    public TMP_Text moneyTMP;

    private void OnEnable()
    {
        UpdateMoneyCounter();
    }

    public void UpdateMoneyCounter()
    {
        moneyTMP.text = "Money: " + statsController.availableMoney.ToString();
    }
}
