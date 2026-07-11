using UnityEngine;

public class Axe : Weapon
{
    public override void UpdateWeaponValues()
    {
        if (recievesWeaponsBuffs)
        {
            damage = defaultDamage +
                PlayerAttributes.AxeDamageBuff(GameState.Instance.progressionData.weaponsLevel);
            attackCooldown = defaultAttackCooldown /
                PlayerAttributes.AxeAttackSpeedDenominator(GameState.Instance.progressionData.weaponsLevel);
        }
    }

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("AxeAttack");
    }
}
