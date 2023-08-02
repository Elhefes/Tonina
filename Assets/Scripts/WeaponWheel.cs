using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
    public Button nextWeaponArrowButton;
    public Button nextWeaponButton;
    public Button previousWeaponArrowButton;
    public Button previousWeaponButton;

    public Animator weaponWheelAnimator;

    public Image currentWeaponImage;
    public Image nextWeaponImage;
    public Image previousWeaponImage;
    public Sprite[] weaponSprites;
    private int currentWeaponInt = 0;

    void Start()
    {

    }

    void Awake()
    {
        nextWeaponArrowButton.onClick.AddListener(NextWeapon);
        nextWeaponButton.onClick.AddListener(NextWeapon);
        previousWeaponArrowButton.onClick.AddListener(PreviousWeapon);
        previousWeaponButton.onClick.AddListener(PreviousWeapon);
    }

    void Update()
    {

    }

    void NextWeapon()
    {
        weaponWheelAnimator.SetTrigger("NextWeapon");
    }

    void PreviousWeapon()
    {
        weaponWheelAnimator.SetTrigger("PreviousWeapon");
    }

    void UpdateWeaponWheelDisplay()
    {

    }
}
