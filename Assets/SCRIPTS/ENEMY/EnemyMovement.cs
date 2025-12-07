using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform endPoint;
    public Transform player;
    public NavMeshAgent agent;

    public float speedMovement;
    public float detectionRange;

    public LayerMask playerLayer;

    public bool followPlayer = false;

    private float distanceToEnd;
    public float initialDistance;

    private KaijuAnimationController animCntrl;
    private bool isMoving = false;

    [SerializeField] private float stunTime;
    public bool isStunned = false;
    public bool cerca = false;


    [Header("EndGame")]
    [SerializeField] private GameObject atomicBomb;
    public float timeToEnd;
    public float nuclerHealth;
    public float nuclerMaxHealth;

    [SerializeField] private GameObject cameraExp;

    private Coroutine reachedDestinyCoroutine;
    private bool isCountdownRunning = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialDistance = Vector3.Distance(transform.position, endPoint.position);
        animCntrl = GetComponent<KaijuAnimationController>();
        nuclerHealth = timeToEnd;
        nuclerMaxHealth = nuclerHealth;
    }

    private void Update()
    {
        if (isStunned)
        {
            StartCoroutine(Stunned());
            return;
        }
        else
        {
            PlayerChase();
            WalkToEndPoint();
            animCntrl.WalkAnim(isMoving);

            UpdateCercaState();
        }

    }

    public void WalkToEndPoint()
    {
        if (endPoint == null || followPlayer == true)
        {
            return;
        }
        else
        {
            agent.SetDestination(endPoint.position);
            isMoving = true;

            if (agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = false;

                if (!cerca)
                {
                    cerca = true;
                    animCntrl.OnKaijuCerca(cerca);

                    if (!isCountdownRunning)
                    {
                        StartDestinyCountdown();
                    }
                }
            }
        }
    }
    private void UpdateCercaState()
    {
        if (cerca && agent.remainingDistance > agent.stoppingDistance + 0.5f)
        {
            cerca = false;
            animCntrl.OnKaijuCerca(cerca);

            StopDestinyCountdown();
        }
    }
    private void StartDestinyCountdown()
    {
        if (reachedDestinyCoroutine != null)
        {
            StopCoroutine(reachedDestinyCoroutine);
        }

        isCountdownRunning = true;
        reachedDestinyCoroutine = StartCoroutine(ReachedDestinyCountdown());
    }

    private void StopDestinyCountdown()
    {
        isCountdownRunning = false;

        if (reachedDestinyCoroutine != null)
        {
            StopCoroutine(reachedDestinyCoroutine);
            reachedDestinyCoroutine = null;
        }
    }
    public void PlayerChase()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, playerLayer))
        {
            followPlayer = true;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            agent.SetDestination(player.position);
            isMoving = true;

            if (cerca)
            {
                cerca = false;
                animCntrl.OnKaijuCerca(cerca);
                StopDestinyCountdown();
            }
        }
        else
        {
            followPlayer = false;
        }
    }

    public float CalculateDistanceToEnd()
    {
        return distanceToEnd = Vector3.Distance(transform.position, endPoint.position);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aquamarine;

        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public IEnumerator Stunned()
    {
        isStunned = true;
        animCntrl.StunnedAnim(isStunned);
        agent.isStopped = true;

        if (cerca)
        {
            StopDestinyCountdown();
        }

        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        agent.isStopped = false;
        animCntrl.StunnedAnim(isStunned);

        if (cerca && !isCountdownRunning)
        {
            StartDestinyCountdown();
        }
    }

    public IEnumerator ReachedDestinyCountdown()
    {
        while (nuclerHealth > 0 && cerca && !followPlayer)
        {
            nuclerHealth -= Time.deltaTime;
            yield return null;
        }

        if (nuclerHealth <= 0 && cerca)
        {
            atomicBomb.SetActive(true);
            AudioManager.Instance.Play("ExplosionNuclear");
            yield return new WaitForSeconds(1f);
            cameraExp.gameObject.SetActive(true);
            StartCoroutine(GameManager.Instance.NuclearLoss());
        }

    }

}
