using System.Collections;
using UnityEngine;

public class SpearWarriorAI : EnemyAI
{
    public GameObject spear;

    protected override IEnumerator PeriodicalTargetChecking()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                if (spear.activeSelf)
                {
                    if (NearestTarget("Jadea", 30f) != null) break;
                    else if (enemyCreature.isAttacker && NearestTarget("Barricade", 8f) != null) break;
                }
                else
                {
                    if (NearestTarget("Jadea", 10f) != null) break;
                    else if (enemyCreature.isAttacker && NearestTarget("Barricade", 8f) != null) break;
                    else if (NearestTarget("Jadea", 20f) != null) break;
                }
                enemyCreature.creatureMovement.target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
                break;
            }
        }
    }
}
