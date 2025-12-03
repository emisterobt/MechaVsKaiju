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

    public GameObject dashParticles;
    public GameObject dashParticlesGlow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grndCheck = GetComponent<CheckGround>();
        pM = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (InputController.Instance.Dash() && !grndCheck.IsGrounded() && canDash && pM.lockMovement == false && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        dashParticles.SetActive(true);
        dashParticlesGlow.SetActive(true);
        canDash = false;
        isDashing = true;
        AudioManager.Instance.Play("Dash");
        float elapsedTime = 0f;


        //CalculateDirection();

        while (elapsedTime < dashDuration)
        {
            rb.useGravity = false;
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
            rb.linearVelocity = new Vector3(0,0,0);
            rb.AddForce(transform.forward * dashForce, ForceMode.Force);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.useGravity = true;
        dashParticles.SetActive(false);
        dashParticlesGlow.SetActive(false);
        isDashing=false;
        AudioManager.Instance.Stop("Dash");
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
