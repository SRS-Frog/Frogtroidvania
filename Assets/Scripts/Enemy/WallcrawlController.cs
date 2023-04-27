using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WallcrawlController : MonoBehaviour
    {
        enum States
        {
            FALLING,
            FORWARD,
            ROTATION
        }
        private States curState = States.FALLING;
    
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
        void FixedUpdate()
        {
            Move();
            
            CheckStateChange();
        }

        private void Move()
        {
            switch(curState)
            {
                case States.FALLING:
                    transform.position += Vector3.down * Time.fixedDeltaTime;
                    break;
                
                case States.FORWARD:
                    transform.position += transform.right * Time.fixedDeltaTime;
                    break;
                case States.ROTATION:
                    transform.RotateAround(transform.position - transform.up * raycastLength, Vector3.forward, -1);
                    break;
            }
        }

        private void CheckStateChange()
        {
            uint bRaycast = BottomRaycasts();

            switch (curState)
            {
                case States.FALLING:
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, climbableLayer);
                    if (hit)
                    {
                        transform.up = hit.normal;
                        curState = States.FORWARD;
                    }

                    break;
                case States.FORWARD:
                    if (!checkBit(bRaycast, 1) && !(right ? checkBit(bRaycast, 2) : checkBit(bRaycast, 0)))
                    {
                        curState = Physics2D.Raycast(transform.position - transform.up * raycastLength - transform.right * raycastOffset,
                            transform.right, raycastOffset, climbableLayer) ? States.CONCAVE_ROT : States.ROTATION;
                    }
                    break;
                case States.ROTATION:
                    if (right ? checkBit(bRaycast, 2) : checkBit(bRaycast, 0))
                    {
                        curState = States.FORWARD;
                        transform.up = Physics2D.Raycast(transform.position + transform.right * raycastOffset,
                            -transform.up, raycastLength, climbableLayer).normal;
                    }
                    break;
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
}
