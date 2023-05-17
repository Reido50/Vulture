using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables

    public enum EnemyStates
    {
        OutOfRange,
        InRange,
        Stunned,
        Covering
    }

    [Header("Detection Debug")]

    [Tooltip("The current state of the enemy")]
    [SerializeField] protected EnemyStates _state;

    [Tooltip("Is the player in sight?")]
    [SerializeField] protected bool _playerInSight = false;

    [Header("Detection")]

    [Tooltip("The distance from the player to trigger an attack state")]
    [SerializeField] protected float _inRangeProximity = 8f;

    [Tooltip("The distance in which the enemy can 'see' the player")]
    [SerializeField] protected float _sightDistance = 30f;

    [Tooltip("The layers that will be allowed in the player detection raycast")]
    [SerializeField] protected LayerMask _sightMask;

    [Header("Movement")]

    [Tooltip("Should the enemy slow down once the player is spotted?")]
    [SerializeField] protected bool _slowWhenPlayerSpotted = true;

    [Tooltip("If slowed, this value will be the speed reduction")]
    [Range(0, 1)]
    [SerializeField] protected float _speedReductionPercentage = 0.5f;

    [Header("Room")]

    [Tooltip("The layermask for room detection")]
    [SerializeField] protected LayerMask _roomMask;

    // Reference to the player GameObject
    protected Transform _playerRef;

    // Reference to the NavMesh Agent
    protected NavMeshAgent _agent;

    // Reference to this enemies weapon script
    protected EnemyWeapon _weapon;

    // Reference to the room this enemy is within
    public Room _currentRoom;

    // Reference to the base speed of the enemy
    protected float _baseSpeed = 0;

    // Reference to the current speed of the enemy
    protected float _currentSpeed = 0;

    // Is this enemy currently mutable in terms of speed/actions?
    protected bool _mutable = true;

    #endregion

    #region Methods

    /// <summary>
    /// Changes and properly transitions all enemy states
    /// </summary>
    /// <param name="newState">The new state of this enemy</param>
    protected virtual void ChangeState(EnemyStates newState)
    {
        _state = newState;

        switch (newState)
        {
            case EnemyStates.OutOfRange:
                _mutable = true;
                break;
            case EnemyStates.InRange:
                _mutable = true;
                break;
            case EnemyStates.Stunned:
                _mutable = false;
                break;
            case EnemyStates.Covering:
                _mutable = false;
                break;
        }
    }

    protected virtual void Awake()
    {
        // Initial reference grabbing
        _state = EnemyStates.OutOfRange;
        _agent = GetComponent<NavMeshAgent>();
        _weapon = GetComponent<EnemyWeapon>();

        _baseSpeed = _agent.speed;

        if (!_agent)
        {
            Debug.LogWarning($"Enemy {this.transform.name} is missing a NavMeshAgent component!");
        }

        if (!_weapon)
        {
            Debug.LogWarning($"Enemy {this.transform.name} is missing a weapon component!");
        }
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
                case EnemyStates.OutOfRange:
                    CheckProximity();
                    break;
                case EnemyStates.InRange:
                    CheckProximity();

                    if (!_playerInSight)
                    {
                        ChangeState(EnemyStates.OutOfRange);
                    }

                    break;
                case EnemyStates.Stunned:
                    break;
                case EnemyStates.Covering:
                    break;
            }

            // Checks for player raycast
            if (!_playerInSight)
            {
                if (CheckSightline())
                {
                    TogglePlayerSightline(true);
                }
            }
            else
            {
                if (!CheckSightline())
                {
                    TogglePlayerSightline(false);
                }
            }
        }
    }

    /// <summary>
    /// Toggles on and off whether a player can be seen
    /// </summary>
    /// <param name="playerSpotted">Is the player currently spotted?</param>
    protected virtual void TogglePlayerSightline(bool playerSpotted)
    {
        _playerInSight = playerSpotted;
        
        if (_slowWhenPlayerSpotted && _mutable)
        {
            _agent.speed = !playerSpotted ? _baseSpeed : (_baseSpeed - (_baseSpeed * _speedReductionPercentage));
        }
    }

    /// <summary>
    /// Checks proximity to player, changes state if outside of parameters
    /// </summary>
    protected virtual void CheckProximity()
    {
        switch(_state)
        {
            case EnemyStates.OutOfRange:

                if (Vector3.Distance(transform.position, _playerRef.position) <= _inRangeProximity && _playerInSight)
                {
                    ChangeState(EnemyStates.InRange);
                }
                return;

            case EnemyStates.InRange:

                if (Vector3.Distance(transform.position, _playerRef.position) > _inRangeProximity)
                {
                    ChangeState(EnemyStates.OutOfRange);
                }
                return;
        }
    }

    /// <summary>
    /// Checks raycast visiblity to player and returns results
    /// </summary>
    /// <returns>True if player is visible, else false</returns>
    protected virtual bool CheckSightline()
    {
        RaycastHit hit;
        Vector3 dir = _playerRef.position - this.transform.position;

        // Simple raycast
        if (Physics.Raycast(transform.position, dir, out hit, _sightDistance, _sightMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            // If the enemy doesn't have a current room OR the found room is different, set as new room
            if (_currentRoom == null || other.GetComponent<Room>().GetRoomID() != _currentRoom.GetRoomID())
            {
                _currentRoom = other.GetComponent<Room>();
            }
        }
    }

    #endregion
}