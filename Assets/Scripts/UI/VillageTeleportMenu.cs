using UnityEngine;

public class VillageTeleportMenu : MonoBehaviour
{
    public Transform[] gateTransforms;
    public GameObject middleGate;
    public Vector3[] middleGatePositionsFrom4rdTo14th;
    public Vector3[] middleGateTransformPositionsFrom4rdTo14th;
    public GameObject[] blueBacks;
    public Player player;
    private int extraFloorsBuilt;
    private int gateIndex;

    public void UpdateExtraFloorsInt() { extraFloorsBuilt = GameState.Instance.progressionData.extraPyramidFloorsBuilt; }

    private void OnEnable()
    {
        UpdateGateIndex();
        UpdateBlueBacks();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1") || Input.GetKeyDown("[1]")) TeleportToVillageGate(0);
        else if (Input.GetKeyDown("2") || Input.GetKeyDown("[2]")) TeleportToVillageGate(1);
        else if (Input.GetKeyDown("3") || Input.GetKeyDown("[3]")) TeleportToVillageGate(2);
        else if (Input.GetKeyDown("0") || Input.GetKeyDown("[0]")) ReturnHome();
        else if (Input.GetKeyDown("h") || Input.GetKeyDown("home")) ReturnHome();
    }

    void UpdateGateIndex()
    {
        if (player.insideKingHouse) gateIndex = 0;
        else if (Vector3.Distance(player.transform.position, gateTransforms[0].position) < 5f) gateIndex = 1;
        else if (Vector3.Distance(player.transform.position, gateTransforms[1].position) < 5f) gateIndex = 2;
        else if (Vector3.Distance(player.transform.position, gateTransforms[2].position) < 5f) gateIndex = 3;
    }

    public void UpdateMiddleGatePosition()
    {
        if (extraFloorsBuilt > 0)
        {
            middleGate.transform.position = middleGatePositionsFrom4rdTo14th[extraFloorsBuilt - 1];
            gateTransforms[1].position = middleGateTransformPositionsFrom4rdTo14th[extraFloorsBuilt - 1];
        }
    }

    void UpdateBlueBacks()
    {
        foreach (GameObject back in blueBacks)
        {
            back.SetActive(false);
        }
        blueBacks[gateIndex].SetActive(true);
    }

    public void TeleportToVillageGate(int gateNumber)
    {
        if (Vector3.Distance(player.transform.position, gateTransforms[gateNumber].position) < 5f) return;
        player.TeleportToVillageGate(gateTransforms[gateNumber]);
    }

    public void ReturnHome() { player.ReturnHome(gameObject); }

}
