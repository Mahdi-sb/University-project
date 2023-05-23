using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ChildUnit : Unit
{
    [SerializeField] public Sprite grownUpSprite;
    [SerializeField] public Sprite explodingSprite;
    [SerializeField] public Sprite explodeSprite;

    private const double grownTime = 10;
    private double grownCounter = 0;

    private ChildState state;
    private SpriteRenderer spriteComponent;

    // Start is called before the first frame update
    void Start()
    {
        spriteComponent = GetComponent<SpriteRenderer>();
        state = ChildState.child;
        damage = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (grownCounter < grownTime)
        {
            grownCounter += Time.deltaTime;
            if (grownCounter >= grownTime)
            {
                Grow();
            }
        }

        if (state == ChildState.grownUp)
        {
            RaycastHit2D [] hits = Physics2D.RaycastAll(transform.position, transform.right , 0.85f);
            foreach(var hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<Enemy>())
                {
                    state = ChildState.exploding;
                    spriteComponent.sprite = explodingSprite;
                    Invoke("ChangeExplodeSprite", 1f);
                    Invoke("Explode", 1.3f);
                }
            }
        }
    }

    private void ChangeExplodeSprite()
    {
        spriteComponent.sprite = explodeSprite;
    }

    private void Explode()
    {
        RaycastHit2D [] hits = Physics2D.RaycastAll(transform.position, transform.right , 0.85f);
        foreach(var hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.DoDamage(damage);
            }
            Destroy(gameObject);
        }
        Death();
        
    }

    private void Grow()
    {
        state = ChildState.grownUp;
        spriteComponent.sprite = grownUpSprite;
    }
}

enum ChildState
{
    child,
    grownUp,
    exploding
}