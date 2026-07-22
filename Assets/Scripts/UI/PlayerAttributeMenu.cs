using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeMenu : MonoBehaviour
{
    public Player player;
    public AttributeUnlockScene attributeUnlockScene;
    public Button acquireButton;
    public GameObject[] infoBubbles;
    public TMP_Text availablePointsText;
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
        availablePointsText.text = "Available Points: " + GameState.Instance.progressionData.availableAttributePoints.ToString();
        selectedAttribute = "";
        UpdateAttributeImages();
        UpdateAqcuireButton();
    }

    private void OnDisable()
    {
        if (attributeUnlockScene.gameObject.activeSelf)
        {
            attributeUnlockScene.ExitScene();
        }
    }

    private void UpdateAqcuireButton()
    {
        if (selectedAttribute == "" || GameState.Instance.progressionData.availableAttributePoints < 1) acquireButton.interactable = false;
        else if (GameState.Instance.progressionData.availableAttributePoints > 0) acquireButton.interactable = true;
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
            if (player != null) player.UpdateAgilityValues();
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

        if (GameState.Instance.progressionData.availableAttributePoints > 0) GameState.Instance.progressionData.availableAttributePoints--;
        selectedAttribute = "";
        availablePointsText.text = "Available Points: " + GameState.Instance.progressionData.availableAttributePoints.ToString();
        UpdateAqcuireButton();
        DisableAllInfoBubbles();

        if (player != null) // Update torches
        {
            if (player.kingHouse != null) player.kingHouse.UpdateNamePlateTorchFlames();
        }

        GameState.Instance.SaveWorld();
    }

    private void UpdateAttributeImages()
    {
        for (int i = 1; i <= GameState.Instance.progressionData.vitalityLevel; i++) { vitalityImages[i - 1].sprite = acquiredSprite; }
        for (int i = 1; i <= GameState.Instance.progressionData.agilityLevel; i++) { agilityImages[i - 1].sprite = acquiredSprite; }
        for (int i = 1; i <= GameState.Instance.progressionData.weaponsLevel; i++) { weaponsImages[i - 1].sprite = acquiredSprite; }
        for (int i = 1; i <= GameState.Instance.progressionData.efficiencyLevel; i++) { efficiencyImages[i - 1].sprite = acquiredSprite; }
        for (int i = 1; i <= GameState.Instance.progressionData.leadershipLevel; i++) { leadershipImages[i - 1].sprite = acquiredSprite; }
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
