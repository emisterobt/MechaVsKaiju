using UnityEngine;

public class EnemyAttackDamage : MonoBehaviour
{
    private EnemyAttackHandler mAttack;
    private void Start()
    {
        mAttack = transform.parent.GetComponent<EnemyAttackHandler>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(mAttack.tailDamage);
            Rigidbody pRb = other.GetComponent<Rigidbody>();
            
        }
        else if (!other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().TakeDamage(mAttack.tailDamage);

        }
    }

}
