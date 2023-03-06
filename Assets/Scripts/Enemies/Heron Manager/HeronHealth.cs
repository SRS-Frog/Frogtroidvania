using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronHealth : MonoBehaviour
{
    public int health = 100;

    public bool isImmune = false;

    public void DamageInflicted(int damage)
    {
        if(isImmune)
        {
            return;
        }

        health -= damage;

        if (health <= 40)
        {
            GetComponent<Animator>().SetBool("isDesperate", true);
        }

        if (health <= 0)
        {
            Defeated();
        }
    }

    public void Defeated()
    {
        Debug.Log("Defeated Heron");
    }

}
