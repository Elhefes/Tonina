using UnityEngine;

public class PlaceableDescriptions : MonoBehaviour
{
    public GameObject[] descriptionParentObjects;
    public GameObject[] unlockedDescriptionObjects;
    public GameObject[] lockedDescriptionObjects;

    private void OnEnable()
    {
        UpdateDescriptionObjects();
    }

    private void UpdateDescriptionObjects()
    {
        foreach (GameObject obj in unlockedDescriptionObjects) obj.SetActive(false);
        foreach (GameObject obj in lockedDescriptionObjects) obj.SetActive(false);

        // TODO: Add locked descriptions for fence and spear rack when they are locked

        for (int i = 0; i < unlockedDescriptionObjects.Length; i++)
        {
            if (i == 2 && !GameState.Instance.progressionData.maizePlaceUnlocked) lockedDescriptionObjects[0].SetActive(true);
            else if (i == 4 && !GameState.Instance.progressionData.fillOkillUnlocked) lockedDescriptionObjects[1].SetActive(true);
            else if (i == 5 && !GameState.Instance.progressionData.towerUnlocked) lockedDescriptionObjects[2].SetActive(true);
            else unlockedDescriptionObjects[i].SetActive(true);
        }
    }

    public void DisableDescriptionParentObjects()
    {
        foreach (GameObject obj in descriptionParentObjects) obj.SetActive(false);
    }
}
