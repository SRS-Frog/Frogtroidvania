using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private AttackScript attackScript;
    private PlayerInput playerInput;
    private InputAction attackAction;
    [SerializeField] GameObject attackPoint;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
        attackScript = attackPoint.GetComponent<AttackScript>();
    }

    private void OnEnable()
    {
        attackAction.performed += AttackControl;
    }

    private void OnDisable()
    {
        attackAction.performed -= AttackControl;
    }

    private void AttackControl(InputAction.CallbackContext context) {
        attackScript.TriggerAttack();
    }
}
