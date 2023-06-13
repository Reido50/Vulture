using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region Variables 

    [Header("Identification")]

    [Tooltip("The unique ID of this room")]
    [SerializeField] private string _roomID = "";

    [Header("Spawning")]


    [Header("Cover")]

    [Tooltip("List of cover transforms")]
    [SerializeField] private Transform _coverParent;

    // A record of which covers are taken
    private Dictionary<Transform, Transform> _coverRecords = new Dictionary<Transform, Transform>();

    // The transforms of every cover position
    private List<Transform> _coverTransforms = new List<Transform>();

    // The list of all possible spawners
    private List<SmartSpawner> _soldierSpawners = new List<SmartSpawner>();


    // The list of all possible spawners
    private List<SmartSpawner> _swarmSpawners = new List<SmartSpawner>();

    // Is the player currently inside this room?
    private bool _playerInside = false;

    #endregion

    #region Methods

    void Start()
    {
        if (_roomID.Length == 0)
        {
            Debug.LogWarning("Each room should have a unique identifier. Try naming it after a function or landmark of the room.");
        }

        // Populate records with length of covers
        foreach (Transform cover in _coverParent)
        {
            _coverTransforms.Add(cover);
            _coverRecords[cover] = null;
        }

        foreach (SmartSpawner spawner in GetComponentsInChildren<SmartSpawner>())
        {
            if (spawner._enemyType == Order.EnemyTypes.Soldier)
            {
                _soldierSpawners.Add(spawner);
            }
            else
            {
                _swarmSpawners.Add(spawner);
            }
        }
    }

    /// <summary>
    /// Getter for a room's ID
    /// </summary>
    /// <returns>The room ID string</returns>
    public string GetRoomID()
    {
        return _roomID;
    }


    /// <summary>
    /// Given a player, returns a cover point that the player cannot currently see
    /// </summary>
    /// <param name="player">Reference to the player transform</param>
    /// <param name="playerMask">A layermask for cover raycast detection</param>
    /// <returns>A transform of the appropriate cover</returns>
    public Transform QueryCover(Transform player, Transform enemy, LayerMask playerMask)
    {
        // Sort transforms by distance to enemy
        List<Transform> temp = new List<Transform>(_coverTransforms);
        temp.Sort((p1, p2) => Vector3.Distance(p1.position, enemy.position).CompareTo(Vector3.Distance(p2.position, enemy.position)));

        for (int i = 0; i < temp.Count; i++)
        {
            // If the cover is already being used, continue
            if (_coverRecords[temp[i]] != null)
            {
                continue;
            }

            RaycastHit hit;
            Vector3 dir = player.position - temp[i].position;

            Debug.DrawRay(temp[i].position, dir * 100, Color.green);

            if (Physics.Raycast(temp[i].position, dir, out hit, 100, playerMask))
            {
                // If the cover can "see" the player, don't use it!
                if (hit.transform.CompareTag("Player"))
                {
                    continue;
                }
            }

            // Player can't see the cover!
            _coverRecords[temp[i]] = enemy;
            return temp[i];
        }

        // Will only return null if there are no valid covers
        return null;
    }
    
    /// <summary>
    /// Returns the cover by reseting the record
    /// </summary>
    /// <param name="returnedCover">Reference to the returned cover</param>
    public void ReturnCover(Transform returnedCover)
    {
        if (_coverRecords[returnedCover] != null)
        {
            _coverRecords[returnedCover] = null;
        }
        else
        {
            Debug.LogWarning("Returning an unused cover!");
        }
    }

    /// <summary>
    /// Toggles whether the player is inside the room or not
    /// </summary>
    /// <param name="isInside">Is the player inside the room?</param>
    public void PlayerToggle(bool isInside)
    {
        _playerInside = isInside;

        // Let the SmartMap know which room is being prioritized
        if (isInside)
        {
            if (SmartMap.instance)
            {
                SmartMap.instance.SetActiveRoom(this);
            }
        }
    }

    /// <summary>
    /// Picks the correct spawners to be activated based on the order
    /// </summary>
    /// <param name="order">The order to be spawned</param>
    /// <returns>True if it was successful, else false</returns>
    public bool SmartSpawn(Order order)
    {
        switch (order._enemy)
        {
            case Order.EnemyTypes.Soldier:

                if (_soldierSpawners.Count == 0)
                {
                    return false;
                }

                for (int i = 0; i < order._enemyAmount; i++)
                {
                    int randIndex = Random.Range(0, _soldierSpawners.Count);
                    _soldierSpawners[randIndex].AcceptOrder(1);
                }

                break;

            case Order.EnemyTypes.Swarm:

                if (_swarmSpawners.Count == 0)
                {
                    return false;
                }

                for (int i = 0; i < order._enemyAmount; i++)
                {
                    int randIndex = Random.Range(0, _swarmSpawners.Count);
                    _swarmSpawners[randIndex].AcceptOrder(1);
                }

                break;
        }

        return true;
    }

    #endregion
}
