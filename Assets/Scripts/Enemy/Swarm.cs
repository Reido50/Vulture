using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : Enemy
{
    #region Variables

    [Header("General Tracer Options")]

    [Tooltip("The damage done by one strike")]
    [SerializeField] private float _strikeDamage = 5f;

    [Header("Layer Masks")]

    [Tooltip("The layermask for detecting walls to climb")]
    [SerializeField] private LayerMask _wallMask;

    [Tooltip("The layermask for detecting floors")]
    [SerializeField] private LayerMask _floorMask;

    // The target of this enemies movement
    private Transform _target;

    public bool _grounded = true;

    #endregion

    #region Methods

    protected override void Start()
    {
        base.Start();

        if (_agent)
        {
            _agent.updateRotation = true;
        }

        // Start by targeting the player
        if (_playerRef)
        {
            _target = _playerRef;
        }

        ChangeState(EnemyStates.OutOfRange);
    }

    protected override void Update()
    {
        base.Update();

        GroundedCheck();

        switch (_state)
        {
            case EnemyStates.OutOfRange:

                if (_agent)
                {
                    _agent.SetDestination(_target.position);
                }

                break;
            case EnemyStates.InRange:
                break;
            case EnemyStates.Stunned:
                break;
            case EnemyStates.Covering:
                break;
        }
    }

    protected override void ChangeState(EnemyStates newState)
    {
        base.ChangeState(newState);

        // Resets (clears out) the path of the navmesh for every change
        if (_agent != null)
        {
            _agent.ResetPath();
            _target = null;
        }

        switch (_state)
        {
            case EnemyStates.OutOfRange:

                if (_playerRef)
                {
                    _target = _playerRef;
                }

                break;
            case EnemyStates.InRange:
                break;
            case EnemyStates.Stunned:
                break;
            case EnemyStates.Covering:
                break;
        }
    }

    private void GroundedCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, _agent.height, _floorMask))
        {
            _grounded = true;
            return;
        }

        _grounded = false;
    }

    #endregion
}
