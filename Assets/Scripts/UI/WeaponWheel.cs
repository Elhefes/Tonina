using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    public Button nextWeaponArrowButton;
    public Button nextWeaponButton;
    public Button previousWeaponArrowButton;
    public Button previousWeaponButton;

    public Animator weaponWheelAnimator;

    private bool weaponWheelCooldown;
    public float coolDownTime;

    public Image nextWeaponArrowImage;
    public Image previousWeaponArrowImage;

    public Image currentWeaponImage;
    public Image nextWeaponImage;
    public Image previousWeaponImage;
    public Image[] weaponSprites;
    private string selectedWeaponOrder;
    private int currentIndex;
    private int weaponIndex;
    private int slices = 5;
    public Weapon[] weapons;

    private bool nextWeaponAutoSwitch = true;
    private bool previousWeaponAutoSwitch;

    public AudioSource soundEffectPlayer;

    public Player player;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    private void OnEnable()
    {
        selectedWeaponOrder = PlayerPrefs.GetString("SelectedWeaponOrder", "0");
        ResetToDefaultWeapon();

        // Auto switch is to next weapon by default
        nextWeaponAutoSwitch = true;
        previousWeaponAutoSwitch = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) NextWeapon();
        if (Input.GetKeyDown(KeyCode.Q)) PreviousWeapon();

        // Auto switch to next / previous weapon when it's not available anymore
        if (!player.onCooldown)
        {
            if (player.weaponOnHand.notAvailable)
            {
                if (nextWeaponAutoSwitch) NextWeapon();
                else if (previousWeaponAutoSwitch) PreviousWeapon();
            }
            if (!weaponWheelCooldown)
            {
                // To prevent stuck or unwanted non-fills
                previousWeaponArrowImage.fillAmount = 1f;
                nextWeaponArrowImage.fillAmount = 1f;
            }
        }
        else
        {
            if (nextWeaponArrowImage.fillAmount < 1f)
            {
                // Add normal fill at the rate of cooldown time of the weapon in hand
                previousWeaponArrowImage.fillAmount += 1f / (weapons[selectedWeaponOrder[weaponIndex] - '0'].attackCooldown / Time.deltaTime);
                nextWeaponArrowImage.fillAmount += 1f / (weapons[selectedWeaponOrder[weaponIndex] - '0'].attackCooldown / Time.deltaTime);
            }
            else
            {
                // Arrows become dark when cooldown starts
                previousWeaponArrowImage.fillAmount = 0f;
                nextWeaponArrowImage.fillAmount = 0f;
            }
        }
    }

    public void NextWeapon()
    {
        if (player.onCooldown) return;
        if (weaponWheelCooldown) return;
        if (selectedWeaponOrder.Length < 2) return;

        int initialWeaponIndexValue = weaponIndex;

        weaponIndex++;
        if (weaponIndex > selectedWeaponOrder.Length - 1)
        {
            weaponIndex = 0;
        }

        while (player.weapons[selectedWeaponOrder[weaponIndex] - '0'].notAvailable 
            || !player.weapons[selectedWeaponOrder[weaponIndex] - '0'].selected)
        {
            weaponIndex++;
            if (weaponIndex > selectedWeaponOrder.Length - 1)
            {
                weaponIndex = 0;
            }
        }

        if (weaponIndex == initialWeaponIndexValue) return; // Prevents switching when projectiles have ran out

        currentIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        StartWeaponWheelCooldown();
        nextWeaponArrowImage.fillAmount = 0f;
        var wep = weapons[selectedWeaponOrder[weaponIndex] - '0'];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("NextInWheel");
        if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));

        nextWeaponAutoSwitch = true;
        previousWeaponAutoSwitch = false;
    }

    public void PreviousWeapon()
    {
        if (player.onCooldown) return;
        if (weaponWheelCooldown) return;
        if (selectedWeaponOrder.Length < 2) return;

        int initialWeaponIndexValue = weaponIndex;

        weaponIndex--;
        if (weaponIndex < 0)
        {
            weaponIndex = selectedWeaponOrder.Length - 1;
        }

        while (player.weapons[selectedWeaponOrder[weaponIndex] - '0'].notAvailable 
            || !player.weapons[selectedWeaponOrder[weaponIndex] - '0'].selected)
        {
            weaponIndex--;
            if (weaponIndex < 0)
            {
                weaponIndex = selectedWeaponOrder.Length - 1;
            }
        }

        if (weaponIndex == initialWeaponIndexValue) return; // Prevents switching when projectiles have ran out

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        StartWeaponWheelCooldown();
        previousWeaponArrowImage.fillAmount = 0f;
        var wep = weapons[selectedWeaponOrder[weaponIndex] - '0'];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("PreviousInWheel");
        if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));

        previousWeaponAutoSwitch = true;
        nextWeaponAutoSwitch = false;
    }

    void StartWeaponWheelCooldown()
    {
        Invoke("ResetWeaponWheelCooldown", coolDownTime);
        weaponWheelCooldown = true;
    }

    void ResetWeaponWheelCooldown()
    {
        weaponWheelCooldown = false;
        previousWeaponArrowImage.fillAmount = 1f;
        nextWeaponArrowImage.fillAmount = 1f;
    }

    public void ResetToDefaultWeapon()
    {
        var wep = weapons[selectedWeaponOrder[0] - '0'];
        currentIndex = 0;
        weaponIndex = 0;
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        // Reset rotation of weapon wheel's circle
        weaponWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
