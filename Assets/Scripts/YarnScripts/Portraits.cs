using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn.Unity;


public class Portraits : DialogueViewBase
{
[SerializeField] RectTransform container;
[SerializeField] TMPro.TextMeshProUGUI text;
[SerializeField] TMPro.TextMeshProUGUI name;
//[SerializeField] Image portrait;
Action advanceHandler;

public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
{
    container.gameObject.SetActive(true);
    text.text = dialogueLine.Text.Text;
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

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        UserRequestedViewAdvancement();
    }
}

}