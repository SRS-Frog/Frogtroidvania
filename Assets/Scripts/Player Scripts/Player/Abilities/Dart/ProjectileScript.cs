using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float lifeTime = 1f;
    public float force;
    public bool enableCrosshairFiring; // depends on enableCrosshairFiring in DartScript.cs

    void Start ()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        if (enableCrosshairFiring) {
            // Rotation logic, similar to the logic in DartScript.cs
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            Vector3 rotation = transform.position - mousePos;

            // Set the velocity of the projectile
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        } else {
            // Set the velocity of the projectile, no need for rotation calc
            rb.velocity = transform.right * force;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update ()
    {

    }

}