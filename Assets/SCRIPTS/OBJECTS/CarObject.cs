using UnityEngine;

public class CarObject : MonoBehaviour, IInteractable
{
    public float damageCollision;
    private bool hasHit;
    public bool isThrown = false;
    public AttackHandler attackHandler;

    public void Interacting()
    {
        if (attackHandler.objectInHand != null)
        {
            return;
        }
        else
        {
            attackHandler.objectInHand = this.gameObject;
            transform.SetParent(attackHandler.mechaHand);
            transform.position = attackHandler.mechaHand.position;
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            Collider collider = transform.GetComponent<Collider>();
            rb.isKinematic = true;
            collider.enabled = false;
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        if (collision.gameObject.CompareTag("Enemy") && isThrown)
        {
            //collision.gameObject.GetComponent<IDamageable>().Damage();
            hasHit = true;// cambiar a destroy tal vez?
        }

        else if (collision.gameObject.CompareTag("Untagged"))//Cambiar a Ground cuando este
        {
            isThrown = false ;//Cambiar a detrpy si es necesario
        }
    }
}
