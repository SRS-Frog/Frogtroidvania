using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    private Rigidbody2D rb;
    [SerializeField] private int healAmount;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        GameObject objectHit = col.gameObject;
        if (objectHit.CompareTag("Player")) {
            objectHit.GetComponent<Health>().ExpandMaxHealth(healAmount);
            Destroy(gameObject);
        }        
    }
}