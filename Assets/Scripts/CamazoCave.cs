using System.Collections.Generic;
using UnityEngine;

public class CamazoCave : MonoBehaviour
{
    public Camazo camazoInCave;
    public CamazoTextLines camazoTextLines;
    public Transform camazoSpawnPosition;

    public bool camazoIsSummoned;

    public GameObject[] bonesOnTables;

    public GameObject coinStack1;
    public GameObject coinStack2;
    public GameObject coinStack3;

    public ParticleSystem[] tableParticleSystems;
    public AudioSource[] tableAudioSources;

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
            camazoIsSummoned = true;
            AddCoinsIfPossible();
            RemoveRandomBone(10);
            camazoInCave.SetEvolutionStage();
            camazoInCave.LoadEvolutionScale();
            camazoTextLines.UpdateCamazoLines();
            GameState.Instance.SaveWorld();
            camazoInCave.animator.SetTrigger("FlyToCaveBridge");
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
        if (GameState.Instance == null)
        {
            Debug.LogError("LoadBones: GameState.Instance is null!");
            return;
        }

        if (GameState.Instance.progressionData == null)
        {
            Debug.LogError("LoadBones: progressionData is null!");
            return;
        }

        // Initialize the list if it doesn't exist in this save
        if (GameState.Instance.progressionData.camazoCaveBoneIndices == null)
        {
            GameState.Instance.progressionData.camazoCaveBoneIndices =
                new List<int>();
        }

        if (bonesOnTables == null)
        {
            Debug.LogError("LoadBones: bonesOnTables array is null!");
            return;
        }

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

        GameState.Instance.progressionData.bonesCamazoHasEaten += amount;

        bool table1Boomed = false;
        bool table2Boomed = false;
        bool table3Boomed = false;
        bool table4Boomed = false;

        // Remove random bones
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, savedIndices.Count);
            int selectedBone = savedIndices[randomIndex];

            // Remove it from the saved list
            savedIndices.RemoveAt(randomIndex);

            // Disable the bone
            bonesOnTables[selectedBone].SetActive(false);

            if (!table1Boomed && selectedBone < 4) table1Boomed = true;
            if (!table2Boomed && selectedBone < 8 && selectedBone > 3) table2Boomed = true;
            if (!table3Boomed && selectedBone < 12 && selectedBone > 7) table3Boomed = true;
            if (!table4Boomed && selectedBone < 16 && selectedBone > 11) table4Boomed = true;
        }
        
        if (table1Boomed) Invoke("Table1Boom", 0.72f);
        if (table2Boomed) Invoke("Table2Boom", 0.48f);
        if (table3Boomed) Invoke("Table3Boom", 0.24f);
        if (table4Boomed) Table4Boom();
    }

    public int GetActiveBoneCount()
    {
        return GameState.Instance.progressionData.camazoCaveBoneIndices.Count;
    }

    private void Table1Boom()
    {
        tableParticleSystems[0].Play();
        tableAudioSources[0].PlayOneShot(tableAudioSources[0].clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }
    private void Table2Boom()
    {
        tableParticleSystems[1].Play();
        tableAudioSources[1].PlayOneShot(tableAudioSources[1].clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }
    private void Table3Boom()
    {
        tableParticleSystems[2].Play();
        tableAudioSources[2].PlayOneShot(tableAudioSources[2].clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }
    private void Table4Boom()
    {
        tableParticleSystems[3].Play();
        tableAudioSources[3].PlayOneShot(tableAudioSources[3].clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
    }
}
