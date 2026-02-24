using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Variables
    #region

    // Floats
    public float MoveSpeed = 3f;

    // GameObjects
    private GameObject Player;

    // NavMeshAgents
    [SerializeField] NavMeshAgent NavAgent;

    #endregion

    // Start - Find Player GameObject
    #region

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    #endregion


    // Update - Move towards the Player
    #region

    // Update is called once per frame
    void Update()
    {
        NavAgent.SetDestination(Player.transform.position);
    }

    #endregion
}
