using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrappleIndicator : MonoBehaviour
{
    // active when you have frapple ability
    private SpriteRenderer spriteRenderer;
    [SerializeField] Color cannotFrapple;
    [SerializeField] Color canFrapple;
    private Vector2 currPos;

    // frapple script
    private FrappleScript frappleScript;

    private void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>(); // get the sprite renderer
        spriteRenderer.color = cannotFrapple;

        frappleScript = transform.parent.GetChild(1).gameObject.GetComponent<FrappleScript>(); // reference the frapple script of the frappleEnd
    }

    private void Update()
    {
        transform.position = currPos;
        Indicate(frappleScript.Frappable(currPos));
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    /// <summary>
    /// Indicate whether a certain position is frappable.
    /// </summary>
    /// <param name="pos">The position the indicator should move to</param>
    /// <param name="frappable">Whether that position is frappable</param>

    private void Indicate(bool frappable)
    {
        if (frappable)
        {
            spriteRenderer.color = canFrapple;
        } else
        {
            spriteRenderer.color = cannotFrapple;
        }
    }

    public void Move(Vector2 pos)
    {
        currPos = pos;
    }
}
