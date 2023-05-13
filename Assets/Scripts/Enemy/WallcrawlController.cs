using System;
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
        [SerializeField] private int damage;

        private float raycastLength;
        private float raycastOffset;

        private Health health;

        // Start is called before the first frame update
        void Start()
        {
            raycastLength = GetComponent<SpriteRenderer>().bounds.size.y/2;
            raycastOffset = GetComponent<SpriteRenderer>().bounds.size.x/2;

            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
            
            CheckStateChange();
            
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (health.health <= 0)
            {
                Destroy(gameObject);
            }
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
                        curState = States.ROTATION;
                    }
                    break;
                case States.ROTATION:
                    if (right ? checkBit(bRaycast, 2) : checkBit(bRaycast, 0))
                    {
                        curState = States.FORWARD;
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

        private void OnCollisionEnter2D(Collision2D col)
        {
            Health h = col.gameObject.GetComponent<Health>();
            if (h && col.gameObject.CompareTag("Player"))
            {
                GameObject p = h.gameObject;
                h.Damage(damage);
                p.GetComponent<Rigidbody2D>().AddForce((p.transform.position - transform.position).normalized * 3f);
            }
        }
    }
}
