using UnityEngine;
using TMPro;

public class CamazoSummoning : MonoBehaviour
{
    public MoneyCounter moneyCounter;
    public TMP_Text summoningText;
    public int summoningFirstCost;
    public int summoningSecondCost;
    public int summoningThirdCost;
    private int currentCost;

    public CamazoCave camazoCave;

    private void OnEnable()
    {
        if (camazoCave.camazoIsSummoned)
        {
            gameObject.SetActive(false);
            return;
        }

        currentCost = summoningFirstCost;
        summoningText.text = "Give " + currentCost + " gold to summon Camazo?";

        moneyCounter.UpdateMoneyCounter();
        moneyCounter.UpdateBuyAvailability(currentCost);
    }

    public void SummonCamazo()
    {
        if (!camazoCave.camazoIsSummoned) camazoCave.SummonCamazo();
        gameObject.SetActive(false);
    }
}
