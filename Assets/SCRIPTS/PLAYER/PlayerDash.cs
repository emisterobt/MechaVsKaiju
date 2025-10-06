using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Modifiers")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;

    [Header("Dash Direction")]
    [SerializeField] private bool useMoveDirection;
    private Rigidbody rb;

    private Vector3 dashDirection;
    private bool canDash = true;
    public bool isDashing = false;

    private CheckGround grndCheck;
    private PlayerMovement pM;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grndCheck = GetComponent<CheckGround>();
        pM = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (InputController.Instance.Dash() && !grndCheck.IsGrounded() && canDash && pM.lockMovement == false)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float elapsedTime = 0f;


        CalculateDirection();

        while (elapsedTime < dashDuration)
        {
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
            rb.AddForce(dashDirection * dashForce, ForceMode.Force);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.useGravity = true;
        isDashing=false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void CalculateDirection()
    {
        if (useMoveDirection)
        {
            dashDirection = transform.forward * InputController.Instance.VerticalMovement() + transform.right * InputController.Instance.HorizontalMovement();
        }
        else
        {
            dashDirection = transform.forward;
            dashDirection.y = 0.0f;
            dashDirection.Normalize();
        }
    }

}
