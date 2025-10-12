using UnityEngine;

public class MissilePickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttackHandler aHandler = other.GetComponent<AttackHandler>();

            if (aHandler != null)
            {
                aHandler.currentMissiles = aHandler.maxMissiles;
                Destroy(this.gameObject);
            }
        }
    }

}
