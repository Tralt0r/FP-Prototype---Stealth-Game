using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent NavAgent;
    [SerializeField] private Animator EnemyAnimator;

    [Header("Wander Settings")]
    public bool enableRandomWander = true;
    public float wanderRadius = 5f;
    public float wanderDelay = 3f;

    private float wanderTimer;

    public Transform runToTarget;

    void Start()
    {
        if (NavAgent == null)
            NavAgent = GetComponent<NavMeshAgent>();

        if (EnemyAnimator == null)
            EnemyAnimator = GetComponent<Animator>();

        wanderTimer = wanderDelay;
    }

    void Update()
    {
        HandleWand();
        UpdateAnimation();
    }

    void HandleWand()
    {
        if (!enableRandomWander) return;

        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderDelay)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            NavAgent.SetDestination(newPos);
            wanderTimer = 0f;
        }
    }

    void UpdateAnimation()
    {
        float speed = NavAgent.velocity.magnitude / NavAgent.speed;
        EnemyAnimator.SetFloat("Speed?", speed);
    }

    // EVENT: move to a specific target
    public void MoveToTarget()
    {
        enableRandomWander = false;
        NavAgent.SetDestination(runToTarget.position);
    }

    // Random point on NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, distance, NavMesh.AllAreas);

        return hit.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}