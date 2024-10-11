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

    public Image currentWeaponImage;
    public Image nextWeaponImage;
    public Image previousWeaponImage;
    public Image[] weaponSprites;
    private string weaponOrder;
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
        weaponOrder = PlayerPrefs.GetString("CustomWeaponOrder", "0123");
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
        if (player.weaponOnHand.notAvailable && !player.onCooldown)
        {
            if (nextWeaponAutoSwitch) NextWeapon();
            else if (previousWeaponAutoSwitch) PreviousWeapon();
        }
    }

    public void NextWeapon()
    {
        if (weaponWheelCooldown) return;
        weaponIndex++;
        if (weaponIndex > weapons.Length - 1)
        {
            weaponIndex = 0;
        }

        while (player.weapons[weaponOrder[weaponIndex] - '0'].notAvailable)
        {
            weaponIndex++;
            if (weaponIndex > weapons.Length - 1)
            {
                weaponIndex = 0;
            }
        }

        currentIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        StartWeaponWheelCooldown();
        var wep = weapons[weaponOrder[weaponIndex] - '0'];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("NextInWheel");
        if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));

        nextWeaponAutoSwitch = true;
        previousWeaponAutoSwitch = false;
    }

    public void PreviousWeapon()
    {
        if (weaponWheelCooldown) return;
        weaponIndex--;
        if (weaponIndex < 0)
        {
            weaponIndex = weapons.Length - 1;
        }

        while (player.weapons[weaponOrder[weaponIndex] - '0'].notAvailable)
        {
            weaponIndex--;
            if (weaponIndex < 0)
            {
                weaponIndex = weapons.Length - 1;
            }
        }

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        StartWeaponWheelCooldown();
        var wep = weapons[weaponOrder[weaponIndex] - '0'];
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
    }

    public void ResetToDefaultWeapon()
    {
        var wep = weapons[weaponOrder[0] - '0'];
        currentIndex = 0;
        weaponIndex = 0;
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        // Reset rotation of weapon wheel's circle
        weaponWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
