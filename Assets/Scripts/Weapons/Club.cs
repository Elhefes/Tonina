using UnityEngine;

public class Club : Weapon
{
    public override void UpdateWeaponValues()
    {
        if (recievesWeaponsBuffs)
        {
            damage = defaultDamage + 
                PlayerAttributes.ClubDamageBuff(GameState.Instance.progressionData.weaponsLevel);
            attackCooldown = defaultAttackCooldown /
                PlayerAttributes.ClubAttackSpeedDenominator(GameState.Instance.progressionData.weaponsLevel);
        }
        if (recievesLeadershipBuffs)
        {
            attackCooldown = defaultAttackCooldown /
                PlayerAttributes.LeadershipAttackSpeedDenominator(GameState.Instance.progressionData.leadershipLevel);
        }
    }

    public override void Attack(Animator animator)
    {
        animator.SetTrigger("ClubAttack");
    }
}
