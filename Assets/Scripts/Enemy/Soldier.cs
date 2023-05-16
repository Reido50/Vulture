using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Enemy
{
    #region Variables

    public enum InRangeActions
    {
        StrafeLeft,
        StrafeRight,
        Charge,
        Stand,
        Cover,
    }

    [Header("Behavior")]

    [Tooltip("The current in range action of the soldier")]
    [SerializeField] private InRangeActions _inRangeAction = InRangeActions.StrafeLeft;

    [Tooltip("The max distance a strafe can be performed")]
    [SerializeField] private float maxStrafeDistance = 10;

    [Tooltip("The time constraints of which a strafe can be performed. X is bottom constraint, Y is top")]
    [SerializeField] private Vector2 _strafeTimeConstraints;

    [Tooltip("The time constraints of which a charge can be performed. X is bottom constraint, Y is top")]
    [SerializeField] private Vector2 _chargeTimeConstraints;

    [Tooltip("The time constraints of which a stand can be performed. X is bottom constraint, Y is top")]
    [SerializeField] private Vector2 _standTimeConstraints;

    [Tooltip("The time constraints of which a cover can be performed. X is bottom constraint, Y is top")]
    [SerializeField] private Vector2 _coverTimeConstraints;

    [Tooltip("Should this enemy shoot while moving?")]
    [SerializeField] private bool _shootWhileMoving = true;

    private Transform _target;
    private Vector3 _targetPosition;

    // The timer for each action
    private float _inRangeTimer = 0;

    // The point in which a new action will begin
    private float _inRangeLimit = 0;

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
            _target = null;
            _targetPosition = Vector3.zero;
        }

        switch (newState)
        {
            case EnemyStates.OutOfRange:

                // Initial following of the player
                if (_agent)
                {
                    _target = _playerRef;
                    _agent.SetDestination(_target.position);
                }

                break;

            case EnemyStates.InRange:

                // Turn firing back on if we weren't shooting while moving
                if (_weapon && !_shootWhileMoving && _playerInSight)
                {
                    _weapon.ToggleFiring(true);
                }

                _inRangeTimer = 0;

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

        // Start by targeting the player
        if (_playerRef)
        {
            _target = _playerRef;
        }

        if (_agent)
        {
            _agent.updateRotation = false;
        }
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
                    if (_agent != null && _target != null)
                    {
                        _agent.SetDestination(_target.position);
                        transform.LookAt(_target);
                    }
                    
                    break;
                case EnemyStates.InRange:

                    // Handle the logic for in-range thinking
                    ThinkInRange();

                    break;
                case EnemyStates.Stunned:
                    break;
            }
        }
    }

    /// <summary>
    /// Changes the in-range action based on parameter
    /// </summary>
    /// <param name="newAction">The new action to perform</param>
    private void ChangeAction(InRangeActions newAction)
    {
        // Resets all action values
        _inRangeAction = newAction;
        _agent.ResetPath();
        _target = null;
        _targetPosition = Vector3.zero;

        switch (newAction)
        {
            case InRangeActions.StrafeLeft:

                // Set new target position
                _inRangeLimit = Random.Range(_strafeTimeConstraints.x, _strafeTimeConstraints.y);
                _targetPosition = transform.position + (-transform.right * (Random.Range(5, maxStrafeDistance)));
                _agent.SetDestination(_targetPosition);

                break;

            case InRangeActions.StrafeRight:

                // Set new target position
                _inRangeLimit = Random.Range(_strafeTimeConstraints.x, _strafeTimeConstraints.y);
                _targetPosition = transform.position + (transform.right * (Random.Range(5, maxStrafeDistance)));
                _agent.SetDestination(_targetPosition);

                break;

            case InRangeActions.Charge:

                // Target the player
                _inRangeLimit = Random.Range(_chargeTimeConstraints.x, _chargeTimeConstraints.y);
                _target = _playerRef;

                break;

            case InRangeActions.Stand:

                _inRangeLimit = Random.Range(_standTimeConstraints.x, _standTimeConstraints.y);

                break;


            case InRangeActions.Cover:

                _inRangeLimit = Random.Range(_coverTimeConstraints.x, _coverTimeConstraints.y);

                break;
        }
    }

    private void ThinkInRange()
    {
        // Checking if the current action is finished
        // If so, start new action
        if (_inRangeTimer > _inRangeLimit)
        {
            _inRangeTimer = 0;
            ChangeAction((InRangeActions)Random.Range(0, 4));
        }

        // Constant behavior for every action
        switch (_inRangeAction)
        {
            case InRangeActions.StrafeLeft:

                transform.LookAt(_playerRef);

                break;
            case InRangeActions.StrafeRight:

                transform.LookAt(_playerRef);

                break;

            case InRangeActions.Charge:

                transform.LookAt(_playerRef);

                if (_agent && _target != null)
                {
                    _agent.SetDestination(_target.position);
                }

                break;
            case InRangeActions.Stand:

                transform.LookAt(_playerRef);

                break;

            case InRangeActions.Cover:

                transform.LookAt(_playerRef);

                break;
        }

        // Update the timer to increment between actions
        _inRangeTimer += Time.deltaTime;
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
