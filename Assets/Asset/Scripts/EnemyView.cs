using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public float health;
    public Animator animator;

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;

            if(health <= 0)
            {
                animator.SetTrigger("Death");
                health = 0;
            }
        }
       

    }
}
