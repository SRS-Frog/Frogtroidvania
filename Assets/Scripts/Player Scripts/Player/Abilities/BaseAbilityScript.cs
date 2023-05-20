using UnityEngine;

// Simple abstract class for abilities
// Currently mainly to control if they trigger in human/frog state
public abstract class BaseAbilityScript : MonoBehaviour {
    public abstract bool IsHumanAbility();
    public abstract bool IsFrogAbility();
    
    private StateManager stateManager;
    public virtual void Awake() {
        stateManager = transform.parent.GetChild(0).gameObject.GetComponent<StateManager>();
        Debug.Log(stateManager);
    }
    public bool CanTriggerAbility() {
        return (IsHumanAbility() == stateManager.isHuman() ||
                IsFrogAbility() == !stateManager.isHuman());
    }
}