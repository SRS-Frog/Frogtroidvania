using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Wallcrawl : MonoBehaviour
{
    [SerializeField] private bool right = true;
    [SerializeField] private LayerMask climbableLayer;

    private float raycastLength;
    private float raycastOffset;

    // Start is called before the first frame update
    void Start()
    {
        raycastLength = GetComponent<SpriteRenderer>().bounds.size.y/2;
        raycastOffset = GetComponent<SpriteRenderer>().bounds.size.x/2;
    }

    // Update is called once per frame
    void Update()
    {
        uint bRaycast = BottomRaycasts();
        if (checkBit(bRaycast, 1) && (right ? checkBit(bRaycast, 2) : checkBit(bRaycast, 0)))
        {
            transform.position += transform.right * Time.deltaTime;
        }
        else if (Physics2D.Raycast(transform.position, transform.right, raycastOffset, climbableLayer))
        {
            transform.RotateAround(transform.position - transform.up * raycastLength, Vector3.forward, 20 * Time.deltaTime);
        }
        else if (checkBit(bRaycast, 1) && !(right ? checkBit(bRaycast, 2) : checkBit(bRaycast, 0)))
        {
            transform.RotateAround(transform.position - transform.up * raycastLength, Vector3.forward, -20 * Time.deltaTime);
        }
        else if(bRaycast == 0)
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
    }

    bool checkBit(uint num, int bit)
    {
        return (num & (1 << bit)) != 0;
    }

    uint BottomRaycasts()
    {
        uint output = 0;
        //Left
        if (Physics2D.Raycast(transform.position - transform.right * raycastOffset, -transform.up, raycastLength, climbableLayer))
        {
            output += 1;
        }
        //Center
        if (Physics2D.Raycast(transform.position, -transform.up, raycastLength, climbableLayer))
        {
            output += 2;
        }
        //Right
        if (Physics2D.Raycast(transform.position + transform.right * raycastOffset, -transform.up, raycastLength, climbableLayer))
        {
            output += 4;
        }

        return output;
    }
}
