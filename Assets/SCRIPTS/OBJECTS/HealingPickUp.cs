using UnityEngine;

public class HealingPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthHandler pHandler = other.GetComponent<PlayerHealthHandler>();

            if (pHandler != null && pHandler.actualHealth < pHandler.maxHealth)
            {
                pHandler.actualHealth = pHandler.maxHealth;
                Destroy(this.gameObject);
            }
        }
    }
}
