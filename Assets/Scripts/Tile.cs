using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Unit unit;
    [SerializeField] private int line;

    private void OnMouseUp()
    {
        if (unit == null && CardManager.instance.GetUnit() != null && CardManager.instance.CanPlace())
        {
            unit = Instantiate(CardManager.instance.GetUnit(), transform);
            unit.transform.localPosition = Vector3.zero;
            unit.tile = this;
            unit.line = line;
            CardManager.instance.Placed();
        }

    }

    public void PlaceUnitAI(Card card)
    {
        unit = Instantiate(card.GetUnit(), this.transform);
        unit.transform.localPosition = Vector3.zero;
        unit.tile = this;
        unit.line = line;
        GameManager.instance.UpdateEnemyMoney(-card.GetPrice());
    }

    public void Empty()
    {
        unit = null;
    }

    public int GetLine()
    {
        return line % 6;
    }

    public bool isFree()
    {
        return unit == null;
    }
}