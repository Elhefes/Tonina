using TMPro;
using UnityEngine;

public class PlayerHeadingHUD : MonoBehaviour
{
    public Player player;
    public Transform toCamazoCaveTPPos;
    public Transform camazoCaveSpawnPos;
    public TMP_Text headingTMP;
    public float requiredTime = 5f;

    private float standingTime = 0f;

    void Update()
    {
        if (Vector3.Distance(player.transform.position, toCamazoCaveTPPos.position) < 2.5f)
        {
            ContinueHeadingTo(camazoCaveSpawnPos.position, "Camazo Cave", false);
        }
        else
        {
            headingTMP.gameObject.SetActive(false);
            standingTime = 0f;
        }
    }

    private void ReEnableMinimap()
    {
        player.ReEnableMinimap();
    }

    private void ContinueHeadingTo(Vector3 destination, string locationText, bool enableMinimap)
    {
        headingTMP.gameObject.SetActive(true);

        standingTime += Time.deltaTime;
        headingTMP.text = "Heading to " + locationText + " in " +
            Mathf.CeilToInt(requiredTime - standingTime).ToString() + "...";

        // Countdown finished
        if (standingTime >= requiredTime)
        {
            headingTMP.gameObject.SetActive(false);
            StartCoroutine(player.TeleportPlayerToSpot(destination));
            if (!enableMinimap) player.mouseLook.minimapInput.gameObject.SetActive(false);
            else Invoke("ReEnableMinimap", 0.33f);
        }
    }
}
