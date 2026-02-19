using System.Collections;
using UnityEngine;

public class MinimapIndicator : MonoBehaviour
{
    public bool isBlue;
    public bool isRed;
    public MinimapCamera minimapCamera;
    private Coroutine transmissionCoroutine;
    private int uniqueID;

    private void Start()
    {
        if (minimapCamera == null) minimapCamera = GameObject.Find("minimapCamera").GetComponent<MinimapCamera>();
        uniqueID = gameObject.GetInstanceID();
    }

    private void OnEnable()
    {
        transmissionCoroutine = StartCoroutine(SendCoordinatesIfOutside());
    }

    private void OnDisable()
    {
        if (transmissionCoroutine != null)
        {
            StopCoroutine(transmissionCoroutine);
            transmissionCoroutine = null;
        }
    }

    IEnumerator SendCoordinatesIfOutside()
    {
        while (gameObject.activeInHierarchy)
        {
            if (minimapCamera != null)
            {
                Vector3 viewPos = minimapCamera.cam.WorldToViewportPoint(transform.position);
                if (viewPos.x > 1f || viewPos.x < 0f || viewPos.y > 1f || viewPos.y < 0f)
                {
                    minimapCamera.RecieveIndicator(uniqueID, viewPos, isBlue, isRed);
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}
