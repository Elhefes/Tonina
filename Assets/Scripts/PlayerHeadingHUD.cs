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
        if (player.blackFader.activeSelf) return;

        if (Vector3.Distance(player.transform.position, toCamazoCaveTPPos.position) < 2.5f)
        {
            ContinueHeadingTo(camazoCaveSpawnPos.position, "Camazo Cave", false, 12f);
        }
        else if (camazoCaveSpawnPos != null && player.transform.position.x < -700 
            && player.transform.position.z > 62)
        {
            ContinueHeadingTo(toCamazoCaveTPPos.position + new Vector3(3f, 0f, 3f), "Jadea", true, 0f);
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

    private void ContinueHeadingTo(Vector3 destination, string locationText, bool enableMinimap, float maxCameraZoom)
    {
        headingTMP.gameObject.SetActive(true);

        standingTime += Time.deltaTime;
        headingTMP.text = "Heading to " + locationText + " in " +
            Mathf.CeilToInt(requiredTime - standingTime).ToString() + "...";

        // Countdown finished
        if (standingTime >= requiredTime)
        {
            headingTMP.gameObject.SetActive(false);
            standingTime = 0f;
            StartCoroutine(player.TeleportPlayerToSpot(destination));
            if (!enableMinimap)
            {
                player.mouseLook.minimapInput.gameObject.SetActive(false);
                player.mouseLook.cameraForcedOnPlayer = true;
            }
            else
            {
                Invoke("ReEnableMinimap", 0.33f);
                player.mouseLook.cameraForcedOnPlayer = false;
            }
            if (maxCameraZoom > 0f) player.mouseLook.maxCameraZoom = maxCameraZoom;
            else player.mouseLook.ResetMaxCameraZoom();
        }
    }
}
