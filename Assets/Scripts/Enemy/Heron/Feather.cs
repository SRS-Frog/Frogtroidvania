using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField] private Vector2 dir;
    [SerializeField] private float speed;

    private void Start()
    {
        Init(dir);
    }

    // Start is called before the first frame update
    void Init(Vector2 dir)
    {
        this.dir = dir.normalized;
        transform.right = this.dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3) dir * (Time.deltaTime * speed);
    }
}
