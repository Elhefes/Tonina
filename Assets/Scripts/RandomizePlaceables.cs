using UnityEngine;

public class RandomizePlaceables : MonoBehaviour
{
    [SerializeField] private GameObject fencesParentObject;
    private GameObject[] fences;
    private int maxFencesAmount = 3;

    private void Start()
    {
        RandomizeFences();
    }

    private void RandomizeFences()
    {
        fences = new GameObject[fencesParentObject.transform.childCount];
        for (int i = 0; i < fencesParentObject.transform.childCount; i++)
        {
            fences[i] = fencesParentObject.transform.GetChild(i).gameObject;
        }

        // Shuffle the array indices
        int[] indices = new int[fences.Length];
        for (int i = 0; i < indices.Length; i++)
            indices[i] = i;

        for (int i = 0; i < indices.Length; i++)
        {
            int rand = Random.Range(i, indices.Length);
            (indices[i], indices[rand]) = (indices[rand], indices[i]);
        }

        // Disable all first
        foreach (var obj in fences)
            obj.SetActive(false);

        // Enable the chosen ones
        for (int i = 0; i < maxFencesAmount; i++)
            fences[indices[i]].SetActive(true);
    }
}
