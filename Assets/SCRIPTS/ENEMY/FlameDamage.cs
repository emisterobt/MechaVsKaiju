using UnityEngine;
using static AttackDamage;

public class FlameDamage : MonoBehaviour
{
    private EnemyAttackHandler eAtk;
    private PlayerHealthHandler pH;

    private void Start()
    {
        eAtk = FindFirstObjectByType<EnemyAttackHandler>();
        pH = FindFirstObjectByType<PlayerHealthHandler>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthHandler>().StartDamageOverTime(eAtk.flameDps, eAtk.flameCoolDown, eAtk.flameDuration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthHandler>().StopDamageOverTime();
        }
    }

    private void OnDisable()
    {
        pH.StopDamageOverTime();

    }
}
