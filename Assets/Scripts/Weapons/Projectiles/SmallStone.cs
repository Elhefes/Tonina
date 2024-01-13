using UnityEngine;

public class SmallStone : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        base.HandleCollision(other.gameObject);

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SlowDownEnemy();
        }
    }
}
