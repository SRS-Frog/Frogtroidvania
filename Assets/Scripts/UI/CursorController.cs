using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursorPassive;
    public Texture2D cursorClicked; // For now, we don't set the separate cursorClicked
    private void Awake() {
        ChangeCursor(cursorPassive);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(Texture2D cursorType) {

        Vector2 clickSpot = new Vector2(cursorType.width/2, cursorType.height/2);
        Cursor.SetCursor(cursorType, clickSpot, CursorMode.Auto);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
