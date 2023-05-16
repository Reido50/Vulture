using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Reference to the singleton instance
    public static GameManager _instance;

    // Reference to the player transform
    private Transform _playerTransform;

    #endregion

    #region Methods

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            // Initial player grab (can be changed later for a more logical approach)
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            if (_playerTransform == null)
            {
                Debug.LogWarning("Missing a player in the scene!");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Getter for player transform reference
    /// </summary>
    /// <returns>Player transform</returns>
    public Transform GetPlayerReference()
    {
        return _playerTransform;
    }

    #endregion
}
