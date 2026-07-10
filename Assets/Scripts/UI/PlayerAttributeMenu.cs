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
        availablePoints = 12;
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
        if (AttributeIsAvailable(attribute)) selectedAttribute = attribute;
        else selectedAttribute = "";
        UpdateAqcuireButton();
    }

    public void AquireThisAttribute()
    {
        int attributeNumber = int.Parse(selectedAttribute[1].ToString());

        if (selectedAttribute[0] == 'V') // Vitality
        {
            vitalityImages[attributeNumber - 1].sprite = acquiredSprite;
            GameState.Instance.progressionData.vitalityLevel++;
        }
        else if (selectedAttribute[0] == 'A') // Agility
        {
            agilityImages[attributeNumber - 1].sprite = acquiredSprite;
            GameState.Instance.progressionData.agilityLevel++;
        }
        else if (selectedAttribute[0] == 'W') // Weapons
        {
            weaponsImages[attributeNumber - 1].sprite = acquiredSprite;
            GameState.Instance.progressionData.weaponsLevel++;
        }
        else if (selectedAttribute[0] == 'E') // Efficiency
        {
            efficiencyImages[attributeNumber - 1].sprite = acquiredSprite;
            GameState.Instance.progressionData.efficiencyLevel++;
        }
        else if (selectedAttribute[0] == 'L') // Leadership
        {
            leadershipImages[attributeNumber - 1].sprite = acquiredSprite;
            GameState.Instance.progressionData.leadershipLevel++;
        }

        if (availablePoints > 0) availablePoints--;
        selectedAttribute = "";
        availablePointsText.text = "Available Points: " + availablePoints.ToString();
        UpdateAqcuireButton();
        DisableAllInfoBubbles();
        //GameState.Instance.SaveWorld();
    }

    private void UpdateAttributeImages()
    {
        // TODO: sets all images to acquired or not acquired based on saved data
    }

    private bool AttributeIsAvailable(string attribute)
    {
        int attributeNumber = int.Parse(attribute[1].ToString());

        if (attribute[0] == 'V')
        {
            if (attributeNumber - 1 == GameState.Instance.progressionData.vitalityLevel) return true;
        }
        else if (attribute[0] == 'A')
        {
            if (attributeNumber - 1 == GameState.Instance.progressionData.agilityLevel) return true;
        }
        else if (attribute[0] == 'W')
        {
            if (attributeNumber - 1 == GameState.Instance.progressionData.weaponsLevel) return true;
        }
        else if (attribute[0] == 'E')
        {
            if (attributeNumber - 1 == GameState.Instance.progressionData.efficiencyLevel) return true;
        }
        else if (attribute[0] == 'L')
        {
            if (attributeNumber - 1 == GameState.Instance.progressionData.leadershipLevel) return true;
        }

        return false;
    }
}
