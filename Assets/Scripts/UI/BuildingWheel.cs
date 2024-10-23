using UnityEngine;
using UnityEngine.UI;

public class BuildingWheel : MonoBehaviour
{
    public Button nextBuildingArrowButton;
    public Button nextBuildingButton;
    public Button previousBuildingArrowButton;
    public Button previousBuildingButton;

    public Animator buildingWheelAnimator;

    private bool buildingWheelCooldown;
    public float coolDownTime;

    public Image currentBuildingImage;
    public Image nextBuildingImage;
    public Image previousBuildingImage;
    public Image[] buildingSprites;
    private int currentIndex;
    private int buildingIndex;
    private int slices = 5;
    public BuildingPlacing[] placeableBuildingsOnPlayer;

    public AudioSource soundEffectPlayer;

    private void OnEnable() { ResetToDefaultBuilding(); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) NextBuilding();
        if (Input.GetKeyDown(KeyCode.Q)) PreviousBuilding();
    }

    public void NextBuilding()
    {
        if (buildingWheelCooldown) return;
        StartBuildingWheelCooldown();
        currentIndex++;
        buildingIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        if (buildingIndex > placeableBuildingsOnPlayer.Length - 1)
        {
            buildingIndex = 0;
        }
        var building = placeableBuildingsOnPlayer[buildingIndex];
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        buildingWheelAnimator.SetTrigger("NextInWheel");
        //if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    public void PreviousBuilding()
    {
        if (buildingWheelCooldown) return;
        StartBuildingWheelCooldown();
        currentIndex--;
        buildingIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        if (buildingIndex < 0)
        {
            buildingIndex = placeableBuildingsOnPlayer.Length - 1;
        }
        var building = placeableBuildingsOnPlayer[buildingIndex];
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        buildingWheelAnimator.SetTrigger("PreviousInWheel");
        //if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    void SwitchToBuilding(int buildingIndex)
    {
        if (this == null) return;
        foreach (BuildingPlacing building in placeableBuildingsOnPlayer)
        {
            building.gameObject.SetActive(false);
        }
        placeableBuildingsOnPlayer[buildingIndex].gameObject.SetActive(true);
    }

    void StartBuildingWheelCooldown()
    {
        Invoke("ResetBuildingWheelCooldown", coolDownTime);
        buildingWheelCooldown = true;
    }

    void ResetBuildingWheelCooldown()
    {
        buildingWheelCooldown = false;
    }

    public void ResetToDefaultBuilding()
    {
        var building = placeableBuildingsOnPlayer[0];
        currentIndex = 0;
        buildingIndex = 0;
        buildingSprites[currentIndex].sprite = building.uiSprite;
        SwitchToBuilding(buildingIndex);
        // Reset rotation of building wheel's circle
        buildingWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
