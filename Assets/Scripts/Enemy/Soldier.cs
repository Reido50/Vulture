using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Enemy
{
    #region Variables

    #endregion

    #region Public Methods

    /// <summary>
    /// Changes and properly transitions all soldier states
    /// </summary>
    /// <param name="newState">The new state of this enemy</param>
    protected override void ChangeState(EnemyStates newState)
    {
        base.ChangeState(newState);

        switch (newState)
        {
            case EnemyStates.Far:
                break;
            case EnemyStates.Close:
                break;
            case EnemyStates.Stunned:
                break;
        }
    }

    #endregion

    #region Private Methods

    protected override void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (_playerRef)
        {
            switch (_state)
            {
                case EnemyStates.Far:

                    // Initial movement setting (PICK UP FROM HERE)
                    _agent.SetDestination(_playerRef.position);

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
