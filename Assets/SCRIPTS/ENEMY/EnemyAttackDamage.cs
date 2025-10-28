using System.Collections;
using UnityEngine;

public class EnemyAttackDamage : MonoBehaviour
{
    private EnemyAttackHandler mAttack;

    PlayerMovement pMove;

    [SerializeField] private float impulseForce;
    private void Start()
    {
        mAttack = transform.parent.GetComponent<EnemyAttackHandler>();
        pMove = FindAnyObjectByType<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(mAttack.tailDamage);

            pMove.PushAway(impulseForce);
            

        }
        else if (!other.CompareTag("Enemy"))
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(mAttack.tailDamage);

            }

        }
    }

}
