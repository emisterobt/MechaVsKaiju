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





}
