using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    #region Variables
    #endregion

    #region Methods

    /// <summary>
    /// Upon death, delete GameObject
    /// </summary>
    protected override void Die()
    {
        base.Die();

        Destroy(this.gameObject);
    }

    #endregion
}
