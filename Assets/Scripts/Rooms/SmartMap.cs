using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartMap : MonoBehaviour
{
    #region Variables

    // Instance for singleton
    public static SmartMap instance;

    // List of all the rooms in the scene (filled at runtime)
    private List<Room> _rooms = new List<Room>();

    [Header("Room Values")]

    [Tooltip("The current active room (with the player in it)")]
    [SerializeField] private Room _activeRoom;

    #endregion

    #region Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Start by finding all rooms
            foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
            {
                _rooms.Add(room.GetComponent<Room>());
            }

            if (_rooms.Count == 0)
            {
                Debug.LogWarning("Your scene is missing rooms - spawning will not work properly!");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Setter for the active room (with the player in it)
    /// </summary>
    /// <param name="room">Reference to the active room</param>
    public void SetActiveRoom(Room room)
    {
        if (_activeRoom != null)
        {
            Vector2 backlog = _activeRoom.Shutdown();

            Segment backlogSegment = new Segment();
            backlogSegment._orders = new List<Order>();
            
            if (backlog.x != 0)
            {
                Order newOrder = new Order();
                newOrder._enemy = Order.EnemyTypes.Soldier;
                newOrder._enemyAmount = (int)backlog.x;

                backlogSegment._orders.Add(newOrder);
            }

            if (backlog.y != 0)
            {
                Order newOrder = new Order();
                newOrder._enemy = Order.EnemyTypes.Swarm;
                newOrder._enemyAmount = (int)backlog.y;

                backlogSegment._orders.Add(newOrder);
            }

            AcceptSegment(backlogSegment);
        }

        _activeRoom = room;
    }

    /// <summary>
    /// Takes a segment and gives the spawning to the current active room
    /// </summary>
    /// <param name="segment">The current segment of the round</param>
    public void AcceptSegment(Segment segment)
    {
        foreach (Order order in segment._orders)
        {
            if (_activeRoom.SmartSpawn(order))
            {
                continue;
            }

            bool roomFound = false;

            foreach (Room room in _rooms)
            {
                if (room.SmartSpawn(order))
                {
                    roomFound = true;
                    break;
                }
            }

            if (!roomFound)
            {
                Debug.LogWarning("No rooms available to fulfill this segment's order!");
            }
        }
    }

    #endregion
}
