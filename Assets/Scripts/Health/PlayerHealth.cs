using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    #region Variables
    #endregion

    #region Methods
    protected override void Die()
    {
        base.Die();
    }

    protected override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
    }

    protected override void Heal(float heal)
    {
        base.Heal(heal);
    }

    #endregion
}
