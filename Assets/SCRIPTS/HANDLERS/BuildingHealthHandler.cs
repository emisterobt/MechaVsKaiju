using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;
    public Animator anim;
    public ParticleSystem particle;
    public ParticleSystem particle2;

    
    public static bool playingDerrumbe = false;

    public GameObject[] escombros;
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
        if (playingDerrumbe == false)
        {
            AudioManager.Instance.Play("Derrumbe");
            playingDerrumbe = true;
            StartCoroutine(ResetDerrumbeSound());
        }
        Instantiate(escombros[RandomNumber()], particle.transform.position, Quaternion.Euler(0, 0, 0));
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(actualHealth);
        }
    }
    private int RandomNumber()
    {

        int escombro = Random.Range(0, escombros.Length);

        return escombro;
    }

    public static IEnumerator ResetDerrumbeSound()
    {
        yield return new WaitForSeconds(3f);
        playingDerrumbe = false;
    }
}
