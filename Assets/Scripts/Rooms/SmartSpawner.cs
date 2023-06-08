using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartSpawner : MonoBehaviour
{
    #region Variables

    [Header("References")]

    [Tooltip("The types of enemies to spawn")]
    [SerializeField] private List<GameObject> _enemies;

    // Reference to the Room this belongs to
    private Room _parentRoom;

    #endregion

    #region Methods

    private void Awake()
    {
        if (GetComponentInParent<Room>())
        {
            _parentRoom = GetComponentInParent<Room>();
        }
        else
        {
            Debug.LogWarning($"Smart Spawner {transform.name} is not a proper child of a room!");
        }

        if (_enemies.Count == 0)
        {
            Debug.LogWarning($"Smart Spawner {transform.name} is missing enemies to spawn!");
            return;
        }
    }

    /// <summary>
    /// Spawns a random enemy from the pool
    /// </summary>
    public void Spawn()
    {
        int index = Random.Range(0, _enemies.Count);

        GameObject newEnemy = Instantiate(_enemies[index], transform.position, Quaternion.identity);
        newEnemy.transform.parent = this.transform;
    }

    #endregion
}
