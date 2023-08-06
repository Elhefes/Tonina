using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public WeaponType type;
    public int damage;
    public Sprite uiSprite;
}

public enum WeaponType
{
    Club,
    Small_stone,
    Spear
}