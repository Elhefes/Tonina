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
        if (selectedAttribute == "") acquireButton.interactable = false;
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

        }
        else if (selectedAttribute == "V2")
        {

        }
        else if (selectedAttribute == "V3")
        {

        }
        else if (selectedAttribute == "V4")
        {

        }
        else if (selectedAttribute == "V5")
        {

        }
        else if (selectedAttribute == "A1") // Agility
        {

        }
        else if (selectedAttribute == "A2")
        {

        }
        else if (selectedAttribute == "A3")
        {

        }
        else if (selectedAttribute == "A4")
        {

        }
        else if (selectedAttribute == "A5")
        {

        }
        else if (selectedAttribute == "W1") // Weapons
        {

        }
        else if (selectedAttribute == "W2")
        {

        }
        else if (selectedAttribute == "W3")
        {

        }
        else if (selectedAttribute == "W4")
        {

        }
        else if (selectedAttribute == "E1") // Efficiency
        {

        }
        else if (selectedAttribute == "E2")
        {

        }
        else if (selectedAttribute == "E3")
        {

        }
        else if (selectedAttribute == "L1") // Leadership
        {

        }
        else if (selectedAttribute == "L2")
        {

        }
        else if (selectedAttribute == "L3")
        {

        }

        selectedAttribute = "";
        UpdateAqcuireButton();
        DisableAllInfoBubbles();
    }
}
