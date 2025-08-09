using UnityEngine;

public class PlaceableDescriptions : MonoBehaviour
{
    public GameObject[] descriptionObjects;

    public void ResetDescriptionObjects()
    {
        foreach (GameObject obj in descriptionObjects) obj.SetActive(false);
    }
}
