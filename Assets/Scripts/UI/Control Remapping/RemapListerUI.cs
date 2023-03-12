using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemapListerUI : MonoBehaviour
{
    private PlayerControls PI;

    [SerializeField] private GameObject BindUISingle;
    [SerializeField] private Transform RemapUIContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        PI = new PlayerControls();

        foreach (var input  in PI)
        {
            GameObject bind = Instantiate(BindUISingle, RemapUIContainer);
            bind.GetComponent<BindUISingle>().Initialise(input);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
