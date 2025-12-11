using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float actualHealth;

    public GameObject fireWorks;
    [SerializeField] private GameObject cameraExp;

    private KaijuAnimationController animCtrl;
    private EnemyAttackHandler attackHandler;
    private void Start()
    {
        actualHealth = maxHealth;
        animCtrl = GetComponent<KaijuAnimationController>();
        attackHandler = GetComponent<EnemyAttackHandler>();
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= damage;
        animCtrl.OnHitAnimation();
        if (actualHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        fireWorks.SetActive(true);
        attackHandler.stopAttacks = true;
        StartCoroutine(ChangeCamera());
        StartCoroutine(GameManager.Instance.Victory());
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        KaijuAnimationController animCntrl = GetComponent<KaijuAnimationController>();

        animCntrl.StunnedAnim(true);
        agent.isStopped = true;
        agent.speed = 0f;
        
    }

    private IEnumerator ChangeCamera()
    {
        yield return new WaitForSeconds(2f);
        cameraExp.gameObject.SetActive(true);
    }
}
