using UnityEngine;

public class VillageTeleportMenu : MonoBehaviour
{
    public Transform[] gateTransforms;
    public GameObject middleGate;
    public Vector3[] middleGatePositionsFrom4rdTo14th;
    public Vector3[] middleGateTransformPositionsFrom4rdTo14th;
    public GameObject[] blueBacks;
    public Player player;
    private int gateIndex;
    private int extraFloorsBuilt;

    private void Start()
    {
        // TODO: get extraFloorsBuilt when save/load works
        if (extraFloorsBuilt > 0) UpdateMiddleGatePosition();
    }

    private void OnEnable()
    {
        UpdateGateIndex();
        UpdateBlueBacks();
    }

    void UpdateGateIndex()
    {
        if (player.insideKingHouse) gateIndex = 0;
        else if (Vector3.Distance(player.transform.position, gateTransforms[0].position) < 5f) gateIndex = 1;
        else if (Vector3.Distance(player.transform.position, gateTransforms[1].position) < 5f) gateIndex = 2;
        else if (Vector3.Distance(player.transform.position, gateTransforms[2].position) < 5f) gateIndex = 3;
    }

    public void IncrementExtraFloorsBuilt() { extraFloorsBuilt++; } // Temporary

    public void UpdateMiddleGatePosition()
    {
        middleGate.transform.position = middleGatePositionsFrom4rdTo14th[extraFloorsBuilt - 1];
        gateTransforms[1].position = middleGateTransformPositionsFrom4rdTo14th[extraFloorsBuilt - 1];
    }

    void UpdateBlueBacks()
    {
        foreach (GameObject back in blueBacks)
        {
            back.SetActive(false);
        }
        blueBacks[gateIndex].SetActive(true);
    }

    public void TeleportToVillageGate(Transform gatePosition)
    {
        if (Vector3.Distance(player.transform.position, gatePosition.position) < 5f) return;
        player.TeleportToVillageGate(gatePosition);
    }

    public void ReturnHome() { player.ReturnHome(gameObject); }

}
