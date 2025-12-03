using UnityEngine;

public class CarObject : MonoBehaviour, IInteractable
{
    public float damageCollision;
    public bool isThrown = false;
    public AttackHandler attackHandler;

    public GameObject explosionParticle;

    private void Start()
    {
        attackHandler = GameObject.FindFirstObjectByType<AttackHandler>();
    }

    public void Interacting()
    {
        if (attackHandler.objectInHand != null)
        {
            return;
        }
        else
        {
            isThrown = false;
            attackHandler.objectInHand = this.gameObject;
            transform.SetParent(attackHandler.mechaHand);
            if (this.gameObject.name.StartsWith("AutoBus"))
            {
                AudioManager.Instance.Play("KidsScream");
            }
            else
            {
                AudioManager.Instance.Play("CarAlarm");
            }
            transform.position = attackHandler.mechaHand.position;
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            Collider collider = transform.GetComponent<Collider>();
            rb.isKinematic = true;
            collider.enabled = false;
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        if (collision.gameObject.CompareTag("Enemy") && isThrown)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damageCollision);
            GameObject explosion = Instantiate(explosionParticle, transform.position, transform.rotation);
            Destroy(explosion, 1);
            AudioManager.Instance.Play("AutoExplosion");
            Destroy(this.gameObject, 1f);
        }

        else if (collision.gameObject.CompareTag("Untagged"))//Cambiar a Ground cuando este
        {
            GameObject explosion = Instantiate(explosionParticle, transform.position, transform.rotation);
            AudioManager.Instance.Play("AutoExplosion");
            Destroy(explosion, 1);
            isThrown = false ;//Cambiar a detrpy si es necesario
        }
    }
}
