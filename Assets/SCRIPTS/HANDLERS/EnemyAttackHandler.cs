using System.Collections;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    public bool isAttacking = false;
    public int attacksDone = 0;
    public int attacksRemaining = 3;

    public EnemyMovement eMove;
    private KaijuAnimationController animCtrl;

    [Header("Flame Attack")]
    public Transform flameOrigin;
    public float flameDuration;
    public float flameRange;
    public float flameCoolDown;
    public float flameChargeTime;
    public float flameDps;
    public ParticleSystem flameParticle;

    [Header("Tail Attack")]
    public float tailDamage;
    public float tailCooldown;
    [SerializeField]
    private Transform tailCollider;

    public bool stopAttacks = false;

    private Coroutine attack;

    private void Start()
    {
        eMove = GetComponent<EnemyMovement>();
        animCtrl = GetComponent<KaijuAnimationController>();
    }

    private void Update()
    {
        if (eMove == null || stopAttacks == true || eMove.isStunned == true)
        {
            return;
        }
        else
        {
            if (eMove.followPlayer == true && isAttacking == false)
            {
                if (attacksDone < attacksRemaining)
                {
                    attack = StartCoroutine(MeleeAttack());
                }
                else if (attacksDone == attacksRemaining && isAttacking == false)
                {
                    attack = StartCoroutine(Flame());
                }
            }

        }
    }


    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        attacksDone += 1;
        yield return new WaitForSeconds(1.2f);
        animCtrl.AttackColaAnim();
        AudioManager.Instance.Play("KaijuCola");
        tailCollider.gameObject.SetActive(true);
        animCtrl.WalkAnim(false);
        eMove.agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        animCtrl.WalkAnim(true);
        tailCollider.gameObject.SetActive(false);
        eMove.agent.isStopped = false;
        yield return new WaitForSeconds(tailCooldown);
        isAttacking = false;
    }

    private IEnumerator Flame()
    {
        isAttacking = true;
        yield return new WaitForSeconds(flameChargeTime);
        flameParticle.gameObject.SetActive(true);
        animCtrl.AttackFireAnim();
        AudioManager.Instance.Play("KaijuFuego");
        //flame Instance o Active
        eMove.agent.isStopped = true;
        animCtrl.WalkAnim(false);
        yield return new WaitForSeconds(flameDuration);
        animCtrl.WalkAnim(true);
        flameParticle.gameObject.SetActive(false);
        attacksDone = 0;
        eMove.agent.isStopped = false;
        yield return new WaitForSeconds(tailCooldown);
        isAttacking = false;
    }
}
