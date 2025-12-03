using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator anim;
    private AttackHandler attackHandler;
    private CheckGround chckGrnd;
    private PlayerMovement pm;
    private PlayerDash pDash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //anim = GetComponent<Animator>();
        attackHandler = GetComponent<AttackHandler>();
        chckGrnd = GetComponent<CheckGround>();
        pm = GetComponent<PlayerMovement>();
        pDash = GetComponent<PlayerDash>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicAnimations();
        Dash();
        Attack();
        Laser();
        isBlocking();
        Misil();
        Throw();
        JumpHolding();

    }

    private void BasicAnimations()
    {
        anim.SetBool("isGrounded", chckGrnd.IsGrounded());
        anim.SetFloat("ForwardMovement", InputController.Instance.VerticalMovement());
        anim.SetFloat("SideMovement", InputController.Instance.HorizontalMovement());
    }

    public void TriggerJump()
    {
        anim.SetTrigger("isImpulsing");
    }

    public void JumpHolding()
    {
        anim.SetBool("JumpHold", InputController.Instance.JumpHold());
    }

    public void Dash()
    {

        anim.SetBool("isDashing", pDash.isDashing);
    }

    public void Attack()
    {
        anim.SetBool("isPunching", attackHandler.isPunching);
        anim.SetInteger("Attack", attackHandler.hitType);
    }

    public void Laser()
    {
        anim.SetBool("Laser", attackHandler.isUsingLaser);
    }

    public void Misil()
    {
        anim.SetBool("Misil",attackHandler.isShootMissile);
    }

    public void isBlocking()
    {
        anim.SetBool("isBlocking", attackHandler.isBlocking);
        attackHandler.shield.SetActive(attackHandler.isBlocking);
        pm.lockMovement = attackHandler.isBlocking;
    }

    public void Throw()
    {
        anim.SetBool("doThrow", attackHandler.isThrowing);
    }

    public void RecieveDamage()
    {
        anim.SetTrigger("isDamaged");
    }

    public void isDead()
    {
        anim.SetBool("IsDead", true);
    }

}
