using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region Variables

    public static RoundManager _instance;

    public enum RoundState
    {
        InRound,
        InBetween,
        InMenu,
    }

    [Header("Options")]

    [Tooltip("The current state of the game")]
    [SerializeField] private RoundState _roundState = RoundState.InRound;

    [Tooltip("The amount of time inbetween rounds")]
    [SerializeField] private float _inBetweenLength = 15;

    [Header("References")]

    [Tooltip("The list of rounds that runs the game loop")]
    [SerializeField] private List<Round> _rounds;

    private int _currentRound = 0;
    private int _currentSegment = 0;
    private int _totalEnemiesRemaining = 0;
    private int _segmentEnemiesRemaining = 0;
    private float _inBetweenTimer = 0;


    #endregion

    #region Methods

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (_rounds.Count > 0)
        {
            ChangeRoundState(RoundState.InBetween);
        }
        else
        {
            Debug.LogWarning("RoundManager is missing rounds!");
        }
    }

    public void ChangeRoundState(RoundState newState)
    {
        _roundState = newState;

        switch (_roundState)
        {
            case RoundState.InRound:

                if (_currentRound >= _rounds.Count)
                {
                    _currentRound = 0;
                    Debug.Log("Ran out of rounds! Restarting the sequence...");
                }

                // Calculate the total number of enemies this round
                _totalEnemiesRemaining = _rounds[_currentRound].GetTotalEnemies();

                Debug.Log($"Current Round: {_rounds[_currentRound].name}");

                // Set the segment value
                if (_rounds[_currentRound]._segments.Count > 0)
                {
                    _currentSegment = 0;
                }
                else
                {
                    Debug.LogWarning("Current round is missing segments!");
                }

                SpawnSegment();

                break;

            case RoundState.InBetween:

                _inBetweenTimer = _inBetweenLength;

                break;

            case RoundState.InMenu:
                break;
        }
    }


    private void Update()
    {
        switch (_roundState)
        {
            case RoundState.InRound:

                if (_totalEnemiesRemaining <= 0)
                {
                    Debug.Log($"Round {_rounds[_currentRound]} over!");
                    _currentRound++;
                    ChangeRoundState(RoundState.InBetween);

                    return;
                }

                if (_segmentEnemiesRemaining <= 0)
                {
                    Debug.Log($"Segment {_currentSegment} of {_rounds[_currentRound]} over!");
                    _currentSegment += 1;
                    SpawnSegment();
                }

                break;

            case RoundState.InBetween:

                if (_inBetweenTimer <= 0)
                {
                    ChangeRoundState(RoundState.InRound);
                }

                _inBetweenTimer -= Time.deltaTime;

                break;

            case RoundState.InMenu:
                break;
        }
    }

    public RoundState GetGameState()
    {
        return _roundState;
    }

    public void RecordEnemyKill(Order.EnemyTypes enemyType)
    {
        _segmentEnemiesRemaining -= 1;
        _totalEnemiesRemaining -= 1;
    }

    public void SpawnSegment()
    {
        _segmentEnemiesRemaining = _rounds[_currentRound]._segments[_currentSegment].GetTotalEnemies();
        SmartMap.instance.AcceptSegment(_rounds[_currentRound]._segments[_currentSegment]);
    }

    #endregion
}
