using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindUISingle : MonoBehaviour
{
    private InputAction action;

    [SerializeField] private TextMeshProUGUI actionNameUI;
    [SerializeField] private TextMeshProUGUI actionBindUI;
    
    // Start is called before the first frame update
    public void Initialise(InputAction action)
    {
        this.action = action;
        actionNameUI.text = action.name;
        actionBindUI.text = action.GetBindingDisplayString();
    }

    // Update is called once per frame
    public void OnClick()
    {
        var rebindOperation = action.PerformInteractiveRebinding().WithBindingGroup("Keyboard+Mouse");
        rebindOperation.OnComplete(operation =>
        {
            actionNameUI.text = action.name;
            actionBindUI.text = action.GetBindingDisplayString();
            Debug.Log(action.bindings);
            rebindOperation?.Dispose();
        });
        rebindOperation.Start();
    }
}
