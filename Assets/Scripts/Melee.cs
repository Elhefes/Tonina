using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage;
    public bool canHit;
    private bool hitted;

    private void OnCollisionEnter(Collision collision)
    {
        if (!canHit) return;
        var obj = collision.gameObject;
        if (obj.CompareTag("Enemy") && !hitted)
        {
            hitted = true;
            obj.GetComponent<EnemyAI>()?.TakeDamage(damage);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hitted)
        {
            hitted = false;
        }
    }
}
