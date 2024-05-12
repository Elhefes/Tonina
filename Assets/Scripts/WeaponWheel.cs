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
    private int currentIndex;
    private int weaponIndex;
    private int slices = 5;
    public Weapon[] weapons;

    public AudioSource soundEffectPlayer;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) NextWeapon();
        if (Input.GetKeyDown(KeyCode.Q)) PreviousWeapon();
    }

    public void NextWeapon()
    {
        if (weaponWheelCooldown) return;
        StartWeaponWheelCooldown();
        currentIndex++;
        weaponIndex++;
        if (currentIndex > slices - 1)
        {
            currentIndex = 0;
        }
        if (weaponIndex > weapons.Length - 1)
        {
            weaponIndex = 0;
        }
        var wep = weapons[weaponIndex];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("NextWeapon");
        if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }

    public void PreviousWeapon()
    {
        if (weaponWheelCooldown) return;
        StartWeaponWheelCooldown();
        currentIndex--;
        weaponIndex--;
        if (currentIndex < 0)
        {
            currentIndex = slices - 1;
        }
        if (weaponIndex < 0)
        {
            weaponIndex = weapons.Length - 1;
        }
        var wep = weapons[weaponIndex];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        player.SwitchWeapon(wep.type);
        weaponWheelAnimator.SetTrigger("PreviousWeapon");
        if (wep.switchSound != null) soundEffectPlayer.PlayOneShot(wep.switchSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
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
        var wep = weapons[0];
        weaponSprites[currentIndex].sprite = wep.uiSprite;
        currentIndex = 0;
        weaponIndex = 0;
        player.SwitchWeapon(wep.type);
        // Reset rotation of weapon wheel's circle
        weaponWheelAnimator.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
