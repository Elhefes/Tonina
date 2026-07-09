using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeMenu : MonoBehaviour
{
    public Button acquireButton;
    public GameObject[] infoBubbles;
    public TMP_Text availablePointsText;
    private int availablePoints;
    private string selectedAttribute;

    public Sprite acquiredSprite;
    public Sprite notAcquiredSprite;

    [Header("Vitality 1-5")]
    public Image[] vitalityImages;

    [Header("Agility 1-5")]
    public Image[] agilityImages;

    [Header("Weapons 1-4")]
    public Image[] weaponsImages;

    [Header("Efficiency 1-3")]
    public Image[] efficiencyImages;

    [Header("Leadership 1-3")]
    public Image[] leadershipImages;

    private void OnEnable()
    {
        DisableAllInfoBubbles();
        availablePointsText.text = "Available Points: " + availablePoints.ToString();
        selectedAttribute = "";
        UpdateAqcuireButton();
    }

    private void UpdateAqcuireButton()
    {
        if (selectedAttribute == "" || availablePoints < 1) acquireButton.interactable = false;
        else if (availablePoints > 0) acquireButton.interactable = true;
    }

    public void DisableAllInfoBubbles()
    {
        foreach (GameObject obj in infoBubbles) obj.SetActive(false);
    }

    public void ToggleThisInfoBubble(GameObject infoBubble)
    {
        bool isEnabled = infoBubble.activeSelf;
        DisableAllInfoBubbles();
        infoBubble.SetActive(!isEnabled);
    }

    public void SelectThisAttribute(string attribute)
    {
        selectedAttribute = attribute;
        UpdateAqcuireButton();
    }

    public void AquireThisAttribute()
    {
        if (selectedAttribute == "V1") // Vitality
        {
            vitalityImages[0].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "V2")
        {
            vitalityImages[1].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "V3")
        {
            vitalityImages[2].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "V4")
        {
            vitalityImages[3].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "V5")
        {
            vitalityImages[4].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "A1") // Agility
        {
            agilityImages[0].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "A2")
        {
            agilityImages[1].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "A3")
        {
            agilityImages[2].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "A4")
        {
            agilityImages[3].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "A5")
        {
            agilityImages[4].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "W1") // Weapons
        {
            weaponsImages[0].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "W2")
        {
            weaponsImages[1].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "W3")
        {
            weaponsImages[2].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "W4")
        {
            weaponsImages[3].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "E1") // Efficiency
        {
            efficiencyImages[0].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "E2")
        {
            efficiencyImages[1].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "E3")
        {
            efficiencyImages[2].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "L1") // Leadership
        {
            leadershipImages[0].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "L2")
        {
            leadershipImages[1].sprite = acquiredSprite;
        }
        else if (selectedAttribute == "L3")
        {
            leadershipImages[2].sprite = acquiredSprite;
        }

        if (availablePoints > 0) availablePoints--;
        selectedAttribute = "";
        availablePointsText.text = "Available Points: " + availablePoints.ToString();
        UpdateAqcuireButton();
        DisableAllInfoBubbles();
    }

    private void UpdateAttributeImages()
    {
        // TODO: sets all images to acquired or not acquired based on saved data
    }
}
