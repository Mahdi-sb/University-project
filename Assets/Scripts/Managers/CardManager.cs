using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    private Card selectedCard;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ClearSelected();
    }

    public void SelectCard(Card card, bool enemy = false)
    {
        ClearSelected();
        if (enemy)
        {
            if (card.GetPrice() <= GameManager.instance.Money)
            {
                SendUnit(true, card.GetPrice());
            }

            return;
        }

        selectedCard = card;
        card.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void SendUnit(bool player, int price)
    {
        GameManager.instance.UpdateMoney(-price, player);
        GameManager.instance.SpawnEnemy(!player,true);
        GameManager.instance.AddIncome(10, player);
    }

    public void ClearSelected()
    {
        if (selectedCard != null)
        {
            selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
            selectedCard = null;
        }
    }

    public bool CanPlace()
    {
        if (selectedCard == null)
            return false;
        return selectedCard.GetPrice() <= GameManager.instance.Money;
    }

    public Unit GetUnit()
    {
        return selectedCard.GetUnit();
    }

    public void Placed()
    {
        GameManager.instance.UpdateMoney(-selectedCard.GetPrice());
        ClearSelected();
    }

    public Card GetSelectedCard()
    {
        return selectedCard;
    }

    public bool IsHammerSelected()
    {
        return selectedCard.GetComponent<Killer>() != null;
    }
}