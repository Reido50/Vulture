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
        _activeRoom = room;
    }

    #endregion
}
