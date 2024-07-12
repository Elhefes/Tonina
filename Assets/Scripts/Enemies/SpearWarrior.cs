using System.Collections;
using UnityEngine;

public class SpearWarrior : Enemy
{
    public GameObject spear;
    public GameObject club;

    void SwitchToClub()
    {
        if (this == null) return;
        spear.SetActive(false);
        weaponOnHand = club.GetComponent<Weapon>();
        club.SetActive(true);
        weaponOnHand.canHit = false;
    }

    protected override IEnumerator Attack()
    {
        onCooldown = true;
        while (creatureMovement.target && shouldAttack)
        {
            weaponOnHand.Attack(creatureMovement.animator);
            yield return new WaitForSeconds(weaponOnHand.attackCooldown);
        }
        SwitchToClub();
        onCooldown = false;
    }
}
