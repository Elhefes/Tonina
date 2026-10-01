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
}
