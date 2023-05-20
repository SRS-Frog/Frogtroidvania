using UnityEngine;
using UnityEngine.InputSystem;

public class DartController : MonoBehaviour
{
    // dart variables
    private DartScript dartScript; // dart script

    //private PlayerStates playerStates;
    private PlayerInput playerInput;

    private InputAction shootDartAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootDartAction = playerInput.actions["Shoot Dart"];

        dartScript = transform.parent.GetChild(2).gameObject.GetComponent<DartScript>(); // reference the frapple script of the frappleEnd
    }

    private void OnEnable()
    {
        shootDartAction.performed += ShootDartControl;
    }

    private void OnDisable()
    {
        shootDartAction.performed -= ShootDartControl;
    }

    private void ShootDartControl(InputAction.CallbackContext context)
    {
        dartScript.Shoot(); // shoot the dart
        Debug.Log("Shoot Dart Pressed");
    }
}