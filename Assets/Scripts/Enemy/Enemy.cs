using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables

    public enum EnemyStates
    {
        Far,
        Close,
        Stunned
    }

    [Header("Enemy Values")]

    [Tooltip("The current state of the enemy")]
    [SerializeField] protected EnemyStates _state;

    // Reference to the player GameObject
    protected Transform _playerRef;

    // Reference to the NavMesh Agent
    protected NavMeshAgent _agent;

    #endregion

    #region Public Methods

    /// <summary>
    /// Changes and properly transitions all enemy states
    /// </summary>
    /// <param name="newState">The new state of this enemy</param>
    protected virtual void ChangeState(EnemyStates newState)
    {
        _state = newState;
    }
    #endregion

    #region Private Methods

    protected virtual void Start()
    {
        _state = EnemyStates.Far;
        _agent = GetComponent<NavMeshAgent>();

        if (GameManager._instance)
        {
            _playerRef = GameManager._instance.GetPlayerReference();
        }
    }

    #endregion
}
