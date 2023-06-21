using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]

    [Tooltip("The reference to the ammo counter text")]
    [SerializeField] private TextMeshProUGUI _ammoCounter;

    [Tooltip("The reference to the health counter text")]
    [SerializeField] private TextMeshProUGUI _healthCounter;

    [Tooltip("The reference to the current round text")]
    [SerializeField] private TextMeshProUGUI _currentRound;

    [Tooltip("The reference to the objective text")]
    [SerializeField] private TextMeshProUGUI _objectiveText;

    [Tooltip("The reference to the world space canvas")]
    public Canvas _worldSpaceCanvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Updates the ammo counter text
    /// </summary>
    /// <param name="bulletsLeft">How many bullets left in current mag</param>
    /// <param name="magSize">The total size of the mag</param>
    public void UpdateAmmoText(int bulletsLeft, int magSize)
    {
        if (_ammoCounter != null)
        {
            //_ammoCounter.SetText(bulletsLeft + " / " + magSize);
            _ammoCounter.SetText($"{bulletsLeft}");
        }
    }

    /// <summary>
    /// Updates current health of player
    /// </summary>
    /// <param name="currentHealth">The current player health to be displayed</param>
    public void UpdateHealthText(float currentHealth)
    {
        if (_healthCounter != null)
        {
            _healthCounter.SetText($"{currentHealth}");
        }
    }

    /// <summary>
    /// Updates the round UI
    /// </summary>
    /// <param name="currRound">What number round is it</param>
    /// <param name="roundName">The name of the current round</param>
    public void UpdateRound(int currRound, string roundName)
    {
        if (_currentRound != null)
        {
            if (currRound == -1)
            {
                _currentRound.SetText("Incoming hostiles...");
            }
            else
            {
                _currentRound.SetText($"Round {currRound}");
            }
        }

        if (_objectiveText != null)
        {
            _objectiveText.SetText(roundName);
        }
    }

}
