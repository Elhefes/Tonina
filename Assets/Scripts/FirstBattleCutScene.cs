using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FirstBattleCutscene : MonoBehaviour
{
    public GameObject textBox;
    public NavMeshAgent[] cutsceneEnemies;
    public AudioSource hawkAudioSource;

    private Vector3[] savedPositions;
    private Quaternion[] savedRotations;

    private void OnEnable()
    {
        if (cutsceneEnemies == null || cutsceneEnemies.Length == 0) return;

        savedPositions = new Vector3[cutsceneEnemies.Length];
        savedRotations = new Quaternion[cutsceneEnemies.Length];

        for (int i = 0; i < cutsceneEnemies.Length; i++)
        {
            if (cutsceneEnemies[i] == null) continue;

            savedPositions[i] = cutsceneEnemies[i].transform.position;
            savedRotations[i] = cutsceneEnemies[i].transform.rotation;
        }

        StartCoroutine(BattleCutscene());
    }

    private void OnDisable()
    {
        foreach (NavMeshAgent agent in cutsceneEnemies) agent.speed = 0f;

        if (cutsceneEnemies == null || savedPositions == null) return;

        for (int i = 0; i < cutsceneEnemies.Length; i++)
        {
            if (cutsceneEnemies[i] == null) continue;

            cutsceneEnemies[i].transform.position = savedPositions[i];
            cutsceneEnemies[i].transform.rotation = savedRotations[i];
        }
    }

    IEnumerator BattleCutscene()
    {
        yield return new WaitForSeconds(0.33f);

        foreach (NavMeshAgent agent in cutsceneEnemies) agent.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        textBox.SetActive(true);

        yield return new WaitForSeconds(3.3f);

        hawkAudioSource.PlayOneShot(hawkAudioSource.clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));

        yield return new WaitForSeconds(0.5f);
        foreach (NavMeshAgent agent in cutsceneEnemies) agent.speed = 3.5f;


        yield return new WaitForSeconds(1.2f);

        textBox.SetActive(false);

        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);
    }
}
