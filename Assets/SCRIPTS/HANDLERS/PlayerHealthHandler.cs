using System.Collections;
using System.Net;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private AttackHandler attackHandler;
    private PlayerAnimationController pAnim;
    private PlayerMovement pMove;

    private Coroutine damageOverTime;
    private bool isTakingDamage;
    public bool tookDmg = false;

    private void Start()
    {
        actualHealth = maxHealth;
        attackHandler = GetComponent<AttackHandler>();
        pAnim = GetComponent<PlayerAnimationController>();
        pMove = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(float damage)
    {
        if (attackHandler.isBlocking == true)
        {
            actualHealth -= (damage - (damage * attackHandler.damageReduction));
        }
        else
        {
            actualHealth -= damage;

            if (tookDmg == false && isTakingDamage == true)
            {
                tookDmg = true;
                pAnim.RecieveDamage();
            }
            else if (isTakingDamage == false)
            {
                pAnim.RecieveDamage();
            }
        }

        if (actualHealth <= 0)
        {
            OnDeath();
        }
    }

    public void StartDamageOverTime(float dps, float interval, float duration = 0f)
    {
        if (isTakingDamage)
        {
            StopCoroutine(damageOverTime);
        }

        damageOverTime = StartCoroutine(DamageOverTime(dps, interval, duration));


        
    }

    public void StopDamageOverTime()
    {
        if (isTakingDamage && damageOverTime != null)
        {
            StopCoroutine(damageOverTime);
            isTakingDamage = false;
            tookDmg = false;
        }
    }


    public IEnumerator DamageOverTime(float dps, float interval, float duration)
    {
        isTakingDamage = true;
        float elapsedTime = 0f;

        while (duration == 0 || elapsedTime < duration)
        {
            TakeDamage(dps * interval);
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }

        isTakingDamage = false;
        tookDmg = false ;
    }



    public void OnDeath()
    {
        //Animacion derrota
        AudioManager.Instance.Play("MuerteMecha");
        pAnim.isDead();
        pMove.died = true;
        StartCoroutine(GameManager.Instance.GameOver());

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.agent.speed = 0;
        KaijuAnimationController animK = enemy.GetComponentInChildren<KaijuAnimationController>();
        animK.OnKaijuWin();
        EnemyAttackHandler enemyAttackHandler = enemy.GetComponent<EnemyAttackHandler>();
        enemyAttackHandler.stopAttacks = true;

    }

    public float GetHealth(float healthRecovered)
    {
        actualHealth = Mathf.Clamp(actualHealth + healthRecovered,0,maxHealth);
        return actualHealth;
    }
}
