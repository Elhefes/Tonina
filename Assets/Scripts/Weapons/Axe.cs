using UnityEngine;

public class Axe : Weapon
{
    public override void Attack(Animator animator)
    {
        animator.SetTrigger("AxeAttack");
    }
}
