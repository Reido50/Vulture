using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment
{
    [Tooltip("The orders in this round segment")]
    public List<Order> _orders;

    [Tooltip("If the prior segment is about to end, begin spawning from new segment instead of waiting")]
    public bool _allowEarlySpawning = true;
}
