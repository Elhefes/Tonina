using UnityEngine;

public class Spear : Projectile 
{
    public override void Attack(Animator animator)
    {
        animator.SetTrigger("SpearAttack");
        base.SpawnProjectile();
    }
}
