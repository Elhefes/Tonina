using UnityEngine;
using System.Collections.Generic;

public class PlaceablesManager : MonoBehaviour
{
    public GameObject[] placeablePrefabs;

    private List<GameObject> activePlaceables = new List<GameObject>();

    // ------------------------------
    // LOAD
    // ------------------------------
    public void ApplyPlaceablesData(PlaceablesData data)
    {
        DestroyAll();

        foreach (var p in data.placeables)
        {
            GameObject prefab = FindPrefabByName(p.prefabName);
            if (prefab == null)
            {
                Debug.LogWarning("Prefab not found: " + p.prefabName);
                continue;
            }

            GameObject obj = Instantiate(prefab, p.GetPosition(), p.GetRotation());
            activePlaceables.Add(obj);
        }
    }

    // ------------------------------
    // SAVE
    // ------------------------------
    public PlaceablesData GeneratePlaceablesData()
    {
        PlaceablesData data = new PlaceablesData();

        FindAllActivePlaceables();

        foreach (GameObject obj in activePlaceables)
        {
            data.placeables.Add(new PlaceableData(
                obj.name.Replace("(Clone)", "").Trim(),
                obj.transform.position,
                obj.transform.rotation
            ));
        }

        data.placeablesCount = activePlaceables.Count;

        return data;
    }

    // ------------------------------
    // HELPERS
    // ------------------------------

    public void DestroyAll()
    {
        foreach (GameObject obj in activePlaceables)
            Destroy(obj);

        activePlaceables.Clear();
    }

    private void FindAllActivePlaceables()
    {
        activePlaceables.Clear();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Placeable");
        foreach (GameObject obj in objects)
        {
            if (obj.transform.parent == null)
                activePlaceables.Add(obj);
        }
    }

    private GameObject FindPrefabByName(string name)
    {
        foreach (var prefab in placeablePrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }
        return null;
    }
}
