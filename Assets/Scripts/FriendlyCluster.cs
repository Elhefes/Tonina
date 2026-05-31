using System.Collections.Generic;
using UnityEngine;

public class FriendlyCluster : MonoBehaviour
{
    private List<JadeaWarrior> friendlies = new List<JadeaWarrior>();
    private List<Vector3> friendlySpawns = new List<Vector3>();

    private void Awake()
    {
        friendlies.Clear();
        friendlySpawns.Clear();

        // Get all Enemy components from children
        JadeaWarrior[] childFriendlies = GetComponentsInChildren<JadeaWarrior>(includeInactive: true);

        foreach (JadeaWarrior friendly in childFriendlies)
        {
            friendlies.Add(friendly);
            friendlySpawns.Add(friendly.transform.position);
        }
    }

    public void ResetAllFriendlies()
    {
        foreach (JadeaWarrior friendly in friendlies) if (friendly != null) friendly.gameObject.SetActive(false);
        SpawnClusterFromPool();
    }

    private void SpawnClusterFromPool()
    {
        for (int i = 0; i < friendlies.Count; i++)
        {
            if (friendlies[i] != null)
            {
                friendlies[i].transform.position = friendlySpawns[i];
                friendlies[i].gameObject.SetActive(true);
                friendlies[i].ResetFriendlyAttributes();
            }
        }
    }

    public void SetFriendlySpeeds(float speed)
    {
        foreach (JadeaWarrior friendly in friendlies)
        {
            friendly.agent.speed = speed;
        }
    }

    public float GetFriendlySpeed()
    {
        if (friendlies[0] != null) return friendlies[0].agent.speed;
        return 0f;
    }
}