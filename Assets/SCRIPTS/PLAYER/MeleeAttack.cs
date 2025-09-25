using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private AttackHandler attackHandler;

    private void Start()
    {
        attackHandler = transform.parent.GetComponent<AttackHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(attackHandler.meleeDamage);
        }
    }

}
