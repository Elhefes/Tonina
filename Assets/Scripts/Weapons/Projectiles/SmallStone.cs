using UnityEngine;

public class SmallStone : Projectile
{
    public override void Attack(Animator animator)
    {
        animator.SetTrigger("StoneAttack");
        base.SpawnProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        base.HandleCollision(other.gameObject);

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SlowDownEnemy();
            Destroy(gameObject);
        }
    }
}
