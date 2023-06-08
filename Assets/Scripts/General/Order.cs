using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    #region Variables
    public enum EnemyTypes
    {
        Soldier,
        Swarm,
    }

    [Tooltip("The type of enemy of this order")]
    public EnemyTypes _enemy;

    [Tooltip("The amount of this enemy to spawn ")]
    public float _enemyAmount = 1;

    #endregion
}
