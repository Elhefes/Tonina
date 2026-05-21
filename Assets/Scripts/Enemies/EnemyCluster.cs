using System.Collections.Generic;
using UnityEngine;

public class EnemyCluster : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    private List<Vector3> enemySpawns = new List<Vector3>();

    private void Start()
    {
        enemies.Clear();
        enemySpawns.Clear();

        // Get all Enemy components from children
        Enemy[] childEnemies = GetComponentsInChildren<Enemy>(includeInactive: true);

        foreach (Enemy enemy in childEnemies)
        {
            enemies.Add(enemy);

            // Save spawn position
            enemySpawns.Add(enemy.transform.position);
            enemy.gameObject.SetActive(false);
        }
    }

    public void SpawnClusterFromPool()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = ObjectPooler.Instance.SpawnEnemyFromPool(
                enemies[i].enemyType,
                enemySpawns[i],
                Quaternion.Euler(Vector3.zero)
            );

            enemy.ResetEnemyAttributes();
        }
    }
}