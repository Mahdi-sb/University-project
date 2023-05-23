using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile;
    public int line;

    private float cooldown;

    [SerializeField] public Sprite idleSprite;
    [SerializeField] public Sprite attackSprite;
    [SerializeField] public Projectile projectile;
    [SerializeField] public float AttackSpeed = 2;
    [SerializeField] public GameObject upgradeButton;
    [SerializeField] public float hp = 3;
    [SerializeField] public bool canAttack = true;
    [SerializeField] public int maximumUpgrade = 2;

    [SerializeField] public int damage = 1;

    private int upgradeLevel = 0;

    void Update()
    {
        if (canAttack)
        {
            AttackScript();
        }

        if (Input.GetMouseButtonDown(0))
        {
            upgradeButton.SetActive(false);
        }

        CheckClick();
    }

    private void AttackScript()
    {
        cooldown += Time.deltaTime;
        if (cooldown > AttackSpeed && GameManager.instance.LineHasEnemy(line))
        {
            cooldown = 0;
            GetComponent<SpriteRenderer>().sprite = attackSprite;
            var prj = Instantiate(projectile, transform);
            prj.SetDamage(damage);
            Invoke("SetIdle", 1);
        }
    }

    private void SetIdle()
    {
        GetComponent<SpriteRenderer>().sprite = idleSprite;
    }

    public void DoDamage()
    {
        hp -= Time.deltaTime;
        if (hp <= 0)
        {
            Death();
        }
    }

    protected void Death()
    {
        tile.Empty();
        if (line > 5)
        {
            AIManager.instance.unitsCount[line - 6]--;
        }

        Destroy(gameObject);
    }

    public void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] cubeHit = Physics2D.RaycastAll(cubeRay, Vector2.zero);

            foreach (var hit in cubeHit)
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    if (CanUpgrade() && this.line <= 5)
                        upgradeButton.SetActive(true);
                }
            }
        }
    }

    public bool CanUpgrade()
    {
        return upgradeLevel < maximumUpgrade;
    }

    public void Upgrade()
    {
        if (!CanUpgrade())
            return;

        if (line <= 5)
        {
            if (GameManager.instance.Money < 100)
                return;
            GameManager.instance.UpdateMoney(-100);
        }
        else
        {
            if (GameManager.instance.OpponentMoney < 100)
                return;
            GameManager.instance.UpdateEnemyMoney(-100);
        }
        upgradeLevel++;
        damage++;
        switch (upgradeLevel)
        {
            case 1:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
        CardManager.instance.Placed();

    }
}

