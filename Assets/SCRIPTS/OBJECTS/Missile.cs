using System.Collections;
using UnityEditor;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public AttackHandler attackHndlr;

    public GameObject explosionEffect;

    private void Start()
    {
        attackHndlr = FindAnyObjectByType<AttackHandler>();
        StartCoroutine(DestroyTimer());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (explosionEffect != null)
        {
            GameObject expEffct = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(expEffct,1);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackHndlr.explosionRadius);

        foreach (Collider collider in colliders)
        {
            IDamageable damgeable = collider.GetComponent<IDamageable>();
            if (collider.CompareTag("Player"))
            {
                return;
            }

            if (damgeable != null)
            {
                collider.GetComponent<IDamageable>().TakeDamage(attackHndlr.missileDamage);
            }

        }
        Destroy(this.gameObject);
    }

    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(attackHndlr.missileDuration);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackHndlr.explosionRadius);
    }
}
