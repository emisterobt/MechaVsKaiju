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
    [SerializeField] public bool isStunned = false;


    [Header("EndGame")]
    [SerializeField] private GameObject atomicBomb;
    [SerializeField] private float timeToEnd;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialDistance = Vector3.Distance(transform.position, endPoint.position);
        animCntrl = GetComponent<KaijuAnimationController>();
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


            if (agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = false;
                //Cuando llega al destino
                StartCoroutine(ReachedDestiny());
                StartCoroutine(GameManager.Instance.GameOver());
            }
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
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        agent.isStopped = false;
        animCntrl.StunnedAnim(isStunned);
    }

    public IEnumerator ReachedDestiny()
    {
        yield return new WaitForSeconds(timeToEnd);
        atomicBomb.SetActive(true);

    }

}
