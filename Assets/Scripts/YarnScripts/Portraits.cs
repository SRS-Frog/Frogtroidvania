using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;


public class Portraits : DialogueViewBase
{
    [Serializable]
    public struct NameImage
    {
        public string name;
        public Sprite sprite;

    }

    [SerializeField] private InputActionAsset PI;
    [SerializeField] private NameImage[] _images;
    private Dictionary<string, Sprite> profilePics = new();

    [SerializeField] RectTransform container;
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] TMPro.TextMeshProUGUI name;
    [SerializeField] Image portrait;
    Action advanceHandler;

    private void Awake()
    {
        foreach (var ni in _images)
        {
            profilePics.Add(ni.name, ni.sprite);
        }
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        container.gameObject.SetActive(true);
        PI.FindActionMap("Player").Disable();

        name.text = dialogueLine.CharacterName;
        text.text = dialogueLine.TextWithoutCharacterName.Text;
        portrait.sprite = profilePics.ContainsKey(dialogueLine.CharacterName) ? profilePics[dialogueLine.CharacterName] : null;
        
        advanceHandler = requestInterrupt;
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        container.gameObject.SetActive(true);
        onDismissalComplete();
    }

    public override void UserRequestedViewAdvancement()
    {
        if(container.gameObject.activeSelf)
        {
            advanceHandler?.Invoke();
        }
    }

    public override void DialogueComplete()
    {
        container.gameObject.SetActive(false);
        PI.FindActionMap("Player").Enable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserRequestedViewAdvancement();
        }
    }

}