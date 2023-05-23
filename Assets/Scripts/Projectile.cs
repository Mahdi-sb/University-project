using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private const float speed = 10f;


    void Update()
    {
        if (transform.position.x > 12)
        {
            Destroy(gameObject);
        }
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Component component;
        if (col.gameObject.TryGetComponent(typeof(Enemy), out component))
        {
            Enemy enemy = (Enemy)component;
            enemy.DoDamage(damage);
            Destroy(gameObject);
            
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}