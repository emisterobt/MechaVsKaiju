using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    private AttackHandler attackHandler;
    private EnemyAttackHandler enemyAttackHandler;
    public GameObject hitParticle;
    public DamageType type;

    private void Start()
    {
        attackHandler = FindAnyObjectByType<AttackHandler>();
        enemyAttackHandler = FindAnyObjectByType<EnemyAttackHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == DamageType.Melee)
        {
            IDamageable damgeable = other.GetComponent<IDamageable>();
            if (other.CompareTag("Player"))
            {
                return;
            }

            if (damgeable != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(attackHandler.meleeDamage);
                if (other.CompareTag("Enemy"))
                {
                    if (hitParticle != null)
                    {
                        hitParticle.SetActive(true);

                    }
                }
                AudioManager.Instance.Play("ImpactoPunch");

            }
        }

        if (type == DamageType.Laser)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.isStunned = true;
                }
                
            }
        }



    }

    private void OnTriggerStay(Collider other)
    {
        if (type == DamageType.Laser)
        {
            IDamageable damgeable = other.GetComponent<IDamageable>();
            if (other.CompareTag("Player"))
            {
                return;
            }

            if (damgeable != null)
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(attackHandler.laserDamage * 0.02f);
            }



        }
    }

    private void OnDisable()
    {
        if (hitParticle == null)
        {
            return ;

        }
        hitParticle.SetActive(false);
    }
    public enum DamageType
    {
        Laser, Melee
    }
}
