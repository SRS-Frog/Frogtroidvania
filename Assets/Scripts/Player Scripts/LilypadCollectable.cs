using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadCollectable : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerHealthController p = col.GetComponent<PlayerHealthController>();
        if (p)
        {
            p.GetComponent<PlayerHealthController>().IncreaseMaxHealth();
            Destroy(gameObject);
        }
    }
}
