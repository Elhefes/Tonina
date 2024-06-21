using UnityEngine;

public class VillageTeleportMenu : MonoBehaviour
{
    public Transform[] gateTransforms;
    public GameObject[] blueBacks;
    public Player player;
    private int gateIndex;

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
