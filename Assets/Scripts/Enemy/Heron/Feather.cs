using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private int damage;
    private Vector2 dir;
    private Vector3 startPos;
    
    public void Init(Vector2 dir)
    {
        this.dir = dir.normalized;
        transform.right = this.dir;
        startPos = transform.position;
    }
    
    void Update()
    {
        transform.position += (Vector3) dir * (Time.deltaTime * speed);
        if (Vector3.Distance(startPos, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        Health h = col.gameObject.GetComponent<Health>();
        if (h && col.gameObject.CompareTag("Player"))
        {
            GameObject p = h.gameObject;
            h.Damage(damage);
            p.GetComponent<Rigidbody2D>().AddForce((p.transform.position - transform.position).normalized * 3f);
        }
    }
}
