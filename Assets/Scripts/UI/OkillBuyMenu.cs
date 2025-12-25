using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OkillBuyMenu : MonoBehaviour
{
    public GameObject closeButtonObject;
    public GameObject buttonsParent;
    public GameObject[] buyPageObjects;

    public Image spearButtonImage;
    public Image fillOkillButtonImage;
    public Image bowButtonImage;
    public Sprite isBoughtSprite;

    public Button spearBuyButton;
    public Button fillOkillBuyButton;
    public Button bowBuyButton;

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
        closeButtonObject.SetActive(true);
        buttonsParent.SetActive(true);
        foreach (GameObject obj in buyPageObjects) obj.SetActive(false);
        FindWhatIsBought();
    }

    private void FindWhatIsBought()
    {
        if (GameState.Instance.progressionData.spearUnlocked)
        {
            spearButtonImage.sprite = isBoughtSprite;
            spearBuyButton.interactable = false;
        }
        if (GameState.Instance.progressionData.fillOkillUnlocked)
        {
            fillOkillButtonImage.sprite = isBoughtSprite;
            fillOkillBuyButton.interactable = false;
        }
        if (GameState.Instance.progressionData.bowUnlocked)
        {
            bowButtonImage.sprite = isBoughtSprite;
            bowBuyButton.interactable = false;
        }

        // Shop progression
        if (GameState.Instance.progressionData.extraPyramidFloorsBuilt >= 2)
        {
            bowBuyPageImage.sprite = bowSprite;
            bowBuyPageText.text = "Bow";
            bowBuyPageButton.interactable = true;
        }
    }

    public void BuySpear()
    {
        GameState.Instance.progressionData.spearUnlocked = true;
        GameState.Instance.SaveWorld();
        ReturnToButtons();
    }

    public void BuyFillOkill()
    {
        GameState.Instance.progressionData.fillOkillUnlocked = true;
        GameState.Instance.SaveWorld();
        ReturnToButtons();
    }

    public void BuyBow()
    {
        GameState.Instance.progressionData.bowUnlocked = true;
        GameState.Instance.SaveWorld();
        ReturnToButtons();
    }
}
