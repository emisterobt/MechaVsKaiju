using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public Transform rayOrigin;

    public LayerMask grndMasks;

    public float detectionDistance;

    public bool rayDraw;

    public bool IsGrounded()
    {
        return Physics.Raycast(rayOrigin.position, -rayOrigin.up,detectionDistance,grndMasks);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (rayDraw && rayOrigin != null)
        {
            Gizmos.DrawRay(rayOrigin.position, -rayOrigin.up * detectionDistance);
        }
    }

}
