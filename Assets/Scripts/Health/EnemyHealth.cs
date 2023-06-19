using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    #region Variables

    public Order.EnemyTypes _enemyType;

    [Tooltip("Prefab reference for damage numbers")]
    [SerializeField] private GameObject _damageNumber;

    #endregion

    #region Methods

    /// <summary>
    /// Spawns enemy damage numbers
    /// </summary>
    /// <param name="dmg">The amount of damage taken</param>
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        if (_damageNumber)
        {
            GameObject dmgNumber = Instantiate(_damageNumber);
            dmgNumber.transform.SetParent(UIManager.instance._worldSpaceCanvas.transform, true);
            dmgNumber.transform.position = transform.position + new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            dmgNumber.GetComponent<DamageNumber>().Initialize(dmg);
        }
    }

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
