using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Variables
    #region

    // Booleans
    private bool IsAwake = false;

    // Floats
    public float MoveSpeed = 3f;
    [SerializeField] private float ViewDistance = 10f;
    [SerializeField] private float ViewAngle = 60f;

    // GameObjects
    private GameObject Player;

    // LayerMasks
    [SerializeField] private LayerMask ObstacleMask;

    // NavMeshAgents
    [SerializeField] NavMeshAgent NavAgent;

    // Script Reference
    [SerializeField] private CS_Timer TimerReference;
    [SerializeField] private CS_GameManager GameManager;

    #endregion

    // Start - Find Player GameObject
    #region

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    #endregion


    // Update - Move towards the Player, and get faster when the time runs out
    #region

    // Update is called once per frame
    void Update()
    {
        CheckLineOfSight();
        if (IsAwake)
        {
            NavAgent.SetDestination(Player.transform.position);
            GoAggro();
        }
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

        // Check distance
        if (DistanceToPlayer <= ViewDistance)
        {
            // Check angle
            float Angle = Vector3.Angle(transform.forward, DirectionToPlayer);

            if (Angle < ViewAngle / 2f)
            {
                // Raycast to check if something blocks view
                if (!Physics.Raycast(transform.position, DirectionToPlayer, DistanceToPlayer, ObstacleMask))
                {
                    IsAwake = true;
                }
            }
        }
    }

    #endregion
}
