using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private KaijuAnimationController animCtrl;

    private void Start()
    {
        actualHealth = maxHealth;
        animCtrl = GetComponent<KaijuAnimationController>();
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= damage;
        animCtrl.OnHitAnimation();
        if (actualHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        StartCoroutine(GameManager.Instance.Victory());
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        KaijuAnimationController animCntrl = GetComponent<KaijuAnimationController>();

        animCntrl.StunnedAnim(true);
        agent.isStopped = true;
        
    }
}
