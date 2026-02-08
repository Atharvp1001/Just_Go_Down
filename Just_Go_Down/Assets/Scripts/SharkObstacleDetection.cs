using UnityEngine;

public class SharkObstacleDetector : MonoBehaviour
{
    public float rayDistance = 5f;
    public float forwardOffset = 1.5f;
    public LayerMask obstacleMask;

    public Vector3 GetAvoidanceDirection()
    {
        Vector3 origin = transform.position + transform.forward * forwardOffset;
        Vector3 avoidance = Vector3.zero;

        Vector3[] rayDirs =
        {
            transform.forward,
            (transform.forward + transform.right).normalized,
            (transform.forward - transform.right).normalized,
            (transform.forward + transform.up).normalized,
            (transform.forward - transform.up).normalized
        };

        foreach (Vector3 dir in rayDirs)
        {
            RaycastHit hit;

            if (Physics.Raycast(origin, dir, out hit, rayDistance, obstacleMask))
            {
                avoidance += hit.normal * (rayDistance - hit.distance);
            }
        }

        return avoidance.normalized;
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position + transform.forward * forwardOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + transform.forward * rayDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + (transform.forward + transform.right).normalized * rayDistance);
        Gizmos.DrawLine(origin, origin + (transform.forward - transform.right).normalized * rayDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + (transform.forward + transform.up).normalized * rayDistance);
        Gizmos.DrawLine(origin, origin + (transform.forward - transform.up).normalized * rayDistance);
    }
}
