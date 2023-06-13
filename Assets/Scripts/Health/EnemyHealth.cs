using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    #region Variables

    public Order.EnemyTypes _enemyType;

    #endregion

    #region Methods

    /// <summary>
    /// Upon death, delete GameObject
    /// </summary>
    protected override void Die()
    {
        base.Die();

        RoundManager._instance.RecordEnemyKill(_enemyType);

        Destroy(this.gameObject);
    }

    #endregion
}
