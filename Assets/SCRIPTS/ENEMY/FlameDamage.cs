using UnityEngine;
using static AttackDamage;

public class FlameDamage : MonoBehaviour
{
    private EnemyAttackHandler eAtk;

    private void Start()
    {
        eAtk = FindFirstObjectByType<EnemyAttackHandler>();
    }

    private void OnTriggerStay(Collider other)
    {

            IDamageable damgeable = other.GetComponent<IDamageable>();
            if (other.CompareTag("Enemy"))
            {
                return;
            }

            if (damgeable != null)
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(eAtk.flameDps * 0.02f);
            }
    }
}
