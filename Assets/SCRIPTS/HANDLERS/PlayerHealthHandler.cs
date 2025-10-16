using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private AttackHandler attackHandler;
    private void Start()
    {
        actualHealth = maxHealth;
        attackHandler = GetComponent<AttackHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(2);
        }
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
        }

        if (actualHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        //Animacion derrota
    }

    public float GetHealth(float healthRecovered)
    {
        actualHealth = Mathf.Clamp(actualHealth + healthRecovered,0,maxHealth);
        return actualHealth;
    }
}
