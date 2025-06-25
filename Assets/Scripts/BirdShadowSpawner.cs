using System.Collections;
using UnityEngine;

public class BirdShadowSpawner : MonoBehaviour
{
    private Coroutine spawnerCoroutine;
    public GameObject[] shadowObjects;
    public GameObject[] shadowChildren;
    private Animator[] animators;
    public AnimationClip[] animationClips;

    private void Start()
    {
        animators = GetComponentsInChildren<Animator>(includeInactive: true);
    }

    void OnEnable()
    {
        spawnerCoroutine = StartCoroutine(SpawnRandomShadowAtRandomZ());
    }

    private IEnumerator SpawnRandomShadowAtRandomZ()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3.75f, 7.5f));
            int r = Random.Range(0, shadowObjects.Length);
            if (shadowObjects[r].activeSelf)
            {
                if (shadowChildren[r].transform.position.x > 180)
                {
                    // Reset animation
                    shadowObjects[r].SetActive(false);
                    shadowObjects[r].SetActive(true);

                    shadowObjects[r].transform.localPosition = new Vector3(0f, 0f, Random.Range(-40f, 30f));
                    PlayRandomAnim(r);
                }
            }
            else
            {
                shadowObjects[r].SetActive(true);
                shadowObjects[r].transform.localPosition = new Vector3(0f, 0f, Random.Range(-40f, 30f));
                PlayRandomAnim(r);
            }
        }
    }

    private void PlayRandomAnim(int r)
    {
        string stateName = animationClips[Random.Range(0, animationClips.Length)].name;
        animators[r].Play(stateName);
    }
}
