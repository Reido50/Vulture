using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Enemy
{
    #region Variables

    [Header("Behavior")]

    [Tooltip("Should this enemy shoot while moving?")]
    [SerializeField] private bool _shootWhileMoving = true;

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
            case EnemyStates.OutOfRange:

                // Initial following of the player
                if (_agent)
                {
                    _agent.SetDestination(_playerRef.position);
                }

                break;

            case EnemyStates.InRange:

                // Turn firing back on if we weren't shooting while moving
                if (_weapon && !_shootWhileMoving && _playerInSight)
                {
                    _weapon.ToggleFiring(true);
                }

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
                case EnemyStates.OutOfRange:

                    // Soldiers chase the player when out of attack proximity
                    if (_agent != null)
                    {
                        _agent.SetDestination(_playerRef.position);
                    }
                    
                    break;
                case EnemyStates.InRange:

                    transform.LookAt(_playerRef.position);

                    break;
                case EnemyStates.Stunned:
                    break;
            }
        }
    }

    /// <summary>
    /// Sets firing on and off based on if player is in raycast
    /// </summary>
    /// <param name="playerSpotted">Is the player spotted?</param>
    protected override void TogglePlayerSightline(bool playerSpotted)
    {
        base.TogglePlayerSightline(playerSpotted);

        // If moving and we don't want to shoot, turn firing off!
        if (_state == EnemyStates.OutOfRange && !_shootWhileMoving)
        {
            _weapon.ToggleFiring(false);
            return;
        }

        _weapon.ToggleFiring(playerSpotted);
    }

    #endregion
}
