using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 4;
    public int lane;
    private float currentSpeed;
    private float speed = 0.5f;
    private Animator animator;

    [SerializeField] private GameObject damageParticle;


    void Start()
    {
        currentSpeed = speed;
        animator = GetComponent<Animator>();
        health = GameManager.instance.EnemyHp;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.8f, 1 << 6);
        if (hit)
        {
            currentSpeed = 0;
            hit.collider.GetComponent<Unit>().DoDamage();
            animator.Play("Enemy1Attack");
        }
        else
        {
            currentSpeed = speed;
            animator.Play("Idle");
        }
        
        transform.Translate(-1 * Time.deltaTime * currentSpeed, 0, 0);
        CheckClick();
    }


    public void DoDamage(int dmg)
    {
        var particle = Instantiate(damageParticle);
        particle.transform.position = transform.position;
        health -= dmg;
        if (health <= 0)
        {
            Death();
        }
    }

    public void DoDamage()
    {
        DoDamage(1);
    }

    private void CheckClick()
    {
        if (CardManager.instance.IsHammerSelected() && CardManager.instance.CanPlace() && Input.GetMouseButtonDown(0))
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] cubeHit = Physics2D.RaycastAll(cubeRay, Vector2.zero);

            foreach (var hit in cubeHit)
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    Death();
                    CardManager.instance.Placed();
                }
            }
        }
    }


    private void Death()
    {
        GameManager.instance.LineKilled(lane);
        if (lane <= 6)
        {
            GameManager.instance.topEnemies.Remove(this);
        }
        else
        {
            GameManager.instance.bottomEnemies.Remove(this);
        }
        Destroy(gameObject);
    }
}