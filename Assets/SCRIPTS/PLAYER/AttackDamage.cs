using System;
using System.Collections;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    private AttackHandler attackHandler;
    public DamageType type;

    private void Start()
    {
        attackHandler = FindAnyObjectByType<AttackHandler>();
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
                AudioManager.Instance.Play("ImpactoPunch");

            }
        }

        if (type == DamageType.Laser)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                enemyMovement.isStunned = true;
            }
        }


        
    }

    private void OnTriggerStay(Collider other)
    {
        if(type == DamageType.Laser)
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


    public enum DamageType
    {
        Laser, Melee
    }
}
