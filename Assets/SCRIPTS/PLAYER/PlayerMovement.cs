using System.Collections;
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
    private PlayerAnimationController playerAnims;
    public CheckGround grndChk;
    public bool lockMovement = false;
    [SerializeField] private bool isJumping = false;

    public bool inGround;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grndChk = GetComponent<CheckGround>();
        playerAnims = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        if (lockMovement != true)
        {
            Movement();
            Jump();
        }


        if (grndChk.IsGrounded())
        {
            inGround = true;
        }
        else
        {
            inGround = false;
        }
    }


    public void Movement()
    {
        rb.linearVelocity = transform.rotation * new Vector3(InputController.Instance.HorizontalMovement() * ActualSpeed(), rb.linearVelocity.y, InputController.Instance.VerticalMovement() * ActualSpeed());
    }

    public void Jump()
    {
        if (InputController.Instance.Jump() && grndChk.IsGrounded() && isJumping == false)
        {
            playerAnims.TriggerJump();
            isJumping = true;
            Invoke("JumpDelay", 0.3f);
        }
    }


    public float ActualSpeed()
    {

        return InputController.Instance.RunInput() ? runSpeed : InputController.Instance.CrouchInput() ? crouchSpeed : walkSpeed;

    }

    public void JumpDelay()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = false;
    }

    public void PushAway(float impulse)
    {
        StartCoroutine(Launch(impulse));
    }
    public IEnumerator Launch(float impulse)
    {
        lockMovement = true;
        float elapsedTime = 0f;
        Debug.Log("P");

        while (elapsedTime < 1f)
        {
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(-transform.forward * impulse, ForceMode.Force);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.useGravity = true;
        lockMovement = false;
        Debug.Log("movimiento habilitado");
    }
}
