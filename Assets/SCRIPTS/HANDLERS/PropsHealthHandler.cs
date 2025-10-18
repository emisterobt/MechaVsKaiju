using UnityEngine;

public class PropsHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;
    public ParticleSystem particle;
    private void Start()
    {
        actualHealth = maxHealth;
        particle.Stop();
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= damage;

        if (actualHealth <= 0)
        {
            particle.Play();
            Invoke("OnDeath", 0.5f);
        }
    }

    public void OnDeath()
    {
        
        gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        particle.Play();
        Invoke("OnDeath", 0.5f);
    }
}
