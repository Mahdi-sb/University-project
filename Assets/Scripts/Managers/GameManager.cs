using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float incomeTime = 10;
    [SerializeField] private float upgradeTime = 30;
    [SerializeField] private float minimumSpawnTime = 4;
    [SerializeField] private int initMoney = 100;
    [SerializeField] private float spawnTime = 16;
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private TextMeshPro moneyText;
    [SerializeField] private TextMeshPro enemyMoneyText;
    [SerializeField] private TextMeshPro incomeText;
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private GameObject difPanel;
    [SerializeField] private Text message;

    public static GameManager instance;

    //Own Properties
    public int Money { get; private set; }
    private int Income { get; set; } = 50;

    //Opponent Properties
    public int OpponentMoney { get; private set; }
    private int OpponentIncome { get; set; } = 50;

    //Fighters
    public int EnemyHp { get; private set; } = 4;

    public List<Enemy> topEnemies = new List<Enemy>();
    public List<Enemy> bottomEnemies = new List<Enemy>();

    private float spawnCounter;
    private float incomeTimer;
    private float upgradeTimer;


    private int[] attackersInLane;





    void Start()
    {
        //For singleton thing
        instance = this;
        // Pause The Game To Show menu
        Time.timeScale = 0;

        SetMoney(initMoney);
        SetOpponentMoney(initMoney);
        UpdateIncomeText();

        attackersInLane = new int[spawners.Length];
        gameEndPanel.SetActive(false);
        moneyText.color = Color.green;
    }

    private void SetOpponentMoney(int initMoney)
    {
        OpponentMoney = this.initMoney;
    }

    public void SetMoney(int money)
    {
        this.Money = money;
        moneyText.SetText(money.ToString());
    }

    void Update()
    {
        GiveIncome();
        SpawnEnemies();
        MakeGameHarder();
    }

    private void MakeGameHarder()
    {
        upgradeTimer += Time.deltaTime;
        if (upgradeTimer >= upgradeTime)
        {
            EnemyHp++;
            upgradeTimer = 0;
        }
    }

    private void GiveIncome()
    {
        incomeTimer += Time.deltaTime;
        if (incomeTimer > incomeTime)
        {
            incomeTimer = 0;
            UpdateMoney(Income);
            UpdateEnemyMoney(OpponentIncome);
        }
    }

    private void SpawnEnemies()
    {
        spawnCounter += Time.deltaTime;
        if (!(spawnCounter > spawnTime)) return;
        SpawnEnemy(true);
        SpawnEnemy(false);
        if (spawnTime > minimumSpawnTime)
        {
            spawnTime -= 0.5f;
        }

        spawnCounter = 0;
    }

    public int CountAttackersInLane(int num)
    {
        return attackersInLane[num];
    }

    public void SpawnEnemy(bool self, bool sent = false)
    {
        var spawnerIndex = (self ? 0 : 6) + Random.Range(0, spawners.Length / 2);
        attackersInLane[spawnerIndex]++;
        var enemy = Instantiate(enemies[0], spawners[spawnerIndex].transform);
        enemy.lane = spawnerIndex;
        if (self)
            topEnemies.Add(enemy);
        else
            bottomEnemies.Add(enemy);
        if (sent)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }

    public bool LineHasEnemy(int line)
    {
        return attackersInLane[line] > 0;
    }

    public void LineKilled(int lane)
    {
        attackersInLane[lane]--;
    }

    public void UpdateMoney(int money, bool self = true)
    {
        if (self)
            SetMoney(this.Money + money);
        else
            UpdateEnemyMoney(money);
    }

    public void UpdateEnemyMoney(int money)
    {
        OpponentMoney += money;
        enemyMoneyText.text = OpponentMoney.ToString();
    }

    public void AddIncome(int income, bool self = true)
    {
        if (self)
        {
            this.Income += income;
            UpdateIncomeText();
        }
        else
        {
            OpponentIncome += income;
        }
    }

    private void UpdateIncomeText()
    {
        incomeText.SetText("+" + this.Income);
    }

    public void Win(bool topPlayer)
    {
        message.text = topPlayer ? "You win!!" : "You Lose!!";
        message.color = topPlayer ? Color.green : Color.red;
        gameEndPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChooseEasy()
    {
        ChooseAI(0);
    }

    public void ChooseNormal()
    {
        ChooseAI(1);
    }


    public void ChooseHard()
    {
        ChooseAI(2);
    }

    private void ChooseAI(int aiNum)
    {
        AIManager.instance.SetAI(aiNum);
        Time.timeScale = 1;
        difPanel.SetActive(false);
    }
}

