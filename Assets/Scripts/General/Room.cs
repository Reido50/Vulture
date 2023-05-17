using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Identification")]

    [Tooltip("The unique ID of this room")]
    [SerializeField] private string _roomID = "";

    [Header("Cover")]

    [Tooltip("List of cover transforms")]
    [SerializeField] private Transform _coverParent;

    // A record of which covers are taken
    private List<int> _coverRecords = new List<int>();

    private List<Transform> _coverTransforms = new List<Transform>();

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
            _coverRecords.Add(0);
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
    public Transform QueryCover(Transform player, LayerMask playerMask)
    {
        for (int i = 0; i < _coverTransforms.Count; i++)
        {
            // If the cover is already being used, continue
            if (_coverRecords[i] == 1)
            {
                continue;
            }

            RaycastHit hit;
            Vector3 dir = player.position - _coverTransforms[i].position;

            Debug.DrawRay(_coverTransforms[i].position, dir * 100, Color.green);

            if (Physics.Raycast(_coverTransforms[i].position, dir, out hit, 100, playerMask))
            {
                // If the cover can "see" the player, don't use it!
                if (hit.transform.CompareTag("Player"))
                {
                    continue;
                }
            }

            // Player can't see the cover!
            _coverRecords[i] = 1;
            return _coverTransforms[i];
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
        int ind = _coverTransforms.IndexOf(returnedCover);

        if (ind < _coverRecords.Count)
        {
            _coverRecords[ind] = 0;
        }
    }
}
