using UnityEngine;

public class AttackModeCreatureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnsParentObject;
    public Transform[] spawns;
    public ToninaWarrior[] friendlies;
    public Enemy[] enemies;
    private Vector3[] originalEnemyPositions;

    public ObjectPooler pooler;

    void Start()
    {
        pooler = ObjectPooler.Instance;

        // Store all spawns in an array
        spawns = new Transform[spawnsParentObject.transform.childCount];
        for (int i = 0; i < spawnsParentObject.transform.childCount; i++)
        {
            spawns[i] = spawnsParentObject.transform.GetChild(i).transform;
        }

        originalEnemyPositions = new Vector3[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            originalEnemyPositions[i] = enemies[i].transform.position;
        }
    }

    public void SetFriendliesActive(bool value)
    {
        foreach (ToninaWarrior warrior in friendlies)
        {
            warrior.gameObject.SetActive(value);
            if (value) warrior.ResetFriendlyAttributes();
        }
    }

    public void SetEnemiesActive(bool value)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(value);
            if (value) enemy.ResetEnemyAttributes();
        }
    }

    public void MoveEnemiesToRandomSpawns()
    {
        // Shuffle positions
        for (int i = 0; i < originalEnemyPositions.Length; i++)
        {
            int rand = Random.Range(i, originalEnemyPositions.Length);
            (originalEnemyPositions[i], originalEnemyPositions[rand]) = (originalEnemyPositions[rand], originalEnemyPositions[i]);
        }

        // Assign shuffled positions back
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].transform.position = originalEnemyPositions[i];
        }
    }

    public void MoveFriendliesToSpawns(int[] spawnArray)
    {
        int creatureIndex = 0;

        for (int i = 0; i < spawns.Length; i++)
        {
            for (int j = 0; j < spawnArray[i]; j++)
            {
                if (creatureIndex >= friendlies.Length)
                {
                    Debug.LogWarning("Not enough creatures for the given spawns array!");
                    return;
                }

                friendlies[creatureIndex].transform.position = spawns[i].position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                creatureIndex++;
            }
        }
    }
}
