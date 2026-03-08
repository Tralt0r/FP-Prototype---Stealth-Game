using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Variables
    #region

    // Booleans
    private bool IsAwake = false;
    private bool IsAggro = false;

    // Floats
    public float MoveSpeed = 3f;
    [SerializeField] private float ViewDistance = 10f;
    [SerializeField] private float ViewAngle = 60f;

    // Patrol
    [SerializeField] private Transform[] PatrolPoints;
    private int CurrentPatrolIndex = 0;

    // GameObjects
    private GameObject Player;

    // LayerMasks
    [SerializeField] private LayerMask ObstacleMask;

    // NavMeshAgents
    [SerializeField] NavMeshAgent NavAgent;

    // Script Reference
    [SerializeField] private CS_Timer TimerReference;
    [SerializeField] private CS_GameManager GameManager;
    [SerializeField] private Animator EnemyAnimator;

    #endregion

    // Start - Find Player GameObject
    #region

    void Start()
    {
        Player = GameObject.Find("Player");
        NavAgent.speed = MoveSpeed;

        EnemyAnimator = GetComponent<Animator>();

        if (PatrolPoints.Length > 0)
        {
            NavAgent.SetDestination(PatrolPoints[CurrentPatrolIndex].position);
        }
    }

    #endregion


    // Update - Move towards the Player, and get faster when the time runs out
    #region

    void Update()
    {
        if (IsAggro)
        {
            ChasePlayer();
        }
        CheckLineOfSight();

        if (IsAwake)
        {
            NavAgent.SetDestination(Player.transform.position);
        }
        else
        {
            HandlePatrol();
        }

        GoAggro();
        float speed = NavAgent.velocity.magnitude / NavAgent.speed;
        EnemyAnimator.SetFloat("Speed?", speed);
    }

    #endregion


    // GoAggro - When timer runs out, enemy moves twice as fast
    #region

    private void GoAggro()
    {
        float ElapsedTime = TimerReference.GetElapsedTime();

        if (ElapsedTime <= 0)
        {
            NavAgent.speed = MoveSpeed * 3;
            NavAgent.acceleration = 20;
        }
    }

    #endregion

    // OnCollisionEnter - Enemy touches player, kill player
    #region

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("entity"))
        {
            GameManager.GameLoss();
        }
    }

    #endregion

    // CheckLineOfSight - See if player is in range, attack
    #region

    private void CheckLineOfSight()
    {
        Vector3 DirectionToPlayer = (Player.transform.position - transform.position).normalized;
        float DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (DistanceToPlayer <= ViewDistance)
        {
            float Angle = Vector3.Angle(transform.forward, DirectionToPlayer);

            if (Angle < ViewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, DirectionToPlayer, DistanceToPlayer, ObstacleMask))
                {
                    IsAwake = true;
                    return;
                }
            }
        }

        if (DistanceToPlayer > ViewDistance * 1.5f)
        {
            IsAwake = false;
        }
    }

    #endregion

    private void HandlePatrol()
    {
        if (PatrolPoints.Length == 0) return;

        if (!NavAgent.pathPending && NavAgent.remainingDistance < 0.5f)
        {
            CurrentPatrolIndex++;
            if (CurrentPatrolIndex >= PatrolPoints.Length)
            {
                CurrentPatrolIndex = 0;
            }

            NavAgent.SetDestination(PatrolPoints[CurrentPatrolIndex].position);
        }
    }

    public void ChasePlayer()
    {
        IsAwake = true;
        IsAggro = true;
        ViewDistance = 100f;
    }
}