using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public Transform grndChecker;

    public LayerMask grndMasks;

    public float detectionRadius;

    public bool rayDraw;

    public Vector3 dimesions;

    public bool IsGrounded()
    {
        return Physics.CheckSphere(grndChecker.position,detectionRadius,grndMasks);
        //return Physics.CheckBox(grndChecker.position, dimesions,Quaternion.identity, grndMasks);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (rayDraw && grndChecker != null)
        {
            Gizmos.DrawWireSphere(grndChecker.position, detectionRadius);
            //Gizmos.DrawWireCube(grndChecker.position,dimesions);
        }
    }

}
