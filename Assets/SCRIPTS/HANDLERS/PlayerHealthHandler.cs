using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    private void Start()
    {
        actualHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetHealth(1);
        }
    }
    public void TakeDamage(float damage)
    {
        actualHealth += damage;

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
