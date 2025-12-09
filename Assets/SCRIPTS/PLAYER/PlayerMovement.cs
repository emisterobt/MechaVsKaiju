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
    private AttackHandler attackHandler;
    private PlayerHealthHandler healthHandler;
    public CheckGround grndChk;
    public bool lockMovement = false;
    [SerializeField] public bool isJumping = false;
    [SerializeField] public bool wasJumping = false;
    public bool allowBlock = true;

    public float magnitude;
    public bool playWalkSound = false;
    public bool inGround;

    public bool died = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grndChk = GetComponent<CheckGround>();
        playerAnims = GetComponent<PlayerAnimationController>();
        attackHandler = GetComponent<AttackHandler>();
        healthHandler = GetComponent<PlayerHealthHandler>();
    }

    void Update()
    {
        if (died == true)
        {
            return;
        }

        if (lockMovement != true)
        {

            if (grndChk.IsGrounded() && !InputController.Instance.JumpHold())
            {
                Jump();
                Movement();
            }
            else
            {
                Jump();
                magnitude = 0f;
            }

        }
        AudioWalking();

        if (InputController.Instance.JumpHold() || InputController.Instance.Jump())
        {
            allowBlock = false;
            StartCoroutine(blockAllowed());
        }

        if (rb.linearVelocity.y <= 0 && grndChk.IsGrounded())
        {




            if (wasJumping == true)
            {
                isJumping = false;
                wasJumping = false;
                AudioManager.Instance.Play("ImpactoCaida");
            }
        }
        else
        {
            inGround = false;
        }
    }


    public void Movement()
    {
        rb.linearVelocity = transform.rotation * new Vector3(InputController.Instance.HorizontalMovement() * ActualSpeed(), rb.linearVelocity.y, InputController.Instance.VerticalMovement() * ActualSpeed());
        magnitude = rb.linearVelocity.magnitude;


    }

    public void Jump()
    {
        if (healthHandler.tookDmg == false)
        {
            if (InputController.Instance.Jump() && grndChk.IsGrounded() && isJumping == false || InputController.Instance.JumpHold() && InputController.Instance.MainAttack() && grndChk.IsGrounded() && isJumping == false)
            {
                playerAnims.TriggerJump();
                allowBlock = false;
                isJumping = true;
                Invoke("JumpDelay", 0.3f);
            }
        }

    }


    public float ActualSpeed()
    {

        return InputController.Instance.RunInput() ? runSpeed : InputController.Instance.CrouchInput() ? crouchSpeed : walkSpeed;

    }

    public void JumpDelay()
    {
        allowBlock = false;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        wasJumping = true;

    }

    public void PushAway(float impulse)
    {
        if (attackHandler.isBlocking == true || attackHandler.isUsingLaser == true)
        {
            return;
        }
        StartCoroutine(Launch(impulse));
    }
    public IEnumerator Launch(float impulse)
    {
        lockMovement = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            rb.useGravity = false;
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.AddForce(-transform.forward * impulse, ForceMode.Force);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.useGravity = true;
        lockMovement = false;
    }

    private void AudioWalking()
    {
        bool shouldPlay = magnitude > 3f && grndChk.IsGrounded() && !isJumping;

        if (shouldPlay && !playWalkSound && lockMovement == false)
        {
            // Start playing if we should play but aren't currently
            AudioManager.Instance.Play("MechaWalk");
            playWalkSound = true;
        }
        else if (!shouldPlay && playWalkSound || lockMovement == true)
        {
            // Stop playing if we shouldn't play but are currently
            AudioManager.Instance.Stop("MechaWalk");
            playWalkSound = false;
        }
    }

    private IEnumerator blockAllowed()
    {
        yield return new WaitForSeconds(1f);
        allowBlock = true;
    }
}
