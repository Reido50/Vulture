using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartSpawner : MonoBehaviour
{
    #region Variables

    [Tooltip("The types of enemies to spawn")]
    public Order.EnemyTypes _enemyType;

    [Tooltip("The amount of time inbetween spawns")]
    [SerializeField] private float _maxSpawnBuffer = 3;

    [Header("References")]

    [Tooltip("The current pool of enemies")]
    [SerializeField] private List<GameObject> _enemies;

    // Reference to the Room this belongs to
    private Room _parentRoom;

    private int _orderAmount = 0;
    private float _spawnBuffer = 0;
    private float _spawnTimer = 0;

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

        _spawnBuffer = Random.Range(_maxSpawnBuffer / 2, _maxSpawnBuffer);
    }

    private void Update()
    {
        if (_orderAmount > 0)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= _spawnBuffer)
            {
                _spawnBuffer = Random.Range(_maxSpawnBuffer / 2, _maxSpawnBuffer);
                _spawnTimer = 0;
                Spawn();
            }
        }
    }

    /// <summary>
    /// Spawns a random enemy from the pool
    /// </summary>
    public void Spawn()
    {
        GameObject newEnemy = Instantiate(SelectEnemy(), transform.position, Quaternion.identity);
        newEnemy.transform.parent = this.transform;

        _orderAmount -= 1;
    }


    public GameObject SelectEnemy()
    {
        switch (_enemyType)
        {
            case Order.EnemyTypes.Soldier:

                return _enemies[0];

            case Order.EnemyTypes.Swarm:

                return _enemies[1];

        }

        return null;
    }

    public void AcceptOrder(int amount)
    {
        _orderAmount += amount;
    }

    #endregion
}
