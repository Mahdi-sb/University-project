using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        
    }

    private void OnMouseDown()
    {
        unit.Upgrade();
    }
    
}
