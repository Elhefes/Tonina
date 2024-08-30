using UnityEngine;

public class SmallStone : Projectile
{
    public GameObject hitSoundObject;

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
            hitSoundObject.SetActive(true);
            hitSoundObject.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
