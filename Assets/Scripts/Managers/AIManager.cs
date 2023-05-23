using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour
{
    [SerializeField] public Tile[] tiles;
    [SerializeField] private Card normalCard;
    [SerializeField] private Card tankCard;
    [SerializeField] private Card mineCard;
    [SerializeField] public Tile[] mineTiles;

    private int aiDifficulty = -1;
    public int[] unitsCount = new int[6];
    public static AIManager instance;
    private float timer;
    private DateTime? lastSentUnit;


    void Start()
    {
        lastSentUnit = null;
        instance = this;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Time.timeScale == 0)
            return;
        Hammer();
        PlaceMines();
        PlaceUnits();
        SendUnit();

    }

    private void SendUnit()
    {
        if (aiDifficulty == 0) return; //Just Normal and Hard

        if (lastSentUnit != null && lastSentUnit.Value.Second == new DateTime().Second) return;
        if (GetMoney() < 150) return;

        CardManager.instance.SendUnit(false, 50);
    }

    private void PlaceUnits()
    {
        for (int i = 0; i < 6; i++)
        {
            if (LineEnemies(i) > unitsCount[i])
            {
                foreach (var tile in tiles)
                {
                    if (tile.GetLine() == i && tile.isFree())
                    {
                        if (unitsCount[i] < 5 && GetMoney() >= normalCard.GetPrice())
                            PlaceUnit(tile, normalCard);
                        else if (unitsCount[i] == 5 && GetMoney() >= tankCard.GetPrice())
                            PlaceUnit(tile, tankCard);
                        else if (unitsCount[i] > 5 && GetMoney() >= mineCard.GetPrice())
                            PlaceUnit(tile, mineCard);
                        return;
                    }
                    else if (tile.GetLine() == i && !tile.isFree() && aiDifficulty == 2 && tile.unit.CanUpgrade())
                    {
                        if (tile.unit.canAttack)
                            tile.unit.Upgrade();
                    }
                }
            }
        }
    }

    private void PlaceMines()
    {
        if (aiDifficulty != 2) return; // Just Hard

        //Don't place mines if it's not early game
        if (timer > 30f) return;

        foreach (var tile in mineTiles)
        {
            if (tile.isFree() && GetMoney() >= mineCard.GetPrice() && Random.Range(0, 2) == 1 &&
                EnemiesCount() == 0)
            {
                PlaceUnit(tile, mineCard);
            }
        }
    }

    private void Hammer()
    {
        if (aiDifficulty == 0) return; //Just Normal and Hard

        //Hammer close enemies
        for (var i = 0; i < GameManager.instance.bottomEnemies.Count; i++)
        {
            var enemy = GameManager.instance.bottomEnemies[i];
            if (enemy.lane > 6 && enemy.transform.position.x < -3.5 && GetMoney() >= 50)
            {
                enemy.DoDamage(10000);
                GameManager.instance.UpdateEnemyMoney(-50);
                i--;
            }
        }
    }

    private int GetMoney()
    {
        return GameManager.instance.OpponentMoney;
    }

    private int LineEnemies(int line)
    {
        return GameManager.instance.CountAttackersInLane(line + 6);
    }

    private int EnemiesCount()
    {
        var sum = 0;
        for (int i = 0; i < 6; i++)
            sum += GameManager.instance.CountAttackersInLane(i + 6);
        return sum;
    }

    private void PlaceUnit(Tile tile, Card card)
    {
        tile.PlaceUnitAI(card);
        unitsCount[tile.GetLine()]++;
    }

    public void SetAI(int aiNum)
    {
        this.aiDifficulty = aiNum;
        //If AI is Hard, Send an extra unit at first of the game
        if (aiDifficulty == 2)
            CardManager.instance.SendUnit(false, 50);
    }
}