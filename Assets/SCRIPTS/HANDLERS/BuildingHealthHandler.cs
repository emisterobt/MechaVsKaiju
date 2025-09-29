using UnityEngine;

public class BuildingHealthHandler : MonoBehaviour , IDamageable
{
    public float maxHealth;
    public float actualHealth;
    public Animator anim;
    private void Start()
    {
        actualHealth = maxHealth;
        anim = GetComponent<Animator>();
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
        anim.SetBool("isFalling", true);
    }
}
