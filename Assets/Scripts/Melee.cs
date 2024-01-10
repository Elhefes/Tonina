using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage;
    public bool canHit;
    private List<GameObject> hitEnemies = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if (!canHit) return;
        var obj = collision.gameObject;
        if (obj.CompareTag("Enemy") && !hitEnemies.Contains(obj))
        {
            hitEnemies.Add(obj);
            obj.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var obj = collision.gameObject;
        if (hitEnemies.Contains(obj))
        {
            hitEnemies.Remove(obj);
        }
    }
}
