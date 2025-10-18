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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialDistance = Vector3.Distance(transform.position, endPoint.position);
    }

    private void Update()
    {
        PlayerChase();
        WalkToEndPoint();
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
            
        }
    }

    public void PlayerChase()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, playerLayer))
        {
            followPlayer = true;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            agent.SetDestination(player.position);
            Debug.Log("En rango del Jugador");
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
}
