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
    public Weapon[] buildings;

    public AudioSource soundEffectPlayer;

    public Player player;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

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
        if (buildingIndex > buildings.Length - 1)
        {
            buildingIndex = 0;
        }
        var wep = buildings[buildingIndex];
        buildingSprites[currentIndex].sprite = wep.uiSprite;
        //player.SwitchWeapon(wep.type);
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
            buildingIndex = buildings.Length - 1;
        }
        var wep = buildings[buildingIndex];
        buildingSprites[currentIndex].sprite = wep.uiSprite;
        //player.SwitchWeapon(wep.type);
        buildingWheelAnimator.SetTrigger("PreviousInWheel");
        //if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
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
        var building = buildings[0];
        currentIndex = 0;
        buildingIndex = 0;
        buildingSprites[currentIndex].sprite = building.uiSprite;
        //player.SwitchWeapon(wep.type);
        // Reset rotation of building wheel's circle
        buildingWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
