using System.Collections;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    public bool isAttacking = false;
    public int attacksDone = 0;
    public int attacksRemaining = 3;

    public EnemyMovement eMove;


    [Header("Flame Attack")]
    public Transform flameOrigin;
    public float flameDuration;
    public float flameRange;
    public float flameCoolDown;
    public float flameChargeTime;

    [Header("Tail Attack")]
    public float tailDamage;
    public float tailCooldown;
    [SerializeField]
    private Transform tailCollider;



    private void Start()
    {
        eMove = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (eMove == null)
        {
            return;
        }
        else
        {
            if (eMove.followPlayer == true && isAttacking == false)
            {
                if (attacksDone < attacksRemaining)
                {
                    StartCoroutine(MeleeAttack());
                }
                else if (attacksDone == attacksRemaining && isAttacking == false)
                {
                    StartCoroutine(Flame());
                }
            }

        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        attacksDone += 1;
        yield return new WaitForSeconds(.6f);
        //Do animation
        tailCollider.gameObject.SetActive(true);
        eMove.agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        tailCollider.gameObject.SetActive(false);
        eMove.agent.isStopped = false;
        yield return new WaitForSeconds(tailCooldown);
        isAttacking = false;
    }

    private IEnumerator Flame()
    {
        isAttacking = true;
        yield return new WaitForSeconds(flameChargeTime);
        //Do Animation
        Debug.Log("Lanzando Fuego");
        //flame Instance o Active
        eMove.agent.isStopped = true;
        yield return new WaitForSeconds(flameDuration);
        isAttacking = false;
        attacksDone = 0;
        eMove.agent.isStopped = false;
    }
}
