using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera cam;

    public GameObject whiteOutsiderMarker;
    public GameObject[] blueOutsiderMarkers;
    public GameObject[] redOutsiderMarkers;

    private Coroutine displayOutsidersCoroutine;
    private List<OutsiderIndicator> flowingList = new List<OutsiderIndicator>(); // list of all recieved indicators
    private List<OutsiderIndicator> temporaryList = new List<OutsiderIndicator>(); // screenshot of flowing list at certain time intervals

    public class OutsiderIndicator
    {
        public int uniqueID;
        public Vector3 position;
        public bool isBlue;
        public bool isRed;
    }

    private void OnEnable()
    {
        if (whiteOutsiderMarker == null || blueOutsiderMarkers.Length < 1 || redOutsiderMarkers.Length < 1) return;

        if (displayOutsidersCoroutine != null)
        {
            StopCoroutine(displayOutsidersCoroutine);
            displayOutsidersCoroutine = null;
        }
        displayOutsidersCoroutine = StartCoroutine(UpdateOutsiderIndicators());
    }

    public void RecieveIndicator(int uniqueID, Vector3 coordinates, bool isBlue, bool isRed)
    {
        ClearPreviousInstancesOfID(uniqueID); // don't allow multiple indicators from same source

        OutsiderIndicator outsiderIndicator = new OutsiderIndicator();
        outsiderIndicator.uniqueID = uniqueID;
        outsiderIndicator.position = coordinates;
        outsiderIndicator.isBlue = isBlue;
        outsiderIndicator.isRed = isRed;
        flowingList.Add(outsiderIndicator);
    }

    IEnumerator UpdateOutsiderIndicators()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.75f);

            whiteOutsiderMarker.SetActive(false);
            foreach (GameObject obj in blueOutsiderMarkers) obj.SetActive(false);
            foreach (GameObject obj in redOutsiderMarkers) obj.SetActive(false);

            if (flowingList.Count > 0)
            {
                temporaryList = new List<OutsiderIndicator>(flowingList);

                List<OutsiderIndicator> reds = temporaryList.Where(i => i.isRed).ToList();
                List<OutsiderIndicator> blues = temporaryList.Where(i => i.isBlue).ToList();
                List<OutsiderIndicator> whites = temporaryList.Where(i => !i.isRed && !i.isBlue).ToList();

                for (int i = 0; i < Mathf.Min(blueOutsiderMarkers.Length, blues.Count); i++)
                {
                    blueOutsiderMarkers[i].transform.localPosition = UpdateOutsiderMarkerPosition(blues[i].position);
                    blueOutsiderMarkers[i].SetActive(true);
                }

                for (int i = 0; i < Mathf.Min(redOutsiderMarkers.Length, reds.Count); i++)
                {
                    redOutsiderMarkers[i].transform.localPosition = UpdateOutsiderMarkerPosition(reds[i].position);
                    redOutsiderMarkers[i].SetActive(true);
                }

                if (whites.Count > 0)
                {
                    whiteOutsiderMarker.transform.localPosition = UpdateOutsiderMarkerPosition(whites[0].position);
                    whiteOutsiderMarker.SetActive(true);
                }

                flowingList.Clear();
            }
        }
    }

    private Vector3 UpdateOutsiderMarkerPosition(Vector3 indicatorPosition)
    {
        Vector3 edgePosition = new Vector3(0f, 0f, 0f);
        edgePosition.x = Mathf.Clamp(-960 + 330 * (Mathf.Clamp(indicatorPosition.x, 0f, 1f)), -960f, -630f);
        edgePosition.y = Mathf.Clamp(291 + 249 * (Mathf.Clamp(indicatorPosition.y, 0f, 1f)), 291f, 540f);
        return edgePosition;
    }

    private void ClearPreviousInstancesOfID(int uniqueID)
    {
        for (int i = flowingList.Count - 1; i >= 0; i--)
        {
            if (flowingList[i].uniqueID == uniqueID)
                flowingList.RemoveAt(i);
        }
    }
}
