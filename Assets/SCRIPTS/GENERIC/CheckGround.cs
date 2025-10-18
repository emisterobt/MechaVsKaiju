using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public Transform grndChecker;

    public LayerMask grndMasks;

    public float detectionRadius;

    public bool rayDraw;

    public bool IsGrounded()
    {
        return Physics.CheckSphere(grndChecker.position,detectionRadius,grndMasks);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (rayDraw && grndChecker != null)
        {
            Gizmos.DrawWireSphere(grndChecker.position,detectionRadius);
        }
    }

}
