using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{

    #region Variables

    [Header("Options")]
    [SerializeField] private float _timeTillDeath = 1f;

    // Reference to the damage text
    private TextMeshProUGUI _damageText;

    // Has the number been setup yet?
    private bool _initialized = false;

    // Internal timer for movement and deletion
    private float _timer = 0;

    #endregion

    #region Methods

    private void Start()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Initializes number with values and references
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Initialize(float damageAmount)
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up * 180);

        if (_damageText)
        {
            _damageText.SetText($"{damageAmount}");
        }

        _initialized = true;
    }

    private void Update()
    {
        if (_initialized)
        {
            // Lower number position and scale over time
            if (_timer < _timeTillDeath)
            {
                transform.position -= Vector3.up * Time.deltaTime;
                transform.localScale -= transform.localScale * Time.deltaTime;

                _timer += Time.deltaTime;

                return;
            }

            Destroy(this.gameObject);
        }
    }

    #endregion
}
