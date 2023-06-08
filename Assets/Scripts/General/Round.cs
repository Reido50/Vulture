using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Round : ScriptableObject
{
    [Header("Segment Values")]

    [Tooltip("The segments that make up this round")]
    public List<Segment> _segments;

    [Header("Options")]

    [Tooltip("The max amount of enemies that can be spawned at one time")]
    public int _maxEnemiesSpawned = 10;
}
