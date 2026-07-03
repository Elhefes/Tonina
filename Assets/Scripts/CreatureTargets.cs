using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central registry of all currently active Creature instances, split by side.
/// Creatures register themselves in Creature.OnEnable() and unregister in
/// Creature.OnDisable(), which keeps these lists correct even when creatures
/// are being reused via an object pooler instead of instantiated/destroyed.
///
/// EnemyAI reads jadeaWarriors (its potential targets), FriendlyAI reads
/// enemies (its potential targets). Which list a creature ends up in is
/// decided by its GameObject tag ("Enemy" or "Jadea"), matching the tags
/// already used elsewhere (FindGameObjectsWithTag calls, etc).
/// </summary>
public static class CreatureTargets
{
    public static readonly List<Creature> enemies = new List<Creature>();
    public static readonly List<Creature> jadeaWarriors = new List<Creature>();

    public static void Register(Creature creature)
    {
        if (creature.CompareTag("Enemy"))
        {
            if (!enemies.Contains(creature)) enemies.Add(creature);
        }
        else if (creature.CompareTag("Jadea"))
        {
            if (!jadeaWarriors.Contains(creature)) jadeaWarriors.Add(creature);
        }
        else if (creature.CompareTag("Player"))
        {
            if (!jadeaWarriors.Contains(creature)) jadeaWarriors.Add(creature);
        }
    }

    public static void Unregister(Creature creature)
    {
        enemies.Remove(creature);
        jadeaWarriors.Remove(creature);
    }

    // Static lists survive between play sessions if the editor's domain
    // reload is disabled. Clear them whenever a play session actually
    // starts so pooled/destroyed creatures from a previous run can't
    // linger as phantom targets.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnLoad()
    {
        enemies.Clear();
        jadeaWarriors.Clear();
    }
}