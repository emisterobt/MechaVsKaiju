using UnityEngine;

public class KaijuAnimationController : MonoBehaviour
{
    public Animator animator;

    public void AttackColaAnim()
    {
        animator.SetTrigger("AttackTail");
    }

    public void AttackFireAnim()
    {
        animator.SetTrigger("AttackFire");
    }

    public void WalkAnim(bool isMoving)
    {
        animator.SetBool("IsWalking", isMoving);
    }

    public void ScreamAnim()
    {
        animator.SetTrigger("Scream");
    }

    public void StunnedAnim(bool isStunned)
    {
        animator.SetBool("IsStunned", isStunned);
    }

    public void OnHitAnimation()
    {
        animator.SetTrigger("Hit");
    }

    
}
