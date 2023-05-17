using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    #region Variables

    [Header("Bullet Variables")]

    [Tooltip("The prefab that will be spawned as a bullet")]
    [SerializeField] protected GameObject _bulletPrefab;

    [Tooltip("The speed of the bullet")]
    [SerializeField] protected float _bulletSpeed = 20;

    [Tooltip("The delay between firing")]
    [SerializeField] protected float _bulletDelay = 1;

    [Tooltip("The offset for bullet instantiation")]
    [SerializeField] protected float _bulletOffset = 1;

    [Header("Aiming Variables")]

    [Tooltip("The percentage of shots that should hit the player")]
    [Range(0, 1)]
    [SerializeField] protected float _aimPercentage = 0.5f;

    [Tooltip("The distance in which shots can miss in each axis")]
    [SerializeField] protected float _maxMissDistance = 2;

    // Is the weapon currently firing?
    private bool _isFiring = false;

    // The current delay timer between firing
    private float _currentDelay = 0;

    // Reference to the player transform
    private Transform _playerReference;

    #endregion

    #region Methods

    protected virtual void Start()
    {
        // Player reference needed for aiming matters, can be changed later if needed
        _playerReference = GameManager._instance.GetPlayerReference();
    }

    protected virtual void Update()
    {
        if (_isFiring)
        {
            _currentDelay += Time.deltaTime;

            if (_currentDelay >= _bulletDelay)
            {
                _currentDelay = 0;
                Fire();
            }
        }
    }

    /// <summary>
    /// Toggles firing state on and off
    /// </summary>
    /// <param name="shouldFire">True if this enemy should be attacking</param>
    public virtual void ToggleFiring(bool shouldFire)
    {
        _isFiring = shouldFire;
    }

    /// <summary>
    /// Handles logic for creating, setup, aiming, and firing of a bullet
    /// </summary>
    protected virtual void Fire()
    {
        if (_bulletPrefab != null)
        {
            // Instaniate the bullet and set as child
            GameObject newBullet = Instantiate(_bulletPrefab, transform.position + transform.forward * _bulletOffset, Quaternion.identity);

            // Set bullet layer
            if (newBullet.GetComponent<Bullet>())
            {
                newBullet.GetComponent<Bullet>().SetBulletLayer(false);
            }

            // Determine chances of successful aiming
            float aimCheck = Random.Range(0f, 1f);
            Vector3 dir = transform.forward;

            // Is an unsuccessful aim chance
            if (aimCheck > _aimPercentage)
            {
                dir += new Vector3(
                    Random.Range(-_maxMissDistance, _maxMissDistance),
                    Random.Range(-_maxMissDistance, _maxMissDistance),
                    Random.Range(-_maxMissDistance, _maxMissDistance));

                dir.Normalize();
            }

            // Add velocity to bullet rigid body and fire!
            Rigidbody body = newBullet.GetComponent<Rigidbody>();
            body.velocity = dir * _bulletSpeed;
        }
    }

    #endregion
}
