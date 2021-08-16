using UnityEngine;

public class RotateGun : MonoBehaviour
{

    public GrapplingGun grappling;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    void Update()
    {
        if (transform.parent != null)
        {
            if (!grappling.IsGrappling())
            {
                desiredRotation = transform.parent.rotation;
            }
            else
            {
                desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
        }
    }


}