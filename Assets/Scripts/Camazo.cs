using System.Collections;
using UnityEngine;

public class Camazo : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    [Header("Movement")]
    public float moveSpeed;
    public float rotationSpeed;
    public float waypointReachedDistance = 0.1f;

    [Header("Waiting")]
    public float waitAtA = 2f;
    public float waitAtC = 2f;

    private void Start()
    {
        if (pointA != null) transform.position = pointA.position;
        if (pointA == null || pointB == null || pointC == null) return;
        StartCoroutine(FlyLoop());
    }

    IEnumerator FlyLoop()
    {
        while (true)
        {
            yield return MoveTo(pointB);
            yield return MoveTo(pointC);

            yield return new WaitForSeconds(waitAtC);

            yield return MoveTo(pointB);
            yield return MoveTo(pointA);

            yield return new WaitForSeconds(waitAtA);
        }
    }

    IEnumerator MoveTo(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > waypointReachedDistance)
        {
            // Direction to destination
            Vector3 direction = (target.position - transform.position).normalized;

            // Smoothly turn toward it
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }

            // Fly forward instead of directly toward the target
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // Snap exactly to the waypoint
        transform.position = target.position;
    }
}