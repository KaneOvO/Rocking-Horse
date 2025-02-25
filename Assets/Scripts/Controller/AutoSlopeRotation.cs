using UnityEngine;

public class AutoSlopeRotation : MonoBehaviour
{
    public float rayDistance = 2.0f;
    public float rotationSpeed = 5.0f;
    public LayerMask groundLayer;
    public GameObject cameraHolder;

    void Start()
    {

    }

    void Update()
    {
        CheckGround();

        if(cameraHolder != null)
        {
            cameraHolder.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayer))
        {
            Vector3 forward = transform.forward;
            Vector3 projectedForward = Vector3.ProjectOnPlane(forward, hit.normal).normalized;

            if (projectedForward == Vector3.zero)
            {
                projectedForward = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized;
            }

            Quaternion targetRotation = Quaternion.LookRotation(projectedForward, hit.normal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
    }
}
