using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Tooltip("")]
    [SerializeField] private TextMeshProUGUI ammoCounter;

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

    public void UpdateAmmoText(int bulletsLeft, int magSize)
    {
        if (ammoCounter != null)
        {
            ammoCounter.SetText(bulletsLeft + " / " + magSize);
        }
    }

}
