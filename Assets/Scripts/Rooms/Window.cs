using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    #region Variables

    [Header("Options")]

    [Tooltip("The material this becomes after locking itself")]
    [SerializeField] private Material _lockedMaterial;

    private Room _parentRoom;
    private int counter = 0;

    #endregion

    #region Methods

    private void Awake()
    {
        if (GetComponentInParent<Room>())
        {
            _parentRoom = GetComponentInParent<Room>();
        }
    }

    /// <summary>
    /// Tells the room to depressurize
    /// </summary>
    public void Break()
    {
        counter++;

        if (_parentRoom)
        {
            _parentRoom.Depressurize();
        }
    }

    /// <summary>
    /// Sets the glass material to the correct locked visual
    /// </summary>
    public void Lock()
    {
        if (_lockedMaterial != null)
        {
            transform.GetChild(0).GetComponent<Renderer>().material = _lockedMaterial;
        }
    }

    #endregion
}
