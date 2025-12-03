using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private AttackHandler attackHandler;
    private PlayerAnimationController pAnim;
    private void Start()
    {
        actualHealth = maxHealth;
        attackHandler = GetComponent<AttackHandler>();
        pAnim = GetComponent<PlayerAnimationController>();
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
            
            pAnim.RecieveDamage();
        }

        if (actualHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        //Animacion derrota
        AudioManager.Instance.Play("MuerteMecha");
        pAnim.isDead();
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
