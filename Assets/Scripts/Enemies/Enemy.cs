using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator; 

    public Transform attackPoint;
    public Transform hitbox;

    public LayerMask pLayer;

    public Rigidbody2D rb;


    public void DealingDamage(int damage)
    {
        
    }
}
