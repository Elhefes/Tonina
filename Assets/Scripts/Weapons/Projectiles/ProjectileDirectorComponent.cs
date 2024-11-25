using System.Collections;
using UnityEngine;

public class ProjectileDirectorComponent : MonoBehaviour
{
    /// <summary>
    /// Enabling this script moves the projectile it's attached to
    /// from point A to B to C,
    /// where A is the starting point,
    /// B is a halfway point (slightly up),
    /// and C is the target.
    /// </summary>

    public CreatureMovement creatureMovement;
    public Rigidbody rb;
    public Transform pointA;
    private Vector3 startingPoint;
    private Transform pointC;
    private Vector3 pointC_CorrectedPosition;
    public float speed;
    public float thresholdDistance;
    public float rotationSpeed;
    [Header("Object Rotation Offset")]
    public float yRotationOffset = -90f;

    private Vector3 pointB;
    private Vector3[] points;
    private int currentPointIndex = 0;
    private bool movingToNextPoint = true;
    private Vector3 direction;

    private Coroutine destroyerCoroutine;

    void Start()
    {
        pointC = creatureMovement.target;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        startingPoint = pointA.position;

        CalculatePointB();
        UpdatePointC();

        points = new Vector3[3];
        points[0] = startingPoint;
        points[1] = pointB;
        points[2] = pointC_CorrectedPosition;

        destroyerCoroutine = StartCoroutine(DestroyObject());
    }

    void Update()
    {
        if (pointC != null)
        {
            UpdatePointC();
            points[2] = pointC_CorrectedPosition;
        }

        if (currentPointIndex < 2)
        {
            CalculatePointB();
            points[1] = pointB;
        }
    }

    void FixedUpdate()
    {
        if (movingToNextPoint)
        {
            MoveToNextPoint();
        }
        else
        {
            MoveForward();
        }
    }

    private void CalculatePointB()
    {
        if (pointA != null && pointC != null)
        {
            Vector3 midPoint = (startingPoint + pointC_CorrectedPosition) / 2;
            float distanceAC = Vector3.Distance(startingPoint, pointC_CorrectedPosition);
            pointB = new Vector3(midPoint.x, midPoint.y + distanceAC * 0.12f, midPoint.z);
        }
    }

    private void UpdatePointC()
    {
        if (pointC != null)
        {
            pointC_CorrectedPosition = new Vector3(pointC.position.x, pointC.position.y + 1.5f, pointC.position.z);
        }
    }

    private void MoveToNextPoint()
    {
        if (currentPointIndex >= points.Length)
        {
            movingToNextPoint = false;
            return;
        }

        Vector3 targetPosition = points[currentPointIndex];
        direction = (targetPosition - transform.position).normalized;
        RotateTowards(direction);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < thresholdDistance)
        {
            currentPointIndex++;
        }
    }

    private void MoveForward()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion offsetRotation = Quaternion.Euler(0, yRotationOffset, 0);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation * offsetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(rb.gameObject);
    }
}
