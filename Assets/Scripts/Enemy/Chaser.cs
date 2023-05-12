using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : Enemy
{
    #region Variables

    #endregion

    #region Override Methods

    /// <summary>
    /// Changes and properly transitions all soldier states
    /// </summary>
    /// <param name="newState">The new state of this enemy</param>
    protected override void ChangeState(EnemyStates newState)
    {
        base.ChangeState(newState);

        // Resets (clears out) the path of the navmesh for every change
        if (_agent != null)
        {
            _agent.ResetPath();
        }

        switch (newState)
        {
            case EnemyStates.Far:

                if (_agent != null)
                {
                    _agent.SetDestination(_playerRef.position);
                }

                break;

            case EnemyStates.Close:
                break;

            case EnemyStates.Stunned:
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_playerRef)
        {
            switch (_state)
            {
                case EnemyStates.Far:

                    // Soldiers chase the player when out of attack proximity
                    if (_agent != null)
                    {
                        _agent.SetDestination(_playerRef.position);
                    }
                    
                    break;
                case EnemyStates.Close:
                    break;
                case EnemyStates.Stunned:
                    break;
            }
        }
    }

    #endregion
}
