using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    [Tooltip("The time until the bullet will despawn naturally")]
    [SerializeField] private float timeTilDestroy = 3f;

    [Header("Collision Values")]

    [Tooltip("The layer for player bullets")]
    [SerializeField] private string _playerBulletLayer;

    [Tooltip("The layer for enemy bullets")]
    [SerializeField] private string _enemyBulletLayer;

    #endregion

    #region Methods

    private void Start()
    {
        // Starts countdown for destruction
        Destroy(this.gameObject, timeTilDestroy);
    }

    /// <summary>
    /// Sets the bullets physics layer accordingly based on ownership
    /// </summary>
    /// <param name="isPlayerBullet">True if the player is shooting this bullet</param>
    public void SetBulletLayer(bool isPlayerBullet)
    {
        string chosenLayer = isPlayerBullet ? _playerBulletLayer : _enemyBulletLayer;
        gameObject.layer = LayerMask.NameToLayer(chosenLayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // CURRENTLY, JUST DESTROY BULLET. UNLESS?????

        Destroy(this.gameObject);
    }

    #endregion
}
