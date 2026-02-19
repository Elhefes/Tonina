using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera cam;
    private Coroutine displayOutsidersCoroutine;
    private List<OutsiderIndicator> flowingList = new List<OutsiderIndicator>();
    private List<OutsiderIndicator> temporaryList = new List<OutsiderIndicator>();

    public class OutsiderIndicator
    {
        public int uniqueID;
        public Vector3 position;
        public bool isBlue;
        public bool isRed;
    }

    private void OnEnable()
    {
        if (displayOutsidersCoroutine != null)
        {
            StopCoroutine(displayOutsidersCoroutine);
            displayOutsidersCoroutine = null;
        }
        displayOutsidersCoroutine = StartCoroutine(UpdateOutsiderIndicators());
    }

    public void RecieveIndicator(int uniqueID, Vector3 coordinates, bool isBlue, bool isRed)
    {
        ClearPreviousInstancesOfID(uniqueID);

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
            yield return new WaitForSeconds(0.5f);
            if (flowingList.Count > 0)
            {
                temporaryList = new List<OutsiderIndicator>(flowingList);
                foreach (OutsiderIndicator outsiderIndicator in temporaryList)
                {
                    
                }
                flowingList.Clear();
            }
        }
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
