using System.Collections.Generic;
using UnityEngine;

public class BarricadesController : MonoBehaviour
{
    Barricade[] barricades;

    private void Start()
    {
        FindBarricades();
    }

    public void FindBarricades()
    {
        GameObject[] barricadeObjects = GameObject.FindGameObjectsWithTag("Barricade");
        List<Barricade> barricadesList = new List<Barricade>();

        foreach (GameObject obj in barricadeObjects)
        {
            Barricade barricade = obj.GetComponent<Barricade>();
            if (barricade != null)
            {
                barricadesList.Add(barricade);
            }
        }

        barricades = barricadesList.ToArray();
    }

    public void RestoreBarricades()
    {
        if (barricades != null)
        {
            foreach (Barricade barricade in barricades)
            {
                barricade.RestoreBarricade();
            }
        }
    }
}
