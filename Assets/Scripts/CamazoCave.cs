using System.Collections.Generic;
using UnityEngine;

public class CamazoCave : MonoBehaviour
{
    public Camazo camazoInCave;
    public Transform camazoSpawnPosition;

    public bool camazoIsSummoned;

    public GameObject[] bonesOnTables;

    public GameObject coinStack1;
    public GameObject coinStack2;
    public GameObject coinStack3;

    private void Start()
    {
        LoadCoins(GameState.Instance.progressionData.coinsPlacedInCamazoCave);
    }

    public void DisappearCamazo()
    {
        camazoInCave.ResetAnimatorStates();
        camazoInCave.animator.SetTrigger("Empty");
        camazoInCave.gameObject.SetActive(false);
        camazoInCave.transform.position = camazoSpawnPosition.position;
        camazoInCave.gameObject.SetActive(true);
        camazoIsSummoned = false;
    }

    public void SummonCamazo()
    {
        if (!camazoIsSummoned)
        {
            camazoInCave.animator.SetTrigger("FlyToCaveBridge");
            camazoIsSummoned = true;
            AddCoinsIfPossible();
            GameState.Instance.SaveWorld();
        }
    }

    private void AddCoinsIfPossible()
    {
        int coinsPlaced = GameState.Instance.progressionData.coinsPlacedInCamazoCave;

        if (coinsPlaced < 3)
        {
            GameState.Instance.progressionData.coinsPlacedInCamazoCave++;
            coinsPlaced++;
        }
        LoadCoins(coinsPlaced);
    }

    private void LoadCoins(int coinsPlaced)
    {
        if (coinsPlaced > 0) coinStack1.SetActive(true);
        if (coinsPlaced > 1) coinStack2.SetActive(true);
        if (coinsPlaced > 2) coinStack3.SetActive(true);
    }

    public void LoadBones()
    {
        List<int> savedIndices =
            GameState.Instance.progressionData.camazoCaveBoneIndices;

        // Disable everything first
        foreach (GameObject bone in bonesOnTables)
        {
            bone.SetActive(false);
        }

        // Enable saved bones
        foreach (int index in savedIndices)
        {
            if (index >= 0 && index < bonesOnTables.Length)
            {
                bonesOnTables[index].SetActive(true);
            }
        }
    }


    public void AddRandomBone(int amount)
    {
        List<int> savedIndices =
            GameState.Instance.progressionData.camazoCaveBoneIndices;

        List<int> availableIndices = new List<int>();

        for (int i = 0; i < bonesOnTables.Length; i++)
        {
            if (!savedIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        amount = Mathf.Min(amount, availableIndices.Count);

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedBone = availableIndices[randomIndex];

            availableIndices.RemoveAt(randomIndex);

            savedIndices.Add(selectedBone);
            bonesOnTables[selectedBone].SetActive(true);
        }
        GameState.Instance.SaveWorld();
    }

    public void RemoveRandomBone(int amount)
    {
        List<int> savedIndices =
            GameState.Instance.progressionData.camazoCaveBoneIndices;

        // Don't try to remove more bones than currently exist
        amount = Mathf.Min(amount, savedIndices.Count);

        // Remove random bones
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, savedIndices.Count);
            int selectedBone = savedIndices[randomIndex];

            // Remove it from the saved list
            savedIndices.RemoveAt(randomIndex);

            // Disable the bone
            bonesOnTables[selectedBone].SetActive(false);
        }
        GameState.Instance.SaveWorld();
    }

    public int GetActiveBoneCount()
    {
        return GameState.Instance.progressionData.camazoCaveBoneIndices.Count;
    }
}
