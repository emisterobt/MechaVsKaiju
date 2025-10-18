using UnityEngine;

public class BuildingHealthHandler : MonoBehaviour , IDamageable
{
    public float maxHealth;
    public float actualHealth;
    public Animator anim;
    public ParticleSystem particle;
    public ParticleSystem particle2;

    public GameObject escombros;
    private void Start()
    {
        actualHealth = maxHealth;
        anim = GetComponent<Animator>();
        particle.Stop();
        particle2.Stop();
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
        particle.Play();
        particle2.Play();
        anim.SetBool("isFalling", true);
        Instantiate(escombros,particle.transform.position,Quaternion.Euler(0,0,0));
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }
}
