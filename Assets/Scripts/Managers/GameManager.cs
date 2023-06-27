using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Reference to the singleton instance
    public static GameManager _instance;

    // Reference to the player transform
    private Transform _playerTransform;

    // Is the game currently paused?
    private bool _isPaused = false;

    private bool _isLowGrav = false;

    private int _brokenCount = 0;

    public delegate void GravChange();
    public static event GravChange OnLowGrav;

    #endregion

    #region Methods

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            // Initial player grab (can be changed later for a more logical approach)
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            if (_playerTransform == null)
            {
                Debug.LogWarning("Missing a player in the scene!");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ToggleGravity(bool toLow)
    {
        if (toLow)
        {
            _brokenCount++;

            if (_brokenCount == 1)
            {
                _isLowGrav = true;
                OnLowGrav();
            }
        }
        else
        {
            _brokenCount--;

            if (_brokenCount <= 0)
            {
                _isLowGrav = false;
                OnLowGrav();
            }
        }
    }

    /// <summary>
    /// Getter for player transform reference
    /// </summary>
    /// <returns>Player transform</returns>
    public Transform GetPlayerReference()
    {
        return _playerTransform;
    }

    /// <summary>
    /// Toggles the pause menu on and off
    /// </summary>
    public void TogglePause()
    {
        _isPaused = !_isPaused;

        Time.timeScale = _isPaused ? 0 : 1;

        UIManager.instance.ToggleOnScreenUI(_isPaused ? UIManager.UIType.Pause : UIManager.UIType.Game);
    }

    public void LoseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
        UIManager.instance.ToggleOnScreenUI(UIManager.UIType.End);
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
