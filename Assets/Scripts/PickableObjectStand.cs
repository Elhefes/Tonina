using UnityEngine;

public class PickableObjectStand : MonoBehaviour
{
    public GameObject pickableObject;

    public void PickObject()
    {
        if (pickableObject != null)
        {
            pickableObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickObject();
        }
    }
}
