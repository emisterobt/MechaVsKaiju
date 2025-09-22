using UnityEngine;
[RequireComponent(typeof(CheckGround), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;

    public float jumpForce;

    public float baseYScale;
    public float crouchYScale;

    public Rigidbody rb;

    public CheckGround grndChk;

    public float dashForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grndChk = GetComponent<CheckGround>();
    }

    void Update()
    {
        Movement();
        Dash();
        Jump();
    }


    public void Movement()
    {
        rb.linearVelocity = transform.rotation * new Vector3(InputController.Instance.HorizontalMovement() * ActualSpeed(), rb.linearVelocity.y, InputController.Instance.VerticalMovement() * ActualSpeed());
    }

    public void Jump()
    {
        if (InputController.Instance.Jump() && grndChk.IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Dash()
    {

        if (InputController.Instance.Dash() && !grndChk.IsGrounded())
        {
            Vector3 direction = GetDirection(transform);
            rb.AddForce(direction * dashForce, ForceMode.Impulse);
        }
    }

    public float ActualSpeed()
    {

        return InputController.Instance.RunInput() ? runSpeed : InputController.Instance.CrouchInput() ? crouchSpeed : walkSpeed;

    }

    public Vector3 GetDirection(Transform player)
    {
        Vector3 direction = new Vector3();

        direction = player.forward * InputController.Instance.VerticalMovement() + player.right * InputController.Instance.HorizontalMovement();

        return direction.normalized;
    }
}
