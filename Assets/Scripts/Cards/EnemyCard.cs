using UnityEngine;

public class EnemyCard : Card
{
    [SerializeField] private Enemy enemy;
    private void OnMouseDown()
    {
        CardManager.instance.SelectCard(this,true);
    }

    public Unit GetUnit()
    {
        return null;
    }
}
