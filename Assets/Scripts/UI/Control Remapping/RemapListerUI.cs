using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemapListerUI : MonoBehaviour
{
    [SerializeField] private InputActionAsset PI;
    
    [Space(10)]
    [SerializeField] private GameObject BindUISingle;
    [SerializeField] private Transform RemapUIContainer;
    [SerializeField] private GameObject remapPrompt;

    [SerializeField] private string[] blacklist = {};
    
    // Start is called before the first frame update
    public void Generate()
    {
        foreach (var input in PI)
        {
            if (blacklist.Contains(input.name)) continue;
            
            GameObject bind = Instantiate(BindUISingle, RemapUIContainer);
            bind.GetComponent<BindUISingle>().Initialise(input, remapPrompt);
        }
    }

    public void Clear()
    {
        while (RemapUIContainer.childCount > 0)
        {
            DestroyImmediate(RemapUIContainer.GetChild(0).gameObject);
        }
    }

    private void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            PI.LoadBindingOverridesFromJson(rebinds);
    }

    public void OnDisable()
    {
        var rebinds = PI.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void ResetBindings()
    {
        PI.RemoveAllBindingOverrides();
    }
}
