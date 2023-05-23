using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private int price;
    [SerializeField] private MonoBehaviour unit;

    void Start()
    {
        priceText.SetText(price.ToString());
    }
    

    private void OnMouseDown()
    {
        if (CardManager.instance.GetSelectedCard() == this)
        {
            CardManager.instance.ClearSelected();
        }
        else
        {
            CardManager.instance.SelectCard(this);
        }
    }

    public int GetPrice()
    {
        return price;
    }

    public Unit GetUnit()
    {
        return (Unit)unit;
    }
}