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

    private void OnEnable()
    {
        ReturnToButtons();
    }

    public void ReturnToButtons()
    {
        closeButtonObject.SetActive(true);
        buttonsParent.SetActive(true);
        foreach (GameObject obj in buyPageObjects) obj.SetActive(false);
        FindWhichAreBought();
    }

    private void FindWhichAreBought()
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
