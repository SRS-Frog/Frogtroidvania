using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindUISingle : MonoBehaviour
{
    [SerializeField] private InputActionReference action;
    private bool isComposite;
    private int[] bindingIndicies = {0, 1};

    [SerializeField] private TextMeshProUGUI actionNameUI;
    [SerializeField] private TextMeshProUGUI actionBind1UI;
    [SerializeField] private TextMeshProUGUI actionBind2UI;

    [SerializeField] private GameObject remapPrompt;
    
    // Start is called before the first frame update
    public void Initialise(InputAction action, GameObject g)
    {
        this.action = ScriptableObject.CreateInstance<InputActionReference>();
        this.action.Set(action);

        remapPrompt = g;
        RefreshUI();
    }

    public void Awake()
    {
        isComposite = action.action.bindings[0].isComposite;
        if (isComposite)
        {
            List<int> compositeIndicies = new List<int>();
            for (int i = 0; i < action.action.bindings.Count; i++)
            {
                if(action.action.bindings[i].isComposite) compositeIndicies.Add(i);
            }

            bindingIndicies[0] = compositeIndicies[0];
            if(compositeIndicies.Count > 1) bindingIndicies[1] = compositeIndicies[1];
        }
    }

    public void Start()
    {
        RefreshUI();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.BoundControlsChanged)
            return;

        RefreshUI();
    }
    
    public void OnClick(bool secondary)
    {
        remapPrompt.SetActive(true);

        if (isComposite)
        {
            int index = bindingIndicies[(secondary ? 1 : 0)] + 1;
            BindComposite(index, secondary);
        }
        else
        {
            var rebindOperation = action.action.PerformInteractiveRebinding(bindingIndicies[secondary ? 1 : 0])
                .WithBindingGroup("Keyboard+Mouse").WithCancelingThrough("<Keyboard>/escape");;
            rebindOperation.OnComplete(operation =>
            {
                remapPrompt.SetActive(false);
                rebindOperation?.Dispose();
            });
            rebindOperation.OnCancel(operation =>
            {
                remapPrompt.SetActive(false);
                rebindOperation?.Dispose();
            });
            rebindOperation.Start();
        }
    }

    void BindComposite(int i, bool secondary)
    {
        var rebindOperation = action.action.PerformInteractiveRebinding(i)
            .WithBindingGroup("Keyboard+Mouse").WithCancelingThrough("<Keyboard>/escape");;
        rebindOperation.OnComplete(operation =>
        {
            rebindOperation?.Dispose();
            if (i + 1 < action.action.bindings.Count && action.action.bindings[i + 1].isPartOfComposite)
            {
                BindComposite(i + 1, secondary);
            }
            else
            {
                remapPrompt.SetActive(false);
            }
        });
        rebindOperation.OnCancel(operation =>
        {
            remapPrompt.SetActive(false);
            rebindOperation?.Dispose();
        });
        rebindOperation.Start();
        remapPrompt.GetComponent<RemapPrompt>().AddString(" for " + action.action.bindings[i].name);
    }

    void RefreshUI()
    {
        actionNameUI.text = action.action.name;
        
        actionBind1UI.text = action.action.GetBindingDisplayString(bindingIndicies[0]);
        if(action.action.bindings.Count > 1) actionBind2UI.text = action.action.GetBindingDisplayString(bindingIndicies[1]);
    }
}
