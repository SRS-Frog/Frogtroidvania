using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueRunner dr;
    private bool triggered = false;
    
    [SerializeField] private string node;
    
    // Start is called before the first frame update
    void Start()
    {
        dr = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.GetComponent<PlayerController>()) return;
        if (!triggered)
        {
            dr.StartDialogue(node);
            triggered = true;
        }
    }
}
