using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FindRainTriggers : MonoBehaviour
{
    // TODO: The collider list should update when new buildings are built at runtime

    private ParticleSystem rainParticleSystem;
    private ParticleSystem.TriggerModule triggerModule;

    void Start()
    {
        // Get the ParticleSystem component attached to the same GameObject
        rainParticleSystem = GetComponent<ParticleSystem>();

        // Get the TriggerModule of the ParticleSystem
        triggerModule = rainParticleSystem.trigger;

        // Find all objects with the tag "RainBlocker"
        GameObject[] rainBlockers = GameObject.FindGameObjectsWithTag("RainBlocker");

        // Enable the trigger module and set its parameters
        if (rainBlockers.Length > 0)
        {
            triggerModule.enabled = true;
            triggerModule.inside = ParticleSystemOverlapAction.Kill;

            // Create a list of colliders
            List<Collider> colliders = new List<Collider>();

            foreach (GameObject blocker in rainBlockers)
            {
                // Get the Collider component of the RainBlocker object
                Collider collider = blocker.GetComponent<Collider>();

                if (collider != null)
                {
                    // Add the collider to the trigger module
                    triggerModule.SetCollider(colliders.Count, collider);
                    colliders.Add(collider);
                }
                else
                {
                    Debug.LogWarning($"GameObject {blocker.name} with tag 'RainBlocker' does not have a Collider component.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No objects with the tag 'RainBlocker' found in the scene.");
        }
    }
}
