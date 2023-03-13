using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemapPrompt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private string addString;

    private void OnEnable()
    {
        addString = "";
        text.text = "Waitng for input" + addString;
    }

    public void AddString(string s)
    {
        addString = s;
        text.text = "Waitng for input" + addString;
    }
}
