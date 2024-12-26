using UnityEngine;

public class KanchoSphereCollider : MonoBehaviour
{
    public Kancho kancho;

    private void OnCollisionEnter(Collision collision)
    {
        kancho.HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        kancho.HandleCollision(other.gameObject);
    }
}
