using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OkillBuyMenu : MonoBehaviour
{
    public MoneyCounter moneyCounter;

    public int spearCost;
    public int fillOkillCost;
    public int bowCost;

    public GameObject menuButtonsParent;

    public GameObject spearBuyMenu;
    public GameObject fillOkillBuyMenu;
    public GameObject bowBuyMenu;

    public GameObject closeButtonObject;

    public Image spearButtonImage;
    public Image fillOkillButtonImage;
    public Image bowButtonImage;
    public Sprite isBoughtSprite;

    // Shop progression: "player can buy more stuff as the game progresses"
    public Button bowBuyPageButton;
    public TMP_Text bowBuyPageText;
    public Image bowBuyPageImage;
    public Sprite bowSprite;

    private void OnEnable()
    {
        ReturnToButtons();
    }

    public void ReturnToButtons()
    {
        menuButtonsParent.SetActive(true);
        closeButtonObject.SetActive(true);
        moneyCounter.UpdateMoneyCounter();
        moneyCounter.buyButton.gameObject.SetActive(false);
        FindWhatIsBought();

        spearBuyMenu.SetActive(false);
        fillOkillBuyMenu.SetActive(false);
        bowBuyMenu.SetActive(false);
    }

    private void FindWhatIsBought()
    {
        if (GameState.Instance.progressionData.spearUnlocked)
        {
            spearButtonImage.sprite = isBoughtSprite;
        }
        if (GameState.Instance.progressionData.fillOkillUnlocked)
        {
            fillOkillButtonImage.sprite = isBoughtSprite;
        }
        if (GameState.Instance.progressionData.bowUnlocked)
        {
            bowButtonImage.sprite = isBoughtSprite;
        }

        // Shop progression
        if (GameState.Instance.progressionData.extraPyramidFloorsBuilt >= 2)
        {
            bowBuyPageImage.sprite = bowSprite;
            bowBuyPageText.text = "Bow";
            bowBuyPageButton.interactable = true;
        }
    }

    public void OpenSpearBuyMenu()
    {
        spearBuyMenu.SetActive(true);
        if (GameState.Instance.progressionData.spearUnlocked || moneyCounter.statsController.availableMoney < spearCost)
        {
            moneyCounter.buyButton.interactable = false;
        }
        else moneyCounter.buyButton.interactable = true;
        moneyCounter.buyButton.gameObject.SetActive(true);
    }

    public void OpenFillOkillBuyMenu()
    {
        fillOkillBuyMenu.SetActive(true);
        if (GameState.Instance.progressionData.fillOkillUnlocked || moneyCounter.statsController.availableMoney < fillOkillCost)
        {
            moneyCounter.buyButton.interactable = false;
        }
        else moneyCounter.buyButton.interactable = true;
        moneyCounter.buyButton.gameObject.SetActive(true);
    }

    public void OpenBowBuyMenu()
    {
        bowBuyMenu.SetActive(true);
        if (GameState.Instance.progressionData.bowUnlocked || moneyCounter.statsController.availableMoney < bowCost)
        {
            moneyCounter.buyButton.interactable = false;
        }
        else moneyCounter.buyButton.interactable = true;
        moneyCounter.buyButton.gameObject.SetActive(true);
    }

    public void BuyThis()
    {
        if (spearBuyMenu.activeSelf)
        {
            GameState.Instance.progressionData.spearUnlocked = true;
            moneyCounter.ReduceMoney(spearCost);
            GameState.Instance.SaveWorld();
            ReturnToButtons();
        }
        else if (fillOkillBuyMenu.activeSelf)
        {
            GameState.Instance.progressionData.fillOkillUnlocked = true;
            moneyCounter.ReduceMoney(fillOkillCost);
            GameState.Instance.SaveWorld();
            ReturnToButtons();
        }
        else if (bowBuyMenu.activeSelf)
        {
            GameState.Instance.progressionData.bowUnlocked = true;
            moneyCounter.ReduceMoney(bowCost);
            GameState.Instance.SaveWorld();
            ReturnToButtons();
        }
    }
}
