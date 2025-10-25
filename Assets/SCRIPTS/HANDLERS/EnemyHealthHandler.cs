using UnityEngine;

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
        Destroy(this.gameObject);
    }
}
