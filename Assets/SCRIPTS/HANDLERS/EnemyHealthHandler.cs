using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private void Start()
    {
        actualHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= damage;

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
