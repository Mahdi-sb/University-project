using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseArea : MonoBehaviour
{
    [SerializeField] private bool topPlayer;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(typeof(Enemy), out _))
        {
            GameManager.instance.Win(!topPlayer);
        }
    }
}
