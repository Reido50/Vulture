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

    [Tooltip("The distance from the player to trigger an attack state")]
    [SerializeField] protected float _attackProximity = 8f;

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

    #region Protected Methods

    protected virtual void Awake()
    {
        // Initial reference grabbing
        _state = EnemyStates.Far;
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        // Grabs player reference AFTER it's set in GameManager
        if (GameManager._instance)
        {
            _playerRef = GameManager._instance.GetPlayerReference();
        }
    }

    protected virtual void Update()
    {
        if (_playerRef)
        {
            switch (_state)
            {
                case EnemyStates.Far:
                    CheckProximity();
                    break;
                case EnemyStates.Close:
                    CheckProximity();
                    break;
                case EnemyStates.Stunned:
                    break;
            }
        }
    }

    /// <summary>
    /// Checks proximity to player, changes state if outside of parameters
    /// </summary>
    protected virtual void CheckProximity()
    {
        switch(_state)
        {
            case EnemyStates.Far:

                if (Vector3.Distance(transform.position, _playerRef.position) <= _attackProximity)
                {
                    ChangeState(EnemyStates.Close);
                }
                return;

            case EnemyStates.Close:

                if (Vector3.Distance(transform.position, _playerRef.position) > _attackProximity)
                {
                    ChangeState(EnemyStates.Far);
                }
                return;
        }
    }

    #endregion
}